[<AutoOpen>]
module DummyPathtracer.Utils

open System

let random = Random()

let degreeToRadians degrees = degrees * MathF.PI / 180.f

let randomFloat32 () = random.NextDouble() |> float32

let randomFloat32MinMax min max = min + (max - min) * randomFloat32 ()

let clamp x min max =
    match x < min, x > max with
    | true, _ -> min
    | _, true -> max
    | _ -> x
