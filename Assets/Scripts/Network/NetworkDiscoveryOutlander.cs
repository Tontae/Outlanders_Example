using System;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using Mirror.Discovery;

[Serializable]
public class ServerFoundUnityEvent : UnityEvent<ServerResponseMsg> { };

public class NetworkDiscoveryOutlander : NetworkDiscoveryBase<ServerRequest, ServerResponseMsg>
{
    #region Server
    public override void Start()
    {
        if (transport == null)
            transport = Transport.active;

        base.Start();
    }

    protected override void ProcessClientRequest(ServerRequest request, IPEndPoint endpoint)
    {
        base.ProcessClientRequest(request, endpoint);
    }

    protected override ServerResponseMsg ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {
        try
        {
            return new ServerResponseMsg
            {
                port = transport.ServerUri().Port,
                playerAmount = NetworkManager.singleton.numPlayers
            };
        }
        catch (NotImplementedException)
        {
            Debug.LogError($"Transport {transport} does not support network discovery");
            throw;
        }
    }
    #endregion

    #region Client
    protected override ServerRequest GetRequest() => new ServerRequest();

    protected override void ProcessResponse(ServerResponseMsg response, IPEndPoint endpoint)
    {
        OnServerFound.Invoke(response);
    }

    #endregion
}
