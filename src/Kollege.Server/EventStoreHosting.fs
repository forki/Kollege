namespace Kollege.Server.GetEventStore
open System.Net
open EventStore.Core
open EventStore.ClientAPI.Embedded
open EventStore.ClientAPI  

type EmbeddedEventStore = {
    VNode: ClusterVNode;
    Connection: IEventStoreConnection;
}

module EventStoreFacade =
  open System
  open System.Net
  open System.IO
  open System.Threading
  open EventStore.Core
  open EventStore.ClientAPI.Embedded
  open EventStore.ClientAPI  
  type EventStoreILogger = EventStore.Common.Log.ILogger

  let useSerilog (logger:Serilog.ILogger) =
    EventStore.Common.Log.LogManager.SetLogFactory(fun s -> SerilogLogger(logger, s) :> EventStoreILogger)

  let start (configure:EmbeddedVNodeBuilder->EmbeddedVNodeBuilder) =
    let noIp = new IPEndPoint(IPAddress.None, 0)
    let dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db")
    
    let build (builder:EmbeddedVNodeBuilder) =

      
      match Directory.Exists(dbPath) with
      | false -> Directory.CreateDirectory(dbPath)|>ignore
      | true -> ()

      builder.Build()

    let vnode = 
        EmbeddedVNodeBuilder
            .AsSingleNode()            
            .RunInMemory()
            //.RunOnDisk(dbPath)
            .OnDefaultEndpoints()
//            .WithExternalTcpOn(noIp)
//            .WithInternalTcpOn(noIp)
//            .WithExternalHttpOn(noIp)
//            .WithInternalHttpOn(noIp) 
            //.NoAdminOnPublicInterface()
            |> configure  
            |> build
    
    vnode.Start()
    let connection = EmbeddedEventStoreConnection.Create(vnode)
    {VNode = vnode; Connection = connection;}    
  
  let get () = start (fun x -> x)

  let stop (onShutdown:unit->unit) es  =      
    use signal = new ManualResetEventSlim()
    es.VNode.NodeStatusChanged.Add(fun statusChanged->
      match statusChanged.NewVNodeState with
      | EventStore.Core.Data.VNodeState.Shutdown -> 
        onShutdown()        
        signal.Set()
      | _ -> ()
    )
    es.VNode.Stop();
    signal.Wait(TimeSpan.FromSeconds(15.0)) |> ignore

