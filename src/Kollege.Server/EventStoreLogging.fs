namespace Kollege.Server.GetEventStore
open System
open Serilog

type SerilogLogger(logger:ILogger) =    

  new(logger:ILogger, sourceContext:string)=
    let contextualLogger = logger.ForContext(Serilog.Core.Constants.SourceContextPropertyName, sourceContext)
    SerilogLogger(contextualLogger)

  new(sourceContext:string) =
    let logger = Serilog.Log.Logger
    SerilogLogger(logger, sourceContext)

  interface EventStore.Common.Log.ILogger with
    member x.Debug(format: string, args: obj []): unit = 
      logger.Debug(format,args)
    
    member x.DebugException(exc: exn, format: string, args: obj []): unit = 
      logger.Debug(exc, format,args)
    
    member x.Error(format: string, args: obj []): unit = 
      logger.Error(format, args)
    
    member x.ErrorException(exc: exn, format: string, args: obj []): unit = 
      logger.Error(exc, format,args)
    
    member x.Fatal(format: string, args: obj []): unit = 
      logger.Fatal(format,args)
    
    member x.FatalException(exc: exn, format: string, args: obj []): unit = 
      logger.Fatal(exc, format,args)
    
    member x.Flush(maxTimeToWait: Nullable<TimeSpan>): unit = ()      
    
    member x.Info(format: string, args: obj []): unit = 
      logger.Information(format,args)
    
    member x.InfoException(exc: exn, format: string, args: obj []): unit = 
      logger.Information(exc, format, args)
    
    member x.Trace(format: string, args: obj []): unit = 
      logger.Verbose(format,args)
    
    member x.TraceException(exc: exn, format: string, args: obj []): unit = 
      logger.Verbose(exc,format,args)