using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Outlander.Player
{
    public class ProficiencySystem : NetworkBehaviour
    {
        public WeaponManager weaponManager { get; private set; }
        // public enum WeaponType
        // {
        //     None,
        //     Sword,
        //     SwordShield,
        //     Shield,
        //     BowQuiver,
        //     BowNoQuiver,
        //     QuiverNoBow
        // }
        // public WeaponType type;

        public enum ProficiencyType
        {
            Walk,
            Run,
            Jump,
            Mine,
            Chop,
            Fish,
            Hunt,
            Swim,
            Climb,
            Craft,
            Repair,
            Buy,
            Sell,
            Endurance,
            Quest,
            SideWalk,
            Dodge,
            Sheild,
            Die,
            Land
        }
        public ProficiencyType pType;

        [System.Serializable]
        public class ProficiencyWeapon
        {
            [SyncVar] public int level;
            [SyncVar] public float currentXp;
            [SyncVar] public WeaponManager.WeaponType weaponType;

            /*public float GetWeaponProficiency()
            {
                return currentXp;
            }*/
        }
        //public List<ProficiencyWeapon> listWeapon = new List<ProficiencyWeapon>();
        public Dictionary<WeaponManager.WeaponType, ProficiencyWeapon> proWeapon = new Dictionary<WeaponManager.WeaponType, ProficiencyWeapon>();

        [SyncVar] public float requiredXp;
        [Range(1f, 300f)]
        public float additionMultiplier = 300;
        [Range(2f, 4f)]
        public float powerMultiplier = 2;
        [Range(7f, 34f)]
        public float divisionMultiplier = 7;
        private float lerpTimer;
        private float delayTimer;

        [SerializeField]
        protected TMP_Text levelTxt, xpTxt, xpPct_txt;

        private AudioPlayer audioPlayer;

        public Image frontXpBar;

        public Image backXpBar;
        public Image backgroundBar;

        private float disappearTime = 1.5f;
        private Color textColor;
        private Color defaultTextColor;
        private Color bar_BG_Color;
        private Color defaultBar_BG_Color;

        private void Awake()
        {
            audioPlayer = GetComponent<AudioPlayer>();
            weaponManager = GetComponent<WeaponManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (isLocalPlayer || isServer)
                InitProficiency();
        }

        void Update()
        {
            if (!isLocalPlayer && !isServer) return;
            if (isLocalPlayer)
            {
                UpdateXpUI();
            }
            
        }

        public void InitProficiency()
        {
            if (!proWeapon.ContainsKey(weaponManager.currentWeaponType)) {
                proWeapon.Add(weaponManager.currentWeaponType, new ProficiencyWeapon { level = 1, currentXp = 0, weaponType = weaponManager.currentWeaponType });
            }
            requiredXp = CalculateRequiredXp();

            frontXpBar.fillAmount = proWeapon[weaponManager.currentWeaponType].currentXp / requiredXp;

            backXpBar.fillAmount = proWeapon[weaponManager.currentWeaponType].currentXp / requiredXp;
            defaultTextColor = levelTxt.color;
            textColor = levelTxt.color;
            defaultBar_BG_Color = backgroundBar.color;
            bar_BG_Color = backgroundBar.color;
        }

        public void UpdateXpUI()
        {
            levelTxt.text = weaponManager.currentWeaponType.ToString() + "Rank " + proWeapon[weaponManager.currentWeaponType].level.ToString();
            float xpFraction = proWeapon[weaponManager.currentWeaponType].currentXp / requiredXp;
            float FXP = frontXpBar.fillAmount;

            if (FXP < xpFraction)
            {
                delayTimer += Time.deltaTime;
                backXpBar.fillAmount = xpFraction;

                if (delayTimer > 0.2)
                {
                    lerpTimer += Time.deltaTime;
                    float percentComplete = lerpTimer / 4;
                    frontXpBar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);

                }
            }
            xpTxt.text = proWeapon[weaponManager.currentWeaponType].currentXp.ToString("0") + "/" + requiredXp.ToString("0");



            if (disappearTime < 0)
            {
                float disappearSpeed = 1f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                levelTxt.color = textColor;
                xpTxt.color = textColor;
                bar_BG_Color.a -= disappearSpeed * Time.deltaTime;
                backgroundBar.color = bar_BG_Color;
                //bar_Back_Color.a -= disappearSpeed * Time.deltaTime;
                //backXpBar.color = bar_Back_Color;
                //bar_Front_Color.a -= disappearSpeed * Time.deltaTime;
                //frontXpBar.color = bar_Front_Color;
                //if (textColor.a < 0)
                //{

                //    rankUp_txt.gameObject.SetActive(false);
                //}
            }
            else
            {
                disappearTime -= Time.deltaTime;
            }
        }

        public void GainEXPFlatRate(float xpGain)
        {
            proWeapon[weaponManager.currentWeaponType].currentXp += xpGain;
            //Debug.Log($"{weaponManager.currentWeaponType}:{proWeapon[weaponManager.currentWeaponType].currentXp}");
            lerpTimer = 0f;
            delayTimer = 0f;
            disappearTime = 1.5f;
            textColor = defaultTextColor;
            xpTxt.color = defaultTextColor;
            levelTxt.color = defaultTextColor;
            //bar_Back_Color = defaultBar_Back_Color;
            //backXpBar.color = defaultBar_Back_Color;
            bar_BG_Color = defaultBar_BG_Color;
            backgroundBar.color = defaultBar_BG_Color;
            //bar_Front_Color = defaultBar_Front_Color;
            //frontXpBar.color = defaultBar_Front_Color;
            while (proWeapon[weaponManager.currentWeaponType].currentXp > requiredXp)
            {
                LevelUp();
            }
            if(isLocalPlayer)
                SaveProficiencyToServer();
        }

        public void LevelUp()
        {
            proWeapon[weaponManager.currentWeaponType].level += 1;
            frontXpBar.fillAmount = 0f;

            backXpBar.fillAmount = 0f;

            proWeapon[weaponManager.currentWeaponType].currentXp = Mathf.RoundToInt(proWeapon[weaponManager.currentWeaponType].currentXp - requiredXp);
            requiredXp = CalculateRequiredXp();

            ProficiencySetStats();

            if (!isLocalPlayer) return;
            audioPlayer.playSound(0);
        }

        public void ProficiencySetStats()
        {
            IPlayer player = GetComponent<IPlayer>();
            float atk = 0, maxHealth = 0, def = 0, maxMana = 0;
            for (int i = 0; i < proWeapon[weaponManager.currentWeaponType].level; i++)
            {
                if (proWeapon[weaponManager.currentWeaponType].level > 30)
                {
                    maxHealth += 20;
                    maxMana += 20;
                    atk += 4;
                    def += 4;
                }
                else if (proWeapon[weaponManager.currentWeaponType].level > 20)
                {
                    maxHealth += 15;
                    maxMana += 15;
                    atk += 3;
                    def += 3;
                }
                else if (proWeapon[weaponManager.currentWeaponType].level > 10)
                {
                    maxHealth += 10;
                    maxMana += 10;
                    atk += 2;
                    def += 2;
                }
                else
                {
                    maxHealth += 5;
                    maxMana += 5;
                    atk += 1;
                    def += 1;
                }
            }
            player.IncreaseStats(maxHealth, atk, def, maxMana);
        }

        private int CalculateRequiredXp()
        {
            int solveForRequiredXp = 0;
            for (int levelCycle = 1; levelCycle <= proWeapon[weaponManager.currentWeaponType].level; levelCycle++)
            {
                solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
            }
            return solveForRequiredXp / 4;
        }

        public int GetLevelWeapon()
        {
            return proWeapon[weaponManager.currentWeaponType].level;
        }

        private void SaveProficiencyToServer()
        {
            JSONUpdateProficiency jsonUpPro = new JSONUpdateProficiency();
            foreach (KeyValuePair<WeaponManager.WeaponType, ProficiencyWeapon> pspw in proWeapon)
            {
                Debug.Log($"{pspw.Value.weaponType} {pspw.Value.level} {pspw.Value.currentXp}");
                jsonUpPro.className = pspw.Value.weaponType switch
                {
                    WeaponManager.WeaponType.None => "none",
                    WeaponManager.WeaponType.Sword => "sword",
                    WeaponManager.WeaponType.BowQuiver => "bow_quiver",
                    _ => "none"
                };
                jsonUpPro.level = pspw.Value.level;
                jsonUpPro.exp = pspw.Value.currentXp;
                Debug.Log($"{jsonUpPro.className} {jsonUpPro.level} {jsonUpPro.exp}");
            }
            PlayerProficiencyMsg message = new PlayerProficiencyMsg
            {
                id = GetComponent<PlayerComponents>().PlayerScriptable.id,
                proficiency = jsonUpPro
            };
            NetworkClient.Send(message);
        }
    }
}
