module DummyPathtracer.Material

open DummyPathtracer.Types

let scatter ray hitRecord material =
    match material with
    | Material _ -> failwith ""
