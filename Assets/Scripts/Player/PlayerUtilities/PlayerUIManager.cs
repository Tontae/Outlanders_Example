using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Outlander.UI;

namespace Outlander.Player
{
    public class PlayerUIManager : PlayerElements
    {
        [Header("Player")]
        private GameObject m_camera;
        [SerializeField] private DamagePopUp damagePopUp;
        //[SerializeField] public GameObject statusUI;

        [Header("Unstuck")]
        [SerializeField] public float unStuckDelay;
        [SerializeField] private bool isRotateMap = true;
        [System.Serializable]
        public class itemdataClass
        {

            public itemdataClass(string itemName, Sprite itemIcon, int amonut)
            {
                this.itemName = itemName;
                this.itemIcon = itemIcon;
                this.amonut = amonut;
            }
            public string itemName { set; get; }
            public Sprite itemIcon { set; get; }
            public int amonut { set; get; }
        }

        [SerializeField] public Queue<itemdataClass> containItem = new Queue<itemdataClass>();
        public bool isShowing;
        [SerializeField] private ObtainedUIScripts itemObtainedPanal;

        private AudioPlayer audioPlayer;

        private void Awake()
        {
            audioPlayer = GetComponent<AudioPlayer>();
            //m_camera.GetComponent<Camera>();
        }

        private void OnDisable()
        {
            if (!isLocalPlayer) return;

            UIManagers.Instance.playerCanvas.tutorialButton.onClick.RemoveListener(UIManagers.Instance.uiTutorial.Show);
            UIManagers.Instance.playerCanvas.inventoryButton.onClick.RemoveListener(Player.PlayerInventoryController.OpenInventory);
            UIManagers.Instance.playerCanvas.settingButton.onClick.RemoveListener(UIManagers.Instance.optionManager.OnOpenSetting);
            UIManagers.Instance.playerCanvas.unstuckButton.onClick.RemoveListener(Player.PlayerUIManager.DisplayUnstuckPanel);
            UIManagers.Instance.playerCanvas.mapButton.onClick.RemoveListener(OnPlayerOpenMap);
        }

        private void Start()
        {
            m_camera = GameObject.FindGameObjectWithTag("MainCamera");

            if (!isLocalPlayer) return;

            UIManagers.Instance.playerCanvas.tutorialButton.onClick.AddListener(UIManagers.Instance.uiTutorial.Show);
            UIManagers.Instance.playerCanvas.inventoryButton.onClick.AddListener(Player.PlayerInventoryController.OpenInventory);
            UIManagers.Instance.playerCanvas.settingButton.onClick.AddListener(UIManagers.Instance.optionManager.OnOpenSetting);
            UIManagers.Instance.playerCanvas.unstuckButton.onClick.AddListener(Player.PlayerUIManager.DisplayUnstuckPanel);
            UIManagers.Instance.playerCanvas.mapButton.onClick.AddListener(OnPlayerOpenMap);

            UIManagers.Instance.playerCanvas.miniMapCamera.transform.SetParent(transform);
            UIManagers.Instance.playerCanvas.miniMapCamera.transform.position = new Vector3(0f, 100f, 0f);
            UIManagers.Instance.playerCanvas.miniMapCamera.transform.localScale = Vector3.one;

            UIManagers.Instance.playerCanvas.countUI.SetActive(true);
            UIManagers.Instance.playerCanvas.compassUI.SetActive(true);
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (containItem.Count > 0 && !isShowing)
            {
                isShowing = true;
                ShowItemThisTheyGot();
            }

            if (unStuckDelay > 0)
            {
                unStuckDelay -= Time.deltaTime;
                if (unStuckDelay < 0)
                    unStuckDelay = 0;
            }
        }

        private void LateUpdate()
        {
            if (!isLocalPlayer) return;
            RotateCompass();
            RotateMiniMap();
        }

