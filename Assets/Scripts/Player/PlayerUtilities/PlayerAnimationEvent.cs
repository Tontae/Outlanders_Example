using System;
using UnityEngine;
// using Mirror;

namespace Outlander.Player
{
    public class PlayerAnimationEvent : MonoBehaviour
    {
        // public int onAttackHitMultiplier;
        public event Action OnSkill;
        public event Action OnResetStage;
        public event Action OnEndSkill;
        public event Action OnAttackHit;
        public event Action OnComboPossible;
        public event Action OnCombo;
        public event Action OnComboReset;
        public event Action OnIdle;
        public event Action OnAttackSound;
        public event Action<float> OnShooting;
        // public event Action<bool> OnResetAnimState;

        public void OnPlayerSkill()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerSkill");
            // #endif
            OnSkill?.Invoke();
        }

        public void OnPlayerResetStage()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerResetStage");
            // #endif
            OnResetStage?.Invoke();
        }

        public void OnPlayerEndSkill()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerEndSkill");
            // #endif
            OnEndSkill?.Invoke();
        }

        public void OnPlayerAttackHit()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerAttackHit");
            // #endif
            OnAttackHit?.Invoke(/*onAttackHitMultiplier*/);
        }

        public void OnPlayerComboPossible()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerComboPossible");
            // #endif
            OnComboPossible?.Invoke();
        }

        public void OnPlayerCombo()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerCombo");
            // #endif
            OnCombo?.Invoke();
        }

        public void OnPlayerComboReset()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerComboReset");
            // #endif
            OnComboReset?.Invoke();
        }

        public void OnPlayerIdle()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerIdle");
            // #endif
            OnIdle?.Invoke();
        }

        public void OnPlayerAttackSound()
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerAttackSound");
            // #endif
            OnAttackSound?.Invoke();
        }

        public void OnPlayerShooting(float _speed)
        {
            // #if UNITY_EDITOR
            //             Debug.Log("OnPlayerShooting");
            // #endif
            OnShooting?.Invoke(_speed);
        }

        // public void OnPlayerResetAnimState(bool _args)
        // {
        //     // #if UNITY_EDITOR
        //     //             Debug.Log("OnPlayerResetAnimState");
        //     // #endif
        //     OnResetAnimState?.Invoke(_args);
        // }
    }
}
