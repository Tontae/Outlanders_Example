using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Outlander.Player
{
    public class LevelSystem : NetworkBehaviour
    {

        [SyncVar, SerializeField] private int level;
        public int Level { get => level; set => level = value; }
        [SyncVar] public float currentXp;
        [SyncVar] public float requiredXp;
        [Range(1f, 300f)]
        public float additionMultiplier = 300;
        [Range(2f, 4f)]
        public float powerMultiplier = 2;
        [Range(7f, 34f)]
        public float divisionMultiplier = 7;
        protected float lerpTimer;
        protected float delayTimer;

        [SerializeField]
        protected TMP_Text levelTxt, xpTxt, xpPct_txt;

        private AudioPlayer audioPlayer;

        public Image frontXpBar;

        public Image backXpBar;

        private void Awake()
        {
            audioPlayer = GetComponent<AudioPlayer>();
        }
        // Start is called before the first frame update

        protected virtual void Start()
        {
            if (isLocalPlayer || isServer)
            {
                requiredXp = CalculateRequiredXp();
                frontXpBar.fillAmount = currentXp / requiredXp;

                backXpBar.fillAmount = currentXp / requiredXp;
            }

            //LevelUp();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer && !isServer) return;
            if (isLocalPlayer)
            {
                UpdateXpUI();
            }
            
        }

        public virtual void UpdateXpUI()
        {
            levelTxt.text = "Lv." + level.ToString();
            float xpFraction = currentXp / requiredXp;
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
            xpTxt.text = currentXp.ToString("0") + "/" + requiredXp.ToString("0");
            xpPct_txt.text = "Exp " + ((currentXp / requiredXp) * 100).ToString("0") + "%";
        }

        public void GainEXPFlatRate(float xpGain)
        {
            currentXp += xpGain;
            lerpTimer = 0f;
            delayTimer = 0f;
            while (currentXp > requiredXp)
            {
                LevelUp();
            }
        }

        public void GainExpScalable(float xpGained, int passedLevel)
        {
            if (passedLevel < level)
            {
                float multiplier = 1 + (level - passedLevel);
                currentXp += xpGained * multiplier;
            }
            else
            {
                currentXp += xpGained;
            }
            lerpTimer = 0f;
            delayTimer = 0f;
        }

        public void LevelUp()
        {
            level++;
            frontXpBar.fillAmount = 0f;

            backXpBar.fillAmount = 0f;

            currentXp = Mathf.RoundToInt(currentXp - requiredXp);
            requiredXp = CalculateRequiredXp();

            Debug.Log($"Level system > isServer:{isServer} isLocal:{isLocalPlayer}");

            int addPointvalue = 5;
            GetComponent<PlayerStatisticManager>().AddPoint(addPointvalue);
            //if (isLocalPlayer)
                //GetComponent<PlayerStatisticManager>().CmdAddPoint(addPointvalue);
            if (!isLocalPlayer) return;
            audioPlayer.playSound(0);
        }

        private int CalculateRequiredXp()
        {
            int solveForRequiredXp = 0;
            for (int levelCycle = 1; levelCycle <= level; levelCycle++)
            {
                solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
            }
            return solveForRequiredXp / 4;
        }
    }

}
