//using System.Collections;
//using System.Collections.Generic;
//using Outlander.Player;
//using UnityEngine;
//using Mirror;
//using System.Linq;
//using UnityEngine.Animations.Rigging;

//public class AnimationScript : PlayerElements
//{
//    [Header("Reference")]

//    private RigLayer axeRig, lanceRig;
//    private Rig rig;
//    private bool action, inUse;
//    // private string Player.Animatoration;
//    AnimatorControllerParameter[] parameters;

//    //private void Awake()
//    //{
//    //    parameters = Player.Animator.parameters.Where(par => !Player.Animator.IsParameterControlledByCurve(par.nameHash)).ToArray();
//    //    for(int i = 0; i < parameters.Length; i++)
//    //    {
//    //        AnimatorControllerParameter par = parameters[i];
//    //        Debug.Log($"type : {par.type} | hash : {par.nameHash} | name : {par.name}");
//    //    }
//    //}

//    void Start()
//    {
//        if (!isLocalPlayer) return;
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        if (!isLocalPlayer) return;
//        AnimationState();
//    }
//    public void GetWeaponType()
//    {
//        if (!isClient) return;
//        if (lanceRig == null || axeRig == null)
//        {
//            axeRig = Player.RigBuilder.layers[0];
//            lanceRig = Player.RigBuilder.layers[1];
//        }

//        if (Player.WeaponManager.currentWeaponType == WeaponManager.WeaponType.Lance)
//        {
//            axeRig.active = false;
//            lanceRig.active = true;

//            rig = lanceRig.rig;
//        }
//        else if (Player.WeaponManager.currentWeaponType == WeaponManager.WeaponType.Axe)
//        {
//            axeRig.active = true;
//            lanceRig.active = false;

//            rig = axeRig.rig;
//        }
//        else
//        {
//            lanceRig.active = false;
//            axeRig.active = false;
//        }
//    }
//    void AnimationState()
//    {
//        Player.Animator.SetBool("climbing", Player.PlayerMovement.climbing);
//        Player.Animator.SetBool("OnGround", Player.PlayerMovement.grounded);
//        Player.Animator.SetBool("onCrouch", Player.PlayerMovement.onCrouch);
//        Player.Animator.SetBool("standUp", !Player.PlayerMovement.sitting);
//        Player.Animator.SetBool("swimming", Player.PlayerMovement.swimming);
//        Player.Animator.SetBool("stunned", Player.PlayerMovement.stunned);

//        Player.Animator.SetBool("weaponaction", Player.PlayerOutlander.GetWeaponAction());

//        if (Player.PlayerMovement.stunned)
//        {
//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.CrossFade("Stunned", 0.1f);
//            }
//        }

//        else if (Player.PlayerMovement.drowned)
//        {
//            Player.Animator.SetBool("swimMove", false);

//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.CrossFade("Drown", 0.1f);
//            }
//            Player.PlayerMovement.drowned = false;
//        }
//        else if (Player.PlayerMovement.sitting)
//        {
//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.CrossFade("SitDown", 0.2f);
//            }
//        }
//        else if (Player.PlayerMovement.wallLerping)
//        {
//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.CrossFade("Idle", 0.2f);
//            }
//        }
//        else if (!Player.PlayerMovement.climbing && !Player.PlayerMovement.swimming && !Player.PlayerMovement.onJump && !Player.PlayerMovement.highground && !Player.PlayerMovement.restricted)
//        {
//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.CrossFade("floating", 0.1f);
//            }

//        }
//        else if (Player.PlayerMovement.isMoving)
//        {
//            Player.Animator.speed = 1;

//            Player.Animator.SetBool("moving", true);

//            if (Player.PlayerMovement.topCheck)
//            {
//                Player.Animator.speed = 1;

//                Player.Animator.SetBool("climbtoTop", true);
//            }
//            else if (Player.PlayerMovement.climbing)
//            {

//                Player.Animator.SetBool("climbtoTop", false);
//                if (Player.PlayerMovement.onClimbJump && Player.PlayerMovement.restricted)
//                {
//                    if (Player.PlayerMovement.movementInput.x == 0)
//                    {
//                        if (!Player.Animator.IsInTransition(0))
//                        {
//                            if (Player.PlayerMovement.movementInput.y > 0) Player.Animator.CrossFade("ClimbJumpTop", 0.1f);
//                        }
//                    }
//                    else
//                    {
//                        if (!Player.Animator.IsInTransition(0))
//                        {
//                            if (Player.PlayerMovement.movementInput.x > 0)
//                                Player.Animator.CrossFade("ClimbJumpRight", 0.1f);
//                            else
//                                Player.Animator.CrossFade("ClimbJumpLeft", 0.1f);
//                        }
//                    }
//                }
//            }

//            else if (Player.PlayerMovement.dashing)
//            {
//                SetRigWeight(0);
//                if (!Player.Animator.IsInTransition(0))
//                {
//                    Player.Animator.CrossFade("Dodge", .1f);
//                }
//            }

//            else if (Player.PlayerMovement.swimming)
//            {
//                SetRigWeight(0);

//                Player.Animator.SetBool("swimming", true);
//                Player.Animator.SetBool("swimMove", true);
//                if (Player.PlayerMovement.isSprint)
//                    Player.Animator.speed += 1f;
//            }
//            else if (!Player.PlayerMovement.swimming)
//            {
//                Player.Animator.SetBool("swimMove", false);

//                Player.Animator.SetBool("swimming", false);

//            }

//            if (Player.PlayerMovement.cameraStyle == Outlander.Player.PlayerCamera.CameraStyle.Combat || Player.PlayerMovement.climbing)
//            {
//                Player.Animator.SetFloat("moveX", Player.PlayerMovement.movementInput.x);
//                Player.Animator.SetFloat("moveZ", Player.PlayerMovement.movementInput.y);
//            }
//            else
//            {
//                Player.Animator.SetFloat("moveX", 0);

