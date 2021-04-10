namespace DummyPathtracer

open System.Numerics

[<Struct>]
type Ray = { Origin: Point3; Direction: Vector3 }

module Ray =
    let at ray (t: float32) =
        (Point3.value ray.Origin) + t * ray.Direction
        |> Point3
