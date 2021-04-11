module DummyPathtracer.Camera

open System.Numerics

open DummyPathtracer
open DummyPathtracer.Types

let create lookFrom lookAt vUp vFov aspectRatio aperture focusDist =
    let theta = degreeToRadians vFov
    let h = tan (theta / 2.f)
    let viewPortHeight = 2.f * h
    let viewportWidth = aspectRatio * viewPortHeight

    let w =
        Vector3.unitVector (Point3.value lookFrom - Point3.value lookAt)

    let u = Vector3.unitVector (Vector3.cross vUp w)
    let v = Vector3.cross w u

    let origin = lookFrom
    let horizontal = focusDist * viewportWidth * u
    let vertical = focusDist * viewPortHeight * v

    let lowerLeftCorner =
        Point3.value origin
        - horizontal / 2.f
        - vertical / 2.f
        - focusDist * w

    let lensRadius = aperture / 2.f

    { CameraOrigin = origin
      LowerLeftCorner = Point3 lowerLeftCorner
      Horizontal = horizontal
      Vertical = vertical
      U = u
      V = v
      W = w
      LensRadius = lensRadius }

let getRay random (s: float32) (t: float32) camera =
    let rd =
        camera.LensRadius
        * Vector3.randomInUnitDisk random

    let offset = camera.U * rd.X + camera.V * rd.Y

    { Ray.Origin =
          Point3.value camera.CameraOrigin + offset
          |> Point3
      Direction =
          (Point3.value camera.LowerLeftCorner)
          + s * camera.Horizontal
          + t * camera.Vertical
          - (Point3.value camera.CameraOrigin)
          - offset }
