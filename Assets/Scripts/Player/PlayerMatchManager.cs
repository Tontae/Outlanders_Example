using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Outlander.Player;
using System.Linq;

namespace Outlander.Network
{
    public class PlayerMatchManager : PlayerElements
    {
        [SerializeField] public List<GameObject> hideObjs;

        public MatchManager myManager;

        //public int playerScore;
        //public int playerKillCount;
        //public int monsterKillCount;
        //public int miniBossKillCount;
        //public int bossKillCount;
        //public int playerRank;
        //public float playerSurviveTime;

        //public float playerDamageCount;
        //public Dictionary<WeaponManager.WeaponType, int> playerKillType = new Dictionary<WeaponManager.WeaponType, int>();
        //public Dictionary<WeaponManager.WeaponType, float> playerDamageType = new Dictionary<WeaponManager.WeaponType, float>();

        public bool isWinner = false;

        void Start()
        {
            InitPlayer();
            if (Player.PlayerScriptable != null)
                gameObject.name = Player.PlayerScriptable.username;
        }

        /* 
            MatchMaking MATCH
        */

        public void SetPlayerObjectName(string _name)
        {
            gameObject.name = _name;
            SetClientObjectName(_name);
        }

        [ClientRpc]
        public void SetClientObjectName(string _name)
        {
            gameObject.name = _name;
        }

        public void InitPlayer()
        {
            if (isLocalPlayer)
            {
                foreach (GameObject temp in hideObjs)
                {
                    temp.SetActive(false);
                }
                isWinner = false;
                Player.PlayerInput.enabled = false;
                Player.PlayerCamera.enabled = false;
                Player.MovementStateMachine.enabled = false;
                Player.OutlanderStateMachine.enabled = false;
                Player.PlayerInventoryController.enabled = false;
                Player.ShopManagerController.enabled = false;
                Player.CharacterController.enabled = false;
                //Player.CapsuleCollider.isTrigger = true;
                //playerScore = 0;
                //playerKillCount = 0;
                //monsterKillCount = 0;
                //miniBossKillCount = 0;
                //bossKillCount = 0;
                //UpdatePlayerKillCount(playerKillCount);
                //CmdInitPlayer();
                //playerDamageCount = 0;
            }
            if (isServer)
            {
                Player.PlayerCamera.enabled = false;
                Player.CharacterController.enabled = false;
                //Player.CapsuleCollider.isTrigger = true;
                //playerScore = 0;
                //playerKillCount = 0;
                //monsterKillCount = 0;
                //miniBossKillCount = 0;
                //bossKillCount = 0;
                //playerDamageCount = 0;
            }
        }

        //[Command]
        //void CmdInitPlayer()
        //{
        //    Player.PlayerCamera.enabled = false;
        //    Player.CharacterController.enabled = false;
        //    //Player.CapsuleCollider.isTrigger = true;
        //    playerScore = 0;
        //    playerKillCount = 0;
        //    monsterKillCount = 0;
        //    miniBossKillCount = 0;
        //    bossKillCount = 0;
        //}

        public void TargetBeginGame()
        {
            Debug.Log($"[Client] Player: {name} is in match| Game Beginning");
            gameObject.SetActive(true);
            //transform.position = RandomPointInAnnulus(GameObject.Find("FirstSpawnPoint").transform.position,0f,10f);
            //Player.CapsuleCollider.isTrigger = false;
            Player.CharacterController.enabled = true;
            Player.ShopManagerController.enabled = true;
            Player.PlayerInventoryController.enabled = true;
            Player.OutlanderStateMachine.enabled = true;
            Player.MovementStateMachine.enabled = true;
            Player.PlayerCamera.enabled = true;
            Player.PlayerInput.enabled = true;
            Player.OutlanderStateMachine.InitializeCharacterData();
            Player.PlayerUIManager.PlayerOnMatch();
            Player.MovementStateMachine.IsStartSumDistance = true;
            foreach (GameObject temp in hideObjs)
            {
                temp.SetActive(true);
            }
            CharacterLoadMsg message = new CharacterLoadMsg
            {
                id = PlayerManagers.Instance.PlayerScriptable.id,
                username = PlayerManagers.Instance.PlayerScriptable.username,
                spawnName = "FirstSpawnPoint",
            };
            NetworkClient.Send(message);
        }

        public Vector3 RandomPointInAnnulus(Vector3 origin, float minRadius, float maxRadius)
        {
            float randomDistance = Random.Range(minRadius, maxRadius);
            Vector2 vector2 = Random.insideUnitCircle.normalized * randomDistance;
            Vector3 point = origin + new Vector3(vector2.x, 0f, vector2.y);
            return point;
        }

        //public void UpdatePlayerKillCount(int killCount)
        //{
        //    playerKillCount = killCount;
        //    Player.PlayerUIManager.UpdateKillCount(killCount);
        //    //CmdUpdatePlayerKillCount(killCount);
        //}

        //[Command]
        //void CmdUpdatePlayerKillCount(int _killCount)
        //{
        //    playerKillCount = _killCount;
        //}

        //public void UpdateMonsterKillCount(bool isMiniBoss, bool isBoss)
        //{
        //    if (isBoss)
        //    {
        //        bossKillCount += 1;
        //    }
        //    else if (isMiniBoss)
        //    {
        //        miniBossKillCount += 1;
        //    }
        //    else
        //    {
        //        monsterKillCount += 1;
        //    }
        //    //CmdUpdateMonsterKillCount(bossKillCount, miniBossKillCount, monsterKillCount);
        //}

        //[Command]
        //void CmdUpdateMonsterKillCount(int bossKill, int miniBossKill, int monsterKill)
        //{
        //    bossKillCount = bossKill;
        //    miniBossKillCount = miniBossKill;
        //    monsterKillCount = monsterKill;
        //}

        [ClientRpc]
        public void RpcBroadcastPlayerExitMatch(string type, string name)
        {
            PlayerManagers.Instance.matchManager.OnReceiveKillActivity("", type, name);
        }
    }
}
