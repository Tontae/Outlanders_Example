// using System.Collections;
// using System.Collections.Generic;
// using Outlander.Player;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class Climbing : MonoBehaviour
// {
//     [Header("References")]
//     public Transform orientation;
//     public Rigidbody rb;
//     public LayerMask whatIsWall;
//     public PlayerMovement Pm;

//     [Header("Climbimg")]
//     public float climbSpeed;
//     public float maxClimbTime;
//     private float climbTimer;

//     private bool climbing = false;

//     [Header("Detection")]
//     public float detectionLength;
//     public float sphereCastRadius;
//     public float maxWallLookAngle;
//     private float wallLookAngle;


//     private RaycastHit frontwallHit;
//     private bool wallFront;

//     [Header("input system")]
//     public static bool ENABLE_INPUT_SYSTEM = true;

//     private void Update()
//     {
//         WallCheck();
//         StateMachine();
//         if (climbing) ClimbingMovement();
//     }

//     //private void StateMachine()
//     //{
//     //    if(wallFront && Input)
//     //}
//     private void WallCheck()
//     {
//         wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontwallHit, detectionLength, whatIsWall);
//         wallLookAngle = Vector3.Angle(orientation.forward, -frontwallHit.normal);

//         if (Pm.grounded)
//         {
//             climbTimer = maxClimbTime;
//         }
//     }

//     private void StartClimbing()
//     {
//         climbing = true;
//         Pm.climbing = true;

//         //Camera FOV
//     }

//     private void ClimbingMovement()
//     {
//         rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
//     }

//     private void StopClimbing()
//     {
//         climbing = false;
//         Pm.climbing = false;
//     }


//     private void StateMachine()
//     {


//         if (wallFront && Pm.MovementInput.y > 0 && wallLookAngle < maxWallLookAngle)
//         {
//             if (!climbing && climbTimer > 0) StartClimbing();
//             //climbingTimer
//             if (climbTimer > 0) climbTimer -= Time.deltaTime;
//             if (climbTimer < 0) StopClimbing();

//         }
//         else
//         {
//             if (climbing)
//             {
//                 StopClimbing();
//             }
//         }


//     }
//     //Climbing code (8/7) https://pastebin.com/gZH4QWmP
// }
