using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMinimap : MonoBehaviour
{
    public Transform player;
    public Camera MinimapCamera;

    void LateUpdate()
    {
        MinimapCamera.gameObject.transform.rotation = Quaternion.Euler(90f, -player.rotation.y, 0f);
    }
}
