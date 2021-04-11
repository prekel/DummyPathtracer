module DummyPathtracer.Camera

open System.Numerics

open DummyPathtracer
open DummyPathtracer.Types

let create () =
    let aspectRatio = 16.f / 9.f
    let viewPortHeight = 2.f
    let viewportWidth = aspectRatio * viewPortHeight
    let focalLength = 1.f

    let origin = Vector3.Zero
    let horizontal = Vector3(viewportWidth, 0.f, 0.f)
    let vertical = Vector3(0.f, viewPortHeight, 0.f)

    let lowerLeftCorner =
        origin
        - horizontal / 2.f
        - vertical / 2.f
        - Vector3(0.f, 0.f, focalLength)

    { CameraOrigin = Point3 origin
      LowerLeftCorner = Point3 lowerLeftCorner
      Horizontal = horizontal
      Vertical = vertical }

let getRay (u: float32) (v: float32) camera =
    { Ray.Origin = camera.CameraOrigin
      Direction =
          (Point3.value camera.LowerLeftCorner)
          + u * camera.Horizontal
          + v * camera.Vertical
          - (Point3.value camera.CameraOrigin) }
