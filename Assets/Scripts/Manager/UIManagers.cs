using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Outlander.UI;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using Outlander.Network;

public class UIElements : MonoBehaviour
{
    private UIManagers _ui;
    public UIManagers ui
    {
        get
        {
            if (_ui == null)
            {
                _ui = UIManagers.Instance;
            }
            return _ui;
        }

        //get => (_ui == null) ? _ui = UIManagers.Instance : _ui;
    }
}

public class UIManagers : SingletonPersistent<UIManagers>
{
    //public static UIManagers singleton;

    public UIWaiting uiWaiting;
    private PlayerInputAction playerInputActions;
    public PlayerInputAction MenuInputAction { get { if (playerInputActions == null) playerInputActions = new PlayerInputAction(); return playerInputActions; } }

    [Header("Manager")]
    public NetworkManagerOutsDB networkManager;

    [Header("Player")]
    [SerializeField] public UIPlayerCanvas playerCanvas;

    [Header("Setting")]
    public OptionManager optionManager;

    [Header("Warning")]
    public UIWarning uiWarning;

    [Header("Login")]
    public UINetwork uiNetwork;

    [Header("Match")]
    public UIMap uiMap;
    public UIMatch uiMatch;
    public UISpawnPhase uiSpawnPhase;
    public UITutorial uiTutorial;
    public AnnounceHelper uiAnnouncement;


    [Header("Canera")]
    public Camera summaryCamera;
    public Camera selectSpawnCamera;

    [Header("Utility")]
    [HideInInspector] public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    [HideInInspector] public const string ValidCharacter = @"[^\u0E00-\u0E7Fa-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./ ]";

    public override void Awake()
    {
        base.Awake();

        summaryCamera.gameObject.SetActive(false);
        selectSpawnCamera.gameObject.SetActive(false);
    }

    public void OnUISelectSpawnPhase()
    {
        uiSpawnPhase.Show();
        uiMatch.matchCanvas.enabled = false;
        uiMatch.timeTitleText.gameObject.SetActive(false);
        uiAnnouncement.GetComponent<Canvas>().enabled = false;
        uiTutorial.Hide();
        uiWaiting.Hide();
		optionManager.OnCloseSetting();
        PlayerManagers.Instance.PlayerComponents.InventoryManager.CloseInventory();
        PlayerManagers.Instance.PlayerComponents.PlayerUIManager.UnDisplayStuckPanel();
        PlayerManagers.Instance.PlayerComponents.PlayerUIManager.OnPlayerCloseMap();
        PlayerManagers.Instance.PlayerComponents.OutlanderStateMachine.InitializeCharacterData();
        PlayerManagers.Instance.PlayerComponents.PlayerInput.enabled = false;
        PlayerManagers.Instance.PlayerComponents.PlayerCamera.enabled = false;
    }

    public void ShowLobbyUI()
    {
        playerCanvas.gameObject.SetActive(false);
        uiMatch.gameObject.SetActive(false);
        uiAnnouncement.gameObject.SetActive(false);
        uiMap.gameObject.SetActive(false);
        playerCanvas.staminaBarFrame.gameObject.SetActive(false);
        playerCanvas.uiShop.shop_Panel.SetActive(false);
        uiWaiting.gameObject.SetActive(true);
        playerCanvas.uiSummary.Hide();
        CursorManager.Instance.shop = false;
    }

    public void ShowMatchStartUI()
    {
        playerCanvas.gameObject.SetActive(true);
        uiMatch.gameObject.SetActive(true);
        uiAnnouncement.gameObject.SetActive(true);
        uiMap.gameObject.SetActive(true);
        uiWaiting.loginPanel.SetActive(false);
        playerCanvas.uiSummary.Hide();

        //Component
        uiMatch.timeTitleText.gameObject.SetActive(true);
        uiAnnouncement.ClearAnnounce();
        uiMap.InitialIcon();
    }

    public void ShowSummaryUI()
    {
        uiMatch.gameObject.SetActive(false);
        uiAnnouncement.ClearAnnounce();
        uiAnnouncement.gameObject.SetActive(false);
    }

    #region UI Scene Control

    public void OnBackFromMatch(DestroyPlayerMsg msg)
    {
        uiAnnouncement.ClearAnnounce();
        PlayerManagers.Instance.ClearMatchData();
        PlayerManagers.Instance.ClearPlayerComponents();
        ShowLobbyUI();
        for (int i = 0; i < uiMatch.killFeedGrid.childCount; i++)
        {
            uiMatch.killFeedGrid.GetChild(i)?.gameObject.SetActive(false);
        }
        if (msg.isNewMatch)
        {
            optionManager.StopBackgroundSound();
        }
        else
        {
            optionManager.SwapMusicBetweenScene(0);
        }
    }

    #endregion

    public void RegisterSuccess()
    {
        uiWarning.CancelWarning();
    }

    public void ButtonLogout()
    {
        // stop client if client-only
        if (NetworkClient.isConnected)
            NetworkManager.singleton.StopClient();
        optionManager.OnCloseSetting();
        uiWaiting.Hide();
        optionManager.SwapMusicBetweenScene(0);
    }
}
