using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerInteractionHelper : MonoBehaviour
    {
        public PlayerComponents Player { get => PlayerOutlander.Player; }
        public PlayerOutlanderStateMachine PlayerOutlander { get; set; }

        public PlayerInteractionHelper(PlayerOutlanderStateMachine _Player)
        {
            PlayerOutlander = _Player;
        }

        NpcShop npcShop = null;

        // Bool
        // private bool PlayerOutlander.OnInteract;

        // //Properties
        // public bool PlayerOutlander.OnInteract { get => PlayerOutlander.OnInteract; set => PlayerOutlander.OnInteract = value; }


        #region Triggered Check
        public void OnPlayerTriggerEnter(Collider other)
        {
            if (other.tag == "NPC_Shop")
            {
                if (!Player.isLocalPlayer)
                    return;
                //Debug.Log($"Found NPC");

                if (npcShop == null)
                {
                    npcShop = other.gameObject.GetComponent<NpcShop>();
                    npcShop.OpenInteractPanel();
                }
            }

            if (other.tag == "NPC_Blacksmith")
            {
                if (!Player.isLocalPlayer)
                    return;

                var currentTarget = other.gameObject.GetComponent<NpcBlack>();
                currentTarget.IdentifyPlayer(this.gameObject);
                currentTarget.OpenInteractPanel();
                currentTarget.SetUpCoin();
            }
        }

        /*public void OnPlayerTriggerStay(Collider other)
        {
            if (other.tag == "NPC_Shop")
            {
                //Debug.Log($"Interact NPC:{PlayerOutlander.OnInteract}");
                if (!PlayerOutlander.OnInteract)
                    return;
                if (!Player.isLocalPlayer)
                    return;

                if (npcShop != null)
                {
                    npcShop.CloseInteractPanel();
                    npcShop.OpenShop();
                    PlayerOutlander.CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.WEAPON_ACTION, false);
                }

                PlayerOutlander.OnInteract = false;
            }

            if (other.tag == "NPC_Blacksmith")
            {
                if (!PlayerOutlander.OnInteract)
                    return;
                if (!Player.isLocalPlayer)
                    return;

                var currentTarget = other.gameObject.GetComponent<NpcBlack>();
                currentTarget.CloseInteractPanel();
                currentTarget.OpenRepairPanal();

                PlayerOutlander.OnInteract = false;
            }
        }*/

        public void OnPlayerTriggerExit(Collider other)
        {
            if (other.tag == "NPC_Shop")
            {
                //Debug.Log($"Bye NPC");
                if (!Player.isLocalPlayer)
                    return;

                if (npcShop != null)
                {
                    npcShop.CloseInteractPanel();
                    PlayerOutlander.Player.ShopManagerController.CloseShop();
                    npcShop = null;
                }
            }
            if (other.tag == "NPC_Blacksmith")
            {
                if (!Player.isLocalPlayer)
                    return;

                var currentTarget = other.gameObject.GetComponent<NpcBlack>();
                currentTarget.CloseInteractPanel();
            }
        }
        #endregion


        public void PlayerInteractCheck()
        {
            if (!PlayerOutlander.OnInteract)
                return;
            if (!Player.isLocalPlayer)
                return;

            if (InteractObjectOutliner.Instance.GetCurrentObject(out ResourecesObject.PickUpResoureObjectScripts pickUpResoureObjectScripts))
            {
                pickUpResoureObjectScripts.gameObject.layer = 16;
                pickUpResoureObjectScripts.CmdPlayerPickUp(Player);
            }
            else if (InteractObjectOutliner.Instance.GetCurrentObject(out OnlineChest onlineChest))
            {
                //var interactObjectOutliner = Player.PlayerCamera.Camera.gameObject.GetComponent<Outlander.InteractObjectOutliner>();

                if (!onlineChest.isOpen)
                {
                    onlineChest.OpenChest();
                    Player.PlayerStatisticsData.OpenChestCount += 1;
                }

                InteractObjectOutliner.Instance.unlockChest = InteractObjectOutliner.Instance.unlockChest ? false : true;

                InteractObjectOutliner.Instance.DestoryPanal();
                PlayerOutlander.CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.WEAPON_ACTION, false);
            }
            else if (InteractObjectOutliner.Instance.GetCurrentObject(out DeadBox deadBox))
            {
                //var interactObjectOutliner = Player.PlayerCamera.Camera.gameObject.GetComponent<Outlander.InteractObjectOutliner>();

                InteractObjectOutliner.Instance.unlockChest = InteractObjectOutliner.Instance.unlockChest ? false : true;

                InteractObjectOutliner.Instance.DestoryPanal();
                PlayerOutlander.CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.WEAPON_ACTION, false);
            }
            else if (InteractObjectOutliner.Instance.GetCurrentObject(out ResourecesObject.DevilFruitScript devilFruitScript))
            {
                devilFruitScript.gameObject.layer = 16;
                devilFruitScript.CmdPlayerPickUp(Player);
            }
            else if (InteractObjectOutliner.Instance.GetCurrentObject(out DropedItemBehavior dropedItemBehavior))
            {
                dropedItemBehavior.gameObject.layer = 16;
                dropedItemBehavior.CmdPlayerPickUp(Player);
            }
            else if (npcShop != null)
            {
                npcShop.CloseInteractPanel();
                npcShop.OpenShop();
                PlayerOutlander.CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.WEAPON_ACTION, false);
            }

            PlayerOutlander.OnInteract = false;
        }
    }
}

