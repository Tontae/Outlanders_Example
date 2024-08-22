using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Outlander.Network;
using System.Linq;
using UnityEngine.InputSystem;
using Outlander.Enemy.Bot;

public enum MatchStatus
{
    IsWaiting,
    IsPreparing,
    IsFull,
    IsStarting,
    IsInGame,
    IsEnd
}

public class MatchManager : NetworkBehaviour
{
    [ReadOnly] public List<PlayerComponents> players = new List<PlayerComponents>();
    private Dictionary<GameObject, object> gameObjectComponents = new Dictionary<GameObject, object>();
    [SyncVar] public int playerAlive;
    [SyncVar] public int emptyPlayerSlot;
    [SyncVar] public int mockPlayer;

    [SyncVar, ReadOnly] public MatchStatus matchStatus;
    [SyncVar, ReadOnly] public float lobbyStartTime;
    [SyncVar, ReadOnly] public float selectStartTime;
    [ReadOnly] public float gameTime;
    private AudioSource announcerAudio;

    [SerializeField] private RedZone redZonePrefab;
    [ReadOnly] public GameObject redZone;
    [ReadOnly] public RedZone redZoneRef;
    public Outlander.Enemy.EnemySpawnerManager enemySpawner;
    public Outlander.ResourecesObject.ResourecesSpawner resourecesSpawner;
    public ChestSpawner chestSpawner;

    private Dictionary<NetworkIdentity, int> killRank = new Dictionary<NetworkIdentity, int>();
    [SyncVar] private bool isEnd = false;
    [SyncVar] private bool isStarting = false;
    [SyncVar] private bool isInGame = false;
    [SyncVar] private bool isFirstKill = true;

    private bool isSelectSpawn = false;
    public string whoKillMe;

    private int curMaxPlayersAlive;
    private bool isBountyPick = false;
    public bool isBountyEnd = false;
    [ReadOnly] public NetworkIdentity selectedBounty;
    private Coroutine surviveCorotine;
    private float waitingTime;
    [SyncVar] public int requirePlayer;
    [SyncVar] public int maxPlayer;
    [SyncVar] public int realMaxPlayer;
    [SyncVar] public string matchId;
    private Vector3 selectedSpawnPoint = Vector3.zero;
    public LayerMask spawnPlayerLayer;

    [Header("Spawn Phase")]
    [SyncVar] public bool isSpawned = false;
    [SyncVar] public bool canInteract = false;
    private bool isRealPlayerChanged = false;

    public Dictionary<GameObject, object> GameObjectComponents { get => gameObjectComponents; set => gameObjectComponents = value; }

    private void Start()
    {
        if (isServer)
        {
            requirePlayer = MatchMaker.Instance.requirePlayerValue;
            maxPlayer = MatchMaker.Instance.maxPlayerValue;
            SetMatchManager();
        }
        if (isClient)
        {
            waitingTime = 0;
            canInteract = false;
            PlayerManagers.Instance.matchManager = this;
            PlayerManagers.Instance.matchId = matchId;
            PlayerManagers.Instance.PlayerComponents.PlayerMatchManager.myManager = this;
            UIManagers.Instance.ShowMatchStartUI();
        }
    }

