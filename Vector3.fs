namespace DummyPathtracer

open System.Numerics

module Vector3 =
    let lengthSquared (v: Vector3) = v.LengthSquared()

    let cross (u: Vector3) (v: Vector3) =
        Vector3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X)

    let unitVector (v: Vector3) = v / v.Length()

    let dot (u: Vector3) (v: Vector3) = u.X * v.X + u.Y * v.Y + u.Z * v.Z

    let random random =
        Vector3(randomFloat32 random, randomFloat32 random, randomFloat32 random)

    let randomMinMax random min max =
        Vector3(
            randomFloat32MinMax random min max,
            randomFloat32MinMax random min max,
            randomFloat32MinMax random min max
        )

    let randomInUnitSphere random =
        let rec recRandom () =
            let p = randomMinMax random -1f 1f

            if p.LengthSquared() >= 1.f then
                recRandom ()
            else
                p

        recRandom ()

    let randomInUnitSphere2 random =
        let p = randomMinMax random -1f 1f

        match p.LengthSquared() >= 1.f with
        | true -> p / (randomFloat32MinMax random 1.f 2.f)
        | _ -> p
