namespace DummyPathtracer

open System.Numerics

[<Struct>]
type Color = Color of Vector3

module Color =
    let value (Color color) = color

    let writeColor out (Color pixelColor) samplesPerPixel =
        let scale = 1.f / (float32 samplesPerPixel)
        let r = pixelColor.X * scale
        let g = pixelColor.Y * scale
        let b = pixelColor.Z * scale

        let r = int (256.f * clamp r 0.f 0.999f)
        let g = int (256.f * clamp g 0.f 0.999f)
        let b = int (256.f * clamp b 0.f 0.999f)

        fprintfn out $"%d{r} %d{g} %d{b}"
