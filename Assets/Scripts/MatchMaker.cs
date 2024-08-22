using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static JSONMatch;
using Mirror.SimpleWeb;
using UnityEngine.Rendering;

namespace Outlander.Network
{
    public enum GameMode
    {
        normal,
        tournament
    }

    [Serializable]
    public class Match
    {
        public MatchManager matchManager;

        public void AddPlayer(PlayerComponents player)
        {
            matchManager.players.Add(player);
            matchManager.GameObjectComponents.Add(player.gameObject, player);
            matchManager.playerAlive = matchManager.players.Count;
            matchManager.lobbyStartTime = MatchMaker.Instance.matchStartTime;
        }

        public void RemovePlayer(PlayerComponents player)
        {
            matchManager.players.Remove(player);
            if (!player.OutlanderStateMachine.OnDie)
            {
                player.PlayerStatisticsData.PlayerRank = matchManager.playerAlive;
                player.PlayerStatisticsData.PlayerSurviveTime = matchManager.gameTime;
                matchManager.playerAlive -= 1;
            }
        }

        public Match() { }
    }

    public class MatchMaker : SingletonPersistent<MatchMaker>
    {
        //public Dictionary<string, Match> matches = new Dictionary<string, Match>();
        [ReadOnly] public Match matchData;
        public NetworkManagerOutsDB networkManager;
        public GameMode gameMode;
        public int maxPlayerValue;
        public int requirePlayerValue;
        public float matchStartTime;
        public int playerWinAmount;

        public float FastStartTime;
        public float WaitingPlayerTime;

        public string matchID;
        public Dictionary<string, string> registeredPlayer = new Dictionary<string, string>();
        public int realMaxPlayer;

        // Start is called before the first frame update

        public override void Awake()
        {
            base.Awake();
            if (requirePlayerValue > maxPlayerValue)
                requirePlayerValue = maxPlayerValue;
        }

        public async Task MatchMakingGame()
        {
            Debug.Log($"[Match] Match : is created.");
            Match match = new Match();
            matchData = match;

            //Debug.Log($"{matches[_matchID].players.Count} || {requireNumerPlayerToStart}");
            GameObject matchGO = Instantiate(networkManager.spawnPrefabs[0]);
            matchGO.name = $"MatchManager";
            matchData.matchManager = matchGO.GetComponent<MatchManager>();
            matchData.matchManager.matchId = matchID;
            matchData.matchManager.realMaxPlayer = realMaxPlayer;
            NetworkServer.Spawn(matchData.matchManager.gameObject);
            await Task.Yield();
            CancelInvoke();
            StartCoroutine(AutoCloseServer(1200f));
        }

        public void JoinGame(PlayerComponents player)
        {
            matchData.AddPlayer(player);
            if (matchData.matchManager.players.Count == maxPlayerValue)
            {
                matchData.matchManager.UpdateMatchStatus(true, true);
                StartCoroutine(networkManager.OnUpdateMatchStatus("Preparing"));
                Debug.Log($"[Match] Match : is full.");
            }
            else if (matchData.matchManager.players.Count >= requirePlayerValue)
            {
                matchData.matchManager.UpdateMatchStatus(false, true);
                StartCoroutine(networkManager.OnUpdateMatchStatus("Preparing"));
                Debug.Log($"[Match] Match : is ready to play.");
            }
            else
            {
                matchData.matchManager.UpdateMatchStatus(false,false);
                StartCoroutine(networkManager.OnUpdateMatchStatus("Waiting"));
            }

            player.PlayerMatchManager.myManager = matchData.matchManager;
        }

        public async Task PlayerDisconnected(PlayerComponents player)
        {
            matchData.RemovePlayer(player);

            if (matchData.matchManager.matchStatus == MatchStatus.IsWaiting || matchData.matchManager.matchStatus == MatchStatus.IsFull || matchData.matchManager.matchStatus == MatchStatus.IsPreparing)
                RemovePlayerFromMatchData(player.PlayerScriptable.id);
            else
                await networkManager.OnUpdatePlayerHistory(player);

            if (matchData.matchManager.players.Count >= requirePlayerValue && (matchData.matchManager.matchStatus == MatchStatus.IsWaiting || matchData.matchManager.matchStatus == MatchStatus.IsFull))
            {
                matchData.matchManager.UpdateMatchStatus(false, true);
                StartCoroutine(networkManager.OnUpdateMatchStatus("Preparing"));
            }
            else if(matchData.matchManager.matchStatus == MatchStatus.IsFull || matchData.matchManager.matchStatus == MatchStatus.IsPreparing)
            {
                matchData.matchManager.UpdateMatchStatus(false, false);
                StartCoroutine(networkManager.OnUpdateMatchStatus("Waiting"));
            }

            Debug.Log($"[Match] Player : {player.PlayerScriptable.id} disconnected from Match | Players remaining : {matchData.matchManager.playerAlive}");
            if (matchData.matchManager.players.Count < requirePlayerValue)
                matchData.matchManager.RpcKickPlayer();

            if (matchData.matchManager.players.Count <= 0)
                StartCoroutine(AutoCloseServer(1f));

            await Task.Yield();
        }

        public IEnumerator AutoCloseServer(float time)
        {
            yield return new WaitForSeconds(time);
            StartCoroutine(OnCloseServer());
        }

        private IEnumerator OnCloseServer()
        {
            yield return StartCoroutine(networkManager.OnUpdateMatchStatus("Ending"));
            Application.Quit();
        }

        public void RemovePlayerFromMatchData(string player_id)
        {
            if (registeredPlayer[player_id] == "exit")
                networkManager.OnServerUpdateMatchPlayer(matchData.matchManager.matchId, player_id, "out");
            else
                networkManager.OnServerUpdateMatchPlayer(matchData.matchManager.matchId, player_id, "lost");
            realMaxPlayer -= 1;
            registeredPlayer.Remove(player_id);
        }
    }

    public static class MatchExtensions
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new Guid(hashBytes);
        }
    }
}
