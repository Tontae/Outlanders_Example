using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Outlander.Player
{
    public class PlayerSpawnManager : NetworkBehaviour
    {
        //[SerializeField] public bool isPlayerDead;

        public string mapName;
        public Vector3 spawnpoint { get; private set; }

        [Header("Respawn UI")]
        [SerializeField] private GameObject uiRespawn;
        [SerializeField] public Button ButtonRespawn;
        [SerializeField] private float respawnTime;
        private float _respawnTime;

        public void Start()
        {
            if (!isLocalPlayer) return;
            ButtonRespawn.onClick.AddListener(CmdRespawnPlayer);
            ButtonRespawn.interactable = false;
            uiRespawn.SetActive(false);

            _respawnTime = respawnTime;
        }

        [Command]
        public void CmdRespawnPlayer()
        {
            // spawnpoint = player.GetComponent<IPlayableHealth>().Spawnpoint;
            //GetComponent<IPlayer>().InitializingPlayer();
            TargetRespawnPlayer();
            //transform.position = new Vector3(1000, -50, 2500);
            //player.GetComponent<IPlayer>().IncreasePlayerHealth(player.GetComponent<IPlayer>().MaxHealth);
            //Debug.Log("[PlayerManager] Cmd Respawn Player");

        }

        [TargetRpc]
        public void TargetRespawnPlayer()
        {
            //Debug.Log($"player:{name} curMap:{SceneManager.GetActiveScene().name} checkMap:{player.GetComponent<IPlayer>().Character.checkpoint.mapName}");
            // if (SceneManager.GetActiveScene().name.Equals(GetComponent<IPlayer>().Character.checkpoint.mapName))
            //     transform.position = new Vector3(spawnpoint.x, spawnpoint.y + 1, spawnpoint.z);
            // else
            //     LoadSceneManager.singleton.OnClientSpawnMoveScene(GetComponent<IPlayer>().Character.checkpoint.mapName, spawnpoint, gameObject);

            //if (SceneManager.GetActiveScene().name.Equals(GetComponent<IPlayer>().Character.checkpoint.mapName))
            //transform.position = new Vector3(spawnpoint.x, spawnpoint.y + 1, spawnpoint.z);
            //else
            //LoadSceneManager.singleton.OnClientSpawnMoveScene(GetComponent<IPlayer>().Character.checkpoint.mapName, spawnpoint, gameObject);
            //GetComponent<IPlayer>().InitializingPlayer();
            //transform.position = new Vector3(1000, -50, 2500);

            //GameBattleRoyalManager.singleton.isSelectSpawnPhase = false;
            //GameBattleRoyalManager.singleton.selectStartTime = 10;
            //GameBattleRoyalManager.singleton.SendPlayersMoveScene();

            transform.GetComponent<PlayerMovementStateMachine>().ResetMovementBoolData();

            _respawnTime = respawnTime;
            //
            GetComponent<PlayerUIManager>().PlayerOnMatch();
        }

        public void SetSpawnPoint(Vector3 newPoint)
        {
            if (spawnpoint == newPoint) return;
            this.spawnpoint = newPoint;
            if (isLocalPlayer)
            {
                CmdSetSpawnPoint(newPoint);

            }
            if (isServer)
                TargetSetSpawnPoint(newPoint);
        }

        [Command]
        private void CmdSetSpawnPoint(Vector3 newPoint)
        {
            this.spawnpoint = newPoint;
        }

        [TargetRpc]
        private void TargetSetSpawnPoint(Vector3 newPoint)
        {
            this.spawnpoint = newPoint;
        }

        // private void SaveCharacterCheckpoint()
        // {
        //     PlayerCheckpointMsg playerCheckpointMsg = new PlayerCheckpointMsg
        //     {
        //         email = GetComponent<IPlayer>().Character.email,
        //         mapName = SceneManager.GetActiveScene().name,
        //         posX = spawnpoint.x,
        //         posY = spawnpoint.y,
        //         posZ = spawnpoint.z
        //     };
        //     NetworkClient.Send(playerCheckpointMsg);
        //     /*CharacterLocation updateCheckpoint = GetComponent<IPlayer>().Character.checkpoint;
        //     updateCheckpoint.mapName = SceneManager.GetActiveScene().name;
        //     updateCheckpoint.posX = spawnpoint.x;
        //     updateCheckpoint.posY = spawnpoint.y;
        //     updateCheckpoint.posZ = spawnpoint.z;*/
        // }
    }
}
