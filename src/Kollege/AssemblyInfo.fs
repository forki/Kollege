namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Kollege")>]
[<assembly: AssemblyProductAttribute("Kollege")>]
[<assembly: AssemblyDescriptionAttribute("A eventsourcing sample app")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
