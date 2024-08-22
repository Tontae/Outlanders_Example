using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Mirror;
using Mirror.SimpleWeb;
using Newtonsoft.Json.Linq;
using Outlander.Character;
using Outlander.Network;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ServerType
{
    Prod,
    Uat,
    Test,
    Dev,
    Localhost,
}

namespace Outlander.UI
{
    public class UINetwork : UIElements
    {
        [System.Serializable]
        public class ServerList
        {
            public string serverDisplay;
            public int serverPort;
            public string serverStatus;
        }

        [System.Serializable]
        public struct ServerBuild
        {
            public ServerType serverType;
            public string serverIp;
            public string webGame;
            public string frontend;
            public string api;
        }

        [Header("Server")]
        public ServerType serverType;
        public List<ServerBuild> serverBuilds;
        public string serverIp { get => serverBuilds[(int)serverType].serverIp; }
        public string webGame { get => serverBuilds[(int)serverType].webGame; }
        public string frontend { get => serverBuilds[(int)serverType].frontend; }
        public string api { get => serverBuilds[(int)serverType].api; }

        [Header("Manager")]
        public NetworkManagerOutsDB manager;
        public MatchMaker matchMaker;

        private void Awake()
        {Debug.Log($"{serverIp}\n{webGame}\n{frontend}\n{api}\nNetworkManager:{(manager == null ? "null" : "Validated")}\nMatchMaker:{(matchMaker == null ? "null" : "Validated")}\nBotSpawner:{(BotSpawner.Instance == null ? "null" : "Validated")}");

            if (Application.platform == RuntimePlatform.LinuxServer)
            {
                string[] strs = System.Environment.GetCommandLineArgs();
                ushort tempPort = ushort.Parse(strs[1]);
                manager.SetToken(strs[2]);
                manager.SetMatchID(strs[3]);
                matchMaker.matchID = strs[3];
                (manager.transport as SimpleWebTransport).port = tempPort;
                if (tempPort >= 9700)
                {
                    BotSpawner.Instance.spawnBot = false;
                    matchMaker.requirePlayerValue = 2;
                    matchMaker.matchStartTime = 60;
                    matchMaker.gameMode = GameMode.tournament;
                }
                else
                {
                    BotSpawner.Instance.spawnBot = true;
                    matchMaker.requirePlayerValue = 1;
                    matchMaker.matchStartTime = 60;
                    matchMaker.gameMode = GameMode.normal;
                }
            }

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                if (!NetworkClient.isConnected && !NetworkServer.active)
                {
                    StartCoroutine(manager.Initialize((bool isInitialize) =>
                    {
                        if (isInitialize)
                        {
                            manager.OnEnterMatch();
                        }
                        else
                        {
                            manager.ClientLostConnection("");
                        }
                    }));
                }
            }

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                matchMaker.matchStartTime = 10;
            }

        }

        public void StartClient()
        {
            JSONLogin login = new JSONLogin
            {
                email = UIManagers.Instance.uiWaiting.emailInput.text,
                password = UIManagers.Instance.uiWaiting.passwordInput.text
            };

            StartCoroutine(APIController.SendLoginData(login, (JSONToken token) =>
            {
                if (token?.token?.token != null)
                {
                    manager.SetToken(token.token.token);
                    manager.SetMatchID("TEST1");
                    manager.OnEnterMatch();
                }
                else
                    UIManagers.Instance.uiWaiting.loginPanel.SetActive(true);
            }));
        }

        public void StartServer()
        {
            manager.StartServer();
        }
    }
}

