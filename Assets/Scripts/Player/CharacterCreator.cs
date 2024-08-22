using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.Player;
using UnityEngine.UI;
using PsychoticLab;

namespace Outlander.Character
{
    public class CharacterCreator : MonoBehaviour
    {
        [Header("Camera")]
        public GameObject profileCamera;

        [Header("Material")]
        public Material mat;

        [Header("Skin Colors")]
        public Color[] skinColor = new Color[4];

        [Header("Eye Colors")]
        public Color[] eyesColor = new Color[6];

        [Header("Hair Colors")]
        public Color[] hairColor = new Color[6];

        public SkinnedMeshRenderer[] body;
        public SkinnedMeshRenderer[] suit;
        public AppearanceScriptable appearanceScriptable;
        public Gender gender = Gender.Male;

        //holds the acttive bits, should be useful for saving
        private int[] bodyEquipped;
        private int[] suitEquipped;
        private int maxBodyParts = 0;
        private int maxSuitParts = 0;

        //dictionary to keep all posible items for fast reference
        private List<Mesh>[,] allObjects;

        //bool isLoaded = false;
        public bool isSummary = false;
        #region Init and Helpers
        // Start is called before the first frame update
        void Start()
        {
            InitBody();
            EnableCharacter();
        }

        public void InitBody()
        {
            //isLoaded = false;
            maxBodyParts = Enum.GetValues(typeof(BodyParts)).Length;
            maxSuitParts = Enum.GetValues(typeof(BodySuits)).Length;

            bodyEquipped = new int[maxBodyParts];
            suitEquipped = new int[maxSuitParts];

            body = new SkinnedMeshRenderer[maxBodyParts];
            suit = new SkinnedMeshRenderer[maxSuitParts];

            allObjects = new List<Mesh>[2, maxBodyParts];

            BuildLists();

            for (int i = 0; i < maxBodyParts; i++)
            {
                bodyEquipped[i] = -1;
            }
            //default startup as male
            bodyEquipped[(int)BodyParts.Face] = 0;
            bodyEquipped[(int)BodyParts.Eyebrow] = 0;
            bodyEquipped[(int)BodyParts.Torso] = 0;
            bodyEquipped[(int)BodyParts.Arm_Upper_Right] = 0;
            bodyEquipped[(int)BodyParts.Arm_Upper_Left] = 0;
            bodyEquipped[(int)BodyParts.Arm_Lower_Right] = 0;
            bodyEquipped[(int)BodyParts.Arm_Lower_Left] = 0;
            bodyEquipped[(int)BodyParts.Hand_Right] = 0;
            bodyEquipped[(int)BodyParts.Hand_Left] = 0;
            bodyEquipped[(int)BodyParts.Hips] = 0;
            bodyEquipped[(int)BodyParts.Leg_Right] = 0;
            bodyEquipped[(int)BodyParts.Leg_Left] = 0;

            SetDefaultAppearance();
        }

        public void SetDefaultAppearance()
        {
            SetSkin(skinColor[0]);
            SetHairColor(hairColor[0]);
            SetEyebrowColor(eyesColor[0]);
            SetGender("Male");
            SetFace(0, "");
            SetHair(0, "");
            SetEyebrow(0, "");
            SetBeard(0, "");
        }

        public void EnableCharacter()
        {
            //activate the look
            for (int i = 0; i < maxBodyParts; i++)
            {
                ActivateItem(i, bodyEquipped[i]);
            }
        }

        public void DisableCharacter()
        {
            //deactivate the look
            for (int i = 0; i < maxBodyParts; i++)
            {
                DeactivateItem(i, bodyEquipped[i]);
            }
        }

        private void DeactivateItem(int itemType, int itemIndex)
        {
            //if we had a previous item 
            if (bodyEquipped[itemType] != -1 && bodyEquipped[itemType] < allObjects[(int)gender, itemType].Count)
            {
                body[itemType].sharedMesh = null;
            }
        }