        /*private void OnDestroy()
        {
            Debug.Log($"Move MiniCam to Origin");
            UIManagers.Instance.playerCanvas.miniMapCamera.transform.SetParent(UIManagers.Instance.playerCanvas.mapButton.transform);
        }*/

        private void RotateCompass()
        {
            UIManagers.Instance.playerCanvas.compassImage.uvRect = new Rect(m_camera.transform.localEulerAngles.y / 360, 0, 1, 1);
        }

        private void RotateMiniMap()
        {
            if (isRotateMap)
            {
                UIManagers.Instance.playerCanvas.miniMapCamera.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                UIManagers.Instance.playerCanvas.PlayerMiniMapIcon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                UIManagers.Instance.playerCanvas.miniMapCamera.transform.localRotation = Quaternion.Euler(90f, 0f, transform.localEulerAngles.y);
                UIManagers.Instance.playerCanvas.PlayerMiniMapIcon.transform.localRotation = Quaternion.Euler(0f, 0f, -transform.localEulerAngles.y);
            }
            /*Vector3 normalized = UIManagers.Instance.uiMap.Divide(UIManagers.Instance.uiMap.map3dParent.InverseTransformPoint(LocalMatchManager.singleton.player.transform.position), UIManagers.Instance.uiMap.map3dEnd.position - UIManagers.Instance.uiMap.map3dParent.position);
            normalized.y = 0;
            Vector3 mapped = UIManagers.Instance.uiMap.Multiply(normalized, UIManagers.Instance.uiMap.map2dEnd.localPosition);
            mapped.z = 0;
            UIManagers.Instance.playerCanvas.miniMap2DRect.localPosition = mapped;*/
        }

