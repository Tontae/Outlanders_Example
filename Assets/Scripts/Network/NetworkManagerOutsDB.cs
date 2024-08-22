using Mirror;
using Mirror.SimpleWeb;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Outlander.Character;
using static MatchManager;
using System.Text;
using System.Collections;
using Outlander.Player;
using System.Linq;
using static JSONMatch;
using Newtonsoft.Json;

namespace Outlander.Network
{
    public class NetworkManagerOutsDB : NetworkManager
    {
        public bool isClientLoadedScene { get => base.clientLoadedScene; set => base.clientLoadedScene = value; }

        struct LoginConnQueue
        {
            public NetworkConnectionToClient conn;
            public CharacterLoadMsg msg;
        }

        private Queue<LoginConnQueue> lc_conns = new Queue<LoginConnQueue>();

        private struct LoginConnStore
        {
            public NetworkConnectionToClient conn;
            public PlayerScriptable ps;
            public PlayerComponents pc;
        }
        private List<LoginConnStore> storeConnections = new List<LoginConnStore>();

        #region Server

        private Coroutine autoCloseServerCoroutine;

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            if(MatchMaker.Instance.matchData != null)
            {
                if(MatchMaker.Instance.matchData.matchManager != null)
                {
                    if (MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsStarting || MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsInGame || MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsEnd)
                    {
                        Debug.Log($"[Server] IP : {conn.address} try to join server.");
                        return;
                    }
                    else
                    {
                        Debug.Log($"[Server] IP : {conn.address} joined server | Player Connected : {NetworkServer.connections.Count} | Player Logined : {numPlayers}.");
                    }
                }
            }

            if(autoCloseServerCoroutine != null)
                StopCoroutine(autoCloseServerCoroutine);

            StartCoroutine(OnServerGetMatchData());

            AudioListener.pause = true;
        }

        public override async void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            Debug.Log($"[Server] OnServerDisconnect");
            if (conn == null)
            {
                Debug.Log($"[Server] Client Disconnect with conn null");
                Debug.Log($"[Server] Check null players{MatchMaker.Instance.matchData.matchManager.players.FindAll(x => x.netIdentity.connectionToServer == null).Count}");
                return;
            }
            else
            {
                if (conn.identity != null)
                {
                    if (conn.identity.gameObject.TryGetComponent(out PlayerComponents pc))
                    {
                        PlayerScriptable pmmAccount = OutlanderDB.singleton.GetPlayer(pc.PlayerScriptable.id);
                        if (!pc.OutlanderStateMachine.OnDie)
                            pc.PlayerMatchManager.RpcBroadcastPlayerExitMatch("DISCONNECT", pc.name);
                        await MatchMaker.Instance.PlayerDisconnected(pc);
                        Debug.Log($"[Server] Player : {pmmAccount.id} disconnected server | Player Connected : {NetworkServer.connections.Count} | Player Logined : {numPlayers}");
                    }
                    else if (conn.identity.gameObject.TryGetComponent(out SpectatorCam sc))
                    {
                        PlayerScriptable account = OutlanderDB.singleton.GetPlayer(sc.email);
                    }
                }
                else
                {
                    Debug.Log($"[Server] Client disconnected server | Player Connected : {NetworkServer.connections.Count} | Player Logined : {numPlayers}");
                }

                if (NetworkServer.connections.Count <= 0)
                {
                    if(MatchMaker.Instance.registeredPlayer.Count <= 0)
                    {
                        StartCoroutine(MatchMaker.Instance.AutoCloseServer(1f));
                    }
                    else
                    {
                        bool isWaiting = false;
                        int i = 0;
                        foreach (KeyValuePair<string, string> temp in MatchMaker.Instance.registeredPlayer)
                        {
                            if (temp.Value == "waiting")
                            {
                                isWaiting = true;
                            }

                            if(i == MatchMaker.Instance.registeredPlayer.Count - 1)
                            {
                                if(!isWaiting)
                                    StartCoroutine(MatchMaker.Instance.AutoCloseServer(1f));
                            }
                            i++;
                        }
                    }
                }

                OutlanderDB.singleton.OnSyncDisconnect(conn);
                NetworkServer.DestroyPlayerForConnection(conn);
            }

            if(NetworkServer.connections.Count <= 0)
            {
                autoCloseServerCoroutine = StartCoroutine(UIManagers.Instance.uiNetwork.matchMaker.AutoCloseServer(600f));
            }

