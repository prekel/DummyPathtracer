module DummyPathtracer.Camera

open System.Numerics

open DummyPathtracer
open DummyPathtracer.Types

let create lookFrom lookAt vUp vFov aspectRatio =
    let theta = degreeToRadians vFov
    let h = tan (theta / 2.f)
    let viewPortHeight = 2.f * h
    let viewportWidth = aspectRatio * viewPortHeight

    let w =
        Vector3.unitVector (Point3.value lookFrom - Point3.value lookAt)

    let u = Vector3.unitVector (Vector3.cross vUp w)
    let v = Vector3.cross w u

    let origin = lookFrom
    let horizontal = viewportWidth * u
    let vertical = viewPortHeight * v

    let lowerLeftCorner =
        Point3.value origin
        - horizontal / 2.f
        - vertical / 2.f
        - w

    { CameraOrigin = origin
      LowerLeftCorner = Point3 lowerLeftCorner
      Horizontal = horizontal
      Vertical = vertical }

let getRay (s: float32) (t: float32) camera =
    { Ray.Origin = camera.CameraOrigin
      Direction =
          (Point3.value camera.LowerLeftCorner)
          + s * camera.Horizontal
          + t * camera.Vertical
          - (Point3.value camera.CameraOrigin) }
