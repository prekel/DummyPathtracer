namespace DummyPathtracer

[<Struct>]
type Sphere = { Center: Point3; Radius: float32 }

[<Struct>]
type Hittable =
    | Sphere of sphere: Sphere
    | HittableList of list: HittableList

and [<Struct>] HittableList = { Objects: Hittable list }

module Hittable =
    let rec hit ray tMin tMax hittable =
        match hittable with
        | Sphere sphere ->
            let oc =
                (Point3.value ray.Origin)
                - (Point3.value sphere.Center)

            let a = ray.Direction.LengthSquared()
            let halfB = Vector3.dot oc ray.Direction

            let c =
                oc.LengthSquared() - sphere.Radius * sphere.Radius

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

                    let outwardNormal =
                        (Point3.value p - Point3.value sphere.Center)
                        / sphere.Radius

                    ValueSome(HitRecord.createFaceNormal ray outwardNormal p t)
        | HittableList list ->
            let init =
                {| ClosestSoFar = tMax
                   Rec = ValueNone
                   HitAnything = false |}

            let ret =
                (init, list.Objects)
                ||> List.fold
                        (fun st object ->
                            let hit = hit ray tMin st.ClosestSoFar object

                            match hit with
                            | ValueSome record ->
                                {| st with
                                       HitAnything = true
                                       ClosestSoFar = record.T
                                       Rec = hit |}
                            | ValueNone -> st)

            ret.Rec
