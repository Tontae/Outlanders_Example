using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIAppearance : MonoBehaviour
{
    [Header("UI Header")]
    [SerializeField] public Toggle[]  toggles;
    private Dictionary<Toggle, GameObject> toggleDetails = new Dictionary<Toggle, GameObject>();

    [Header("UI Detail")]
    [SerializeField] public List<GameObject> detailPanels;

    [Header("UI Color")]
    [SerializeField] public GameObject skinColor;
    [SerializeField] public GameObject hairColor;
    [SerializeField] public GameObject eyesColor;

    private void Awake()
    {
        toggles = GetComponentsInChildren<Toggle>();
        for(int i = 0;i<toggles.Length;i++)
        {
            toggleDetails.Add(toggles[i], detailPanels[i]);
        }
    }

    private void Update()
    {
        foreach(Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                toggleDetails[toggle].SetActive(true);
                if (toggle.transform.GetChild(0).transform.localPosition.x < 100)
                {
                    toggle.transform.GetChild(0).transform.localPosition = new Vector3(toggle.transform.GetChild(0).transform.localPosition.x + (Time.deltaTime * 500f), toggle.transform.GetChild(0).transform.localPosition.y, toggle.transform.GetChild(0).transform.localPosition.z);
                    if(toggle.transform.GetChild(0).transform.localPosition.x >= 100)
                    {
                        return;
                    }
                }
            }
            else
            {
                toggleDetails[toggle].SetActive(false);
                if (toggle.transform.GetChild(0).transform.localPosition.x > 0)
                {
                    toggle.transform.GetChild(0).transform.localPosition = new Vector3(toggle.transform.GetChild(0).transform.localPosition.x - (Time.deltaTime * 500f), toggle.transform.GetChild(0).transform.localPosition.y, toggle.transform.GetChild(0).transform.localPosition.z);
                    if (toggle.transform.GetChild(0).transform.localPosition.x <= 0)
                    {
                        return;
                    }
                }
            }
        }

        if (detailPanels[0].activeSelf)
        {
            skinColor.SetActive(true);
            hairColor.SetActive(false);
            eyesColor.SetActive(false);
        }

        if (detailPanels[1].activeSelf)
        {
            skinColor.SetActive(false);
            hairColor.SetActive(false);
            eyesColor.SetActive(true);
        }

        if (detailPanels[2].activeSelf || detailPanels[3].activeSelf)
        {
            skinColor.SetActive(false);
            hairColor.SetActive(true);
            eyesColor.SetActive(false);
        }
    }

    public void SetDefault()
    {
        toggles[0].isOn = true;
    }
}
