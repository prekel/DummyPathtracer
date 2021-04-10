namespace DummyPathtracer

open System.Numerics

module Vector3 =
    let lengthSquared (v: Vector3) = v.LengthSquared()

    let cross (u: Vector3) (v: Vector3) =
        Vector3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X)

    let unitVector (v: Vector3) = v / v.Length()

    let dot (u: Vector3) (v: Vector3) = u.X * v.X + u.Y * v.Y + u.Z * v.Z
