using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager singleton;

    private void Awake()
    {
        if(singleton != null)
        {
            Destroy(this);
        }
        singleton = this;
        DontDestroyOnLoad(this);
    }

    public void OnclientSelectSpawnPosition(Vector3 position)
    {
        PlayerManagers.Instance.PlayerComponents.CharacterController.enabled = false;
        PlayerManagers.Instance.PlayerGO.transform.position = position + new Vector3(0,2,0);
        PlayerManagers.Instance.PlayerComponents.CharacterController.enabled = true;
    }
}