        // enable game object and add it to the enabled objects list
        private void ActivateItem(int itemType, int itemIndex)
        {
            if (itemIndex >= allObjects[(int)gender, itemType].Count)
            {
                itemIndex = -1;
            }
            //if we had a previous item 
            if (bodyEquipped[itemType] != -1)
            {
                if (bodyEquipped[itemType] < allObjects[(int)gender, itemType].Count)
                {
                    body[itemType].sharedMesh = null;
                }
                if (itemIndex != -1)
                {
                    bodyEquipped[itemType] = itemIndex;
                    body[itemType].sharedMesh = allObjects[(int)gender, itemType][itemIndex];
                }
            }
            else
            {
                if (itemIndex != -1)
                {
                    bodyEquipped[itemType] = itemIndex;
                    body[itemType].sharedMesh = allObjects[(int)gender, itemType][itemIndex];
                }
            }
        }
        #endregion

        #region Setting Appearance
        public void SetFace(int index, string name)
        {
            if (index < allObjects[(int)gender, (int)BodyParts.Face].Count)
            {
                ActivateItem((int)BodyParts.Face, index);
            }
        }
        

        public void SetEyebrow(int index, string name)
        {
            if (index < allObjects[(int)gender, (int)BodyParts.Eyebrow].Count)
            {
                ActivateItem((int)BodyParts.Eyebrow, index);
            }
        }

        public void SetHair(int index, string name)
        {
            //handle no hair option
            if(index < allObjects[(int)gender, (int)BodyParts.Hair].Count)
            {
                ActivateItem((int)BodyParts.Hair, index);
            }
        }

        //Set bear
        public void SetBeard(int index, string name)
        {
            if (index < allObjects[(int)gender, (int)BodyParts.Beard].Count)
            {
                ActivateItem((int)BodyParts.Beard, index);
            }
        }

        public void SetSkin(Color _color)
        {
            mat.SetColor("_Color_Skin_1", _color);
            float currentHue = 0f;
            float currentSat = 0f;
            float currentVal = 0f;
            Color.RGBToHSV(_color, out currentHue, out currentSat, out currentVal);
            currentVal -= 0.1f;
            mat.SetColor("_Color_Beard_2", Color.HSVToRGB(currentHue, currentSat, currentVal));
            currentVal -= 0.2f;
            mat.SetColor("_Color_Skin_2", Color.HSVToRGB(currentHue, currentSat, currentVal));
            currentVal -= 0.2f;
            mat.SetColor("_Color_Tattoo", Color.HSVToRGB(currentHue, currentSat, currentVal));
        }

        public void SetHairColor(Color color)
        {
            mat.SetColor("_Color_Hair", color);
        }

        public void SetEyebrowColor(Color color)
        {
            mat.SetColor("_Color_Eye", Color.black);
            mat.SetColor("_Color_Eyebrow", color);
        }

        public void SetBeardColor(Color color)
        {
            mat.SetColor("_Color_Beard_1", color);
        }

        public void SetGender(string name)
        {
            Gender newGender;

            if (name.CompareTo("Male") == 0)
            {
                newGender = Gender.Male;
            }
            else
            {
                newGender = Gender.Female;
            }
            if (newGender != gender)
            {
                DisableCharacter();
                gender = newGender;
                EnableCharacter();
            }
        }
        #endregion

        #region Setting Equipment

        #endregion

        #region Builders

