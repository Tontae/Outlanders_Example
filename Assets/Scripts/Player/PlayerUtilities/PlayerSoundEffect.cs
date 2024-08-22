using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerSoundEffect : MonoBehaviour
    {
        private PlayerAnimationEvent playerAnimationEvent;

        [SerializeField] public AudioClip[] SwordSound;
        [SerializeField] public AudioClip[] BowSound;
        // [SerializeField] public AudioClip[] StaffSound;
        [SerializeField] public AudioClip[] MoveSound;
        [SerializeField] public AudioClip[] SkillSound;
        [SerializeField] public AudioSource PlayerSound;
        [SerializeField] public AudioSource AttackSound;
        private Animator anim;

        private void OnEnable()
        {
            playerAnimationEvent = GetComponent<PlayerAnimationEvent>();
        }

        private void OnDisable()
        {

        }

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void PlayerAttackSound(int Sound)
        {
            AttackSound.clip = SwordSound[Sound];
            AttackSound.Play();
        }

        private void PlayerMoveSound(int Sound)
        {
            PlayerSound.clip = MoveSound[Sound];
            PlayerSound.Play();
        }

        //X+Z+
        private void walkingXZ()
        {
            if (anim.GetFloat("moveX") > 0f && anim.GetFloat("moveZ") > 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X+Z0
        private void walkingX0()
        {
            if (anim.GetFloat("moveX") > 0f && anim.GetFloat("moveZ") == 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X+Z-
        private void walkingXz()
        {
            if (anim.GetFloat("moveX") > 0f && anim.GetFloat("moveZ") < 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X0Z+
        private void walking0Z()
        {
            if (anim.GetFloat("moveX") == 0f && anim.GetFloat("moveZ") > 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X0Z-
        private void walking0z()
        {
            if (anim.GetFloat("moveX") == 0f && anim.GetFloat("moveZ") < 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X-Z+
        private void walkingxZ()
        {
            if (anim.GetFloat("moveX") < 0f && anim.GetFloat("moveZ") > 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X-Z0
        private void walkingx0()
        {
            if (anim.GetFloat("moveX") < 0f && anim.GetFloat("moveZ") == 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        //X-Z-
        private void walkingxz()
        {
            if (anim.GetFloat("moveX") < 0f && anim.GetFloat("moveZ") < 0)
            {
                PlayerSound.clip = MoveSound[3];
                PlayerSound.Play();
            }
        }
        private void PlayerSkillSound(int Sound)
        {
            AttackSound.clip = SkillSound[Sound];
            AttackSound.Play();
        }



    }

}
