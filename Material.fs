namespace DummyPathtracer

[<Struct>]
type Material = Material of Undefined

module Material =
    let scatter ray hitRecord material =
        match material with
        | Material _ -> failwith ""