        private void BuildLists()
        {
            BuildBody((int)BodyParts.Face, "Chr_Face");
            BuildBody((int)BodyParts.Hair, "Chr_Hair");
            BuildBody((int)BodyParts.Eyebrow, "Chr_Eyebrow");
            BuildBody((int)BodyParts.Beard, "Chr_Beard");
            BuildBody((int)BodyParts.Torso, "Chr_Torso");
            BuildBody((int)BodyParts.Arm_Upper_Left, "Chr_ArmUpperLeft");
            BuildBody((int)BodyParts.Arm_Upper_Right, "Chr_ArmUpperRight");
            BuildBody((int)BodyParts.Arm_Lower_Left, "Chr_ArmLowerLeft");
            BuildBody((int)BodyParts.Arm_Lower_Right, "Chr_ArmLowerRight");
            BuildBody((int)BodyParts.Hand_Left, "Chr_HandRight");
            BuildBody((int)BodyParts.Hand_Right, "Chr_HandLeft");
            BuildBody((int)BodyParts.Hips, "Chr_Hips");
            BuildBody((int)BodyParts.Leg_Left, "Chr_LegLeft");
            BuildBody((int)BodyParts.Leg_Right, "Chr_LegRight");

            allObjects[0, (int)BodyParts.Face] = appearanceScriptable.maleFace;
            allObjects[0, (int)BodyParts.Hair] = appearanceScriptable.hair;
            allObjects[0, (int)BodyParts.Eyebrow] = appearanceScriptable.maleEyebrow;
            allObjects[0, (int)BodyParts.Beard] = appearanceScriptable.beard;
            allObjects[0, (int)BodyParts.Torso] = appearanceScriptable.maleTorso;
            allObjects[0, (int)BodyParts.Arm_Lower_Left] = appearanceScriptable.maleArmLowerLeft;
            allObjects[0, (int)BodyParts.Arm_Lower_Right] = appearanceScriptable.maleArmLowerRight;
            allObjects[0, (int)BodyParts.Arm_Upper_Left] = appearanceScriptable.maleArmUpperLeft;
            allObjects[0, (int)BodyParts.Arm_Upper_Right] = appearanceScriptable.maleArmUpperRight;
            allObjects[0, (int)BodyParts.Hand_Left] = appearanceScriptable.maleHandLeft;
            allObjects[0, (int)BodyParts.Hand_Right] = appearanceScriptable.maleHandRight;
            allObjects[0, (int)BodyParts.Hips] = appearanceScriptable.maleHips;
            allObjects[0, (int)BodyParts.Leg_Left] = appearanceScriptable.maleLegLeft;
            allObjects[0, (int)BodyParts.Leg_Right] = appearanceScriptable.maleLegRight;

            allObjects[1, (int)BodyParts.Face] = appearanceScriptable.femaleFace;
            allObjects[1, (int)BodyParts.Hair] = appearanceScriptable.hair;
            allObjects[1, (int)BodyParts.Eyebrow] = appearanceScriptable.femaleEyebrow;
            allObjects[1, (int)BodyParts.Beard] = appearanceScriptable.beard;
            allObjects[1, (int)BodyParts.Torso] = appearanceScriptable.femaleTorso;
            allObjects[1, (int)BodyParts.Arm_Lower_Left] = appearanceScriptable.femaleArmLowerLeft;
            allObjects[1, (int)BodyParts.Arm_Lower_Right] = appearanceScriptable.femaleArmLowerRight;
            allObjects[1, (int)BodyParts.Arm_Upper_Left] = appearanceScriptable.femaleArmUpperLeft;
            allObjects[1, (int)BodyParts.Arm_Upper_Right] = appearanceScriptable.femaleArmUpperRight;
            allObjects[1, (int)BodyParts.Hand_Left] = appearanceScriptable.femaleHandLeft;
            allObjects[1, (int)BodyParts.Hand_Right] = appearanceScriptable.femaleHandRight;
            allObjects[1, (int)BodyParts.Hips] = appearanceScriptable.femaleHips;
            allObjects[1, (int)BodyParts.Leg_Left] = appearanceScriptable.femaleLegLeft;
            allObjects[1, (int)BodyParts.Leg_Right] = appearanceScriptable.femaleLegRight;

            BuildSuit((int)BodySuits.HelmetAttachment, "Chr_HelmetAttachment");
            BuildSuit((int)BodySuits.Helmet, "Chr_Helmet");
            BuildSuit((int)BodySuits.Cuirass, "Chr_Cuirass");
            BuildSuit((int)BodySuits.ShoulderAttach_Left, "Chr_ShoulderAttachLeft");
            BuildSuit((int)BodySuits.ShoulderAttach_Right, "Chr_ShoulderAttachRight");
            BuildSuit((int)BodySuits.Arm_Upper_Left, "Chr_ArmUpperLeft");
            BuildSuit((int)BodySuits.Arm_Upper_Right, "Chr_ArmUpperRight");
            BuildSuit((int)BodySuits.Arm_Lower_Left, "Chr_ArmLowerLeft");
            BuildSuit((int)BodySuits.Arm_Lower_Right, "Chr_ArmLowerRight");
            BuildSuit((int)BodySuits.Hand_Left, "Chr_HandRight");
            BuildSuit((int)BodySuits.Hand_Right, "Chr_HandLeft");
            BuildSuit((int)BodySuits.Viel, "Chr_Viel");
            BuildSuit((int)BodySuits.Cuisses, "Chr_Cuisses");
            BuildSuit((int)BodySuits.KneeAttach_Left, "Chr_KneeAttachLeft");
            BuildSuit((int)BodySuits.KneeAttach_Right, "Chr_KneeAttachRight");
            BuildSuit((int)BodySuits.Leg_Left, "Chr_LegLeft");
            BuildSuit((int)BodySuits.Leg_Right, "Chr_LegRight");
        }

