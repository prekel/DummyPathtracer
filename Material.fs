module DummyPathtracer.Material

open DummyPathtracer.Types

let scatter rIn hitRecord material random =
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
