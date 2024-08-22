using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FPSchecker : MonoBehaviour
{
    [SerializeField]
    Text text;

    float deltatime;
    // Update is called once per frame
    void Update()
    {
        //deltatime += (Time.deltaTime - deltatime) + .01f;
        //float fps = 1.0f / deltatime;
        //text.text = Mathf.Ceil(fps).ToString();
    }
}
