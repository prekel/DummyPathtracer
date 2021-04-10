namespace DummyPathtracer

type Sphere =
    { Center: Point3
      Radius: float32 }
    interface IHittable with
        member this.Hit ray tMin tMax =
            let oc =
                (Point3.value ray.Origin)
                - (Point3.value this.Center)

            let a = ray.Direction.LengthSquared()
            let halfB = Vector3.dot oc ray.Direction

            let c =
                oc.LengthSquared() - this.Radius * this.Radius

            let discriminant = halfB * halfB - a * c

            if (discriminant < 0.f) then
                ValueNone
            else
                let sqrtd = sqrt discriminant

                let root1 = (-halfB - sqrtd) / a
                let root2 = (-halfB + sqrtd) / a

                match root1 < tMin || tMax < root1, root2 < tMin || tMax < root2 with
                | true, true -> ValueNone
                | a, b ->
                    let t =
                        match a, b with
                        | true, false -> root2
                        | _ -> root1

                    let p = Ray.at ray t

                    let normal =
                        (Point3.value p - Point3.value this.Center)
                        / this.Radius

                    ValueSome({ P = p; Normal = normal; T = t })
