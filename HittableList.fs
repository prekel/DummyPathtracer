namespace DummyPathtracer

[<Struct>]
type HittableList =
    { Objects: IHittable list }

    interface IHittable with
        member this.Hit ray tMin tMax =
            let init =
                {| ClosestSoFar = tMax
                   Rec = ValueNone
                   HitAnything = false |}

            let ret =
                (init, this.Objects)
                ||> List.fold
                        (fun st object ->
                            let hit = object.Hit ray tMin st.ClosestSoFar

                            match hit with
                            | ValueSome record ->
                                {| st with
                                       HitAnything = true
                                       ClosestSoFar = record.T
                                       Rec = hit |}
                            | ValueNone -> st)

            ret.Rec
