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
  open EventStore.Core
  open EventStore.ClientAPI.Embedded
  open EventStore.ClientAPI  

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
            //.RunInMemory()
            .RunOnDisk(dbPath)
            .OnDefaultEndpoints()
//            .WithExternalTcpOn(noIp)
//            .WithInternalTcpOn(noIp)
//            .WithExternalHttpOn(noIp)
//            .WithInternalHttpOn(noIp) 
            |> configure  
            |> build

    vnode.Start()
    let connection = EmbeddedEventStoreConnection.Create(vnode)
    {VNode = vnode; Connection = connection;}    
  
  let get () = start (fun x -> x)

  let stop es =
      es.Connection.Close()

