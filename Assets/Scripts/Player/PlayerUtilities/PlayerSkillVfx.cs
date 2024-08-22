// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Outlander.Player
// {
//public class PlayerSkillVfx : MonoBehaviour
//{
//    [SerializeField] private GameObject[] vfxs;
//    private PlayerOutlander player;

//    private void Awake()
//    {
//        player = GetComponent<PlayerOutlander>();
//    }

//    public void CreateVfx(int index)
//    {
//        GameObject _vfx = Instantiate(vfxs[index], transform.position, transform.rotation);
//        if (_vfx.GetComponent<PlayerSkill>())
//        {
//            _vfx.GetComponent<PlayerSkill>().player = player;
//            _vfx.GetComponent<PlayerSkill>().playerObject = this.gameObject;
//            _vfx.GetComponent<PlayerSkill>().playerAttackPoint = player.AttackPoint1;
//            _vfx.GetComponent<PlayerSkill>().damagePopUpMons = player.damagePopUpMons;
//        }
//        Destroy(_vfx, 1.5f);
//    }
//}

// }

