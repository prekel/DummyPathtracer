[<AutoOpen>]
module DummyPathtracer.Utils

open System

let random = Random()

let degreeToRadians degrees = degrees * MathF.PI / 180.f

let randomDouble () = random.NextDouble() |> float32

let randomDoubleMinMax min max = min + (max - min) * randomDouble ()

let clamp x min max =
    match x < min, x > max with
    | true, _ -> min
    | _, true -> max
    | _ -> x
