namespace DummyPathtracer

open System.Numerics

module Vector3 =
    let lengthSquared (v: Vector3) = v.LengthSquared()

    let cross (u: Vector3) (v: Vector3) =
        Vector3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X)

[<Struct>]
type Point3 = Point3 of Vector3

[<Struct>]
type Color = Color of Vector3

module Color =
    let writeColor sw (Color pixelColor) =
        fprintfn
            sw
            $"%d{int (255.999f * pixelColor.X)} %d{int (255.999f * pixelColor.Y)} %d{int (255.999f * pixelColor.Z)}"
