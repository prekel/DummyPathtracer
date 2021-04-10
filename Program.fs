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
            let r = double i / double (imageWidth - 1)
            let g = double j / double (imageHeight - 1)
            let b = 0.25

            let ir = int (255.999 * r)
            let ig = int (255.999 * g)
            let ib = int (255.999 * b)

            fprintfn sw $"%d{ir} %d{ig} %d{ib}"

    printfn "Done."

    0
