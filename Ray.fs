module DummyPathtracer.Ray

open DummyPathtracer.Types

let at ray (t: float32) =
    (Point3.value ray.Origin) + t * ray.Direction
    |> Point3
