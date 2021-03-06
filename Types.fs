namespace DummyPathtracer.Types

open System.Numerics

[<Struct>]
type Color = Color of Vector3

[<Struct>]
type Point3 = Point3 of Vector3

[<Struct>]
type Ray = { Origin: Point3; Direction: Vector3 }

[<Struct>]
type Material =
    | Lambertian of albedo: Color
    | Metal of metalAlbedo: Color * fuzz: float32
    | Dielectric of indexOfRefraction: float32

[<Struct>]
type HitRecord =
    { P: Point3
      Normal: Vector3
      T: float32
      FrontFace: bool
      Material: Material }

[<Struct>]
type Sphere =
    { Center: Point3
      Radius: float32
      Material: Material }

[<Struct>]
type Hittable =
    | Sphere of sphere: Sphere
    | HittableList of list: HittableList

and [<Struct>] HittableList = { Objects: Hittable array }

type Camera =
    { CameraOrigin: Point3
      LowerLeftCorner: Point3
      Horizontal: Vector3
      Vertical: Vector3
      U: Vector3
      V: Vector3
      W: Vector3
      LensRadius: float32 }
