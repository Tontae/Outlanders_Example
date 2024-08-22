using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.Player;
using UnityEngine.InputSystem;
using Mirror;
using Outlander.Network;

public class PlayerAFKCheck : PlayerElements
{

    public int idleTimeSetting = 300;
    public float lastIdleTime;
    public Vector2 mousePos;
    public bool isKeyboard;
    // Start is called before the first frame update
    private void Start()
    {
        lastIdleTime = 0f;
        if (isLocalPlayer)
            mousePos = Mouse.current.position.ReadValue();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"AFK : {IdleCheck()}");
        if (isLocalPlayer)
        {
            isKeyboard = Keyboard.current.anyKey.isPressed;
            if (PlayerManagers.Instance.matchStatus == MatchStatus.IsInGame)
            {
                if (Keyboard.current.anyKey.isPressed || mousePos != Mouse.current.position.ReadValue())
                {
                    mousePos = Mouse.current.position.ReadValue();
                    ServerCheckAFK();
                }
            }
        }

        if (isServer)
        {
            if(MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsInGame)
            {
                if (IdleCheck())
                {
                    if(Player.PlayerIdentity.connectionToClient.isReady)
                        Player.PlayerIdentity.connectionToClient.Disconnect();
                }
            }
        }
    }

    [Command]
    void ServerCheckAFK()
    {
        lastIdleTime = MatchMaker.Instance.matchData.matchManager.gameTime;
    }

    public bool IdleCheck()
    {
        return MatchMaker.Instance.matchData.matchManager.gameTime - lastIdleTime > idleTimeSetting;
    }


    /*public int idleTimeSetting = 30;
    float idleTime = 0;
    Vector2 mousePos;
    // Start is called before the first frame update
    private void Awake()
    {
        if (isLocalPlayer)
            mousePos = Mouse.current.position.ReadValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.wasUpdatedThisFrame || mousePos != Mouse.current.position.ReadValue()) {
            mousePos = Mouse.current.position.ReadValue();
            idleTime = 0;
            return;
        }

        idleTime += Time.deltaTime;

        if (idleTime > idleTimeSetting)
            NetworkClient.Shutdown();
    }*/
}
