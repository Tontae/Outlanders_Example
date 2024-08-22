using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace Outlander.Player
{
    public class PlayerSkillUI : MonoBehaviour
    {
        #region Variable
        [Header("Player Handler")]
        // public static PlayerSkillUI instance;
        public GameObject playerOutRef;
        private Outlander.Player.ProficiencySystem playerProficiency;
        private Outlander.Player.WeaponManager playerWeapon;
        [SerializeField] private Outlander.Player.WeaponManager.WeaponType currentWeapon;
        private Outlander.UI.PlayerUIActionHandler playerUIActionHandler;

        [Header("Skill Group")]
        [SerializeField] private GameObject skillGroup_1;
        private List<float> currentCooldownGroup_1;

        [SerializeField] private GameObject skillGroup_2;
        private List<float> currentCooldownGroup_2;

        private List<float> currentCooldown;

        [SerializeField] private GameObject swapSkillButton;

        [SerializeField] private GameObject leftItem;
        [SerializeField] private GameObject rightItem;
        [SerializeField] private GameObject lockedSkill;

        [System.Serializable]
        public class SkillCooldown
        {
            [SerializeField] public Image cooldownImg;
            [SerializeField] public TextMeshProUGUI cooldownTxt;
            [SerializeField] public float cooldown;
            [SerializeField] public bool isCooldown;
            // public float currentCooldown;
        }

        [System.Serializable]
        public class SkillGroup
        {
            [SerializeField] public List<SkillCooldown> skillCooldownGroup;
        }


        [Space(10), SerializeField] public List<SkillGroup> skillCooldownGroups = new List<SkillGroup>();

        // [SerializeField] private List<SkillCooldown> skillCooldownsFirstHalf = new List<SkillCooldown>();
        // [SerializeField] private List<SkillCooldown> skillCooldownsSecondHalf = new List<SkillCooldown>();
        [Space(10), SerializeField] public List<SkillCooldown> potionSkillCooldown = new List<SkillCooldown>();

        private List<GameObject> lockedSkillList;

        [Header("Potion")]
        private List<float> currentPotionCooldown;

        private bool onFirstSkillGroup = true;

        [Header("Rank Up Text")]
        [SerializeField] private GameObject rankUp_Obj;
        private float disappearTime;
        private Color textColor;
        private Color defaultTextColor;

        [Header("Map")]
        [SerializeField] private GameObject map;

        #endregion

        #region Start/Update
        private void OnEnable()
        {
            // PlayerInput playerInput = playerOutRef.gameObject.GetComponent<PlayerInput>();
            playerUIActionHandler = playerOutRef.gameObject.GetComponent<Outlander.UI.PlayerUIActionHandler>();

            playerUIActionHandler.OnUISwapSkill += SwapSkillGroup;
        }

        private void OnDisable()
        {
            playerUIActionHandler.OnUISwapSkill -= SwapSkillGroup;
        }

        private void Awake()
        {
            // instance = this;

            playerProficiency = playerOutRef.GetComponent<Outlander.Player.ProficiencySystem>();
            playerWeapon = playerOutRef.GetComponent<Outlander.Player.WeaponManager>();
            lockedSkillList = new List<GameObject>();
        }

        void Start()
        {
            if (!playerOutRef.GetComponent<Mirror.NetworkIdentity>().isLocalPlayer) return;
            InitSkill();
        }

        void Update()
        {
            if (!playerOutRef.GetComponent<Mirror.NetworkIdentity>().isLocalPlayer) return;
            CheckCurrentWepon();
            // InputHandler();
            UpdateCooldownOnGroup();
            UpdatePotionUI();
            UpdateLockedSkill();
            // UpdateRankUpText();
            // if (onFirstSkillGroup)
            // {
            //     UpdateUIOnGruop_1();
            // }
            // else
            // {
            //     UpdateUIOnGruop_2();
            // }

            UpdateCooldownUI();
        }

        #endregion

        #region SkillStatus
        private void CheckCurrentWepon()
        {
            if (currentWeapon == playerWeapon.currentWeaponType) return;

            // Debug.Log("InitSkill");
            InitSkill();

            switch (playerWeapon.currentWeaponType)
            {
                case Outlander.Player.WeaponManager.WeaponType.None:
                    lockedSkill.SetActive(false);
                    skillGroup_1.SetActive(false);
                    skillGroup_2.SetActive(false);
                    swapSkillButton.SetActive(false);
                    break;
                default:
                    lockedSkill.SetActive(true);
                    skillGroup_1.SetActive(true);
                    skillGroup_2.SetActive(false);
                    swapSkillButton.SetActive(true);
                    break;
            }
        }

        private void InitSkill()
        {
            currentWeapon = playerWeapon.currentWeaponType;

            // currentCooldownGroup_1 = new List<float>();
            // currentCooldownGroup_2 = new List<float>();

            currentCooldown = new List<float>();
            currentPotionCooldown = new List<float>();

            // currentCooldownGroup_1.Clear();
            // currentCooldownGroup_2.Clear();
            currentPotionCooldown.Clear();

            currentCooldown.Clear();
            lockedSkillList.Clear();

            for (int i = 0; i < skillCooldownGroups.Count; i++)
            {
                foreach (var item in skillCooldownGroups[i].skillCooldownGroup)
                {
                    currentCooldown.Add(0);
                }
            }

            // foreach (var skillCooldown in skillCooldownsFirstHalf)
            // {
            //     currentCooldownGroup_1.Add(0);
            // }
            // foreach (var skillCooldown in skillCooldownsSecondHalf)
            // {
            //     currentCooldownGroup_2.Add(0);
            // }

            foreach (var skillCooldown in potionSkillCooldown)
            {
                currentPotionCooldown.Add(0);
            }

            for (int i = 0; i < 4; i++)
            {
                lockedSkill.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                lockedSkillList.Add(lockedSkill.transform.GetChild(i).gameObject);
            }
        }

        void UpdateLockedSkill()
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerProficiency.proWeapon[playerWeapon.currentWeaponType].level >= ((i + 1) * 5))
                {
                    lockedSkillList[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    // if (lockedSkillList == null) return;
                    if (playerWeapon.currentWeaponType != Outlander.Player.WeaponManager.WeaponType.None)
                        lockedSkillList[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{playerWeapon.currentWeaponType}\nRank {((i + 1) * 5)}";
                    // if (lockedSkillList[i].GetComponentInChildren<TextMeshProUGUI>()) { Debug.Log($"lock not null"); }
                }
            }
        }

        #endregion

        #region UpdateCooldown & CooldownUI
        void UpdateCooldownOnGroup()
        {
            // if (!playerOutRef.GetComponent<Outlander.Player.PlayerOutlanderMovement>().grounded) return;
            //Subtract Cooldown
            for (int i = 0; i < currentCooldown.Count; i++)
            {
                currentCooldown[i] -= Time.deltaTime;
            }

            // for (int i = 0; i < currentCooldownGroup_1.Count; i++)
            // {
            //     if (currentCooldownGroup_1[i] > 0)
            //     {
            //         currentCooldownGroup_1[i] -= Time.deltaTime;
            //     }
            // }

            // for (int i = 0; i < currentCooldownGroup_2.Count; i++)
            // {
            //     if (currentCooldownGroup_2[i] > 0)
            //     {
            //         currentCooldownGroup_2[i] -= Time.deltaTime;
            //     }
            // }

            for (int i = 0; i < currentPotionCooldown.Count; i++)
            {
                if (currentPotionCooldown[i] > 0)
                {
                    currentPotionCooldown[i] -= Time.deltaTime;
                }
            }
        }

        void UpdateCooldownUI()
        {
            // int skillPalateIndex = 0;
            // if (onFirstSkillGroup)
            // {
            //     skillPalateIndex = 0;
            // }
            // else
            // {
            //     skillPalateIndex = 1;
            // }

            foreach (var item in skillCooldownGroups[GetSkillPalateIndex()].skillCooldownGroup)
            {
                if (playerOutRef.GetComponent<PlayerOutlanderStateMachine>().PlayerMP < 20f) return;

                item.cooldownImg.fillAmount = currentCooldown[GetSkillPalateIndex()] / item.cooldown;
                item.cooldownTxt.text = currentCooldown[GetSkillPalateIndex()].ToString("#");

                if (item.cooldownImg.fillAmount > 0)
                    item.isCooldown = true;
                else
                    item.isCooldown = false;
            }
        }

        // void UpdateUIOnGruop_1()
        // {
        //     //Update UI
        //     for (int i = 0; i < skillCooldownsFirstHalf.Count; i++)
        //     {
        //         if (playerOutRef.GetComponent<Outlander.Player.PlayerOutlander>().Mana < 20f) return;

        //         skillCooldownsFirstHalf[i].cooldownImg.fillAmount = currentCooldownGroup_1[i] / skillCooldownsFirstHalf[i].cooldown;
        //         skillCooldownsFirstHalf[i].cooldownTxt.text = currentCooldownGroup_1[i].ToString("#");
        //     }
        // }

        // void UpdateUIOnGruop_2()
        // {
        //     //Update UI
        //     for (int i = 0; i < skillCooldownsSecondHalf.Count; i++)
        //     {
        //         if (playerOutRef.GetComponent<Outlander.Player.PlayerOutlander>().Mana < 20f) return;

        //         skillCooldownsSecondHalf[i].cooldownImg.fillAmount = currentCooldownGroup_2[i] / skillCooldownsSecondHalf[i].cooldown;
        //         skillCooldownsSecondHalf[i].cooldownTxt.text = currentCooldownGroup_2[i].ToString("#");
        //     }
        // }

        void UpdatePotionUI()
        {
            // InventoryManager inventoryManager = playerOutRef.GetComponent<InventoryManager>();

            // for (int i = 0; i < potionSkillCooldown.Count; i++)
            // {
            //     switch (i)
            //     {
            //         case 0:
            //             potionSkillCooldown[i].cooldownImg.fillAmount = currentPotionCooldown[i] / potionSkillCooldown[i].cooldown;
            //             potionSkillCooldown[i].cooldownTxt.text = currentPotionCooldown[i].ToString("#");

            //             if (potionSkillCooldown[i].cooldownImg.fillAmount > 0)
            //                 potionSkillCooldown[i].isCooldown = true;
            //             else
            //                 potionSkillCooldown[i].isCooldown = false;
            //             break;
            //         case 1:
            //             potionSkillCooldown[i].cooldownImg.fillAmount = currentPotionCooldown[i] / potionSkillCooldown[i].cooldown;
            //             potionSkillCooldown[i].cooldownTxt.text = currentPotionCooldown[i].ToString("#");

            //             if (potionSkillCooldown[i].cooldownImg.fillAmount > 0)
            //                 potionSkillCooldown[i].isCooldown = true;
            //             else
            //                 potionSkillCooldown[i].isCooldown = false;
            //             break;
            //     }
            // }



            // if (inventoryManager.IsEnoughHealthPotion())
            // {
            //     // Debug.Log($"Left item is enough : {inventoryManager.IsEnoughHealthPotion()}");
            //     leftItem.SetActive(true);
            // }
            // else
            // {
            //     // Debug.Log($"Left item is enough : {inventoryManager.IsEnoughHealthPotion()}");
            //     leftItem.SetActive(false);
            //     potionSkillCooldown[0].cooldownTxt.text = "";
            // }

            // if (inventoryManager.IsEnoughManaPotion())
            // {
            //     // Debug.Log($"Right item is enough : {inventoryManager.IsEnoughManaPotion()}");
            //     rightItem.SetActive(true);
            // }
            // else
            // {
            //     // Debug.Log($"Right item is enough : {inventoryManager.IsEnoughManaPotion()}");
            //     rightItem.SetActive(false);
            //     potionSkillCooldown[1].cooldownTxt.text = "";
            // }

        }

        void UpdateRankUpText()
        {
            if (rankUp_Obj.gameObject.activeSelf == true)
            {
                disappearTime -= Time.deltaTime;

                if (disappearTime < 0)
                {
                    rankUp_Obj.SetActive(false);
                    //float disappearSpeed = 1f;
                    //textColor.a -= disappearSpeed * Time.deltaTime;
                    //rankUp_txt.color = textColor;
                    //if (textColor.a < 0)
                    //{

                    //    rankUp_txt.gameObject.SetActive(false);
                    //}
                }
            }

        }

        #endregion

        #region Scene
        public void ShowRankupTxt()
        {
            disappearTime = 3f;
            rankUp_Obj.SetActive(true);
            //rankUp_txt.gameObject.SetActive(true);
            //disappearTime = 1f;
            //rankUp_txt.color = defaultTextColor;
            //textColor = defaultTextColor;

        }

        public void SwapSkillGroup()
        {
            if (onFirstSkillGroup)
            {
                skillGroup_1.SetActive(false);
                skillGroup_2.SetActive(true);
                onFirstSkillGroup = false;
            }
            else
            {
                skillGroup_1.SetActive(true);
                skillGroup_2.SetActive(false);
                onFirstSkillGroup = true;
            }
        }

        //if (inventoryManager.IsEnoughIteminSlot1())
        //{
        //    // Debug.Log($"Left item is enough : {inventoryManager.IsEnoughHealthPotion()}");
        //    leftItem.SetActive(true);
        //}
        //else
        //{
        //    // Debug.Log($"Left item is enough : {inventoryManager.IsEnoughHealthPotion()}");
        //    leftItem.SetActive(false);
        //    potionSkillCooldown[0].cooldownTxt.text = "";
        //}

        public void ToggleMap()
        {
            map.SetActive(!map.activeSelf);
        }
        //if (inventoryManager.IsEnoughIteminSlot2())
        //{
        //    // Debug.Log($"Right item is enough : {inventoryManager.IsEnoughManaPotion()}");
        //    rightItem.SetActive(true);
        //}
        //else
        //{
        //    // Debug.Log($"Right item is enough : {inventoryManager.IsEnoughManaPotion()}");
        //    rightItem.SetActive(false);
        //    potionSkillCooldown[1].cooldownTxt.text = "";
        //}

        public void ActiveSkill(int index)
        {
            // if (onFirstSkillGroup)
            // {
            //     if (currentCooldownGroup_1[index] <= 0)
            //     {
            //         currentCooldownGroup_1[index] = skillCooldownsFirstHalf[index].cooldown;
            //     }
            // }
            // else
            // {
            //     if (currentCooldownGroup_2[index] <= 0)
            //     {
            //         currentCooldownGroup_2[index] = skillCooldownsSecondHalf[index].cooldown;
            //     }
            // }

            // int skillPalateIndex = 0;
            // if (onFirstSkillGroup)
            // {
            //     skillPalateIndex = 0;
            // }
            // else
            // {
            //     skillPalateIndex = 1;
            // }

            if (currentCooldown[index] <= 0)
            {
                currentCooldown[index] = skillCooldownGroups[GetSkillPalateIndex()].skillCooldownGroup[index].cooldown;
            }
        }

        public void UsePotion(int index)
        {
            if (currentPotionCooldown[index] <= 0)
            {
                currentPotionCooldown[index] = potionSkillCooldown[index].cooldown;
            }
        }

        #endregion

        public float GetSkillCooldown(int index)
        {
            // int skillPalateIndex = 0;
            // if (onFirstSkillGroup)
            // {
            //     skillPalateIndex = 0;
            // }
            // else
            // {
            //     skillPalateIndex = 1;
            // }

            return skillCooldownGroups[GetSkillPalateIndex()].skillCooldownGroup[index].cooldown;
        }

        public bool GetBoolSkillCooldown(int index)
        {
            // int skillPalateIndex = 0;
            // if (onFirstSkillGroup)
            // {
            //     skillPalateIndex = 0;
            // }
            // else
            // {
            //     skillPalateIndex = 1;
            // }

            return skillCooldownGroups[GetSkillPalateIndex()].skillCooldownGroup[index].isCooldown;
        }

        // public float GetCoolDownFirstHalf(int index)
        // {
        //     return skillCooldownsFirstHalf[index].cooldown;
        // }

        // public float GetCooldownSecondHalf(int index)
        // {
        //     return skillCooldownsSecondHalf[index].cooldown;
        // }

        private int GetSkillPalateIndex()
        {
            // if (onFirstSkillGroup)
            // {
            //     skillPalateIndex = 0;
            // }
            // else
            // {
            //     skillPalateIndex = 1;
            // }

            return (onFirstSkillGroup) ? 0 : 1;
        }

        public float GetCooldownItem(int index) => potionSkillCooldown[index].cooldown;
        public bool GetBoolItemCooldown(int index) => potionSkillCooldown[index].isCooldown;

        protected virtual void OnSwapSkill(InputValue value)
        {
            // Debug.Log("OnSwapSkill");
            SwapSkillGroup();
        }
    }

}
