using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Outlander.Player
{
    public class PlayerClass : NetworkBehaviour
    {
        // public WeaponManager weaponManager { get; private set; }
        // // public PlayerOutlander playerOutlander { get; private set; }
        // // public PlayerSwordman playerSwordman { get; private set; }
        // // public PlayerArcher playerArcher { get; private set; }

        // // public delegate void OnPlayerClass();
        // // public OnPlayerClass onPlayerClass;

        // [SerializeField] private List<GameObject> playerClass = new List<GameObject>();

        // private WeaponManager.WeaponType is_classChanged;

        // private void Start()
        // {
        //     weaponManager = GetComponent<WeaponManager>();
        //     playerClass[0].SetActive(true);
        //     playerClass[0].GetComponent<Outlander.Player.PlayerBareHand>().enabled = true;
        //     //ClassChange();
        //     // playerOutlander = GetComponent<PlayerOutlander>();


        //     // playerSwordman = new PlayerSwordman();
        //     // playerArcher = new PlayerArcher();
        // }

        // private void Update()
        // {
        //     if (isClient || isServer)
        //         ClassChange();
        // }

        // private void ClassChange()
        // {
        //     if (is_classChanged == weaponManager.currentWeaponType) return;

        //     bool _active = true;

        //     switch (is_classChanged)
        //     {
        //         case WeaponManager.WeaponType.None:
        //             playerClass[0].SetActive(!_active);
        //             playerClass[0].GetComponent<PlayerBareHand>().enabled = false;
        //             break;
        //         case WeaponManager.WeaponType.Sword:
        //             playerClass[1].SetActive(!_active);
        //             playerClass[1].GetComponent<PlayerSwordman>().enabled = false;
        //             break;
        //         case WeaponManager.WeaponType.BowQuiver:
        //             playerClass[2].SetActive(!_active);
        //             playerClass[2].GetComponent<PlayerArcher>().enabled = false;
        //             break;
        //         default:
        //             playerClass[0].SetActive(!_active);
        //             playerClass[0].GetComponent<PlayerBareHand>().enabled = false;
        //             break;
        //     }

        //     switch (weaponManager.currentWeaponType)
        //     {
        //         case WeaponManager.WeaponType.None:
        //             playerClass[0].SetActive(_active);
        //             playerClass[0].GetComponent<PlayerBareHand>().enabled = true;
        //             is_classChanged = WeaponManager.WeaponType.None;
        //             break;
        //         case WeaponManager.WeaponType.Sword:
        //             playerClass[1].SetActive(_active);
        //             playerClass[1].GetComponent<PlayerSwordman>().enabled = true;
        //             is_classChanged = WeaponManager.WeaponType.Sword;
        //             break;
        //         case WeaponManager.WeaponType.BowQuiver:
        //             playerClass[2].SetActive(_active);
        //             playerClass[2].GetComponent<PlayerArcher>().enabled = true;
        //             is_classChanged = WeaponManager.WeaponType.BowQuiver;
        //             break;
        //         default:
        //             playerClass[0].SetActive(_active);
        //             playerClass[0].GetComponent<PlayerBareHand>().enabled = true;
        //             is_classChanged = WeaponManager.WeaponType.None;
        //             break;
        //     }
        // }
    }
}