        // called from the BuildLists method
        void BuildBody(int itemIndex, string characterPart)
        {
            SkinnedMeshRenderer[] rootTransform = transform.GetChild(1).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            SkinnedMeshRenderer targetRoot = null;

            foreach (SkinnedMeshRenderer t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }

            body[itemIndex] = targetRoot;

            SkinnedMeshRenderer go = targetRoot;

            if (!mat)
                mat = go.material;
            else
                go.material = mat;
        }

        void BuildSuit(int itemIndex, string characterPart)
        {
            SkinnedMeshRenderer[] rootTransform = transform.GetChild(2).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            SkinnedMeshRenderer targetRoot = null;

            foreach (SkinnedMeshRenderer t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }

            suit[itemIndex] = targetRoot;
        }

        public void OnDisableSuit()
        {
            if (bodyEquipped == null) return;
            //DeactivateItem((int)BodyParts.Main_Weapon, equipped[(int)BodyParts.Main_Weapon]);
            //DeactivateItem((int)BodyParts.Sub_Weapon, equipped[(int)BodyParts.Sub_Weapon]);
            //DeactivateItem((int)BodyParts.Helm, equipped[(int)BodyParts.Helm]);
            //DeactivateItem((int)BodyParts.Cuirass, equipped[(int)BodyParts.Cuirass]);
            //DeactivateItem((int)BodyParts.Cuisses, equipped[(int)BodyParts.Cuisses]);
            //DeactivateItem((int)BodyParts.Gauntlets, equipped[(int)BodyParts.Gauntlets]);
            //DeactivateItem((int)BodyParts.Greaves, equipped[(int)BodyParts.Greaves]);
            //DeactivateItem((int)BodyParts.Viel, equipped[(int)BodyParts.Viel]);
        }
        #endregion

        #region Save and Load

        public void RestoreForSummary(PlayerScriptable character)
        {
            isSummary = true;
            RestoreState(character);
        }

