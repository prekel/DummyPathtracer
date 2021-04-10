namespace DummyPathtracer

open System.Numerics

module Vector3 =
    let lengthSquared (v: Vector3) = v.LengthSquared()

    let cross (u: Vector3) (v: Vector3) =
        Vector3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X)

type Point3 = Vector3

type Color = Vector3

module Color =
    let writeColor sw (color: Color) =
        fprintfn sw $"%d{int (255.999f * color.X)} %d{int (255.999f * color.Y)} %d{int (255.999f * color.Z)}"
