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
  open System.Net
  open EventStore.Core
  open EventStore.ClientAPI.Embedded
  open EventStore.ClientAPI  

  let get () =
    let noIp = new IPEndPoint(IPAddress.None, 0)
    let vnode = 
        EmbeddedVNodeBuilder
            .AsSingleNode()
            .RunInMemory()
            .WithExternalTcpOn(noIp)
            .WithInternalTcpOn(noIp)
            .WithExternalHttpOn(noIp)
            .WithInternalHttpOn(noIp)
            .Build()
    vnode.Start()
    let connection = EmbeddedEventStoreConnection.Create(vnode)
    {VNode = vnode; Connection = connection;}

  let stop es =
      es.Connection.Close()
      es.VNode.Stop()

