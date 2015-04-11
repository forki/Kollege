namespace Kollege.Messaging

open System

type IPublisher = 
    abstract member Publish : message:'TMessage -> unit

type ISubscriber =
  abstract member Subscribe : onMessage:Action<'TMessage> -> unit