namespace DummyPathtracer

open System.Numerics

[<Struct>]
type HitRecord =
    { P: Point3
      Normal: Vector3
      T: float32 }

type IHittable =
    abstract Hit : ray: Ray -> tMin: float32 -> tMax: float32 -> HitRecord voption
