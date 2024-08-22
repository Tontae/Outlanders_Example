using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITutorial : UIElements
{
    [System.Serializable]
    public class ContentDetail
    {
        public Sprite Image;
        [SerializeField]
        [TextArea(5, 10)]
        public string description;
    }

    [System.Serializable]
    public class TutorialContent
    {
        public string header;
        public List<ContentDetail> contents;
    }

    [Header("UI")]
    [SerializeField] Canvas tutorialCanvas;
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] GameObject selection;
    [SerializeField] Image contentImage;
    [SerializeField] TextMeshProUGUI contentDescription;
    [SerializeField] int currentContentIndex;
    [SerializeField] int currentContentDetailIndex;

    [Header("Button")]
    [SerializeField] Button closeButton;
    [SerializeField] Button bgCloseButton;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    [Header("Prefab")]
    [SerializeField] GameObject uiDot;

    [Header("Content")]
    [SerializeField] public List<TutorialContent> contents;
    List<Toggle> toggles = new List<Toggle>();

    [Header("Transition")]
    public bool fadeIn;
    public bool fadeOut;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
        bgCloseButton.onClick.AddListener(Hide);
        previousButton.onClick.AddListener(PreviousButton);
        nextButton.onClick.AddListener(NextButton);
    }

    private void Start()
    {
        if(contents != null && contents.Count > 0)
        {
            header.text = contents[0].header;
            if (contents[0].contents != null && contents[0].contents.Count > 0)
            {
                tutorialCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    }

    private void Update()
    {
        if(fadeIn || fadeOut)
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
                    //UIManagers.Instance.uiLobby.optionButton.interactable = true;
                    UIManagers.Instance.playerCanvas.tutorialButton.interactable = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void CheckHeader()
    {
        if(header.text == "Controller")
        {
            contentImage.SetNativeSize();
            contentImage.rectTransform.localPosition = new Vector3(contentImage.rectTransform.localPosition.x, 5f, contentImage.rectTransform.localPosition.z);
            contentImage.rectTransform.localScale = new Vector3(0.9f,0.9f,0.9f);

            selection.transform.localPosition = new Vector3(contentImage.rectTransform.localPosition.x, -335f, contentImage.rectTransform.localPosition.z);
        }
        else
        {
            contentImage.SetNativeSize();
            contentImage.rectTransform.localPosition = new Vector3(contentImage.rectTransform.localPosition.x, 50f, contentImage.rectTransform.localPosition.z);
            contentImage.rectTransform.localScale = Vector3.one;

            selection.transform.localPosition = new Vector3(contentImage.rectTransform.localPosition.x, -235f, contentImage.rectTransform.localPosition.z);
        }
    }

    private void OnTutorialToggleChange(bool value)
    {
        int i = 0;
        foreach (var toggle in toggles)
        {
            if (toggle.isOn)
            {
                Debug.LogWarning($"{currentContentDetailIndex}");
                currentContentDetailIndex = i;
                contentImage.sprite = contents[currentContentIndex].contents[currentContentDetailIndex].Image;
                contentDescription.text = contents[currentContentIndex].contents[currentContentDetailIndex].description;

                if (currentContentIndex == 0)
                {
                    if (currentContentDetailIndex == 0)
                        previousButton.gameObject.SetActive(false);
                    else
                        previousButton.gameObject.SetActive(true);
                }

                if (currentContentIndex == contents.Count - 1)
                {
                    if (currentContentDetailIndex == contents[currentContentIndex].contents.Count - 1)
                        nextButton.gameObject.SetActive(false);
                    else
                        nextButton.gameObject.SetActive(true);
                }

                return;
            }
            i++;
        }
    }

    void PreviousButton()
    {
        if(contents != null && contents.Count >= 0)
        {
            if (currentContentDetailIndex > 0)
            {
                currentContentDetailIndex -= 1;
                selection.transform.GetChild(currentContentDetailIndex).GetComponent<Toggle>().isOn = true;
            }
            else
            {
                if (currentContentIndex > 0)
                {
                    currentContentDetailIndex = contents[currentContentIndex - 1].contents.Count - 1;
                    currentContentIndex -= 1;
                    toggles.Clear();
                    foreach (Transform child in selection.transform)
                    {
                        child.GetComponent<Toggle>().onValueChanged.RemoveListener(OnTutorialToggleChange);
                        Destroy(child.gameObject);
                    }
                    for (int i = 0;i< contents[currentContentIndex].contents.Count;i++)
                    {
                        GameObject sel = Instantiate(uiDot, selection.transform);
                        sel.GetComponent<Toggle>().onValueChanged.AddListener(OnTutorialToggleChange);
                        sel.GetComponent<Toggle>().group = selection.GetComponent<ToggleGroup>();
                        toggles.Add(sel.GetComponent<Toggle>());
                        if (i == contents[currentContentIndex].contents.Count-1)
                        {
                            sel.GetComponent<Toggle>().isOn = true;
                        }
                    }
                }
            }
            if(currentContentDetailIndex == 0 && currentContentIndex == 0)
            {
                previousButton.gameObject.SetActive(false);
            }
            header.text = contents[currentContentIndex].header;
            contentImage.sprite = contents[currentContentIndex].contents[currentContentDetailIndex].Image;
            contentDescription.text = contents[currentContentIndex].contents[currentContentDetailIndex].description;
            nextButton.gameObject.SetActive(true);
            CheckHeader();
        }
    }

    void NextButton()
    {
        if (contents != null && contents.Count >= 0)
        {
            if (currentContentDetailIndex < contents[currentContentIndex].contents.Count-1)
            {
                currentContentDetailIndex += 1;
                selection.transform.GetChild(currentContentDetailIndex).GetComponent<Toggle>().isOn = true;
            }
            else
            {
                if (currentContentIndex < contents.Count-1)
                {
                    currentContentDetailIndex = 0;
                    currentContentIndex += 1;
                    toggles.Clear();
                    foreach (Transform child in selection.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    for (int i = 0; i < contents[currentContentIndex].contents.Count; i++)
                    {
                        GameObject sel = Instantiate(uiDot, selection.transform);
                        sel.GetComponent<Toggle>().onValueChanged.AddListener(OnTutorialToggleChange);
                        sel.GetComponent<Toggle>().group = selection.GetComponent<ToggleGroup>();
                        toggles.Add(sel.GetComponent<Toggle>());
                        if (i == 0)
                        {
                            sel.GetComponent<Toggle>().isOn = true;
                        }
                    }
                }
            }
            if (currentContentDetailIndex == contents[currentContentIndex].contents.Count - 1 && currentContentIndex == contents.Count - 1)
            {
                nextButton.gameObject.SetActive(false);
            }
            header.text = contents[currentContentIndex].header;
            contentImage.sprite = contents[currentContentIndex].contents[currentContentDetailIndex].Image;
            contentDescription.text = contents[currentContentIndex].contents[currentContentDetailIndex].description;
            previousButton.gameObject.SetActive(true);
            CheckHeader();
        }
    }

    public void Hide()
    {
        fadeIn = false;
        fadeOut = true;
        CursorManager.Instance.tutorial = false;
        //UIManagers.Instance.uiReadyCharacter.buttonTutorial.interactable = true;
        UIManagers.Instance.playerCanvas.tutorialButton.interactable = true;
    }

    public void Show()
    {
        if (!CursorManager.Instance.tutorial)
        {
            if (CursorManager.Instance.selectmap || CursorManager.Instance.login || CursorManager.Instance.loading || CursorManager.Instance.option || CursorManager.Instance.summary || CursorManager.Instance.lobby) return;
            gameObject.SetActive(true);
            if (!CursorManager.Instance.lobby)
            {
                PlayerManagers.Instance.PlayerComponents.PlayerUIManager.OnPlayerCloseMap();
                PlayerManagers.Instance.PlayerComponents.PlayerUIManager.UnDisplayStuckPanel();
                PlayerManagers.Instance.PlayerComponents.InventoryManager.CloseInventory();
            }
            fadeIn = true;
            fadeOut = false;
            CursorManager.Instance.tutorial = true;
            //UIManagers.Instance.uiReadyCharacter.buttonTutorial.interactable = false;
            UIManagers.Instance.playerCanvas.tutorialButton.interactable = false;
            header.text = contents[0].header;
            contentImage.sprite = contents[0].contents[0].Image;
            contentDescription.text = contents[0].contents[0].description;
            toggles.Clear();
            foreach (Transform child in selection.transform)
            {
                Destroy(child.gameObject);
                child.GetComponent<Toggle>().onValueChanged.RemoveListener(OnTutorialToggleChange);
            }
            for (int i = 0; i < contents[0].contents.Count; i++)
            {
                GameObject sel =  Instantiate(uiDot, selection.transform);
                sel.GetComponent<Toggle>().onValueChanged.AddListener(OnTutorialToggleChange);
                sel.GetComponent<Toggle>().group = selection.GetComponent<ToggleGroup>();
                sel.GetComponent<Toggle>().isOn = true;
                toggles.Add(sel.GetComponent<Toggle>());
            }
            currentContentIndex = 0;
            currentContentDetailIndex = 0;
            selection.transform.GetChild(currentContentDetailIndex).GetComponent<Toggle>().isOn = true;
            previousButton.gameObject.SetActive(false);
            if (contents[0].contents.Count == 1 && contents.Count == 1)
            {
                nextButton.gameObject.SetActive(false);
            }
            else
            {
                nextButton.gameObject.SetActive(true);
            }
            CheckHeader();
        }
        else
        {
            Hide();
        }
    }
}