    private void Update()
    {
        if (isServer)
        {
            if(matchStatus == MatchStatus.IsWaiting)
            {
                isRealPlayerChanged = false;
                lobbyStartTime = MatchMaker.Instance.WaitingPlayerTime;
            }
            else if (matchStatus == MatchStatus.IsPreparing || matchStatus == MatchStatus.IsFull)
            {
                if (playerAlive == MatchMaker.Instance.realMaxPlayer && !isRealPlayerChanged)
                {
                    isRealPlayerChanged = true;
                    lobbyStartTime = MatchMaker.Instance.FastStartTime;
                }
                else if (playerAlive < MatchMaker.Instance.realMaxPlayer && isRealPlayerChanged)
                {
                    isRealPlayerChanged = false;
                    lobbyStartTime = MatchMaker.Instance.WaitingPlayerTime;
                }

                if (lobbyStartTime > 0)
                {
                    lobbyStartTime -= Time.deltaTime;
                    emptyPlayerSlot = maxPlayer - playerAlive;
                    if ((lobbyStartTime % 2f > 0f && lobbyStartTime % 2f < 0.3f) || (lobbyStartTime % 5f > 0f && lobbyStartTime % 5f < 0.3f))
                    {
                        CalculateMockPlayer();
                    }
                }
                else if (!isStarting)
                {
                    lobbyStartTime = 0;
                    matchStatus = MatchStatus.IsStarting;

                    curMaxPlayersAlive = playerAlive + BotSpawner.Instance.InitBot(this);
                    playerAlive = curMaxPlayersAlive;
                    SetRealMaxPlayer(curMaxPlayersAlive);
                    //BotSpawner.Instance.MoveBot();

                    StartCoroutine(MatchMaker.Instance.networkManager.OnUpdateMatchStatus("Starting"));

                    foreach (KeyValuePair<string,string> temp in MatchMaker.Instance.registeredPlayer)
                    {
                        if (temp.Value == "waiting")
                            MatchMaker.Instance.RemovePlayerFromMatchData(temp.Key);
                    }

                    isStarting = true;
                    return;
                }
            }
            else if (matchStatus == MatchStatus.IsStarting)
            {
                if (selectStartTime > 0)
                {
                    selectStartTime -= Time.deltaTime;
                }
                else if (!isInGame)
                {
                    selectStartTime = 0;
                    matchStatus = MatchStatus.IsInGame;
                    Invoke(nameof(PickKillerBounty), 300f);
                    StartCoroutine(MatchMaker.Instance.networkManager.OnUpdateMatchStatus("InGame"));
                    isInGame = true;
                    return;
                }
            }
            else if (matchStatus == MatchStatus.IsInGame)
            {
                gameTime += Time.deltaTime;
                if (playerAlive <= curMaxPlayersAlive - 5)
                    if (!isBountyPick)
                        PickKillerBounty();
                if (!isSpawned && gameTime > 5f)
                {
                    isSpawned = true;
                    redZoneRef = Instantiate(redZonePrefab);
                    redZone = redZoneRef.gameObject;
                    redZone.name = $"Red Zone";
                    redZoneRef.myManager = this;
                    NetworkServer.Spawn(redZone);
                    //RpcRedZoneAppear(redZone);
                }
            }
            if (playerAlive <= MatchMaker.Instance.playerWinAmount && (matchStatus == MatchStatus.IsInGame || matchStatus == MatchStatus.IsStarting) && !isEnd)
            {
                selectStartTime = 0;
                matchStatus = MatchStatus.IsEnd;
                isSpawned = true;
                //playerAlive = 0;
                foreach (PlayerComponents player in players)
                    if (!player.OutlanderStateMachine.OnDie)
                        TargetSendWinner(player.PlayerIdentity.connectionToClient, playerAlive, gameTime);
                isInGame = true;
                isBountyPick = false;
                isBountyEnd = false;
                isEnd = true;
                CancelInvoke();
                StopAllCoroutines();
            }
        }
        if (isClient)
        {
            PlayerManagers.Instance.matchStatus = matchStatus;
            UIManagers.Instance.playerCanvas.playerAmount.text = Mathf.Clamp(playerAlive, 1f, 100f).ToString("0");
            //LocalMatchManager.singleton.playerAmount.text = playerAlive.ToString("0");
            if (matchStatus == MatchStatus.IsWaiting)
            {
                waitingTime += Time.deltaTime;
            }
            else if (matchStatus == MatchStatus.IsPreparing || matchStatus == MatchStatus.IsFull)
            {
                UIManagers.Instance.playerCanvas.playerAmount.text = Mathf.Clamp(playerAlive, 1f, 100f).ToString("0");
                UIManagers.Instance.uiWaiting.loadingBar.fillAmount = UIManagers.Instance.uiWaiting.FillProgressBar(playerAlive, realMaxPlayer);
                UIManagers.Instance.uiWaiting.loadingPercent.text = $"{UIManagers.Instance.uiWaiting.FillProgressPercent()}%";

                if (lobbyStartTime <= 10f && lobbyStartTime >= 9f)
                {
                    if (announcerAudio == null)
                        announcerAudio = UIManagers.Instance.optionManager.PlayClipAtUIReturnAudioSource(UIManagers.Instance.uiAnnouncement.GetAnnounceSoundIndex(4));
                }
                else
                    announcerAudio = null;

                UIManagers.Instance.uiMatch.timeText.text = lobbyStartTime.ToString("0");
            }
            else if (matchStatus == MatchStatus.IsStarting && selectStartTime > 0)
            {
                if (!isSelectSpawn)
                {
                    UIManagers.Instance.optionManager.SendMatchStart();
                    isSelectSpawn = true;
                }
                if (!isSpawned)
                {
                    SelectSpawnPhase();
                    UIManagers.Instance.uiSpawnPhase.selectSpawnTimeCount.text = selectStartTime.ToString("0");
                }
            }
            else if (matchStatus == MatchStatus.IsInGame)
            {
                if (!isSpawned)
                {
                    RandomSpawnPhase();
                }
                return;
            }
            else if (matchStatus == MatchStatus.IsEnd)
            {
                if (!isEnd)
                {
                    isEnd = true;
                    isBountyPick = false;
                    isBountyEnd = false;
                    if (!isSpawned)
                    {
                        RandomSpawnPhase();
                    }
                }
            }
        }
    }

