namespace DummyPathtracer

open System.Numerics

[<Struct>]
type Color = Color of Vector3

module Color =
    let value (Color color) = color

    let toStructIntTuple (Color pixelColor) samplesPerPixel =
        let scale = 1.f / (float32 samplesPerPixel)
        let r = sqrt (pixelColor.X * scale)
        let g = sqrt (pixelColor.Y * scale)
        let b = sqrt (pixelColor.Z * scale)

        let r = int (256.f * clamp r 0.f 0.999f)
        let g = int (256.f * clamp g 0.f 0.999f)
        let b = int (256.f * clamp b 0.f 0.999f)

        struct (r, g, b)

    let writeColor out pixelColor samplesPerPixel =
        let struct (r, g, b) =
            toStructIntTuple pixelColor samplesPerPixel

        fprintfn out $"%d{r} %d{g} %d{b}"
