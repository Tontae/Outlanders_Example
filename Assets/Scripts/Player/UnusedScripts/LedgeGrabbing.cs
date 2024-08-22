// using System.Collections;
// using System.Collections.Generic;
// using Outlander.Player;
// using UnityEngine;

// public class LedgeGrabbing : MonoBehaviour
// {
//     [Header("References")]
//     public PlayerMovement pm;
//     public Transform orientation;
//     public Transform cam;
//     public Rigidbody rb;

//     [Header("Ledge Grabbing")]

//     public float moveToLedgeSpeed;
//     public float maxLedgeGrabDistance;

//     public float minTimeOnLedge;
//     private float timeOnLedge;

//     public bool holding;

//     [Header("Ledge Jumping")]
//     public KeyCode jumpKey = KeyCode.Space;
//     public float ledgeJumpForwardForce;
//     public float ledgeJumpUpwardForce;

//     [Header("Ledge Detection")]
//     public float ledgeDetectionLength;
//     public float ledgeSphereCastRadius;
//     public LayerMask whatIsLedge;

//     private Transform lastLedge;
//     public Transform currentLedge;

//     private RaycastHit ledgeHit;

//     [Header("Exiting")]
//     public bool exitingLedge;
//     public float exitLedgeTime;
//     private float exitLedgeTimer;

//     private void Update()
//     {
//         LedgeDetection();
//         SubState();
//     }

//     private void SubState()
//     {
//         bool anyInputKeyPress = pm.MovementInput.x != 0 || pm.MovementInput.y != 0;

//         if (holding)
//         {
//             FreezeRigidOnLedge();

//             timeOnLedge += Time.deltaTime;

//             if (timeOnLedge > minTimeOnLedge && anyInputKeyPress)
//             {
//                 ExitLedgeHold();
//             }

//             if (Input.GetKeyDown(jumpKey)) LedgeJump();

//         }
//         else if (exitingLedge)
//         {
//             if (exitLedgeTimer > 0) exitLedgeTimer -= Time.deltaTime;
//             else exitingLedge = false;
//         }
//     }

//     private void LedgeDetection()
//     {
//         bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);
//         Debug.DrawRay(transform.position, cam.forward, Color.black);

//         if (!ledgeDetected) return;

//         float distanceToledge = Vector3.Distance(transform.position, ledgeHit.transform.position);

//         if (ledgeHit.transform == lastLedge) return;

//         if (distanceToledge < maxLedgeGrabDistance && !holding) EnterLedgeHold();

//     }

//     private void LedgeJump()
//     {
//         ExitLedgeHold();

//         Invoke(nameof(DelayedJumpForce), 0.05f);
//     }

//     private void DelayedJumpForce()
//     {
//         Vector3 forceToAdd = cam.forward * ledgeJumpForwardForce + orientation.up * ledgeJumpForwardForce;
//         rb.velocity = Vector3.zero;
//         rb.AddForce(forceToAdd, ForceMode.Impulse);
//     }

//     private void EnterLedgeHold()
//     {

//         holding = true;

//         pm.unlimited = true;
//         pm.restricted = true;

//         currentLedge = ledgeHit.transform;
//         lastLedge = ledgeHit.transform;

//         rb.useGravity = false;
//         rb.velocity = Vector3.zero;
//     }

//     private void FreezeRigidOnLedge()
//     {
//         rb.useGravity = false;

//         Vector3 directionToLedge = currentLedge.position - transform.position;
//         float distanceToLedge = Vector3.Distance(transform.position, currentLedge.position);

//         if (distanceToLedge > 1f)
//         {
//             if (rb.velocity.magnitude < moveToLedgeSpeed)
//             {
//                 rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
//             }
//         }
//         else
//         {
//             if (!(pm.movementState == PlayerMovement.MovementState.Freeze))
//             {
//                 pm.freeze = true;
//             }
//             if (pm.unlimited) pm.unlimited = false;
//         }

//         if (distanceToLedge > maxLedgeGrabDistance)
//         {
//             ExitLedgeHold();
//         }
//     }

//     private void ExitLedgeHold()
//     {

//         exitingLedge = true;
//         exitLedgeTimer = exitLedgeTime;

//         holding = false;
//         timeOnLedge = 0f;

//         pm.restricted = false;
//         pm.freeze = false;

//         rb.useGravity = true;

//         StopAllCoroutines();
//         Invoke(nameof(ResetLastLedge), 1f);
//     }

//     private void ResetLastLedge()
//     {
//         lastLedge = null;
//     }

// }
