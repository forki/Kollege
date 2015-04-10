// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open System
open System.IO
open System.Threading
open Kollege.Server.GetEventStore
open Serilog
open Serilog.Configuration

let createLogger() =  
  let logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".log")
  LoggerConfiguration()
    .WriteTo.ColoredConsole()
    .WriteTo.RollingFile(logDir)
    .CreateLogger()

[<EntryPoint>]
let main argv = 
  let logger = createLogger()
  Log.Logger <- logger
  EventStoreFacade.useSerilog logger 

  let es = EventStoreFacade.get()  
  printfn "Running EventStore in embedded mode"
  let cancelEvent = new ManualResetEvent(false)
  Console.CancelKeyPress.Add (fun x ->
    let onStop = fun () -> cancelEvent.Set() |> ignore
    EventStoreFacade.stop onStop es    
  )
  cancelEvent.WaitOne() |> ignore  
  0 // return an integer exit code
