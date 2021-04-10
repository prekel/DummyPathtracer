namespace DummyPathtracer

open System.Numerics

[<Struct>]
type Ray = { Origin: Point3; Direction: Vector3 }

module Ray =
    let at ray (t: float32) =
        let (Point3 origin) = ray.Origin
        origin + t * ray.Direction |> Point3
