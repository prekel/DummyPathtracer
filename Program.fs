open System
open System.Numerics
open System.IO

open DummyPathtracer
open DummyPathtracer.Types

let rayColor r (world: Hittable) depth random =
    let rec recRayColor (r: Ray) world depth =
        match depth <= 0 with
        | true -> Color(Vector3.Zero)
        | _ ->
            match Hittable.hit r 0.001f infinityf world with
            | ValueNone ->
                let unitDirection = r.Direction |> Vector3.unitVector
                let t = 0.5f * (unitDirection.Y + 1.f)

                (1.f - t) * Vector3.One
                + t * Vector3(0.5f, 0.7f, 1.f)
                |> Color
            | ValueSome tempRec ->
                match Material.scatter r tempRec random tempRec.Material with
                | ValueSome (struct (scattered, attenuation)) ->
                    recRayColor scattered world (depth - 1)
                    |> Color.value
                    |> (*) (Color.value attenuation)
                    |> Color
                | ValueNone -> Color(Vector3.Zero)

    recRayColor r world depth

[<Struct>]
type ScanlineRenderParams =
    { Camera: Camera
      ImageWidth: int
      ImageHeight: int
      SamplesPerPixel: int
      MaxDepth: int
      World: Hittable }

let renderScanlineParams q j =
    async {
        printfn $"Started: %d{j}"
        let random = Random()

        let y =
            [| 0 .. q.ImageWidth - 1 |]
            |> Array.map
                (fun i ->
                    let pixelColor =
                        [| 0 .. q.SamplesPerPixel - 1 |]
                        |> Array.map
                            (fun _ ->
                                let u =
                                    (float32 i + randomFloat32 random)
                                    / float32 (q.ImageWidth - 1)

                                let v =
                                    (float32 j + randomFloat32 random)
                                    / float32 (q.ImageHeight - 1)

                                let r = Camera.getRay random u v q.Camera

                                rayColor r q.World q.MaxDepth random
                                |> Color.value)
                        |> Array.sum
                        |> Color

                    Color.toStructIntTuple pixelColor q.SamplesPerPixel)

        printfn $"Ended: %d{j}"
        return y
    }

let randomScene () =
    let random = Random()

    let groundMaterial =
        Lambertian ^ Color(Vector3(0.5f, 0.5f, 0.5f))

    let ground =
        Sphere
            { Center = Point3(Vector3(0.f, -1000.f, 0.f))
              Radius = 1000.f
              Material = groundMaterial }

    let genSphere a b =
        let a = float32 a
        let b = float32 b

        let chooseMat = randomFloat32 random

        let center =
            Point3(Vector3(a + 0.9f * randomFloat32 random, 0.2f, b + 0.9f * randomFloat32 random))

        if (Point3.value center - Vector3(4.f, 0.2f, 0.f))
           |> Vector3.length > 0.9f then

            let sphereMaterial =
                match chooseMat with
                | choose when choose < 0.8f ->
                    let albedo =
                        Color(Vector3.random random * Vector3.random random)

                    Lambertian albedo
                | choose when choose < 0.95f ->
                    let albedo =
                        Color(Vector3.randomMinMax random 0.5f 1f)

                    let fuzz = randomFloat32MinMax random 0.f 0.5f
                    Metal(albedo, fuzz)
                | _ -> Dielectric 1.5f

            Sphere
                { Center = center
                  Radius = 0.2f
                  Material = sphereMaterial }
            |> ValueSome
        else
            ValueNone

    let spheres =
        Array.allPairs [| -11 .. 11 - 1 |] [| -11 .. 11 - 1 |]
        |> Array.map (fun (a, b) -> genSphere a b)
        |> Array.filter ValueOption.isSome
        |> Array.map ValueOption.get

    let material1 = Dielectric 1.5f

    let sphere1 =
        Sphere
            { Center = Point3(Vector3(0.f, 1.f, 0.f))
              Radius = 1.f
              Material = material1 }

    let material2 =
        Lambertian ^ Color(Vector3(0.4f, 0.2f, 0.1f))

    let sphere2 =
        Sphere
            { Center = Point3(Vector3(-4.f, 1.f, 0.f))
              Radius = 1.f
              Material = material2 }

    let material3 =
        Metal(Color(Vector3(0.7f, 0.6f, 0.5f)), 0.f)

    let sphere3 =
        Sphere
            { Center = Point3(Vector3(4.f, 1.f, 0.f))
              Radius = 1.f
              Material = material3 }

    Hittable.HittableList
        { HittableList.Objects =
              [| ground; sphere1; sphere2; sphere3 |]
              |> Array.append spheres }


[<EntryPoint>]
let main _ =
    let aspectRatio = 3.f / 2.f

    let imageWidth = 1200
    let imageHeight = int (float32 imageWidth / aspectRatio)
    let samplesPerPixel = 100 // TODO
    let maxDepth = 50

    let world = randomScene ()

    let lookFrom = Point3(Vector3(13.f, 2.f, 3.f))
    let lookAt = Point3(Vector3(0.f, 0.f, 0.f))
    let vUp = Vector3(0.f, 1.f, 0.f)
    let distToFocus = 10.f
    let aperture = 0.1f

    let camera =
        Camera.create lookFrom lookAt vUp 20f aspectRatio aperture distToFocus

    let q =
        { Camera = camera
          ImageWidth = imageWidth
          ImageHeight = imageHeight
          SamplesPerPixel = samplesPerPixel
          MaxDepth = maxDepth
          World = world }

    let qwe =
        [| imageHeight - 1 .. -1 .. 0 |]
        |> Array.map (renderScanlineParams q)
        |> Async.Parallel
        |> Async.RunSynchronously

    use sw =
        new StreamWriter(Path.Combine(__SOURCE_DIRECTORY__, "image.ppm"))

    fprintfn sw $"P3\n%d{imageWidth} %d{imageHeight}\n255"

    qwe
    |> Array.iter (Array.iter (fun (struct (r, g, b)) -> fprintfn sw $"%d{r} %d{g} %d{b}"))

    printfn "Done."

    0
