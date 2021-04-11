module DummyPathtracer.Color

open DummyPathtracer.Types

let value (Color color) = color

let toRgb (Color pixelColor) samplesPerPixel =
    let scale = 1.f / (float32 samplesPerPixel)
    let r = sqrt (pixelColor.X * scale)
    let g = sqrt (pixelColor.Y * scale)
    let b = sqrt (pixelColor.Z * scale)

    let r = byte (256.f * clamp r 0.f 0.999f)
    let g = byte (256.f * clamp g 0.f 0.999f)
    let b = byte (256.f * clamp b 0.f 0.999f)

    struct (r, g, b)

let writeRgb out rgb =
    let struct (r: byte, g: byte, b: byte) = rgb
    fprintfn out $"%d{r} %d{g} %d{b}"
