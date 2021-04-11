module DummyPathtracer.Vector3

open System.Numerics
open DummyPathtracer

let lengthSquared (v: Vector3) = v.LengthSquared()

let cross (u: Vector3) (v: Vector3) =
    Vector3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X)

let unitVector (v: Vector3) = v / v.Length()

let dot (u: Vector3) (v: Vector3) = u.X * v.X + u.Y * v.Y + u.Z * v.Z

let random random =
    Vector3(randomFloat32 random, randomFloat32 random, randomFloat32 random)

let randomMinMax random min max =
    Vector3(randomFloat32MinMax random min max, randomFloat32MinMax random min max, randomFloat32MinMax random min max)

let randomInUnitSphere random =
    let rec recRandom () =
        let p = randomMinMax random -1f 1f

        if p.LengthSquared() >= 1.f then
            recRandom ()
        else
            p

    recRandom ()

let randomUnitVector random = randomInUnitSphere random |> unitVector

let randomInHemisphere random normal =
    let inUnitSphere = randomInUnitSphere random

    if dot inUnitSphere normal > 0.f then
        inUnitSphere
    else
        -inUnitSphere

let nearZero (v: Vector3) =
    let eps = 1e-8f
    abs v.X < eps && abs v.Y < eps && abs v.Z < eps

let reflect v n = v - 2.f * (dot v n) * n

let refract uv n (etaIOverEtaT: float32) =
    let cosTheta = min (dot -uv n) 1.f
    let rOutPErp = etaIOverEtaT * (uv + cosTheta * n)

    let rOutParallel =
        - sqrt(abs (1.f - lengthSquared rOutPErp)) * n

    rOutPErp + rOutParallel

let randomInUnitDisk random =
    let rec recRandomInUnitDisk () =
        let p =
            Vector3(randomFloat32MinMax random -1.f 1.f, randomFloat32MinMax random -1.f 1.f, 0.f)

        if p.LengthSquared() >= 1.f then
            recRandomInUnitDisk ()
        else
            p

    recRandomInUnitDisk ()

let length (v: Vector3) = v.Length()
