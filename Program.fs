open System
open System.Numerics
open System.IO

open DummyPathtracer

let rec rayColor r (world: Hittable) depth random =
    match depth <= 0 with
    | true -> Color(Vector3.Zero)
    | _ ->
        match Hittable.hit r 0.f infinityf world with
        | ValueNone ->
            let unitDirection = r.Direction |> Vector3.unitVector
            let t = 0.5f * (unitDirection.Y + 1.f)

            (1.f - t) * Vector3.One
            + t * Vector3(0.5f, 0.7f, 1.f)
            |> Color
        | ValueSome tempRec ->
            let target =
                (Point3.value tempRec.P)
                + tempRec.Normal
                + Vector3.randomInUnitSphere random

            rayColor
                { Ray.Origin = tempRec.P
                  Direction = target - (Point3.value tempRec.P) }
                world
                (depth - 1)
                random
            |> Color.value
            |> (*) 0.5f
            |> Color

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

                                let r = Camera.getRay u v q.Camera

                                rayColor r q.World q.MaxDepth random
                                |> Color.value)
                        |> Array.sum
                        |> Color

                    Color.toStructIntTuple pixelColor q.SamplesPerPixel)

        printfn $"Ended: %d{j}"
        return y
    }

[<EntryPoint>]
let main _ =
    let aspectRatio = 16.f / 9.f

    let imageWidth = 400
    let imageHeight = int (float32 imageWidth / aspectRatio)
    let samplesPerPixel = 100
    let maxDepth = 50

    let world =
        Hittable.HittableList
            { HittableList.Objects =
                  [| Sphere
                      { Center = Point3(Vector3(0.f, 0.f, -1.f))
                        Radius = 0.5f }
                     Sphere
                         { Center = Point3(Vector3(0f, -100.5f, -1f))
                           Radius = 100f } |] }

    let camera = Camera.create ()

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
