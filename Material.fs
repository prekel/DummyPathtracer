module DummyPathtracer.Material

open DummyPathtracer.Types

let scatter rIn hitRecord random material =
    match material with
    | Lambertian albedo ->
        let scatterDirection =
            hitRecord.Normal + Vector3.randomUnitVector random

        let scatterDirection =
            match Vector3.nearZero scatterDirection with
            | true -> hitRecord.Normal
            | false -> scatterDirection

        let scattered =
            { Origin = hitRecord.P
              Direction = scatterDirection }

        let attenuation = albedo
        ValueSome(struct (scattered, attenuation))
    | Metal metalAlbedo ->
        let reflected =
            Vector3.reflect (Vector3.unitVector rIn.Direction) hitRecord.Normal

        let scattered =
            { Origin = hitRecord.P
              Direction = reflected }

        let attenuation = metalAlbedo

        match Vector3.dot scattered.Direction hitRecord.Normal > 0.f with
        | true -> ValueSome(struct (scattered, attenuation))
        | false -> ValueNone
