open DummyPathtracer

open System.IO

[<EntryPoint>]
let main _ =
    let imageWidth = 256
    let imageHeight = 256

    use sw =
        new StreamWriter(Path.Combine(__SOURCE_DIRECTORY__, "image.ppm"))

    fprintfn sw $"P3\n%d{imageWidth} %d{imageHeight}\n255"

    for j in [ imageHeight - 1 .. -1 .. 0 ] do
        printfn $"Scanlines remaining: %d{j}"

        for i in [ 0 .. imageWidth - 1 ] do
            let pixelColor =
                Color(float32 i / float32 (imageWidth - 1), float32 j / float32 (imageHeight - 1), 0.25f)

            Color.writeColor sw pixelColor

    printfn "Done."

    0
