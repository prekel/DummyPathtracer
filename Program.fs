open System.Numerics
open System.IO

open DummyPathtracer

let rec rayColor r (world: Hittable) depth =
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
        | ValueSome rec' ->
            let target =
                (Point3.value rec'.P)
                + rec'.Normal
                + Vector3.randomInUnitSphere ()

            rayColor
                { Ray.Origin = rec'.P
                  Direction = target - (Point3.value rec'.P) }
                world
                (depth - 1)
            |> Color.value
            |> (*) 0.5f
            |> Color


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
                  [ Sphere
                      { Center = Point3(Vector3(0.f, 0.f, -1.f))
                        Radius = 0.5f }
                    Sphere
                        { Center = Point3(Vector3(0f, -100.5f, -1f))
                          Radius = 100f } ] }

    let camera = Camera.create ()

    use sw =
        new StreamWriter(Path.Combine(__SOURCE_DIRECTORY__, "image.ppm"))

    fprintfn sw $"P3\n%d{imageWidth} %d{imageHeight}\n255"

    for j in [ imageHeight - 1 .. -1 .. 0 ] do
        printfn $"Scanlines remaining: %d{j}"

        for i in [ 0 .. imageWidth - 1 ] do
            let pixelColor =
                [ 0 .. samplesPerPixel - 1 ]
                |> List.map
                    (fun _ ->
                        let u =
                            (float32 i + randomFloat32 ())
                            / float32 (imageWidth - 1)

                        let v =
                            (float32 j + randomFloat32 ())
                            / float32 (imageHeight - 1)

                        let r = Camera.getRay u v camera
                        rayColor r world maxDepth |> Color.value)
                |> List.sum
                |> Color

            Color.writeColor sw pixelColor samplesPerPixel

    printfn "Done."

    0