            StartCoroutine(OnServerGetMatchData());
        }

        public override void OnStartServer()
        {
            Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().enabled = false;
            Debug.Log($"[Server] Start server IP : {networkAddress}");
            OutlanderDB.singleton.CreateDatabase();

            NetworkServer.RegisterHandler<TokenMsg>(OnGetPlayerData);
            NetworkServer.RegisterHandler<InventoryMsg>(OnUpdateInventory);
            NetworkServer.RegisterHandler<CharacterLoadMsg>(OnLoadCharacter);
            NetworkServer.RegisterHandler<PlayerProficiencyMsg>(OnUpdatePlayerProficiency);
            NetworkServer.RegisterHandler<PlayerAchievementMsg>(OnUpdatePlayerAchievement);
            NetworkServer.RegisterHandler<PlayerEquipmentMsg>(OnUpdatePlayerEquipment);
            NetworkServer.RegisterHandler<PlayerItemStatusMsg>(OnUpdateItemStatus);
            NetworkServer.RegisterHandler<ChatMessage>(OnRecivedMessage);
            NetworkServer.RegisterHandler<InstantiatePlayerMsg>(OnInstantiatePlayer);
            NetworkServer.RegisterHandler<DestroyPlayerMsg>(OnDestroyPlayer);
            NetworkServer.RegisterHandler<PlayerExitMatchMessage>(OnPlayerExit);

            UIManagers.Instance.uiWaiting.loginPanel.SetActive(false);

            autoCloseServerCoroutine = StartCoroutine(UIManagers.Instance.uiNetwork.matchMaker.AutoCloseServer(600f));
        }

        public override void OnStopServer()
        {
            Debug.Log($"[Server] Stop server");
            UIManagers.Instance.uiWaiting.loginPanel.SetActive(true);
        }

        public void InitServer(string server)
        {
            networkAddress = server;
            SimpleWebTransport simpleWeb = transport.GetComponent<SimpleWebTransport>();

            if (server.Contains("localhost"))
            {
                // Set port and uncheck ssl
                networkAddress = "localhost";
                simpleWeb.port = 9999;
                simpleWeb.sslEnabled = false;
                simpleWeb.clientUseWss = false;
            }
            else
            {
                // Set port and check ssl
                simpleWeb.port = 443;
                simpleWeb.sslEnabled = true;
                simpleWeb.clientUseWss = true;
            }
            Debug.LogWarning($"Connect to : {server} | in Port : {simpleWeb.port}");
            StartClient();
        }

        public void SetToken(string _token)
        {
            Player_Token = _token;
        }
        public void SetMatchID(string _matchID)
        {
            Match_ID = _matchID;
        }
        #endregion

        #region Client
        public override void OnStartClient()
        {
            Debug.Log($"[Client] Start client");

            OutlanderDB.singleton.CreateDatabase();

            NetworkClient.RegisterHandler<LoginSuccessMsg>(PlayerManagers.Instance.OnLoginSuccess);
            NetworkClient.RegisterHandler<SendPlayerDatabaseMsg>(OutlanderDB.singleton.OnSyncPlayerDatabase);
            NetworkClient.RegisterHandler<InstantiatePlayerMsg>(PlayerManagers.Instance.OnInstantiatePlayerSuccess);
            NetworkClient.RegisterHandler<DestroyPlayerMsg>(UIManagers.Instance.OnBackFromMatch);

            base.clientLoadedScene = true;

            Physics.simulationMode = SimulationMode.FixedUpdate;
        }

        public override void OnClientConnect()
        {
            Debug.Log($"[Client] Client connected");
            base.OnClientConnect();
            if (Complete)
            {
                TokenMsg tokenMsg = new TokenMsg
                {
                    token = Player_Token,
                    matchID = Match_ID
                };
                NetworkClient.Send(tokenMsg);
            }
        }
        
        public override void OnStopClient()
        {
            Debug.Log($"[Client] Client disconnect");

            UIManagers.Instance.playerCanvas.playerScreenUI.SetActive(false);
            UIManagers.Instance.playerCanvas.mapButton.gameObject.SetActive(false);
            UIManagers.Instance.playerCanvas.countUI.SetActive(false);
            UIManagers.Instance.playerCanvas.compassUI.SetActive(false);
            UIManagers.Instance.uiMatch.gameObject.SetActive(false);

            //PlayerUI
            UIManagers.Instance.uiTutorial.Hide();
            UIManagers.Instance.optionManager.OnCloseSetting();

            //Lobby
            UIManagers.Instance.optionManager.PlayerDisconnected();

            //Inventory
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.inventory_ItemContainer);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_MainWeapon_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_SubWeapon_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_Cuirass_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_Cuisses_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_Gauntlets_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_Greaves_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_Viel_Slot);
            DetachGameObjectAndDestroy(UIManagers.Instance.playerCanvas.uiInventory.equip_Helm_Slot);

            PlayerManagers.Instance.characterCreator.OnInitMesh(false);

            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time = 3f;

            //Camera
            UIManagers.Instance.summaryCamera.gameObject.SetActive(false);
            
            UIManagers.Instance.playerCanvas.miniMapCamera.transform.SetParent(UIManagers.Instance.playerCanvas.mapButton.transform);

            CursorManager.Instance.lobby = false;
            CursorManager.Instance.login = true;

            UIManagers.Instance.optionManager.SwapMusicBetweenScene(0);
            OutlanderDB.singleton.ClientClearDB();

            PlayerManagers.Instance.characterCreator.InitBody();

            if (!NetworkClient.ready)
            {
                if(PlayerManagers.Instance.disconnectReason == "")
                {
                    if (PlayerManagers.Instance.matchStatus == MatchStatus.IsWaiting)
                    {
                        StartCoroutine(ClientConnectServer());
                        return;
                    }
                    else
                    {
                        ClientLostConnection("LOST");
                    }
                }
                else
                {
                    ClientLostConnection("");
                }
            }
            else
            {
                ClientLostConnection("SER");
            }
        }

        #endregion

        #region NetworkMessage

        #region Account

        public void OnGetPlayerData(NetworkConnectionToClient conn, TokenMsg msg)
        {
            StartCoroutine(APIController.SendGetPlayerData(msg.token, (JSONPlayerData player) =>
            {
                if (player != null)
                {
                    if(UIManagers.Instance.uiNetwork.serverType != ServerType.Localhost)
                    {
                        if (OutlanderDB.singleton.IsAccountIdOnline(player.data.id))
                        {
                            LoginSuccessMsg loginfailMsg = new LoginSuccessMsg
                            {
                                id = "SAME"
                            };
                            conn.Send(loginfailMsg);
                            return;
                        }

                        if (MatchMaker.Instance.matchData != null)
                        {
                            if (MatchMaker.Instance.matchData.matchManager != null)
                            {
                                if (MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsStarting || MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsInGame || MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsEnd)
                                {
                                    LoginSuccessMsg loginfailMsg = new LoginSuccessMsg
                                    {
                                        id = "GONE"
                                    };
                                    conn.Send(loginfailMsg);
                                    return;
                                }
                            }
                        }
                    }

                    PlayerScriptable playerData = ScriptableObject.CreateInstance<PlayerScriptable>();

                    playerData.walletAddress = (player.data.walletAddress == null) ? "" : player.data.walletAddress.ToString();
                    playerData.id = player.data.id;
                    playerData.token = msg.token;
                    playerData.conn = conn.connectionId;
                    playerData.username = player.data.username;
                    playerData.role = player.data.role;
                    playerData.isActive = player.data.isActive;
                    playerData.level = player.data.level;
                    playerData.experience = player.data.experience;
                    playerData.gender = player.data.appearance.gender;
                    playerData.appearance = new CharacterAppearance[4];

                    for (int i = 0; i < playerData.appearance.Length; i++)
                    {
                        playerData.appearance[i] = new CharacterAppearance();
                    }

                    playerData.appearance[(int)BodyParts.Face].id = player.data.appearance.face.id == "" || player.data.appearance.face.id == "face" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.maleFace[0].name : player.data.appearance.face.id;
                    playerData.appearance[(int)BodyParts.Face].color = player.data.appearance.skin.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.skinColor[0]) : player.data.appearance.skin.color;

                    playerData.appearance[(int)BodyParts.Hair].id = player.data.appearance.hair.id == "" || player.data.appearance.face.id == "hair" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.hair[0].name : player.data.appearance.hair.id;
                    playerData.appearance[(int)BodyParts.Hair].color = player.data.appearance.hair.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.hairColor[0]) : player.data.appearance.hair.color;

                    playerData.appearance[(int)BodyParts.Beard].id = player.data.appearance.beard.id == "" || player.data.appearance.face.id == "beard" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.beard[0].name : player.data.appearance.beard.id;
                    playerData.appearance[(int)BodyParts.Beard].color = player.data.appearance.beard.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.hairColor[0]) : player.data.appearance.beard.color;

                    playerData.appearance[(int)BodyParts.Eyebrow].id = player.data.appearance.eyebrows.id == "" || player.data.appearance.face.id == "eyebrow" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.maleEyebrow[0].name : player.data.appearance.eyebrows.id;
                    playerData.appearance[(int)BodyParts.Eyebrow].color = player.data.appearance.eyebrows.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.eyesColor[0]) : player.data.appearance.eyebrows.color;

                    playerData.equipeditemList = new string[Enum.GetNames(typeof(SuitParts)).Length];
                    OutlanderDB.singleton.CreatePlayer(playerData);

                    AutoLoadDatabase(conn, true);

                    MatchMaker.Instance.registeredPlayer[playerData.id] = "inGame";

                    LoginSuccessMsg loginMsg = new LoginSuccessMsg
                    {
                        id = playerData.id
                    };
                    conn.Send(loginMsg);

                    base.clientLoadedScene = true;
                }
            }));
        }

        public IEnumerator OnClientRefreshToken(Action<bool> callback)
        {
            JSONRefreshToken oldToken = new JSONRefreshToken()
            {
                token = Refresh_Token
            };

            callback(true);
            yield return null;
            //yield return StartCoroutine(APIController.SendRefreshToken(oldToken, (JSONToken newToken) =>
            //{
            //    if (newToken.message == "Refresh token successful")
            //    {
            //        Player_Token = newToken.token.token;
            //        Refresh_Token = newToken.refreshToken;
            //        callback(true);
            //    }
            //    else
            //    {
            //        callback(false);
            //    }
            //}));
        }

        #endregion

        #region Character

        public async void OnInstantiatePlayer(NetworkConnectionToClient conn, InstantiatePlayerMsg msg)
        {
            PlayerScriptable player = OutlanderDB.singleton.GetPlayer(msg.playerID);
            GameObject playerModel = Instantiate(playerPrefab);
            if (player.role.Equals("admin"))
            {
                NotSusScript nss = playerModel.AddComponent<NotSusScript>();
                nss.spectatorCam = Instantiate(spawnPrefabs[1]);
            }

            NetworkServer.AddPlayerForConnection(conn, playerModel);

            //Create Match
            if (MatchMaker.Instance.matchData.matchManager == null)
            {
                await MatchMaker.Instance.MatchMakingGame();
            }

            PlayerComponents pc = conn.identity.gameObject.GetComponent<PlayerComponents>();
            pc.PlayerMatchManager.SetPlayerObjectName(player.username);
            pc.PlayerScriptable = player;
            pc.PlayerAppearance.RpcSetAppearance(player.appearance, player.gender);

            InstantiatePlayerMsg instantiateMsg = new InstantiatePlayerMsg { };

            if (MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsWaiting || MatchMaker.Instance.matchData.matchManager.matchStatus == MatchStatus.IsPreparing)
            {
                try
                {
                    MatchMaker.Instance.JoinGame(pc);
                    conn.Send(instantiateMsg);
                }
                catch
                {
                    conn.Disconnect();
                }
            }
            else
            {
                conn.Disconnect();
            }

            storeConnections.Add(new LoginConnStore { conn = conn, ps = player, pc = pc });
        }

        void GetPlayerData(Action<string> callBack)
        {
            UIManagers.Instance.uiWaiting.loginPanel.SetActive(false);
            StartCoroutine(APIController.SendGetPlayerData(Player_Token, (JSONPlayerData player) =>
            {
                if (player != null)
                {
                    callBack(player.data.id);

                    PlayerManagers.Instance.PlayerScriptable = ScriptableObject.CreateInstance<PlayerScriptable>();
                    PlayerManagers.Instance.PlayerScriptable.walletAddress = (player.data.walletAddress == null) ? "" : player.data.walletAddress.ToString();
                    PlayerManagers.Instance.PlayerScriptable.id = player.data.id;
                    PlayerManagers.Instance.PlayerScriptable.token = Player_Token;
                    PlayerManagers.Instance.PlayerScriptable.username = player.data.username;
                    PlayerManagers.Instance.PlayerScriptable.role = player.data.role;
                    PlayerManagers.Instance.PlayerScriptable.isActive = player.data.isActive;
                    PlayerManagers.Instance.PlayerScriptable.level = player.data.level;
                    PlayerManagers.Instance.PlayerScriptable.experience = player.data.experience;
                    if (player.data.appearance.gender == 2)
                        PlayerManagers.Instance.PlayerScriptable.gender = 0;
                    else
                        PlayerManagers.Instance.PlayerScriptable.gender = 1;

                    PlayerManagers.Instance.PlayerScriptable.appearance = new CharacterAppearance[4];

                    for (int i = 0; i < PlayerManagers.Instance.PlayerScriptable.appearance.Length; i++)
                    {
                        PlayerManagers.Instance.PlayerScriptable.appearance[i] = new CharacterAppearance();
                    }

                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Face].id = player.data.appearance.face.id == "" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.maleFace[0].name : player.data.appearance.face.id;
                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Face].color = player.data.appearance.skin.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.skinColor[0]) : player.data.appearance.skin.color;

                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Hair].id = player.data.appearance.hair.id == "" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.hair[0].name : player.data.appearance.hair.id;
                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Hair].color = player.data.appearance.hair.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.hairColor[0]) : player.data.appearance.hair.color;

                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Beard].id = player.data.appearance.beard.id == "" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.beard[0].name : player.data.appearance.beard.id;
                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Beard].color = player.data.appearance.beard.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.hairColor[0]) : player.data.appearance.beard.color;

                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Eyebrow].id = player.data.appearance.eyebrows.id == "" ? PlayerManagers.Instance.characterCreator.appearanceScriptable.maleEyebrow[0].name : player.data.appearance.eyebrows.id;
                    PlayerManagers.Instance.PlayerScriptable.appearance[(int)BodyParts.Eyebrow].color = player.data.appearance.eyebrows.color == "" ? ColorUtility.ToHtmlStringRGB(PlayerManagers.Instance.characterCreator.eyesColor[0]) : player.data.appearance.eyebrows.color;

                    PlayerManagers.Instance.PlayerScriptable.equipeditemList = new string[Enum.GetNames(typeof(SuitParts)).Length];

                    StartCoroutine(APIController.GetPlayerSound(Player_Token, (JSONPlayerSound sound) =>
                    {
                        if (sound != null)
                        {
                            UIManagers.Instance.optionManager.OnSetDefaultVolume(sound.soundData.master, sound.soundData.bgm, sound.soundData.sfx);
                        }
                    }));
                }
                else
                {
                    callBack("");
                }
            }));
        }

        public void OnDestroyPlayer(NetworkConnectionToClient conn, DestroyPlayerMsg msg)
        {
            NetworkServer.RemovePlayerForConnection(conn,true);
        }

        public void OnPlayerExit(NetworkConnectionToClient conn, PlayerExitMatchMessage msg)
        {
            MatchMaker.Instance.registeredPlayer[msg.playerId] = "exit";
        }

        public void OnUpdatePlayerProficiency(NetworkConnectionToClient conn, PlayerProficiencyMsg msg)
        {
            PlayerScriptable characterData = OutlanderDB.singleton.GetPlayer(msg.id);

            foreach(CharacterProficiency temp in characterData.proficiency)
            {
                if(temp.className == msg.proficiency.className)
                {
                    temp.level = msg.proficiency.level;
                    temp.exp = msg.proficiency.exp;
                    break;
                }
            }
            AutoLoadDatabase(conn, false);
        }
        public void OnUpdatePlayerAchievement(NetworkConnectionToClient conn,PlayerAchievementMsg msg)
        {
            PlayerScriptable characterData = OutlanderDB.singleton.GetPlayer(msg.id);

            AutoLoadDatabase(conn,false);
        }
        public void OnUpdatePlayerEquipment(NetworkConnectionToClient conn, PlayerEquipmentMsg msg)
        {
            PlayerScriptable characterData = OutlanderDB.singleton.GetPlayer(msg.id);
            PlayerComponents pc = conn.identity.GetComponent<PlayerComponents>();
            if (msg.equipment != null)
            {
                characterData.equipeditemList = msg.equipment;
                
                OutlanderDB.singleton.UpdatePlayer(characterData);
                AutoLoadDatabase(conn, false);
                pc.PlayerScriptable = characterData;
                pc.WeaponManager.RpcChangeWeaponType(characterData.equipeditemList[0]);
                pc.PlayerCustume.RpcSetEquipmentAppearance(characterData.equipeditemList);
            }
        }

        public void OnRecivedMessage(NetworkConnectionToClient conn, ChatMessage msg)
        {
            ChatMessage chatMsg = new ChatMessage
            {
                username = msg.username,
                message = msg.message
            };
            NetworkServer.SendToAll(chatMsg);
        }

        public async void OnLoadCharacter(NetworkConnectionToClient conn, CharacterLoadMsg msg)
        {
            LoginConnQueue lcq = new LoginConnQueue { conn = conn, msg = msg };
            if (!lc_conns.Contains(lcq))
                lc_conns.Enqueue(lcq);

            await OnLoadCharacterQueue();
            if (lc_conns.Count > 0)
                OnLoadCharacter(lc_conns.Peek().conn, lc_conns.Peek().msg);
        }
        public async Task OnLoadCharacterQueue()
        {
            NetworkConnectionToClient cur_conn = lc_conns.Peek().conn;
            CharacterLoadMsg msg = lc_conns.Peek().msg;
            lc_conns.Dequeue();

            Debug.Log($"[Server] Account : {msg.username} Spawn player");
            AutoLoadDatabase(cur_conn, false);
            await Task.Yield();
        }

        #endregion

        #region Inventory

        public void OnUpdateInventory(NetworkConnectionToClient conn, InventoryMsg msg)
        {
            PlayerScriptable character = OutlanderDB.singleton.GetPlayer(msg.id);

            if (msg.jsonCollectItem != null)
            {
                foreach (var item in character.inventory.items)
                {
                    foreach (var temp in msg.jsonCollectItem.item)
                    {
                        if (item.id_gen == temp.id_gen)
                        {
                            item.id_backend_gen = temp.id_backgen;
                            conn.identity.GetComponent<InventoryManager>().TargetRecieveDataAfterAddInventory(item.id_gen, temp.id_backgen, msg.id);
                        }
                    }
                }

                #region Add item to CharacterScripable

                foreach (var item in msg.jsonCollectItem.item)
                {
                    bool isCoin = false;
                    bool isMultipleItem = false;

                    foreach (var _item in OutlanderDB.singleton.GetItemList())
                    {
                        if (item.modelId == _item.itemId)
                        {
                            if (_item.isMultipleItem) isMultipleItem = true;
                            if (_item.mainType == Type.Coin) isCoin = true;

                            if (!isCoin)
                            {
                                if (!isMultipleItem)
                                {
                                    CharacterItem charitem = new CharacterItem
                                    {
                                        id = item.modelId,
                                        quantities = (int)item.qty,
                                        id_gen = item.id_gen,
                                        id_backend_gen = item.id_backgen,
                                        durable = (int)item.durable,
                                        enhance = (int)item.enhancePoint,
                                    };

                                    character.inventory.items.Add(charitem);
                                }
                                else
                                {
                                    bool found = false;

                                    foreach (var myitem in character.inventory.items)
                                    {
                                        if (myitem.id == item.modelId)
                                        {
                                            myitem.quantities += (int)item.qty;

                                            found = true;
                                            break;
                                        }
                                    }

                                    if (!found)
                                    {
                                        CharacterItem charitem = new CharacterItem
                                        {
                                            id = item.modelId,
                                            quantities = (int)item.qty,
                                            id_gen = item.id_gen,
                                            id_backend_gen = item.id_backgen,
                                            durable = (int)item.durable,
                                            enhance = (int)item.enhancePoint,
                                        };

                                        character.inventory.items.Add(charitem);

                                    }
                                }
                            }
                            else
                            {
                                if (item.modelId == "999") //bronze
                                    character.inventory.characterCurrency.bronze += (int)item.qty;
                                if (item.modelId == "998") //silver
                                    character.inventory.characterCurrency.silver += (int)item.qty;
                                if (item.modelId == "997") //gold
                                    character.inventory.characterCurrency.gold += (int)item.qty;
                            }
                            break;
                        }
                    }
                }
            }

            if (msg.jsonUseItem != null)
            {
                foreach (var item in msg.jsonUseItem.item)
                {
                    bool isCoin = false;
                    bool isMultipleItem = false;

                    foreach (var _item in OutlanderDB.singleton.GetItemList())
                    {
                        if (item.modelId == _item.itemId)
                        {
                            if (_item.isMultipleItem) isMultipleItem = true;
                            if (_item.mainType == Type.Coin) isCoin = true;

                            if (!isCoin)
                            {
                                if (!isMultipleItem)
                                {
                                    foreach (var myitem in character.inventory.items)
                                    {
                                        if (myitem.id_gen == item.id_gen)
                                        {
                                            character.inventory.items.Remove(myitem);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var myitem in character.inventory.items)
                                    {
                                        if (myitem.id == item.modelId)
                                        {
                                            myitem.quantities -= (int)item.qty;

                                            if (myitem.quantities <= 0)
                                            {
                                                character.inventory.items.Remove(myitem);
                                            }

                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.modelId == "999") //bronze
                                    character.inventory.characterCurrency.bronze -= (int)item.qty;
                                if (item.modelId == "998") //silver
                                    character.inventory.characterCurrency.silver -= (int)item.qty;
                                if (item.modelId == "997") //gold
                                    character.inventory.characterCurrency.gold -= (int)item.qty;
                            }

                            break;
                        }
                    }
                }
            }
            #endregion

            OutlanderDB.singleton.UpdatePlayer(character);
            AutoLoadDatabase(conn, false);
        }
        public void OnUpdateItemStatus(NetworkConnectionToClient conn, PlayerItemStatusMsg msg)
        {

        }

        #endregion

        #region Match
        private int onTryingConnect = 0;

        public IEnumerator OnServerGetMatchData()
        {
            Debug.Log($"[Match detail] Update player in match {Match_ID} detail.");
            JSONGetMatchDetail match = new JSONGetMatchDetail()
            {
                roomID = Match_ID,
                key = Player_Token
            };
            yield return StartCoroutine(APIController.ServerSendGetMatchDetail(match, (JSONMatch matchData) =>
            {
                if (matchData.message == "Get Game Room Success.")
                {
                    MatchMaker.Instance.realMaxPlayer = matchData.data.countPlayer;
                    for (int i = 0; i < matchData.data.playerMatchData.Count; i++)
                    {
                        for (int j = 0; j < matchData.data.playerMatchData[i].playerList.Count; j++)
                        {
                            if (MatchMaker.Instance.registeredPlayer.ContainsKey(matchData.data.playerMatchData[i].playerList[j].playerID)) continue;
                            MatchMaker.Instance.registeredPlayer.Add(matchData.data.playerMatchData[i].playerList[j].playerID, "waiting");
                        }
                    }
                }
                else
                {
                    MatchMaker.Instance.realMaxPlayer = 1;
                }
            }));
        }

        public IEnumerator OnClientGetMatchData(string playerId,Action<bool> callback)
        {
            TokenMsg tokenMsg = new TokenMsg()
            {
                token = Player_Token,
                matchID = Match_ID
            };

            yield return StartCoroutine(APIController.ClientSendGetMatchDetail(tokenMsg, (JSONMatch matchData) =>
            {
                if (matchData.message == "Get Game Room Success.")
                {
                    for (int i = 0; i < matchData.data.playerMatchData.Count; i++)
                    {
                        if (matchData.data.playerMatchData[i].partyCode == "NO_PARTY")
                        {
                            for (int j = 0; j < matchData.data.playerMatchData[i].playerList.Count; j++)
                            {
                                if (playerId == matchData.data.playerMatchData[i].playerList[j].playerID)
                                {
                                    Port = matchData.data.port.ToString();
                                    if (matchData.data.playerMatchData[i].playerList[j].status != "Played")
                                    {
                                        Debug.Log($"[Match detail] Player : {playerId} have data in this match.");
                                        callback(true);
                                        break;
                                    }
                                    else
                                    {
                                        Debug.Log($"[Match detail] Player : {playerId} played match : {Match_ID} already.");
                                        UIManagers.Instance.uiWaiting.lostReason.text = "You have finished playing this match.";
                                        callback(false);
                                        break;
                                    }
                                }
                                else if(i == matchData.data.playerMatchData.Count - 1 && j == matchData.data.playerMatchData[i].playerList.Count - 1)
                                {
                                    Debug.Log($"[Match detail] Don't have player : {playerId} in match : {Match_ID}.");
                                    UIManagers.Instance.uiWaiting.lostReason.text = "You don't have data in this match.";
                                    callback(false);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log($"[Match detail] Don't have player : {playerId} in match : {Match_ID}.");
                    callback(false);
                }
            }));
        }

        public void OnEnterMatch()
        {
            PlayerManagers.Instance.matchId = Match_ID;

            if (UIManagers.Instance.uiNetwork.serverIp.Equals("localhost"))
            {
                Complete = true;
                JSONCreateMatch match = new JSONCreateMatch
                {
                    roomID = Match_ID,
                    maxPlayer = 30,
                    minPlayer = 1,
                    gameMode = "normal",
                    map = "map_1",
                    playMode = "solo"
                };
                InitServer($"{UIManagers.Instance.uiNetwork.serverIp}");
            }
            else
            {
                InitServer($"{UIManagers.Instance.uiNetwork.serverIp}?port={Port}&t=");
            }
        }

        IEnumerator ClientConnectServer()
        {
            yield return new WaitForSeconds(1f);
            if (onTryingConnect++ <= 10)
                InitServer(networkAddress);
            else
                ClientLostConnection("TRY");
        }

        public void ClientLostConnection(string reason)
        {
            Debug.Log("[Client] Lost connection");
            if(reason == "TRY")
            {
                UIManagers.Instance.uiWaiting.lostReason.text = "There is no match that you try to enter.";
                if (PlayerManagers.Instance.PlayerScriptable == null)
                    GetPlayerData((string playerId) =>
                    {
                        if (playerId != "")
                            OnClientUpdateMatchPlayer("out");
                    });
                else
                    OnClientUpdateMatchPlayer("out");
            }
            else if (reason == "LOST")
            {
                UIManagers.Instance.uiWaiting.lostReason.text = "Your connection is unstable.";
            }
            else if (reason == "SER")
            {
                UIManagers.Instance.uiWaiting.lostReason.text = "Match is close, please return to lobby.";
            }

            if (PlayerManagers.Instance.matchStatus == MatchStatus.IsWaiting || PlayerManagers.Instance.matchStatus == MatchStatus.IsFull || PlayerManagers.Instance.matchStatus == MatchStatus.IsPreparing)
            {
                UIManagers.Instance.uiWaiting.loadingPanel.SetActive(true);
            }
            else
            {
                UIManagers.Instance.uiWaiting.loadingPanel.SetActive(false);
            }
            UIManagers.Instance.uiWaiting.Show();
            UIManagers.Instance.uiWaiting.lostPanel.SetActive(true);
        }

        public static string GetRandomMatchID()
        {
            string _id = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if (random < 26)
                {
                    _id += (char)(random + 65);
                }
                else
                {
                    _id += (random - 26).ToString();
                }
            }
            return _id;
        }

        public void OnServerUpdateMatchPlayer(string matchId,string playerId, string _type)
        {
            JSONServerUpdateMatchPlayer match = new JSONServerUpdateMatchPlayer
            {
                roomID = matchId,
                playerID = playerId,
                type = _type,
                key = Player_Token
            };
            StartCoroutine(APIController.ServerSendMatchUpdatePlayer(match));
        }

        public void OnClientUpdateMatchPlayer(string type)
        {
            JSONClientUpdateMatchPlayer player = new JSONClientUpdateMatchPlayer
            {
                roomID = Match_ID,
                playerID = PlayerManagers.Instance.PlayerScriptable.id,
                type = type
            };
            StartCoroutine(APIController.ClientSendMatchUpdatePlayer(Player_Token, player));
        }

        public IEnumerator OnUpdateMatchStatus(string _status)
        {
            JSONUpdateMatchStatus match = new JSONUpdateMatchStatus
            {
                roomID = Match_ID,
                status = _status,
                key = Player_Token
            };
            yield return StartCoroutine(APIController.SendMatchUpdateStatus(match));
        }

        public async Task OnUpdatePlayerHistory(PlayerComponents _player)
        {
            PlayerScriptable account = OutlanderDB.singleton.GetPlayer(_player.PlayerScriptable.id);

            JSONUpdateHistory match = new JSONUpdateHistory
            {
                roomID = Match_ID,
                playerID = account.id,
                playerKill = _player.PlayerStatisticsData.PlayerKillCount,
                monsterKill = _player.PlayerStatisticsData.MonsterKillCount,
                miniBossKill = _player.PlayerStatisticsData.MiniBossKillCount,
                bossKill = _player.PlayerStatisticsData.BossKillCount,
                score = _player.PlayerUIManager.CalculateScore(_player.PlayerStatisticsData.PlayerRank, _player.PlayerStatisticsData.PlayerSurviveTime, _player.PlayerStatisticsData.PlayerKillCount, _player.PlayerStatisticsData.MonsterKillCount + _player.PlayerStatisticsData.MiniBossKillCount + _player.PlayerStatisticsData.BossKillCount),
                coin = _player.PlayerUIManager.CalculateCoin(_player.PlayerStatisticsData.PlayerRank, _player.PlayerStatisticsData.PlayerSurviveTime, _player.PlayerStatisticsData.PlayerKillCount, _player.PlayerStatisticsData.MonsterKillCount + _player.PlayerStatisticsData.MiniBossKillCount + _player.PlayerStatisticsData.BossKillCount),
                duration = _player.PlayerStatisticsData.PlayerSurviveTime,
                surviveNumber = _player.PlayerStatisticsData.PlayerRank,
                level_exp = _player.PlayerUIManager.CalculateLevelExp(_player.PlayerStatisticsData.PlayerRank, _player.PlayerStatisticsData.PlayerSurviveTime, _player.PlayerStatisticsData.PlayerKillCount, _player.PlayerStatisticsData.MonsterKillCount, _player.PlayerStatisticsData.MiniBossKillCount, _player.PlayerStatisticsData.BossKillCount),
                rank_exp = _player.PlayerUIManager.CalculateRankExp(_player.PlayerStatisticsData.PlayerRank, _player.PlayerStatisticsData.PlayerSurviveTime, _player.PlayerStatisticsData.PlayerKillCount, _player.PlayerStatisticsData.MonsterKillCount + _player.PlayerStatisticsData.MiniBossKillCount + _player.PlayerStatisticsData.BossKillCount),
                deal_damage = _player.PlayerStatisticsData.TotalPlayerDamage,
                collect_item = _player.PlayerStatisticsData.PickupItemCount.Values.Sum(),
                craft_item = _player.PlayerStatisticsData.CraftItemCountDict.Values.Sum(),
                use_health_potion = _player.PlayerStatisticsData.HealthPotionUseCount,
                use_mana_potion = _player.PlayerStatisticsData.ManaPotionUseCount,
                god_slayer = _player.PlayerStatisticsData.IsGodSlayer ? 1 : 0,
                traveler = _player.PlayerStatisticsData.TravelDistance,
                wardog = _player.PlayerStatisticsData.PlayerKillDamageData.Values.Sum(temp => temp.totalDamage) >= 300f ? 1 : 0,
                not_fear_the_reaper = _player.PlayerStatisticsData.PlayerRank != 1 ? 1 : 0,
            };
            match.proficiency_exp = new List<Proficiency>();

            foreach (WeaponManager.WeaponType weapon in Enum.GetValues(typeof(WeaponManager.WeaponType)))
            {
                if (weapon is WeaponManager.WeaponType.BowQuiver or WeaponManager.WeaponType.Die)
                    continue;
                _player.PlayerStatisticsData.PlayerKillDamageData.TryGetValue(weapon, out PlayerStatisticsData.KillDamage playerKillWeapon);
                _player.PlayerStatisticsData.MonsKillDamageData.TryGetValue(weapon, out PlayerStatisticsData.KillDamage monKillWeapon);
                _player.PlayerStatisticsData.MiniBossKillDamageData.TryGetValue(weapon, out PlayerStatisticsData.KillDamage miniKillWeapon);
                _player.PlayerStatisticsData.BossKillDamageData.TryGetValue(weapon, out PlayerStatisticsData.KillDamage bossKillWeapon);
                if (playerKillWeapon.killCount == 0 && monKillWeapon.killCount == 0 && miniKillWeapon.killCount == 0 && bossKillWeapon.killCount == 0)
                    continue;
                Proficiency profi = new Proficiency();
                profi.weapon = weapon.ToString();
                profi.exp = _player.PlayerUIManager.CalculateProficiencyExp(playerKillWeapon.killCount, monKillWeapon.killCount, miniKillWeapon.killCount, bossKillWeapon.killCount);
                match.proficiency_exp.Add(profi);
            }

            //Debug.Log(JsonConvert.SerializeObject(match));

            StartCoroutine(APIController.SendPlayerHistory(match, account));
            await Task.Yield();
        }

        #endregion

        #region Enemy
        // public GameObject InstantiateEnemy(GameObject _enemy, Vector3 _pos)
        // {
        //     GameObject enemy = Instantiate(_enemy, _pos, Quaternion.identity) as GameObject;
        //     // StartCoroutine(NetworkTranformReset(enemy));
        //     NetworkServer.Spawn(enemy);
        //     return enemy;
        // }
        #endregion

        private void AutoLoadDatabase(NetworkConnectionToClient conn,bool isLogin)
        {
            //Debug.Log("[Server] Update server database");

            OutlanderDB.singleton.UpdateDatabase();

            PlayerScriptable player = OutlanderDB.singleton.GetPlayerByConn(conn);

            SendPlayerDatabaseMsg playerDatabaseMsg = new SendPlayerDatabaseMsg
            {
                isLogin = isLogin,
                playerScriptable = player
            };
            conn.Send(playerDatabaseMsg);
        }

        #endregion

        #region AutoConnect

        public static string[] Datas;

        public static bool Complete = false;
        public static string Player_Token { get; private set; }
        public static string Refresh_Token { get; private set; }
        public static string Match_ID { get; private set; }
        public static string Port { get; private set; }

        public IEnumerator Initialize(Action<bool> callBack)
        {
            string URL = Application.absoluteURL;
            string URLSplied = URL.Replace("https://" + UIManagers.Instance.uiNetwork.webGame + "/", "");

            if (URLSplied.Length > 1)
            {
                string URLRemovePass = URLSplied.Remove(0, 8);

                string base64Encoded = URLRemovePass;
                byte[] data = Convert.FromBase64String(base64Encoded);
                string base64Decoded = Encoding.UTF8.GetString(data);

                Datas = base64Decoded.Split(":|:");

                if (Datas.Length >= 2)
                {
                    Player_Token = Datas[0];
                    Refresh_Token = Datas[1];
                    Match_ID = Datas[2];

                    yield return StartCoroutine(OnClientRefreshToken((bool isRefresh) =>
                    {
                        GetPlayerData((string playerId) =>
                        {
                            if (playerId != "")
                            {
                                StartCoroutine(OnClientGetMatchData(playerId, (bool isGetMatch) =>
                                {
                                    Complete = isGetMatch;
                                    callBack(Complete);
                                }));
                            }
                            else
                            {
                                Debug.Log("[Web] your account is Unauthorized.");
                                UIManagers.Instance.uiWaiting.lostReason.text = "Your account is Unauthorized, please return to login.";
                                Complete = false;
                                callBack(Complete);
                            }
                        });
                    }));
                }
                else
                {
                    Debug.Log("[Web] Incomplete infomation.");
                    UIManagers.Instance.uiWaiting.lostReason.text = "Incomplete infomation, please return to find new match.";
                    Complete = false;
                    callBack(Complete);
                }
            }
            else
            {
                Debug.Log("[Web] URL is wrong.");
                UIManagers.Instance.uiWaiting.lostReason.text = "Wrong URL format, please return to lobby.";
                Complete = false;
                callBack(Complete);
            }
        }

        #endregion

        private void DetachGameObjectAndDestroy(Transform parentTrans)
        {
            int tempChildCount = parentTrans.childCount;
            for (int i = 0; i < tempChildCount; i++)
            {
                GameObject temp = parentTrans.GetChild(0).gameObject;
                temp.transform.parent = null;
                Destroy(temp);
            }
        }
    }
}

