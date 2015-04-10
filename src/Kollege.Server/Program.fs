// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open System
open System.Threading
open Kollege.Server.GetEventStore

[<EntryPoint>]
let main argv = 
  let es = EventStoreFacade.get()  
  printfn "Running EventStore in embedded mode"
  let cancelEvent = new ManualResetEvent(false)
  Console.CancelKeyPress.Add (fun x ->
    EventStoreFacade.stop es
    cancelEvent.Set() |> ignore
  )
  cancelEvent.WaitOne() |> ignore  
  0 // return an integer exit code
