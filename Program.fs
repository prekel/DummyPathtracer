open DummyPathtracer

open System.Numerics
open System.IO

let hitSphere (Point3 center) radius r =
    let (Point3 origin) = r.Origin
    let oc = origin - center
    let a = Vector3.dot r.Direction r.Direction
    let b = 2.f * Vector3.dot oc r.Direction
    let c = (Vector3.dot oc oc) - radius * radius
    let discriminant = b * b - 4.f * a * c
    discriminant > 0.f

let rayColor r =
    if hitSphere (Point3(Vector3(0.f, 0.f, -1.f))) 0.5f r then
        Color(Vector3.UnitX)
    else
        let unitDirection = r.Direction |> Vector3.unitVector
        let t = 0.5f * (unitDirection.Y + 1.f)

        (1.f - t) * Vector3.One
        + t * Vector3(0.5f, 0.7f, 1.f)
        |> Color

[<EntryPoint>]
let main _ =
    let aspectRatio = 16.f / 9.f

    let imageWidth = 400
    let imageHeight = int (float32 imageWidth / aspectRatio)

    let viewportHeight = 2.f
    let viewportWidth = aspectRatio * viewportHeight
    let focalLength = 1.f

    let origin = Point3(Vector3.Zero)
    let (Point3 originV) = origin
    let horizontal = Vector3(viewportWidth, 0.f, 0.f)
    let vertical = Vector3(0.f, viewportHeight, 0.f)

    let lowerLeftCorner =
        originV
        - horizontal / 2.f
        - vertical / 2.f
        - Vector3(0.f, 0.f, focalLength)

    use sw =
        new StreamWriter(Path.Combine(__SOURCE_DIRECTORY__, "image.ppm"))

    fprintfn sw $"P3\n%d{imageWidth} %d{imageHeight}\n255"

    for j in [ imageHeight - 1 .. -1 .. 0 ] do
        printfn $"Scanlines remaining: %d{j}"

        for i in [ 0 .. imageWidth - 1 ] do
            let u = float32 i / float32 (imageWidth - 1)
            let v = float32 j / float32 (imageHeight - 1)

            let r =
                { Origin = origin
                  Direction =
                      lowerLeftCorner + u * horizontal + v * vertical
                      - originV }

            let pixelColor = rayColor (r)

            Color.writeColor sw pixelColor

    printfn "Done."

    0
