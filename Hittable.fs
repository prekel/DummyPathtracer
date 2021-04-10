namespace DummyPathtracer

open System.Numerics

[<Struct>]
type HitRecord =
    { P: Point3
      Normal: Vector3
      T: float32
      FrontFace: bool }

module HitRecord =
    let faceNormal r outwardNormal record =
        let frontFace =
            Vector3.dot r.Direction outwardNormal < 0.f

        { record with
              FrontFace = frontFace
              Normal =
                  if frontFace then
                      outwardNormal
                  else
                      -outwardNormal }

    let createFaceNormal r outwardNormal p t =
        let frontFace =
            Vector3.dot r.Direction outwardNormal < 0.f

        { P = p
          T = t
          FrontFace = frontFace
          Normal =
              if frontFace then
                  outwardNormal
              else
                  -outwardNormal }

type IHittable =
    abstract Hit : ray: Ray -> tMin: float32 -> tMax: float32 -> HitRecord voption