    [ClientRpc]
    void SetRealMaxPlayer(int playerAmount)
    {
        UIManagers.Instance.playerCanvas.uiSummary.summaryAllPlayer.text = "/" + playerAmount.ToString();
    }

    void CalculateMockPlayer()
    {
        int ran = Random.Range(0, 2);
        if (mockPlayer < emptyPlayerSlot)
        {
            if (mockPlayer + ran < emptyPlayerSlot)
                mockPlayer += ran;
            else
                mockPlayer = emptyPlayerSlot;
        }
        else
            mockPlayer = emptyPlayerSlot;
    }


    public void SetMatchManager()
    {
        matchStatus = MatchStatus.IsWaiting;
        Outlander.Enemy.EnemySpawnerManager.instance.InitEnemy(this);
        Outlander.ResourecesObject.ResourecesSpawner.instance.InitInteractObject(this);
        ChestSpawner.instance.InitChest(this);
        //BotSpawner.Instance.InitBot(this);
        lobbyStartTime = MatchMaker.Instance.matchStartTime;
        selectStartTime = 15f;
        gameTime = 0f;
        isSpawned = false;
        isStarting = false;
        isInGame = false;
        isEnd = false;
        isBountyPick = false;
        isBountyEnd = false;
        isFirstKill = true;
        killRank.Clear();
        CancelInvoke();
        StopAllCoroutines();
    }

    public void UpdateMatchStatus(bool isFull, bool canStart)
    {
        if (isFull && canStart)
        {
            matchStatus = MatchStatus.IsFull;
        }
        if (!isFull && canStart)
        {
            matchStatus = MatchStatus.IsPreparing;
        }
        if (!isFull && !canStart)
        {
            matchStatus = MatchStatus.IsWaiting;
        }
    }

    public void RemovePlayerFromMatchManger(PlayerComponents player)
    {
        //Debug.Log($"Server remove player:{player.name}");
        player.PlayerStatisticsData.PlayerRank = playerAlive;
        player.PlayerStatisticsData.PlayerSurviveTime = gameTime;
        playerAlive -= 1;

        if (playerAlive == 0 && (matchStatus == MatchStatus.IsInGame || matchStatus == MatchStatus.IsStarting))
            TargetSendWinner(player.PlayerIdentity.connectionToClient, playerAlive, gameTime);
        else if (matchStatus == MatchStatus.IsInGame || matchStatus == MatchStatus.IsStarting)
            if (player.OutlanderStateMachine.OnDie)
                TargetSendPlayerOnSummary(player.PlayerIdentity.connectionToClient, playerAlive, gameTime);

        if (killRank.ContainsKey(player.PlayerIdentity))
        {
            killRank.Remove(player.PlayerIdentity);
        }
        if (selectedBounty?.netId == player.netId)
        {
            selectedBounty = null;
            StopCoroutine(surviveCorotine);
            if (!isBountyEnd)
            {
                BroadcastKillerBounty(false, player.netId);
                RpcBoardcastHeadHunterKilled();
            }
            isBountyEnd = true;
        }
        player.PlayerStatisticsData.EndMatchNormal = true;
    }