        public void RestoreState(PlayerScriptable character)
        {
            switch (character.gender)
            {
                case 0:
                    SetGender("Male");
                    break;
                case 1:
                    SetGender("Female");
                    break;
            }

            if (character.appearance[(int)BodyParts.Face].id != "")
            {
                for (int i = 0; i < allObjects[(int)gender, (int)BodyParts.Face].Count; i++)
                {
                    if (CheckAppearance(allObjects[(int)gender, (int)BodyParts.Face][i].name, character.appearance[(int)BodyParts.Face].id))
                    {
                        //Debug.Log($"This is : {allObjects[(int)gender, (int)BodyParts.Face][i].name} and {temp.id}");
                        SetFace(i, character.appearance[(int)BodyParts.Face].id);
                    }
                }
            }
            if (character.appearance[(int)BodyParts.Hair].id != "")
            {
                for (int i = 0; i < allObjects[(int)gender, (int)BodyParts.Hair].Count; i++)
                {
                    if (CheckAppearance(allObjects[(int)gender, (int)BodyParts.Hair][i].name, character.appearance[(int)BodyParts.Hair].id))
                    {
                        //Debug.Log($"This is : {allObjects[(int)gender, (int)BodyParts.Hair][i].name} and {temp.id}");
                        SetHair(i, character.appearance[(int)BodyParts.Hair].id);
                    }
                }
            }
            if (character.appearance[(int)BodyParts.Eyebrow].id != "")
            {
                for (int i = 0; i < allObjects[(int)gender, (int)BodyParts.Eyebrow].Count; i++)
                {
                    if (CheckAppearance(allObjects[(int)gender, (int)BodyParts.Eyebrow][i].name, character.appearance[(int)BodyParts.Eyebrow].id))
                    {
                        //Debug.Log($"This is : {allObjects[(int)gender, (int)BodyParts.Eyebrow][i].name} and {temp.id}");
                        SetEyebrow(i, character.appearance[(int)BodyParts.Eyebrow].id);
                    }
                }
            }
            if (character.appearance[(int)BodyParts.Beard].id != "")
            {
                for (int i = 0; i < allObjects[(int)gender, (int)BodyParts.Beard].Count; i++)
                {
                    if (CheckAppearance(allObjects[(int)gender, (int)BodyParts.Beard][i].name, character.appearance[(int)BodyParts.Beard].id))
                    {
                        //Debug.Log($"This is : {allObjects[(int)gender, (int)BodyParts.Beard][i].name} and {temp.id}");
                        SetBeard(i, character.appearance[(int)BodyParts.Beard].id);
                    }
                }
            }

            Color skinColorLoad = character.appearance[(int)BodyParts.Face].color.ToColor();
            SetSkin(skinColorLoad);
            Color hairColorLoad = character.appearance[(int)BodyParts.Hair].color.ToColor();
            SetHairColor(hairColorLoad);
            Color eyesbrowColorLoad = character.appearance[(int)BodyParts.Eyebrow].color.ToColor();
            SetEyebrowColor(eyesbrowColorLoad);
            Color beardColorLoad = character.appearance[(int)BodyParts.Beard].color.ToColor();
            SetBeardColor(beardColorLoad);

            isSummary = false;
            //isLoaded = true;

            EnableCharacter();
        }

        public void OnInitMesh(bool _close)
        {
            OnCloseHead(_close);
            OnCloseCuirass(_close);
            OnCloseGauntlets(_close);
            OnCloseCuisess(_close);
            OnCloseGreave(_close);

            foreach (SkinnedMeshRenderer tmp in suit)
            {
                tmp.sharedMesh = null;
            }
        }

        public void OnCloseHead(bool _close)
        {
            body[(int)BodyParts.Face].gameObject.SetActive(!_close);
            body[(int)BodyParts.Hair].gameObject.SetActive(!_close);
            body[(int)BodyParts.Eyebrow].gameObject.SetActive(!_close);
            body[(int)BodyParts.Beard].gameObject.SetActive(!_close);
        }

        public void OnCloseCuirass(bool _close)
        {
            body[(int)BodyParts.Torso].gameObject.SetActive(!_close);
            body[(int)BodyParts.Arm_Upper_Left].gameObject.SetActive(!_close);
            body[(int)BodyParts.Arm_Upper_Right].gameObject.SetActive(!_close);
        }

        public void OnCloseGauntlets(bool _close)
        {
            body[(int)BodyParts.Arm_Lower_Left].gameObject.SetActive(!_close);
            body[(int)BodyParts.Arm_Lower_Right].gameObject.SetActive(!_close);
            body[(int)BodyParts.Hand_Left].gameObject.SetActive(!_close);
            body[(int)BodyParts.Hand_Right].gameObject.SetActive(!_close);
        }

        public void OnCloseCuisess(bool _close)
        {
            body[(int)BodyParts.Hips].gameObject.SetActive(!_close);
        }

        public void OnCloseGreave(bool _close)
        {
            body[(int)BodyParts.Leg_Left].gameObject.SetActive(!_close);
            body[(int)BodyParts.Leg_Right].gameObject.SetActive(!_close);
        }
        #endregion
        private bool CheckAppearance(string name, string playerId)
        {
            if (name.Split("_")[name.Split("_").Length - 1] == playerId.Split("_")[playerId.Split("_").Length - 1])
                return true;
            else
                return false;
        }
    }
}