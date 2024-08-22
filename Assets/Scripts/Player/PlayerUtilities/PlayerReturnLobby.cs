using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Outlander.Network;

namespace Outlander.Player
{
    public class PlayerReturnLobby : PlayerElements
    {
        //[SerializeField] private GameObject uiReturnLobby;
        //[SerializeField] public Button ButtonLobby;
        //[SerializeField] public Button ButtonStart;
        bool isStartNew = false;
        public bool isLeave = false;
        public float returnTime = 30f;

        public IEnumerator CountDownToLobby()
        {
            PlayerManagers.Instance.matchManager.canInteract = false;
            while (returnTime > 0)
            {
                yield return new WaitForSeconds(1f);
                returnTime -= 1f;
            }
            UIManagers.Instance.optionManager.SendLeaveFromGame();
            ForceToLobby();
        }

        public void ButtonLeaveMatch()
        {
            ForceToLobby();
        }

        private void ForceToLobby()
        {
            StopAllCoroutines();
            Player.PlayerUIManager.StopAllCoroutines();
            isLeave = true;
            returnTime = 0;
            Player.CharacterController.enabled = false;
            //CursorManager.Instance.lobby = true;
            //transform.position = Vector3.zero;
            //Player.PlayerUIManager.PlayerOnLobby();
            //Player.OutlanderStateMachine.DropAllItemOnDie();
            //Player.RuneSystemManager.ResetPlayerBag();
            //Player.MovementStateMachine.ResetMovementBoolData();
            //StartCoroutine(WaitToServerUnspawn());
            NetworkClient.Disconnect();
            UIManagers.Instance.optionManager.LeaveMatch();
        }

        //IEnumerator WaitToServerUnspawn()
        //{
        //    yield return new WaitForSeconds(1f);
        //    isLeave = false;
        //    Player.PlayerMatchManager.InitPlayer();
        //    //Player.OutlanderStateMachine.InitializeCharacterData();
        //    DestroyPlayerMsg destroyPlayerMsg = new DestroyPlayerMsg
        //    {
        //        isNewMatch = isStartNew
        //    };
        //    NetworkClient.Send(destroyPlayerMsg);
        //}

        //[Command]
        //public async void CmdReturnPlayer()
        //{
        //    if (!Player.OutlanderStateMachine.OnDie)
        //        Player.PlayerMatchManager.RpcBroadcastPlayerExitMatch("DISCONNECT", name);
        //    await MatchMaker.Instance.PlayerDisconnected(Player);
        //}
    }
}