    public void RemoveBotFromMatchManger(BotBehaviorManager bot)
    {
        //Debug.Log($"Server remove bot:{bot.name}");
        playerAlive -= 1;
        
        if (killRank.ContainsKey(bot.netIdentity))
        {
            killRank.Remove(bot.netIdentity);
        }
        if (selectedBounty?.netId == bot.netId)
        {
            selectedBounty = null;
            StopCoroutine(surviveCorotine);
            if (!isBountyEnd)
            {
                BroadcastKillerBounty(false, bot.netId);
                RpcBoardcastHeadHunterKilled();
            }
            isBountyEnd = true;
        }
        StartCoroutine(DestroyBotGameObject(bot.gameObject, 10f));
    }

    private IEnumerator DestroyBotGameObject(GameObject bot, float time) 
    {
        //bot.name = $"Destroy {bot.name}";
        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(bot);
    }

    public void SelectSpawnPhase()
    {
        if (isSpawned) return;
        CursorManager.Instance.selectmap = true;
        CursorManager.Instance.lobby = false;
        if (UIManagers.Instance.uiSpawnPhase.selectSpawnAudioGO == null)
            UIManagers.Instance.OnUISelectSpawnPhase();

        Ray ray = UIManagers.Instance.selectSpawnCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Mouse.current.leftButton.isPressed)
        {
            //if (Physics.Raycast(ray, out hit, 3000))
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, spawnPlayerLayer.value))
            {
                if (!hit.collider.CompareTag("UnSelected"))
                {
                    //isClick = 1;
                    UIManagers.Instance.uiSpawnPhase.selectPoint.SetActive(true);
                    UIManagers.Instance.uiSpawnPhase.selectPoint.transform.position = Input.mousePosition;
                    selectedSpawnPoint = ray.origin;
                }
            }
        }
        //if (!Mouse.current.leftButton.isPressed && isClick == 1)
        //{
        //    isClick = 2;
        //}

        //if (isClick == 2)
        //{

        //    isClick = 0;
        //    if (Physics.Raycast(ray, out hit, float.PositiveInfinity, spawnPlayerLayer.value))
        //    {
        //        if (!hit.collider.CompareTag("UnSelected"))
        //        {
        //            Debug.Log($"[Spawner] Position : {hit.point} | MousePos : {Input.mousePosition} | Object : {hit.collider.gameObject.name}");
        //            UIManagers.Instance.uiSpawnPhase.Hide();
        //            UIManagers.Instance.uiMatch.timeText.text = "";
        //            UIManagers.Instance.uiMatch.timeTitleText.text = "";
        //            PlayerManagers.Instance.PlayerGO.GetComponent<PlayerMatchManager>().TargetBeginGame();
        //            LoadSceneManager.singleton.OnclientSelectSpawnPosition(hit.point);
        //            UIManagers.Instance.uiMatch.matchCanvas.enabled = true;
        //            UIManagers.Instance.uiAnnouncement.uiAnnounceCanvas.enabled = true;
        //            isSpawned = true;
        //            canInteract = true;
        //            CursorManager.Instance.selectmap = false;

        //            UIManagers.Instance.optionManager.SwapMusicBetweenScene(1);
        //        }
        //        else
        //        {
        //            Debug.Log("[Spawner] Can't select this area");
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("[Spawner] Can't select this area");
        //    }
        //}
    }

    public void RandomSpawnPhase()
    {
        if (isSpawned) return;
        Vector3 randomPos;
        if (selectedSpawnPoint == Vector3.zero)
            randomPos = new Vector3(UnityEngine.Random.Range(630f, 1390f), 100f, UnityEngine.Random.Range(2120f, 2880f));
        else
            randomPos = selectedSpawnPoint;
        //Vector3 randomPos = new Vector3(1270.66284f, 100f, 2183.28418f);

        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, float.PositiveInfinity, spawnPlayerLayer.value))
        {
            if (!hit.collider.CompareTag("UnSelected"))
            {
                //Debug.Log($"[Spawner] Position : {hit.point} | RandomPos : {randomPos} | Object : {hit.collider.gameObject.name}");

                UIManagers.Instance.uiSpawnPhase.Hide();
                UIManagers.Instance.uiMatch.timeText.text = "";
                UIManagers.Instance.uiMatch.timeTitleText.text = "";
                PlayerManagers.Instance.PlayerComponents.PlayerMatchManager.TargetBeginGame();
                LoadSceneManager.singleton.OnclientSelectSpawnPosition(hit.point);
                UIManagers.Instance.uiMatch.matchCanvas.enabled = true;
                UIManagers.Instance.uiAnnouncement.uiAnnounceCanvas.enabled = true;
                isSpawned = true;
                canInteract = true;
                CursorManager.Instance.selectmap = false;
                UIManagers.Instance.optionManager.SwapMusicBetweenScene(1);
            }
            else
            {
                RandomSpawnPhase();
            }
        }
    }

    public void OnReceiveKillActivity(string killer, string type, string victim)
    {
        if (matchStatus != MatchStatus.IsInGame && matchStatus != MatchStatus.IsEnd) return;
        //Debug.Log($"{victim}={NetworkClient.localPlayer.name}:{victim.Equals(NetworkClient.localPlayer.name)}=>{whoKillMe}");
        if (victim.Equals(NetworkClient.localPlayer.name))
            whoKillMe = killer == "" ? type switch
            {
                "DROWN" => "Drowning",
                "FALL" => "Falling",
                "REDZONE" => "Red Zone",
                _ => "Admin"
            } : killer;
        GameObject killFeed = Resources.Load<GameObject>("UI-UX/KillFeed_UI/KillFeed");
        GameObject killFeedGO = Instantiate(killFeed, UIManagers.Instance.uiMatch.killFeedGrid);
        TMPro.TextMeshProUGUI[] tmps = killFeedGO.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        tmps[0].text = killer;
        if (tmps[0].preferredWidth > 50f)
            tmps[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tmps[0].preferredWidth);
        tmps[1].text = victim;
        if (tmps[1].preferredWidth > 50f)
            tmps[1].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tmps[1].preferredWidth);
        killFeedGO.GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite = Resources.Load<Sprite>($"UI-UX/KillFeed_UI/{type}");
        Destroy(killFeedGO, 10f);
    }

    [TargetRpc]
    void TargetSendWinner(NetworkConnection conn, int curPlayerAlive, float gameTime)
    {
        if (PlayerManagers.Instance.PlayerComponents.PlayerMatchManager.isWinner) return;
        PlayerManagers.Instance.PlayerComponents.PlayerMatchManager.isWinner = true;
        StartCoroutine(WinnerWinnerChickenGrinder(curPlayerAlive, gameTime));
    }

    IEnumerator WinnerWinnerChickenGrinder(int curPlayerAlive, float gameTime)
    {
        UIManagers.Instance.uiAnnouncement.ClearAnnounce();
        PlayerManagers.Instance.matchManager.canInteract = false;
        UIManagers.Instance.uiMatch.matchCanvas.enabled = true;
        UIManagers.Instance.uiMatch.timeTitleText.gameObject.SetActive(true);
        UIManagers.Instance.uiMatch.timeTitleText.text = "Match Ends In";
        int timeToLobby = 10;
        if (curPlayerAlive == 0)
            timeToLobby = 0;
        while (timeToLobby != 0)
        {
            UIManagers.Instance.uiMatch.timeTitleText.gameObject.SetActive(true);
            UIManagers.Instance.uiMatch.timeText.text = timeToLobby.ToString("0");
            yield return new WaitForSeconds(1f);
            timeToLobby -= 1;
        }
        UIManagers.Instance.uiMatch.timeTitleText.text = "Start In";
        UIManagers.Instance.uiMatch.timeTitleText.gameObject.SetActive(false);
        PlayerManagers.Instance.PlayerComponents.PlayerUIManager.OnGameSummary(true, 1, gameTime);
        //conn.identity.GetComponent<Outlander.Player.PlayerUIManager>().OnGameSummary(true, 1);
        Camera.main.transform.GetChild(0).gameObject.SetActive(false);
    }

    [TargetRpc]
    private void TargetSendPlayerOnSummary(NetworkConnection conn, int playerRank, float gameTime)
    {
        PlayerManagers.Instance.PlayerComponents.PlayerUIManager.OnGameSummary(false, playerRank, gameTime);
        PlayerManagers.Instance.PlayerComponents.PlayerCamera.Camera.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void IncreaseKillRank(PlayerComponents player, PlayerComponents victim)
    {
        if (!killRank.TryAdd(player.PlayerIdentity, 1))
            killRank[player.PlayerIdentity] += 1;
        killRank.TryGetValue(victim.PlayerIdentity, out int count);
        //player.PlayerMatchManager.playerKillCount = killRank[player.PlayerIdentity];
        UpdateKillCountPlayer(player.connectionToClient, killRank[player.PlayerIdentity], isFirstKill, count >= 5 ? true : false);
        bool isFirst = false;
        bool isBounty = selectedBounty == victim.netIdentity;
        if (isFirstKill)
        {
            isFirstKill = false;
            isFirst = true;
            BroadcastFirstKill();
        }
        if (!isBountyEnd)
            if (isBounty)
                GetBountyReward(player.netIdentity.connectionToClient);
        player.PlayerStatisticsData.UpdatePlayerKill(player.WeaponManager.currentWeaponType, isBounty, isFirst, count >= 5 ? true : false);
        
    }

    public void IncreaseKillRank(NetworkIdentity bot, NetworkIdentity victim)
    {
        if (!killRank.TryAdd(bot, 1))
            killRank[bot] += 1;
        if (isFirstKill)
        {
            isFirstKill = false;
            BroadcastFirstKill();
        }
    }

    public void IncreaseKillRank(PlayerComponents player, NetworkIdentity victim)
    {
        if (!killRank.TryAdd(player.PlayerIdentity, 1))
            killRank[player.PlayerIdentity] += 1;
        killRank.TryGetValue(victim, out int count);
        //player.PlayerMatchManager.playerKillCount = killRank[player.PlayerIdentity];
        UpdateKillCountPlayer(player.connectionToClient, killRank[player.PlayerIdentity], isFirstKill, count >= 5 ? true : false);
        bool isFirst = false;
        bool isBounty = selectedBounty == victim;
        if (isFirstKill)
        {
            isFirstKill = false;
            isFirst = true;
            BroadcastFirstKill();
        }
        if (!isBountyEnd)
            if (isBounty)
                GetBountyReward(player.netIdentity.connectionToClient);
        player.PlayerStatisticsData.UpdatePlayerKill(player.WeaponManager.currentWeaponType, isBounty, isFirst, count >= 5 ? true : false);
    }

    [TargetRpc]
    private void UpdateKillCountPlayer(NetworkConnection conn, int playerKillCount, bool isFrist, bool isGodSlayer)
    {
        PlayerManagers.Instance.PlayerComponents.PlayerStatisticsData.PlayerKillCount = playerKillCount;
        ClientTriggerEventManager.Instance.KillPlayer(playerKillCount, isFrist, isGodSlayer);
    }

    [ClientRpc]
    private void BroadcastFirstKill()
    {
        ClientTriggerEventManager.Instance.FirstKill();
    }

    private void PickKillerBounty()
    {
        CancelInvoke();
        isBountyPick = true;
        int maxKill = 0;
        bool isfirst = false;
        List<NetworkIdentity> killers = new List<NetworkIdentity>();
        foreach (KeyValuePair<NetworkIdentity, int> temp in killRank.OrderByDescending(key => key.Value))
        {
            if (!isfirst)
            {
                maxKill = temp.Value;
                isfirst = true;
            }

            if (temp.Value == maxKill)
                killers.Add(temp.Key);
        }
        if (killers.Count >= 1)
        {
            int randomWanted = Random.Range(0, killers.Count);
            selectedBounty = killers[randomWanted];
            BroadcastKillerBounty(true, killers[randomWanted].netId);
        }
        else if (killers.Count == 0)
        {
            isfirst = false;
            foreach (var playerComp in players.OrderByDescending(comp => comp.PlayerStatisticsData.MonsterKillCount + comp.PlayerStatisticsData.MiniBossKillCount * 10f))
            {
                if (!isfirst)
                {
                    maxKill = playerComp.PlayerStatisticsData.MonsterKillCount + playerComp.PlayerStatisticsData.MiniBossKillCount * 10;
                    isfirst = true;
                }

                if (playerComp.PlayerStatisticsData.MonsterKillCount + playerComp.PlayerStatisticsData.MiniBossKillCount * 10 == maxKill)
                    killers.Add(playerComp.netIdentity);
            }
            if (killers.Count >= 1)
            {
                int randomWanted = Random.Range(0, killers.Count);
                selectedBounty = killers[randomWanted];
                BroadcastKillerBounty(true, killers[randomWanted].netId);
            }
            else if (killers.Count == 0)
            {

                int randomWanted = Random.Range(0, players.Count);
                if (players[randomWanted].OutlanderStateMachine.OnDie)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (!players[i].OutlanderStateMachine.OnDie)
                        {
                            randomWanted = i;
                            break;
                        }
                    }
                }
                selectedBounty = players[randomWanted].PlayerIdentity;
                BroadcastKillerBounty(true, players[randomWanted].netId);
            }
        }
        surviveCorotine = StartCoroutine(SurviveCountDown());
    }

    IEnumerator SurviveCountDown()
    {
        yield return new WaitForSeconds(120f);
        if (!isBountyEnd)
        {
            if (GameObjectComponents[selectedBounty.gameObject] as PlayerComponents != null)
            {
                GetBountyReward(selectedBounty.connectionToClient);
                (GameObjectComponents[selectedBounty.gameObject] as PlayerComponents).PlayerStatisticsData.IsSurviveBounty = true;
            }
            BroadcastKillerBounty(false, selectedBounty.netId);
        }
        isBountyEnd = true;
    }

    [TargetRpc]
    public void GetBountyReward(NetworkConnection target)
    {
        //if (target.identity.TryGetComponent(out BotBehaviorManager _)) return;
        ItemScriptable reward = Resources.Load<ItemScriptable>("Items/Matt/Coin/BronzeCoin");
        PlayerManagers.Instance.PlayerComponents.InventoryManager.AddItemToinventory(reward, 100, null);
        PlayerManagers.Instance.PlayerComponents.PlayerUIManager.GetObtainItemData(reward.itemName, OutlanderDB.singleton.GetSpritebyName(reward.spriteName), 100);
    }

    [ClientRpc]
    private void BroadcastKillerBounty(bool isBounty, uint wantedId)
    {
        if (!isBounty)
            ClientTriggerEventManager.Instance.BountyDisappear(wantedId);
        else
            ClientTriggerEventManager.Instance.BountyAppear(wantedId);
    }

    [ClientRpc]
    private void RpcBoardcastHeadHunterKilled()
    {
        ClientTriggerEventManager.Instance.HeadHunterKilled();
    }

    [ClientRpc]
    public void RpcKickPlayer()
    {
        KickLastPlayer();
    }

    private async void KickLastPlayer()
    {
        await MatchMaker.Instance.PlayerDisconnected(PlayerManagers.Instance.PlayerComponents);
    }

    //private void OnPlayerChange(int oldValue, int newValue)
    //{
    //    if (matchStatus is MatchStatus.IsWaiting or MatchStatus.IsPreparing or MatchStatus.IsFull)
    //    {
    //        if (newValue == realMaxPlayer)
    //            lobbyStartTime = 3f;
    //    }

    //    UIManagers.Instance.playerCanvas.playerAmount.text = Mathf.Clamp(playerAlive, 1f, 100f).ToString("0");
    //    UIManagers.Instance.uiWaiting.loadingBar.fillAmount = (float)playerAlive / (float)realMaxPlayer;
    //    UIManagers.Instance.uiWaiting.loadingPercent.text = $"{UIManagers.Instance.uiWaiting.loadingBar.fillAmount * 100f}%";
    //}
}
