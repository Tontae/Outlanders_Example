// using Mirror;
// using Outlander.Item;
// using Outlander.Manager;
// using Outlander.Network;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Outlander.Player
// {
//     public class PlayerDieHelper : NetworkBehaviour
//     {
//         public PlayerOutlanderStateMachine PlayerOutlander { get; }
//         public PlayerDieHelper(PlayerOutlanderStateMachine _PlayerOutlander)
//         {
//             PlayerOutlander = _PlayerOutlander;
//         }

//         #region Die
//         public void DieAction()
//         {
//             if (PlayerOutlander.isLocalPlayer) DropAllItemOnDie();

//             if (!PlayerOutlander.Player.MovementStateMachine.IsSwim)
//                 PlayerOutlander.Player.Animator.Play("die");

//             PlayerOutlander.Player.PlayerInputManager.SetInputDisable();

//             PlayerOutlander.gameObject.layer = 0;

//             if (PlayerOutlander.isLocalPlayer && PlayerOutlander.PlayerHP <= 0)
//             {
//                 PlayerOutlander.PlayerHP = 1;
//                 global::ClientTriggerEventManager.Instance.IsKilled();
//             }

//             if (!PlayerOutlander.isServer) return;
//             if (PlayerOutlander.EnemyDamageBufferLog.TryDequeue(out GameObject lastPlayerHit))
//             {
//                 if (lastPlayerHit.TryGetComponent(out PlayerComponents killerComp))
//                 {
//                     PlayerOutlander.Player.PlayerMatchManager.myManager.IncreaseKillRank(killerComp, PlayerOutlander.Player);
//                     PlayerOutlander.RpcBroadcastKill(lastPlayerHit.name, killerComp.WeaponManager.currentWeaponType.ToString(), name);
//                     if (PlayerOutlander.Player.PlayerMatchManager.myManager.selectedBounty == PlayerOutlander.netIdentity)
//                         killerComp.PlayerMatchManager.myManager.GetBountyReward(killerComp.PlayerIdentity.connectionToClient);
//                 }
//                 else if (lastPlayerHit.TryGetComponent(out IMonster iMons))
//                 {
//                     string enemy = iMons.IsBoss ? PlayerDamageRecieveType.BOSS.ToString() : PlayerDamageRecieveType.MONSTER.ToString();
//                     PlayerOutlander.RpcBroadcastKill(enemy, enemy, name);
//                 }
//                 else
//                 {
//                     if (lastPlayerHit.name.Equals("MONSTER"))
//                         PlayerOutlander.RpcBroadcastKill("MONSTER", PlayerDamageRecieveType.MONSTER.ToString(), name);
//                     else
//                         PlayerOutlander.RpcBroadcastKill("", lastPlayerHit.name, name);
//                 }

//                 PlayerOutlander.Player.PlayerMatchManager.myManager?.RemovePlayerFromMatchManger(PlayerOutlander.Player);
//             }
//         }

//         public void DropAllItemOnDie()
//         {
//             if (PlayerOutlander.Player.InventoryManager.itemObjectInBag.Count > 0)
//             {
//                 List<RemoveItemInfo> removitemList = new List<RemoveItemInfo>();
//                 List<ItemScriptable> items = new List<ItemScriptable>();
//                 List<int> amounts = new List<int>();
//                 foreach (InventoryItemBehavior item in PlayerOutlander.Player.InventoryManager.itemObjectInBag)
//                 {
//                     InventoryItemBehavior inventoryItem = item.GetComponent<InventoryItemBehavior>();

//                     switch (inventoryItem.thisItem.mainType)
//                     {
//                         default: break;

//                         case Type.Equipment:
//                             if (inventoryItem.IsEquiped) PlayerOutlander.Player.EquipmentManager.ForceUnequipItem(item);
//                             break;
//                         case Type.MainWeapon:
//                             if (inventoryItem.IsEquiped) PlayerOutlander.Player.EquipmentManager.ForceUnequipItem(item);
//                             break;
//                         case Type.SubWeapon:
//                             if (inventoryItem.IsEquiped) PlayerOutlander.Player.EquipmentManager.ForceUnequipItem(item);
//                             break;
//                         case Type.Rune:
//                             if (inventoryItem.IsEquiped) PlayerOutlander.Player.EquipmentManager.ForceUnequipItem(item);
//                             break;
//                     }

//                     items.Add(inventoryItem.thisItem);
//                     amounts.Add(inventoryItem.Amount);
//                     removitemList.Add(
//                         new RemoveItemInfo
//                         {
//                             removeItemObject = item,
//                             qty = inventoryItem.Amount
//                         });
//                 }

//                 if (PlayerOutlander.Player.PlayerInventoryController.Bronze > 0)
//                 {
//                     items.Add(OutlanderDB.singleton.GetItemScriptable("000"));
//                     amounts.Add(PlayerOutlander.Player.PlayerInventoryController.Bronze);
//                 }

//                 PlayerOutlander.Player.InventoryManager.RemoveItemFromInventory(removitemList);
//                 PlayerOutlander.Player.InventoryManager.equipedGeneralDictionary.Clear();
//                 PlayerOutlander.Player.InventoryManager.SetItemSlotToDefaultSprite();
//                 CmdInstantiateDropAllObj(items, amounts);
//             }
//             PlayerOutlander.Player.PlayerInventoryController.Bronze = 0;
//         }

//         [Command]
//         private void CmdInstantiateDropAllObj(List<ItemScriptable> items, List<int> amounts)
//         {
//             Vector3 objectposition = gameObject.transform.position + new Vector3(0, 0, 1);
//             GameObject newDropItem = Instantiate(PlayerOutlander.DeadBox, objectposition, Quaternion.identity);

//             newDropItem.GetComponent<DeadBox>().AssignDropItemData(items, amounts);
//             newDropItem.GetComponent<NetworkMatch>().matchId = PlayerOutlander.Player.PlayerMatchManager.MatchID.ToGuid();

//             NetworkServer.Spawn(newDropItem);
//         }
//         #endregion
//     }

// }