        public void PlayerOnMatch()
        {
            if (!isLocalPlayer) return;

            for (int i = 0; i < UIManagers.Instance.playerCanvas.uiSummary.summaryIcon.transform.childCount; i++)
            {
                UIManagers.Instance.playerCanvas.uiSummary.summaryIcon.transform.GetChild(i).gameObject.SetActive(false);
            }
            OnOpenPlayerUI();
            UIManagers.Instance.uiMatch.timeTitleText.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void PlayerOnLobby()
        {
            UIManagers.Instance.playerCanvas.uiSummary.Hide();
            UIManagers.Instance.uiWaiting.Show();
            UIManagers.Instance.playerCanvas.miniMapCamera.transform.parent = UIManagers.Instance.playerCanvas.mapButton.transform;
            OnCloseAllMatchUI();
        }

        public void OnGameSummary(bool isVictory, int playerRank, float gameTime)
        {
            UIManagers.Instance.playerCanvas.uiSummary.Show();
            if (isVictory)
            {
                PlayerManagers.Instance.characterCreator.GetComponent<Animator>().SetBool("isVictory", true);
                PlayerManagers.Instance.characterCreator.GetComponent<Animator>().SetBool("isDefeat", false);
                UIManagers.Instance.optionManager.SwapMusicBetweenScene(2);
            }
            else
            {
                PlayerManagers.Instance.characterCreator.GetComponent<Animator>().SetBool("isVictory", false);
                PlayerManagers.Instance.characterCreator.GetComponent<Animator>().SetBool("isDefeat", true);
                UIManagers.Instance.optionManager.SwapMusicBetweenScene(3);
            }

            UIManagers.Instance.ShowSummaryUI();
            PlayerManagers.Instance.gameEnded = true;
            OnCloseAllMatchUI();
            StartCoroutine(Player.PlayerReturnLobby.CountDownToLobby());
            ShowSummaryResult(isVictory, playerRank, gameTime);
            Player.PlayerStatisticsData.ClientPrepareCountData();
        }
        public void UpdatePlayerHeath(float health)
        {
            UIManagers.Instance.playerCanvas.playerHealthImage.fillAmount = health;
        }

        public void UpdatePlayerMana(float mana)
        {
            UIManagers.Instance.playerCanvas.playerManaImage.fillAmount = mana;
        }

        /*public void UpdatePlayerFP(float fp)
        {
            float mfp = GetComponent<IStatus>().MaxFpoint;
            playerFP.fillAmount = (fp / mfp);
            playerFPNumber.text = $"FP {fp.ToString("0")}/{mfp.ToString("0")}";
        }*/

        public void GetObtainItemData(string itemName, Sprite itemSprite, int amount)
        {
            containItem.Enqueue(new itemdataClass(itemName, itemSprite, amount));

        }

        public void ShowItemThisTheyGot()
        {
            //GameObject tempDisplayPannal = Instantiate(itemObtainedPanal);
            itemdataClass temp = containItem.Dequeue();
            ObtainedUIScripts tempUI = Instantiate(itemObtainedPanal);
            tempUI.displayItemImage.sprite = temp.itemIcon;
            tempUI.displayText.text = temp.itemName + " x " + temp.amonut.ToString();
            tempUI.timeToDestroy = 1f / (float)(containItem.Count + 1);
            tempUI.puim = this;
            tempUI.StartInvoke();
            //containItem.RemoveAt(0);
        }

        public void UpdateKillCount(int nums)
        {
            UIManagers.Instance.playerCanvas.killNumber.text = $"{nums}";
        }

        void OnCloseAllMatchUI()
        {
            OnClosePlayerUI();
            OnPlayerCloseMap();
            Player.InventoryManager.CloseInventory();
            Player.PlayerUIManager.UnDisplayStuckPanel();
            UIManagers.Instance.optionManager.OnCloseSetting();
            UIManagers.Instance.uiTutorial.Hide();
            UIManagers.Instance.playerCanvas.uiShop.shop_Panel.SetActive(false);
            CursorManager.Instance.shop = false;
        }

        public void OnOpenPlayerUI()
        {
            UIManagers.Instance.playerCanvas.playerScreenUI.SetActive(true);
            UIManagers.Instance.playerCanvas.mapButton.gameObject.SetActive(true);
            UIManagers.Instance.playerCanvas.countUI.SetActive(true);
            UIManagers.Instance.playerCanvas.compassUI.SetActive(true);
            UIManagers.Instance.playerCanvas.statusUI.SetActive(true);
        }

        public void OnClosePlayerUI()
        {
            UIManagers.Instance.playerCanvas.playerScreenUI.SetActive(false);
            UIManagers.Instance.playerCanvas.mapButton.gameObject.SetActive(false);
            UIManagers.Instance.playerCanvas.countUI.SetActive(false);
            UIManagers.Instance.playerCanvas.compassUI.SetActive(false);
            UIManagers.Instance.playerCanvas.statusUI.SetActive(false);
        }

        public void OnPlayerCloseMap()
        {
            UIManagers.Instance.uiMap.Hide();
            UIManagers.Instance.playerCanvas.mapButton.interactable = true;
            UIManagers.Instance.uiAnnouncement.indicatorGo.SetActive(true);
            UIManagers.Instance.uiAnnouncement.OnOpenMap(false);
        }

        public void OnPlayerOpenMap()
        {
            if (CursorManager.Instance.selectmap || CursorManager.Instance.lobby || CursorManager.Instance.option || CursorManager.Instance.login || CursorManager.Instance.summary || CursorManager.Instance.loading) return;
            UIManagers.Instance.uiMap.Show();
            UIManagers.Instance.playerCanvas.mapButton.interactable = false;

            //Close other UI
            UIManagers.Instance.uiTutorial.Hide();
            Player.PlayerUIManager.UnDisplayStuckPanel();
            Player.InventoryManager.CloseInventory();
            UIManagers.Instance.uiAnnouncement.indicatorGo.SetActive(false);
            UIManagers.Instance.uiAnnouncement.OnOpenMap(true);
        }

        public void MapInput()
        {
            if (CursorManager.Instance.map)
                OnPlayerCloseMap();
            else
                OnPlayerOpenMap();
        }

        //public void OnOpenOption(InputValue value)
        //{
        //    ClickOpenOption();
        //}

        public int CalculateScore(int _rank, float _surviveTime, int _playerKill, int monsterKill)
        {
            int placementScore = 0;
            int surviveTimeScore = 0;
            int playerKillScore = _playerKill * 10;
            int mosterKillScore = monsterKill * 5;

            if (_rank > 10)
                placementScore = 2;
            else if (_rank > 5 && _rank <= 10)
                placementScore = 10;
            else if (_rank > 3 && _rank <= 5)
                placementScore = 15;
            else if (_rank > 1 && _rank <= 3)
                placementScore = 20;
            else
                placementScore = 30;

            if (_surviveTime < 180f)
                surviveTimeScore = 2;
            else if (_surviveTime >= 180f && _surviveTime < 300f)
                surviveTimeScore = 5;
            else if (_surviveTime >= 300f && _surviveTime < 420f)
                surviveTimeScore = 10;
            else if (_surviveTime >= 420f && _surviveTime < 600f)
                surviveTimeScore = 15;
            else if (_surviveTime >= 600f && _surviveTime < 900f)
                surviveTimeScore = 20;
            else
                surviveTimeScore = 25;
            //Debug.Log($"Rank : {_rank} | Score :  {placementScore} + {surviveTimeScore} + {playerKillScore} + {mosterKillScore} = {placementScore + surviveTimeScore + playerKillScore + mosterKillScore}");
            return placementScore + surviveTimeScore + playerKillScore + mosterKillScore;
        }

        public int CalculateCoin(int _rank, float _surviveTime, int _playerKill, int monsterKill)
        {
            int placementScore = 0;
            int surviveTimeScore = 0;
            int playerKillScore = _playerKill * 5;
            int mosterKillScore = monsterKill * 5;

            if (_rank > 10)
                placementScore = 5;
            else if (_rank > 5 && _rank <= 10)
                placementScore = 15;
            else if (_rank > 3 && _rank <= 5)
                placementScore = 20;
            else if (_rank > 1 && _rank <= 3)
                placementScore = 30;
            else
                placementScore = 50;

            if (_surviveTime < 180f)
                surviveTimeScore = 5;
            else if (_surviveTime >= 180f && _surviveTime < 300f)
                surviveTimeScore = 10;
            else if (_surviveTime >= 300f && _surviveTime < 420f)
                surviveTimeScore = 15;
            else if (_surviveTime >= 420f && _surviveTime < 600f)
                surviveTimeScore = 25;
            else if (_surviveTime >= 600f && _surviveTime < 900f)
                surviveTimeScore = 30;
            else
                surviveTimeScore = 40;
            //Debug.Log($"Rank : {_rank} | Score :  {placementScore} + {surviveTimeScore} + {playerKillScore} + {mosterKillScore} = {placementScore + surviveTimeScore + playerKillScore + mosterKillScore}");
            return placementScore + surviveTimeScore + playerKillScore + mosterKillScore;
        }

        public int CalculateLevelExp(int _rank, float _surviveTime, int _playerKill, int monsterKill, int minibossKill, int bossKill)
        {
            int placementScore = 0;
            int surviveTimeScore = 0;
            int playerKillScore = _playerKill * 100;
            int mosterKillScore = monsterKill * 50;
            int minibossKillScore = minibossKill * 100;
            int bossScore = bossKill * 150;

            if (_rank > 10)
                placementScore = 20;
            else if (_rank > 5 && _rank <= 10)
                placementScore = 100;
            else if (_rank > 3 && _rank <= 5)
                placementScore = 150;
            else if (_rank > 1 && _rank <= 3)
                placementScore = 200;
            else
                placementScore = 300;

            if (_surviveTime < 180f)
                surviveTimeScore = 20;
            else if (_surviveTime >= 180f && _surviveTime < 300f)
                surviveTimeScore = 50;
            else if (_surviveTime >= 300f && _surviveTime < 420f)
                surviveTimeScore = 100;
            else if (_surviveTime >= 420f && _surviveTime < 600f)
                surviveTimeScore = 150;
            else if (_surviveTime >= 600f && _surviveTime < 900f)
                surviveTimeScore = 200;
            else
                surviveTimeScore = 250;
            //Debug.Log($"Rank : {_rank} | Score :  {placementScore} + {surviveTimeScore} + {playerKillScore} + {mosterKillScore} = {placementScore + surviveTimeScore + playerKillScore + mosterKillScore}");
            return placementScore + surviveTimeScore + playerKillScore + mosterKillScore + minibossKillScore + bossKill;
        }

        public int CalculateRankExp(int _rank, float _surviveTime, int _playerKill, int monsterKill)
        {
            int placementScore = 0;
            int surviveTimeScore = 0;
            int playerKillScore = _playerKill * 10;
            int mosterKillScore = monsterKill * 5;

            if (_rank > 10)
                placementScore = 2;
            else if (_rank > 5 && _rank <= 10)
                placementScore = 10;
            else if (_rank > 3 && _rank <= 5)
                placementScore = 15;
            else if (_rank > 1 && _rank <= 3)
                placementScore = 20;
            else
                placementScore = 30;

            if (_surviveTime < 180f)
                surviveTimeScore = 2;
            else if (_surviveTime >= 180f && _surviveTime < 300f)
                surviveTimeScore = 5;
            else if (_surviveTime >= 300f && _surviveTime < 420f)
                surviveTimeScore = 10;
            else if (_surviveTime >= 420f && _surviveTime < 600f)
                surviveTimeScore = 15;
            else if (_surviveTime >= 600f && _surviveTime < 900f)
                surviveTimeScore = 20;
            else
                surviveTimeScore = 25;
            //Debug.Log($"Rank : {_rank} | Score :  {placementScore} + {surviveTimeScore} + {playerKillScore} + {mosterKillScore} = {placementScore + surviveTimeScore + playerKillScore + mosterKillScore}");
            return placementScore + surviveTimeScore + playerKillScore + mosterKillScore;
        }

        public int CalculateProficiencyExp(int playerKill, int monsterKill, int minibossKill, int bossKill)
        {
            int playerKillScore = playerKill * 5;
            int mosterKillScore = monsterKill * 2;
            int minibossKillScore = minibossKill * 5;
            int bossScore = bossKill * 10;

            //Debug.Log($"Rank : {_rank} | Score :  {placementScore} + {surviveTimeScore} + {playerKillScore} + {mosterKillScore} = {placementScore + surviveTimeScore + playerKillScore + mosterKillScore}");
            return playerKillScore + mosterKillScore + minibossKillScore + bossKill;
        }

        //[Command]
        //void CmdSummaryScore(int _score)
        //{
        //    Player.PlayerMatchManager.playerScore = _score;
        //}

        //[Command]
        //void CmdShowSummary(bool isVictory)
        //{
        //    int playerAlives = GetComponent<Network.PlayerMatchManager>().myManager.playerAlive;
        //    ShowSummary(isVictory, playerAlives);
        //}

        private void ShowSummaryResult(bool isVictory, int _playerAlives, float gameTime)
        {
            if (isVictory)
            {
                UIManagers.Instance.playerCanvas.uiSummary.summaryIcon.transform.GetChild(0).gameObject.SetActive(true);
                UIManagers.Instance.playerCanvas.uiSummary.summaryRank.text = "#" + _playerAlives.ToString();

                UIManagers.Instance.playerCanvas.uiSummary.summaryKillBy.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                int rankI = Mathf.Clamp(_playerAlives, 1, 5);
                UIManagers.Instance.playerCanvas.uiSummary.summaryIcon.transform.GetChild(rankI).gameObject.SetActive(true);
                UIManagers.Instance.playerCanvas.uiSummary.summaryRank.text = "#" + (_playerAlives + 1).ToString();

                UIManagers.Instance.playerCanvas.uiSummary.summaryKillBy.transform.parent.gameObject.SetActive(true);
                UIManagers.Instance.playerCanvas.uiSummary.summaryKillBy.text = PlayerManagers.Instance.matchManager.whoKillMe;
                PlayerManagers.Instance.matchManager.whoKillMe = string.Empty;
            }
            //UIManagers.Instance.playerCanvas.uiSummary.summaryCoin.text = "soon";
            //UIManagers.Instance.playerCanvas.uiSummary.summaryBonus.text = "Bonus+" + "soon";

            //UIManagers.Instance.playerCanvas.uiSummary.summaryGameTime.text =  $"{Mathf.FloorToInt(gameTime / 60f).ToString("0")}m {(Mathf.FloorToInt(gameTime) % 60f).ToString("00")}s";
            //UIManagers.Instance.playerCanvas.uiSummary.summaryPlayerKillCount.text = Player.PlayerMatchManager.playerKillCount.ToString();
            //UIManagers.Instance.playerCanvas.uiSummary.summaryMonsKillCount.text = Player.PlayerMatchManager.monsterKillCount.ToString();
            //UIManagers.Instance.playerCanvas.uiSummary.summaryMiniBossKillCount.text = Player.PlayerMatchManager.miniBossKillCount.ToString();
            //UIManagers.Instance.playerCanvas.uiSummary.summaryBossKillCount.text = Player.PlayerMatchManager.bossKillCount.ToString();
            
            //int resultScore = CalculateScore(isVictory ? _playerAlives : _playerAlives + 1,
            //    gameTime,
            //    Player.PlayerMatchManager.playerKillCount,
            //    Player.PlayerMatchManager.monsterKillCount + Player.PlayerMatchManager.miniBossKillCount + Player.PlayerMatchManager.bossKillCount);

            //UIManagers.Instance.playerCanvas.uiSummary.summaryScore.text = resultScore.ToString();

            //Player.PlayerMatchManager.playerScore = resultScore;

            //CmdSummaryScore(Player.PlayerMatchManager.playerScore);

            UIManagers.Instance.playerCanvas.uiSummary.summaryPlayerName.text = gameObject.name;
            UIManagers.Instance.playerCanvas.uiSummary.summaryPlayerLevel.text = "LV " + "0000";
            UIManagers.Instance.playerCanvas.uiSummary.summaryPlayerExp.text = "0000";
        }

        public void UnstuckInput()
        {
            if (CursorManager.Instance.unstuck)
            {
                UnDisplayStuckPanel();
            }
            else
            {
                DisplayUnstuckPanel();
            }
        }

        public void DisplayUnstuckPanel()
        {
            if (CursorManager.Instance.selectmap || CursorManager.Instance.lobby || CursorManager.Instance.option || CursorManager.Instance.login || CursorManager.Instance.summary || CursorManager.Instance.loading || unStuckDelay > 0) return;

            UIManagers.Instance.uiWarning.ShowDoubleWarningButton("Would you like to unstuck\nyour Character ?", EnableUnStuckPanel, UnDisplayStuckPanel);
            CursorManager.Instance.unstuck = true;

            UIManagers.Instance.uiTutorial.Hide();
            Player.PlayerUIManager.OnPlayerCloseMap();
            Player.InventoryManager.CloseInventory();
            UIManagers.Instance.playerCanvas.unstuckButton.interactable = false;
        }

        public void UnDisplayStuckPanel()
        {
            CursorManager.Instance.unstuck = false;
            UIManagers.Instance.playerCanvas.unstuckButton.interactable = true;
            UIManagers.Instance.uiWarning.CancelWarning();
        }

        public void EnableUnStuckPanel()
        {
            unStuckDelay = 2f;

            //transform.position += new Vector3(0, 0.1f, 0);

            Player.Animator.Play("Idle");

            Player.OutlanderStateMachine.ResetPlayerBoolData();
            Player.MovementStateMachine.ResetMovementBoolData();

            UnDisplayStuckPanel();
        }

        [TargetRpc]
        public void TargetShowDamagePopUp(float damageAmount, Color color)
        {
            DamagePopUp damagePop = Instantiate(damagePopUp, transform.position + (Vector3.up * 2), Quaternion.identity);
            damagePop.Setup(damageAmount, color);
        }

        [TargetRpc]
        public void TargetShowDamagePopUp(string text, Color color)
        {
            DamagePopUp damagePop = Instantiate(damagePopUp, transform.position + (Vector3.up * 2), Quaternion.identity);
            damagePop.Setup(text, color);
        }

        [TargetRpc]
        public void TargetShowDamagePopUp(string text, Color color, Vector3 posHit)
        {
            DamagePopUp damagePop = Instantiate(damagePopUp, posHit, Quaternion.identity);
            damagePop.Setup(text, color);
        }

        [TargetRpc]
        public void TargetShowDamagePopUp(float damageAmount, Color color, Vector3 posHit)
        {
            DamagePopUp damagePop = Instantiate(damagePopUp, posHit, Quaternion.identity);
            damagePop.Setup(damageAmount, color);

            GameObject Fx = Instantiate(Player.OutlanderStateMachine.FxHit, posHit, Quaternion.identity);
            Destroy(Fx, 0.5f);
        }

        [TargetRpc]
        public void TargetPlayerHitSfx(string playerWeapon, string targetWeapon, bool isBlock, Vector3 posHit)
        {
            //Debug.Log($"{playerWeapon} {targetWeapon}");
            if (string.IsNullOrEmpty(playerWeapon))
                playerWeapon = "Barehand";
            else
                playerWeapon = playerWeapon.Split(' ').Length == 1 ? playerWeapon.Split(' ')[0] : playerWeapon.Split(' ')[1];
            if (string.IsNullOrEmpty(targetWeapon))
                targetWeapon = "Barehand";
            else
                targetWeapon = targetWeapon.Split(' ').Length == 1 ? targetWeapon.Split(' ')[0] : targetWeapon.Split(' ')[1];

            /*switch (playerWeapon)
            {
                case "Barehand":
                case "Wood":
                    if (isBlock)
                        audioPlayer.PlaySound(UnityEngine.Random.value <= 0.5f ? 24 : 25, posHit);
                    else
                        audioPlayer.PlaySound(23, posHit);
                    break;
                case "Sword":
                    if (isBlock)
                    {
                        switch (targetWeapon)
                        {
                            case "Barehand":
                                audioPlayer.PlaySound(26, posHit);
                                break;
                            case "Sword":
                                audioPlayer.PlaySound(27, posHit);
                                break;
                            case "Wood":
                            case "Axe":
                            case "Lance":
                                audioPlayer.PlaySound(28, posHit);
                                break;
                        }
                    }
                    else
                        audioPlayer.PlaySound(26, posHit);
                    break;
                case "Axe":
                    if (isBlock)
                    {
                        switch (targetWeapon)
                        {
                            case "Barehand":
                                audioPlayer.PlaySound(29, posHit   );
                                break;
                            case "Sword":
                                audioPlayer.PlaySound(27, posHit);
                                break;
                            case "Wood":
                            case "Axe":
                            case "Lance":
                                audioPlayer.PlaySound(28, posHit);
                                break;
                        }
                    }
                    else
                        audioPlayer.PlaySound(29, posHit);
                    break;
                case "Lance":
                    if (isBlock)
                    {
                        switch (targetWeapon)
                        {
                            case "Barehand":
                                audioPlayer.PlaySound(30, posHit);
                                break;
                            case "Sword":
                                audioPlayer.PlaySound(27, posHit);
                                break;
                            case "Wood":
                            case "Axe":
                            case "Lance":
                                audioPlayer.PlaySound(28, posHit);
                                break;
                        }
                    }
                    else
                        audioPlayer.PlaySound(30, posHit);
                    break;
            }*/

            string catstr = string.Concat(playerWeapon, targetWeapon);
            if (!isBlock)
                switch (playerWeapon)
                {
                    case "Barehand":
                    case "Wood":
                        audioPlayer.PlaySound(23, posHit);
                        break;
                    case "Sword":
                        audioPlayer.PlaySound(26, posHit);
                        break;
                    case "Axe":
                        audioPlayer.PlaySound(29, posHit);
                        break;
                    case "Lance":
                        audioPlayer.PlaySound(30, posHit);
                        break;
                }
            else
                switch (catstr)
                {
                    case "BarehandBarehand":
                    case "BarehandWood":
                    case "BarehandSword":
                    case "BarehandAxe":
                    case "BarehandLance":
                    case "WoodBarehand":
                    case "WoodWood":
                    case "WoodSword":
                    case "WoodAxe":
                    case "WoodLance":
                        audioPlayer.PlaySound(UnityEngine.Random.value <= 0.5f ? 24 : 25, posHit);
                        break;
                    case "AxeAxe":
                    case "AxeLance":
                    case "AxeWood":
                    case "LanceAxe":
                    case "LanceLance":
                    case "LanceWood":
                    case "SwordAxe":
                    case "SwordLance":
                    case "SwordWood":
                        audioPlayer.PlaySound(28, posHit);
                        break;
                    case "SwordSword":
                    case "AxeSword":
                    case "LanceSword":
                        audioPlayer.PlaySound(27, posHit);
                        break;
                    case "SwordBarehand":
                        audioPlayer.PlaySound(26, posHit);
                        break;
                    case "AxeBarehand":
                        audioPlayer.PlaySound(29, posHit);
                        break;
                    case "LanceBarehand":
                        audioPlayer.PlaySound(30, posHit);
                        break;
                }
        }

        [TargetRpc]
        public void TargetPlayerIsHitSfx(string playerWeapon, string targetWeapon, bool isBlock, bool isRedZone)
        {
            //Debug.Log($"{targetWeapon} {playerWeapon}");
            if (isRedZone)
                return;
            if (!isBlock)
            {
                audioPlayer.playSound(20);
                return;
            }

            if (string.IsNullOrEmpty(playerWeapon))
                playerWeapon = "Barehand";
            else
                playerWeapon = playerWeapon.Split(' ').Length == 1 ? playerWeapon.Split(' ')[0] : playerWeapon.Split(' ')[1];
            if (string.IsNullOrEmpty(targetWeapon))
                targetWeapon = "Barehand";
            else
                targetWeapon = targetWeapon.Split(' ').Length == 1 ? targetWeapon.Split(' ')[0] : targetWeapon.Split(' ')[1];

            string catstr = string.Concat(targetWeapon, playerWeapon);
            switch (catstr)
            {
                case "BarehandBarehand":
                case "BarehandWood":
                case "BarehandSword":
                case "BarehandAxe":
                case "BarehandLance":
                case "WoodBarehand":
                case "WoodWood":
                case "WoodSword":
                case "WoodAxe":
                case "WoodLance":
                    audioPlayer.playSound(UnityEngine.Random.value <= 0.5f ? 24 : 25);
                    break;
                case "AxeAxe":
                case "AxeLance":
                case "AxeWood":
                case "LanceAxe":
                case "LanceLance":
                case "LanceWood":
                case "SwordAxe":
                case "SwordLance":
                case "SwordWood":
                    audioPlayer.playSound(28);
                    break;
                case "SwordSword":
                case "AxeSword":
                case "LanceSword":
                    audioPlayer.playSound(27);
                    break;
                case "SwordBarehand":
                    audioPlayer.playSound(26);
                    break;
                case "AxeBarehand":
                    audioPlayer.playSound(29);
                    break;
                case "LanceBarehand":
                    audioPlayer.playSound(30);
                    break;
            }
        }
    }
}