//                if (Player.PlayerMovement.movementInput.y != 0)
//                {
//                    Player.Animator.SetFloat("moveZ", 1);
//                }
//                else if (Player.PlayerMovement.movementInput.x != 0)
//                {
//                    Player.Animator.SetFloat("moveZ", 1);
//                }
//            }
//        }

//        else if (!Player.PlayerMovement.isMoving)
//        {
//            Player.Animator.speed = 1;

//            Player.Animator.SetBool("moving", false);

//            if (Player.PlayerMovement.climbing)
//            {
//                SetRigWeight(0);
//                if (Player.PlayerMovement.jumpFromWall)
//                {
//                    Player.Animator.speed = 1;
//                    if (!Player.Animator.IsInTransition(0))
//                    {
//                        Player.Animator.CrossFade("JumpFromWall", 0.1f);
//                    }
//                }
//                else if (Player.PlayerMovement.topCheck) Player.Animator.speed = 1;

//                else Player.Animator.speed = 0;

//            }

//            else if (Player.PlayerMovement.swimming)
//            {
//                SetRigWeight(0);
//                Player.Animator.SetBool("swimMove", false);
//            }

//            else if (Player.PlayerMovement.dashing)
//            {
//                SetRigWeight(0);
//                if (!Player.Animator.IsInTransition(0))
//                {
//                    Player.Animator.CrossFade("Dodge", .1f);
//                }
//            }
//        }

//        if (Player.PlayerMovement.onJump && !Player.PlayerMovement.climbing && !Player.PlayerMovement.readyToJump)
//        {
//            SetRigWeight(0);

//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.CrossFade("jump", 0.01f);
//            }
//        }

//        if (Player.PlayerMovement.grounded)
//        {
//            Player.Animator.SetBool("climbtoTop", false);
//        }

//        if (Player.PlayerMovement.useConsumable)
//        {
//            SetRigWeight(0);

//            if (!Player.Animator.IsInTransition(0))
//            {
//                Player.Animator.Play("Drinking");
//            }
//        }
//        if (Player.PlayerOutlander.GetWeaponAction() && !Player.PlayerOutlander.IsAiming && Player.PlayerMovement.grounded && !Player.PlayerMovement.dashing)
//        {
//            if (PlayerManagers.Instance.matchManager.canInteract)
//                Player.PlayerMovement.Stamina -= 0.05f;

//            if (action == false)
//            {
//                if (!Player.Animator.IsInTransition(0))
//                {
//                    Player.Animator.CrossFade("weapon_action", 0.1f);
//                    action = true;
//                }
//            }
//        }
//        else
//        {
//            if (action)
//                action = false;
//        }
//    }

   
//    [Command]
//    private void CmdSetRigWeight(float weight) => RpcSetRigWeight(weight);

//    [ClientRpc]
//    public void RpcSetRigWeight(float weight)
//    {
//        if (!rig) return;
//        StartCoroutine(SetWeight(weight));
//    }

//    private IEnumerator SetWeight(float weight)
//    {
//        yield return new WaitForEndOfFrame();
//        rig.weight = weight;
//    }
//    public void SetRigWeight(float weight)
//    {
//        if (!rig) return;
//        CmdSetRigWeight(weight);
//        StartCoroutine(SetWeight(weight));
//    }
//    void MoveFromAnim(AnimationEvent myEvent)
//    {
//        if (myEvent != null && !string.IsNullOrEmpty(myEvent.stringParameter))
//            StartCoroutine(AddPlayerForce(myEvent.intParameter, myEvent.stringParameter, myEvent.floatParameter));
//    }
//    void ResetHit()
//    {
//        Player.Animator.SetBool("isHit", false);
//    }
//    IEnumerator AddPlayerForce(int force, string dir, float sec)
//    {
//        float startTime = Time.time;
//        if (dir == "up")
//        {
//            while (Time.time - startTime < sec)
//            {
//                Player.Rigidbody.AddForce(transform.up * force, ForceMode.Acceleration);

//                yield return null;
//            }
//        }
//        else if (dir == "down")
//        {
//            while (Time.time - startTime < sec)
//            {
//                Player.Rigidbody.AddForce(-transform.up * force, ForceMode.Acceleration);
//                yield return null;

//            }
//        }
//        else if (dir == "right")
//        {
//            while (Time.time - startTime < sec)
//            {
//                transform.GetComponent<Rigidbody>().AddForce(transform.right * force, ForceMode.Acceleration);
//                yield return null;
//            }
//        }
//        else if (dir == "left")
//        {
//            while (Time.time - startTime < sec)
//            {
//                Player.Rigidbody.AddForce(-transform.right * force, ForceMode.Acceleration);
//                yield return null;
//            }
//        }
//        else if (dir == "forward")
//        {
//            while (Time.time - startTime < sec)
//            {
//                Player.Rigidbody.AddForce(transform.forward * force, ForceMode.Acceleration);
//                yield return null;
//            }
//        }
//        else if (dir == "backward")
//        {
//            while (Time.time - startTime < sec)
//            {
//                Player.Rigidbody.AddForce(-transform.forward * force, ForceMode.Acceleration);
//                yield return null;
//            }
//        }
//        else if (dir == "stop")
//        {
//            while (Time.time - startTime < sec)
//            {
//                if (!Player.PlayerMovement.restricted)
//                {
//                    Player.PlayerMovement.restricted = true;
//                }
//                yield return null;
//            }
//            Player.PlayerMovement.restricted = false;
//        }
//    }
//}
