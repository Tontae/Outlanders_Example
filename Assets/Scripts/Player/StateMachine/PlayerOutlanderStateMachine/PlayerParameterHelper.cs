using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerParameterHelper : MonoBehaviour
    {
        public PlayerOutlanderStateMachine PlayerOutlander { get; }
        public PlayerParameterHelper(PlayerOutlanderStateMachine _PlayerOutlander)
        {
            PlayerOutlander = _PlayerOutlander;
        }

        public float SetParameter(PlayerAttributeFloatType _type, float _amount)
        {
            switch (_type)
            {
                case PlayerAttributeFloatType.HP:
                    return PlayerOutlander.PlayerHP += _amount;
                case PlayerAttributeFloatType.MP:
                    return PlayerOutlander.PlayerMP += _amount;
                case PlayerAttributeFloatType.MAXHP:
                    return PlayerOutlander.PlayerMaxHP += _amount;
                case PlayerAttributeFloatType.MAXMP:
                    return PlayerOutlander.PlayerMaxMP += _amount;
                case PlayerAttributeFloatType.ATK:
                    return PlayerOutlander.PlayerAtkDmg += _amount;
                case PlayerAttributeFloatType.DEF:
                    return PlayerOutlander.PlayerDef += _amount;
                case PlayerAttributeFloatType.CRITICAL_RATE:
                    return PlayerOutlander.PlayerCritRate += _amount;
                case PlayerAttributeFloatType.CRITICAL_DAMAGE:
                    return PlayerOutlander.PlayerCritDmg += _amount;

                default:
                    return 0;
            }
        }

        public bool GetParameter(PlayerAttributeBoolType _type)
        {
            switch (_type)
            {
                case PlayerAttributeBoolType.DIE:
                    return PlayerOutlander.OnDie;
                case PlayerAttributeBoolType.NORMAL_ATTACK:
                    return PlayerOutlander.OnFireInput;
                case PlayerAttributeBoolType.WEAPON_ACTION:
                    return PlayerOutlander.OnWeaponAction;

                default:
                    return false;
            }
        }

        public void SetParameter<T>(T _param, T _newParam)
        {
            _param = _newParam;
        }
    }
}

