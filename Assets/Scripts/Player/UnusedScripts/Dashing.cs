// using System.Collections;
// using System.Collections.Generic;
// using Outlander.Player;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class Dashing : MonoBehaviour
// {
//     // Start is called before the first frame update
//     [Header("Refferences")]
//     public Transform orientation;//Use Player Obj
//     public Transform playerCam;
//     private Rigidbody rb;
//     private PlayerMovement pm;

//     [Header("Dashing")]
//     public float dashForce;
//     public float dashUpwardForce;
//     public float dashDuration;
//     public bool onDashing;
//     private Vector3 delayedForceToApple;

//     [Header("Cooldown")]
//     public float dashCd;
//     private float dashCdTimer;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         pm = GetComponent<PlayerMovement>();
//     }

//     void Update()
//     {
//         if (dashCdTimer > 0)
//         {
//             Debug.Log("On Cooldown");
//             dashCdTimer -= Time.deltaTime;
//         }
//     }

//     private void Dash()
//     {
//         if (dashCdTimer > 0)
//         {
//             return;
//         }

//         else
//         {
//             Debug.Log("Do this f*cking condition!");
//             dashCdTimer = dashCd;
//         }

//         onDashing = true;
//         pm.dashing = true;

//         Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

//         delayedForceToApple = forceToApply;

//         Invoke(nameof(DelayedDashForce), 0.025f);

//         Invoke(nameof(ResetDash), dashDuration);
//     }


//     private void DelayedDashForce()
//     {
//         rb.AddForce(delayedForceToApple, ForceMode.Impulse);
//     }


//     private void ResetDash()
//     {
//         Debug.Log("Reset");
//         pm.dashing = false;
//         onDashing = false;

//     }
//     // Update is called once per frame


//     public void OnDodge(InputValue value)
//     {
//         Dash();
//     }
// }
