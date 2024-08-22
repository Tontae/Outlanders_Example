using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.UI;
using Outlander.Player;
using Outlander.Network;
using Mirror;
using Outlander.Character;

public class PlayerManagers : SingletonPersistent<PlayerManagers>
{
    public PlayerScriptable PlayerScriptable { get; set; }
    public GameObject PlayerGO { get; private set; }
    public NetworkIdentity PlayerIdentity { get; private set; }
    public NetworkConnectionToClient PlayerConnection { get; private set; }
    public PlayerComponents PlayerComponents
    {
        get
        {
            if (playerComponent == null) return playerComponent = NetworkClient.localPlayer.GetComponent<PlayerComponents>();
            else return playerComponent;
        }
        set { playerComponent = value; }
    }

    private PlayerComponents playerComponent;

    public CharacterCreator characterCreator;

    [Header("Match")]
    public MatchManager matchManager;
    public MatchStatus matchStatus;
    public string matchId;
    public bool gameEnded;
    public string disconnectReason;

    public override void Awake()
    {
        base.Awake();
        matchStatus = MatchStatus.IsWaiting;
        gameEnded = false;
    }

    public void OnLoginSuccess(LoginSuccessMsg msg)
    {
        if(msg.id == "SAME")
        {
            disconnectReason = msg.id;
            UIManagers.Instance.uiWaiting.lostReason.text = "Someone is currently playing match.";
            NetworkClient.Disconnect();
            return;
        }
        else if (msg.id == "GONE")
        {
            disconnectReason = msg.id;
            UIManagers.Instance.optionManager.SendMatchStart();
            UIManagers.Instance.uiWaiting.lostReason.text = "Match is started. please return to find new match.";
            NetworkClient.Disconnect();
            return;
        }
        disconnectReason = "";
        CursorManager.Instance.login = false;
        CursorManager.Instance.lobby = true;
        PlayerScriptable = OutlanderDB.singleton.GetPlayer(msg.id);

        //Player Canvas
        UIManagers.Instance.playerCanvas.playerNameText.text = PlayerScriptable.username;
        UIManagers.Instance.playerCanvas.levelText.text = "";

        InstantiatePlayerMsg instantiatePlayerMsg = new InstantiatePlayerMsg
        {
            playerID = msg.id,
            matchID = matchId
        };
        NetworkClient.Send(instantiatePlayerMsg);
    }

    public void ClearMatchData()
    {
        matchManager = null;
    }
    
    public void OnInstantiatePlayerSuccess(InstantiatePlayerMsg msg)
    {
        if (msg.playerID == "KICK")
            UIManagers.Instance.optionManager.OpenLobby();
        else
        {
            SetPlayerComponents();
            //PlayerComponents.PlayerMatchManager.MatchMakingGame();
            //UIManagers.Instance.optionManager.StopBackgroundSound();
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time = 0.5f;
            Camera.main.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void SetPlayerComponents()
    {
        PlayerGO = NetworkClient.localPlayer.gameObject;
        PlayerIdentity = NetworkClient.localPlayer;
        PlayerConnection = NetworkClient.localPlayer.connectionToClient;
        PlayerComponents = PlayerGO.GetComponent<PlayerComponents>();
        PlayerComponents.PlayerCustume.SetCustume();
        PlayerComponents.PlayerScriptable = PlayerScriptable;
        characterCreator.RestoreState(PlayerScriptable);
    }

    public void ClearPlayerComponents()
    {
        PlayerGO = null;
        PlayerIdentity = null;
        PlayerConnection = null;
        PlayerComponents = null;
    }
}
