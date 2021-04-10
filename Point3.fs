namespace DummyPathtracer

open System.Numerics

[<Struct>]
type Point3 = Point3 of Vector3

module Point3 =
    let value (Point3 point3) = point3
