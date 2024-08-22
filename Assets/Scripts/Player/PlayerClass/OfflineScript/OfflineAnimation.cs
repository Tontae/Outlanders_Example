using System.Collections;
using System.Collections.Generic;
using Outlander.Player;
using UnityEngine;

public class OfflineAnimation : MonoBehaviour
{
    [Header("Reference")]
    private Animator anim;
    private OfflineMovement playermovement;
    // private string animation;
    public GameObject playerObj;

    void Start()
    {
        anim = playerObj.GetComponent<Animator>();

        playermovement = GetComponent<OfflineMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimationState();
    }

    void AnimationState()
    {
        anim.SetBool("climbing", playermovement.climbing);
        anim.SetBool("OnGround", playermovement.grounded);
        anim.SetBool("standUp", !playermovement.sitting);

        if (playermovement.drowned)
        {
            if (!anim.IsInTransition(0))
            {
                anim.CrossFade("Drown", 0.1f);
            }
            playermovement.drowned = false;
        }
        else if (playermovement.sitting)
        {
            if (!anim.IsInTransition(0))
            {
                anim.CrossFade("SitDown", 0.2f);
            }
        }
        else if (playermovement.wallLerping)
        {
            if (!anim.IsInTransition(0))
            {
                anim.CrossFade("Idle", 0.2f);
            }
        }
        else if (!playermovement.climbing && !playermovement.swimming && !playermovement.onJump && !playermovement.highground && !playermovement.restricted)
        {
            if (!anim.IsInTransition(0))
            {
                anim.CrossFade("floating", 0.1f);
            }

        }
        else if (playermovement.isMoving)
        {
            anim.speed = 1;

            anim.SetBool("moving", true);
            anim.SetFloat("moveZ", playermovement.movementInput.y);

            if (playermovement.topCheck)
            {
                anim.speed = 1;

                anim.SetBool("climbtoTop", true);
            }
            else if (playermovement.climbing)
            {

                anim.SetBool("climbtoTop", false);
                if (playermovement.onClimbJump)
                {
                    if (playermovement.movementInput.x == 0)
                    {
                        if (!anim.IsInTransition(0))
                        {
                            if (playermovement.movementInput.y > 0) anim.CrossFade("ClimbJumpTop", 0.1f);
                        }
                    }
                    else
                    {
                        if (!anim.IsInTransition(0))
                        {
                            if (playermovement.movementInput.x > 0)
                                anim.CrossFade("ClimbJumpRight", 0.1f);
                            else
                                anim.CrossFade("ClimbJumpLeft", 0.1f);
                        }

                    }
                }
            }

            else if (playermovement.dashing)
            {
                anim.Play("Dodge");
            }

            else if (playermovement.swimming)
            {
                anim.SetBool("swimming", true);
                anim.SetBool("swimMove", true);
                if (playermovement.isSprint)
                    anim.speed += 1f;
            }
            else if (!playermovement.swimming)
            {
                anim.SetBool("swimMove", false);

                anim.SetBool("swimming", false);

            }



            if (playermovement.cameraStyle == PlayerCamera.CameraStyle.Combat.ToString() || playermovement.climbing)
            {
                anim.SetFloat("moveX", playermovement.movementInput.x);
            }
            else
            {
                anim.SetFloat("moveX", 0);

                if (playermovement.movementInput.y <= 0)
                {
                    anim.SetFloat("moveZ", Mathf.Abs(playermovement.movementInput.x));
                }
            }
        }

        else if (!playermovement.isMoving)
        {
            anim.speed = 1;

            anim.SetBool("moving", false);

            if (playermovement.climbing)
            {
                if (playermovement.jumpFromWall)
                {
                    anim.speed = 1;
                    if (!anim.IsInTransition(0))
                    {
                        anim.CrossFade("JumpFromWall", 0.1f);
                    }
                }
                else if (playermovement.topCheck) anim.speed = 1;

                else anim.speed = 0;

            }

            else if (playermovement.swimming)
            {
                anim.SetBool("swimMove", false);
            }
        }

        if (playermovement.onJump && !playermovement.climbing && !playermovement.readyToJump)
        {
            if (!anim.IsInTransition(0))
            {
                anim.CrossFade("jump", 0.01f);
            }
        }

        if (playermovement.grounded)
        {
            anim.SetBool("climbtoTop", false);
        }
    }
}
