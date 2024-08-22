using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;


public class UIMap : UIElements
{
    [Header("MAP")]
    public RectTransform map2dEnd;
    public Transform map3dParent;
    public Transform map3dEnd;
    private Vector3 normalized, mapped;
    public Canvas mapCanvas;

    [Header("2D Icon")]
    public RectTransform player2D;
    public RectTransform boss2D;
    public List<RectTransform> miniBoss2D;
    public RectTransform secretShop2D;
    public RectTransform redZone2D;
    public RectTransform mapRedZone2D;
    public RectTransform redZoneInner2D;

    [Header("Red Zone")]
    private GameObject redZoneGo;
    private RedZone redZoneScript;

    [Header("PositionEvent")]
    private Vector3 secretShop;
    private GameObject boss;
    private List<GameObject> miniBoss;

    [Header("Transition")]
    public bool fadeIn;
    public bool fadeOut;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        ClientTriggerEventManager.OnRedZoneAppear += StoreRedZone;
        ClientTriggerEventManager.OnSecretShopAppear += StoreSecretShop;
        ClientTriggerEventManager.OnSecretShopDisappear += RemoveSecretShop;
        ClientTriggerEventManager.OnBossSpawn += BossOnMap;
        ClientTriggerEventManager.OnMiniBossSpawn += MiniBossOnMap;
    }

    public void InitialIcon()
    {
        mapCanvas.enabled = false;
        miniBoss = new List<GameObject>();
        miniBoss2D.ForEach(x => x.gameObject.SetActive(false));
        boss2D.gameObject.SetActive(false);
        secretShop = Vector3.zero;
        secretShop2D.gameObject.SetActive(false);
        redZone2D.sizeDelta = new Vector2(1000f, 1000f);
        redZoneInner2D.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (fadeIn || fadeOut)
        {
            if (fadeIn)
            {
                if (canvasGroup.alpha < 1f)
                {
                    canvasGroup.alpha += Time.deltaTime * 2f;
                }
                if (canvasGroup.alpha >= 1f)
                {
                    fadeIn = false;
                    fadeOut = false;
                }
            }
            if (fadeOut)
            {
                if (canvasGroup.alpha > 0f)
                {
                    canvasGroup.alpha -= Time.deltaTime * 2f;
                }
                if (canvasGroup.alpha <= 0f)
                {
                    fadeIn = false;
                    fadeOut = false;
                    mapCanvas.enabled = false;
                }
            }
        }

        if(PlayerManagers.Instance.PlayerGO != null)
        {
            normalized = Divide(map3dParent.InverseTransformPoint(PlayerManagers.Instance.PlayerGO.transform.position), map3dEnd.position - map3dParent.position);
            normalized.y = 0;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            player2D.localPosition = mapped;
            player2D.localRotation = Quaternion.Euler(0, 0, 360 - PlayerManagers.Instance.PlayerGO.transform.eulerAngles.y);
        }  

        if (redZoneGo != null)
        {
            normalized = Divide(map3dParent.InverseTransformPoint(redZoneGo.transform.position), map3dEnd.position - map3dParent.position);
            normalized.y = 0;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            redZone2D.localPosition = mapped;

            float redZoneRadius = redZoneScript.capsuleCollider.radius * 2;
            float normalizedX = (redZoneRadius / map3dEnd.localPosition.x) * map2dEnd.localPosition.x;
            //float normalizedZ = (redZoneRadius / map3dEnd.position.z) * map2dEnd.position.y;
            //Debug.Log($"({redZoneRadius} / {map3dEnd.localPosition.x}) * {map2dEnd.localPosition.x} = {normalizedX}");

            redZone2D.sizeDelta = Vector2.one * normalizedX;

            //It's work
            mapRedZone2D.position = transform.position;
        }
        else
        {
            redZone2D.sizeDelta = new Vector2(1000f, 1000f);
            redZoneInner2D.gameObject.SetActive(false);
        }

        if(secretShop != Vector3.zero)
        {
            secretShop2D.gameObject.SetActive(true);
            normalized = Divide(map3dParent.InverseTransformPoint(secretShop), map3dEnd.position - map3dParent.position);
            normalized.y = 0;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            secretShop2D.localPosition = mapped;
        }
        else
        {
            secretShop2D.gameObject.SetActive(false);
        }

        if(boss != null)
        {
            boss2D.gameObject.SetActive(true);
            normalized = Divide(map3dParent.InverseTransformPoint(boss.transform.position), map3dEnd.position - map3dParent.position);
            normalized.y = 0;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            boss2D.localPosition = mapped;
        }
        else
        {
            boss2D.gameObject.SetActive(false);
        }

        if (miniBoss != null)
        {
            if (miniBoss.Count > 0)
            {
                for (int i = 0; i < miniBoss.Count; i++)
                {
                    if (miniBoss[i] != null && miniBoss[i].activeSelf)
                    {
                        miniBoss2D[i].gameObject.SetActive(true);
                        normalized = Divide(map3dParent.InverseTransformPoint(miniBoss[i].transform.position), map3dEnd.position - map3dParent.position);
                        normalized.y = 0;
                        mapped = Multiply(normalized, map2dEnd.localPosition);
                        mapped.z = 0;
                        miniBoss2D[i].localPosition = mapped;
                    }
                    else
                    {
                        miniBoss2D[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    public Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.z * b.y, a.y * b.z);
    }

    private void StoreRedZone(RedZone redzone)
    {
        redZoneGo = redzone.gameObject;
        redZoneScript = redzone;
    }

    private void StoreSecretShop(Vector3 secretShop)
    {
        this.secretShop = secretShop;
    }

    private void RemoveSecretShop()
    {
        this.secretShop = Vector2.zero;
    }

    private void BossOnMap(uint boss)
    {
        this.boss = NetworkClient.spawned[boss].gameObject;
    }

    private void MiniBossOnMap(List<uint> miniBoss)
    {
        List<GameObject> miniBossGO = new List<GameObject>();
        foreach (uint miniBossItem in miniBoss)
            miniBossGO.Add(NetworkClient.spawned[miniBossItem].gameObject);
        this.miniBoss = miniBossGO;
    }

    public void SetRedZoneInnerSize(Vector3 innerPos, float size)
    {
        redZoneInner2D.gameObject.SetActive(true);
        normalized = Divide(map3dParent.InverseTransformPoint(innerPos), map3dEnd.position - map3dParent.position);
        normalized.y = 0;
        mapped = Multiply(normalized, map2dEnd.localPosition);
        mapped.z = 0;
        mapped.x -= 13f;
        mapped.y += 15f;
        redZoneInner2D.localPosition = mapped;

        float redZoneRadius = size * 2;
        float normalizedX = (redZoneRadius / map3dEnd.localPosition.x) * map2dEnd.localPosition.x;
        //float normalizedZ = (redZoneRadius / map3dEnd.position.z) * map2dEnd.position.y;
        //Debug.Log($"({redZoneRadius} / {map3dEnd.localPosition.x}) * {map2dEnd.localPosition.x} = {normalizedX}");

        redZoneInner2D.sizeDelta = Vector2.one * normalizedX;
    }

    public void Show()
    {
        mapCanvas.enabled = true;
        fadeIn = true;
        fadeOut = false;
        CursorManager.Instance.map = true;
    }

    public void Hide()
    {
        fadeIn = false;
        fadeOut = true;
        CursorManager.Instance.map = false;
    }
}
