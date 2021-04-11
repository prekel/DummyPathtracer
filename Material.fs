module DummyPathtracer.Material

open System.Numerics

open DummyPathtracer.Types

let private reflectance cosine refIdx =
    let r0 = (1.f - refIdx) / (1.f + refIdx)
    let r0 = r0 * r0
    r0 + (1.f - r0) * (1.f - cosine) ** 5.f

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
    | Metal (metalAlbedo, fuzz) ->
        let reflected =
            Vector3.reflect (Vector3.unitVector rIn.Direction) hitRecord.Normal

        let scattered =
            { Origin = hitRecord.P
              Direction =
                  reflected
                  + fuzz * Vector3.randomInUnitSphere random }

        let attenuation = metalAlbedo

        match Vector3.dot scattered.Direction hitRecord.Normal > 0.f with
        | true -> ValueSome(struct (scattered, attenuation))
        | false -> ValueNone
    | Dielectric indexOfRefraction ->
        let attenuation = Color(Vector3.One)

        let refractionRatio =
            if hitRecord.FrontFace then
                (1.f / indexOfRefraction)
            else
                indexOfRefraction

        let unitDirection = Vector3.unitVector rIn.Direction

        let cosTheta =
            min (Vector3.dot -unitDirection hitRecord.Normal) 1.f

        let sinTheta = sqrt (1.f - cosTheta * cosTheta)

        let direction =
            match refractionRatio * sinTheta > 1.f
                  || reflectance cosTheta refractionRatio > randomFloat32 random with
            | true -> Vector3.reflect unitDirection hitRecord.Normal
            | false -> Vector3.refract unitDirection hitRecord.Normal refractionRatio

        let scattered =
            { Origin = hitRecord.P
              Direction = direction }

        ValueSome(scattered, attenuation)
