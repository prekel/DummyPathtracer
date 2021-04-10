open System.IO
open System.Drawing

[<Struct>]
type PpmImage =
    { Width: int
      Height: int
      Pixels: struct (byte * byte * byte) array }

let readPpmImage (sr: TextReader) =
    try
        match sr.ReadLine() with
        | "P3" ->
            match sr.ReadLine().Split() |> Array.toList with
            | [ width; height ] ->
                match sr.ReadLine() with
                | "255" ->
                    let width = int width
                    let height = int height

                    let u =
                        [| 0 .. width * height - 1 |]
                        |> Array.map
                            (fun _ ->
                                let ar = sr.ReadLine().Split() |> Array.map byte
                                struct (ar.[0], ar.[1], ar.[2]))

                    Ok(
                        { Width = width
                          Height = height
                          Pixels = u }
                    )
                | _ -> Error "Max color must be 255"
            | _ -> Error "Width Height must be 2"
        | _ -> Error "Header must be \"P3\""
    with ex -> Error <| string ex


let ppmImageToBitmap ppm =
    let bitmap = new Bitmap(ppm.Width, ppm.Height)

    for i in [ 0 .. ppm.Height - 1 ] do
        for j in [ 0 .. ppm.Width - 1 ] do
            let struct (r, g, b) = ppm.Pixels.[i * ppm.Width + j]
            bitmap.SetPixel(j, i, Color.FromArgb(int r, int g, int b))

    bitmap

[<EntryPoint>]
let main _ =
    use sr =
        new StreamReader(Path.Combine(__SOURCE_DIRECTORY__, "../image.ppm"))

    let ppm = readPpmImage sr

    match ppm with
    | Ok ppm ->
        let bitmap = ppmImageToBitmap ppm
        bitmap.Save(Path.Combine(__SOURCE_DIRECTORY__, "../image.png"))
    | Error er -> failwith er

    0
