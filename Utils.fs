[<AutoOpen>]
module DummyPathtracer.Utils

open System

type Undefined = exn

let inline (^) a b = a b

let inline unimplemented a =
    match a with
    | "" -> raise ^ NotImplementedException()
    | _ -> raise ^ NotImplementedException a

let degreeToRadians degrees = degrees * MathF.PI / 180.f

let randomFloat32 (random: Random) = random.NextDouble() |> float32

let randomFloat32MinMax random min max =
    min + (max - min) * randomFloat32 random

let clamp x min max =
    if x < min then min
    elif x > max then max
    else x
