using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Outlander.UI;

public class CheckServerStatus : MonoBehaviour
{
    [SerializeField] Outlander.UI.UIServerToggleGroup uistg;

    public int playerAmount = -1;

    public int goodValue;
    public int fullValue;

    //public readonly Dictionary<int, ServerResponseMsg> discoveredServers = new Dictionary<int, ServerResponseMsg>();
    [SerializeField] private NetworkDiscoveryOutlander networkDiscovery;
    [SerializeField] public UINetwork uiNetwork;

    void OnValidate()
    {   
        //UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
        //UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
    }

    private void Start()
    {
        //discoveredServers.Clear();

        //dropdown.
        //networkDiscovery.StartDiscovery();
        //Debug.Log($"Server:{serverAddress.text} status:{NetworkManager.singleton.isNetworkActive} playerAmount:{playerAmount}");
    }

    public void SetSelectServer(string serverName, Color serverStatus)
    {
        //foreach(UINetwork.ServerList tmp in uiNetwork.serverList)
        //{
        //    if(serverName == tmp.serverDisplay)
        //    {
        //        UIManagers.Instance.uiNetwork.serverSelect = tmp.serverIP;
        //    }
        //}
    }

    //public void OnDiscoveredServer(ServerResponseMsg info)
    //{
    //    // Note that you can check the versioning to decide if you can connect to the server or not using this method
    //    discoveredServers[info.port] = info;
    //    //Debug.Log($"Port:{info.port} is online numPlayer:{info.playerAmount}");
    //    uistg.Invoke("SetActiveServer", 0);
    //}
}
