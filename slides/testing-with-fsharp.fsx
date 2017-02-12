(**
- title : TestingWithFSharp
- description : Introduction to Testing with FSharp
- author : Ernesto Barros
- theme : night
- transition : default

***

### Difficulties with SOA Testing

- Samples : General wisdom
- Samples : Why it's hard
- Samples : Unit testing vs End-to-End testing

---

### Functional Testing

![FsReveal](images/2017-02-12 16_59_41-Clipboard.png)

---

### Test Pyramid

<div class='text-left'>
The pyramid argues that you should do much more automated testing through unit tests than you should through traditional GUI based testing.
</div>

![FsReveal](images/2017-02-12 18_12_05-Clipboard.png)

---

<div class='text-left'>
#### SOA testers need to specifically possess certain unique qualities and approaches, such as as the following:

- Test the application with a developer-oriented mindset and not from the traditional user perspective
- Have a wider knowledge of all aspects in IT from OS administration and database administration to programming languages and networking
- Have fair knowledge in software development
- Have a thorough understanding of SOA paradigms
</div>

---

### Why it's hard
todo

---

<div class='text-left'>
### Just Say No to More End-to-End Tests

Unit Tests vs. End-to-End Tests
With end-to-end tests, you have to wait: first for the entire product to be built, then for it to be deployed, and finally for all end-to-end tests to run. When the tests do run, flaky tests tend to be a fact of life. And even if a test finds a bug, that bug could be anywhere in the product.
</div>

![FsReveal](images/2017-02-12 19_03_51-Clipboard.png)
---

***

### FsCheck

- Samples : Property-Based testing

*)

#r @"..\packages\test\FsCheck\lib\net45\FsCheck.dll"
open FsCheck
open System
open System.Text.RegularExpressions

let genChinaTin () =
    gen {
        let! firstChar = 
            Gen.elements ["C"; "W"; "H"; "M"; "T"]
            |> Gen.listOfLength 1 
        let secondPartMin = 17
        let secondPartMax = 18
        let! secondPartLength = Gen.choose (secondPartMin, secondPartMax)
        let! secondPart = 
            Gen.elements [0 .. 9]
            |> Gen.map string
            |> Gen.listOfLength secondPartLength
        let lastPartLength = secondPartMax - (List.length secondPart)
        let! lastPart =
            Gen.elements <| List.append ['a' .. 'z'] ['A' .. 'Z']
            |> Gen.map string
            |> Gen.listOfLength lastPartLength
        return List.concat [firstChar; secondPart; lastPart] |> List.reduce (+)
    }

let tin = Gen.oneof [genChinaTin(); genChinaTin()] |> Gen.sample 0 10

(*** include-value: tin ***)

(** 

---

### Check with FsCheck
*)

let ``Valid China TIN(s) are counted as valid`` () =
    let tinRegex = Regex(@"[CWHMT](\d{17}[a-zA-Z]|\d{18})")
    let tin = genChinaTin() |> Gen.sample 0 1 |> List.head
    let isValid = tinRegex.IsMatch tin
    if not isValid then Console.WriteLine tin
    isValid

let runCheck () =
    Check.Quick ``Valid China TIN(s) are counted as valid``
    Check.One({ Config.Quick with MaxTest=1000000 }, ``Valid China TIN(s) are counted as valid``)

(**
---

### Computation expressions: Introduction
*)

type Option<'T> =
    | Some of 'T
    | None

type MaybeBuilder() =
    member this.Bind(x, f) = 
        printfn "Tuple: %A" (x, f)
        match x with
        | Some a -> f a
        | None -> None

    member this.Return(x) = 
        Some x
   
let divideBy bottom top =
    let result = if bottom = 0 then None else Some(top/bottom)
    result

let maybe = new MaybeBuilder()

let executeWorkflow () =
    maybe 
        {
        let! a = 100000 |> divideBy 10
        let! b = a |> divideBy 10
        let! c = b |> divideBy 10
        let bye = None
        let! d = c |> divideBy 10
        Console.WriteLine "The workflow succeeded!"
        return d
        }

(*** include-value: executeWorkflow() ***)

(**
---
#### Pyramid of Doom  

    [lang=js]
    step1(function (value1) {
        step2(function (value2) {
            step3(function (value3) {
                step4(function (value4) {
                    step5(function (value5) {
                        step6(function (value6) {
                            // Do something with value6
                        });
                    });
                });
            });
        });
    });

---

### The Reality of a Developer's Life 

**When I show my boss that I've fixed a bug:**
  
![When I show my boss that I've fixed a bug](http://www.topito.com/wp-content/uploads/2013/01/code-07.gif)
  
**When your regular expression returns what you expect:**
  
![When your regular expression returns what you expect](http://www.topito.com/wp-content/uploads/2013/01/code-03.gif)
  
*from [The Reality of a Developer's Life - in GIFs, Of Course](http://server.dzone.com/articles/reality-developers-life-gifs)*

***

### End-to-End testing

- Samples : CSV Type Provider
- Samples : SQL database Type Provider

***

### Security

- Samples : KeePassLib

***

### Canopy UI testing

- Samples : without password

***

### Organizing tests

- Samples : xUnit, Unquote, Fucha, FsCheck
- Samples : xUnit capturing output to Excel

***

*)
