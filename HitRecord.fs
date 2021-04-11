module DummyPathtracer.HitRecord

open DummyPathtracer.Types

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

let createFaceNormal r outwardNormal p t material =
    let frontFace =
        Vector3.dot r.Direction outwardNormal < 0.f

    { P = p
      T = t
      FrontFace = frontFace
      Normal =
          if frontFace then
              outwardNormal
          else
              -outwardNormal
      Material = material }
