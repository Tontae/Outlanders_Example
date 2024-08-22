using Mirror;
using Outlander.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustume : PlayerElements
{
    private SkinnedMeshRenderer helmet;
    private SkinnedMeshRenderer helmProp;
    private SkinnedMeshRenderer cuirass;
    private SkinnedMeshRenderer viel;
    private SkinnedMeshRenderer shoulder_Left;
    private SkinnedMeshRenderer shoulder_Right;
    private SkinnedMeshRenderer arm_Upper_Left;
    private SkinnedMeshRenderer arm_Upper_Right;
    private SkinnedMeshRenderer arm_Lower_Left;
    private SkinnedMeshRenderer arm_Lower_Right;
    private SkinnedMeshRenderer hand_Left;
    private SkinnedMeshRenderer hand_Right;
    private SkinnedMeshRenderer cuisess;
    private SkinnedMeshRenderer knee_Left;
    private SkinnedMeshRenderer knee_Right;
    private SkinnedMeshRenderer leg_Left;
    private SkinnedMeshRenderer leg_Right;

    private Transform main_weapon_parent;

    public SkinnedMeshRenderer Helmet
    {
        get { return helmet; }
        set
        {
            if (helmet != value)
            {
                helmet = value;
            }
        }
    }
    public SkinnedMeshRenderer HelmProp
    {
        get { return helmProp; }
        set
        {
            if (helmProp != value)
            {
                helmProp = value;
            }
        }
    }
    public SkinnedMeshRenderer Cuirass
    {
        get { return cuirass; }
        set
        {
            if (cuirass != value)
            {
                cuirass = value;
            }
        }
    }
    public SkinnedMeshRenderer Viel
    {
        get { return viel; }
        set
        {
            if (viel != value)
            {
                viel = value;
            }
        }
    }
    public SkinnedMeshRenderer Shoulder_Left
    {
        get { return shoulder_Left; }
        set
        {
            if (shoulder_Left != value)
            {
                shoulder_Left = value;
            }
        }
    }
    public SkinnedMeshRenderer Shoulder_Right
    {
        get { return shoulder_Right; }
        set
        {
            if (shoulder_Right != value)
            {
                shoulder_Right = value;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Upper_Left
    {
        get { return arm_Upper_Left; }
        set
        {
            if (arm_Upper_Left != value)
            {
                arm_Upper_Left = value;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Upper_Right
    {
        get { return arm_Upper_Right; }
        set
        {
            if (arm_Upper_Right != value)
            {
                arm_Upper_Right = value;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Lower_Left
    {
        get { return arm_Lower_Left; }
        set
        {
            if (arm_Lower_Left != value)
            {
                arm_Lower_Left = value;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Lower_Right
    {
        get { return arm_Lower_Right; }
        set
        {
            if (arm_Lower_Right != value)
            {
                arm_Lower_Right = value;
            }
        }
    }
    public SkinnedMeshRenderer Hand_Left
    {
        get { return hand_Left; }
        set
        {
            if (hand_Left != value)
            {
                hand_Left = value;
            }
        }
    }
    public SkinnedMeshRenderer Hand_Right
    {
        get { return hand_Right; }
        set
        {
            if (hand_Right != value)
            {
                hand_Right = value;
            }
        }
    }
    public SkinnedMeshRenderer Cuisess
    {
        get { return cuisess; }
        set
        {
            if (cuisess != value)
            {
                cuisess = value;
            }
        }
    }
    public SkinnedMeshRenderer Knee_Left
    {
        get { return knee_Left; }
        set
        {
            if (knee_Left != value)
            {
                knee_Left = value;
            }
        }
    }
    public SkinnedMeshRenderer Knee_Right
    {
        get { return knee_Right; }
        set
        {
            if (knee_Right != value)
            {
                knee_Right = value;
            }
        }
    }
    public SkinnedMeshRenderer Leg_Left
    {
        get { return leg_Left; }
        set
        {
            if (leg_Left != value)
            {
                leg_Left = value;
            }
        }
    }
    public SkinnedMeshRenderer Leg_Right
    {
        get { return leg_Right; }
        set
        {
            if (leg_Right != value)
            {
                leg_Right = value;
            }
        }
    }

    SkinnedMeshRenderer[] rootTransform;

    //[Header("Upper")]
    //public Mesh helmet_Mesh;
    //public Mesh helmProp_Mesh;
    //public Mesh cuirass_Mesh;
    //public Mesh viel_Mesh;
    //public Mesh shoulder_Left_Mesh;
    //public Mesh shoulder_Right_Mesh;
    //public Mesh arm_Upper_Left_Mesh;
    //public Mesh arm_Upper_Right_Mesh;
    //public Mesh arm_Lower_Left_Mesh;
    //public Mesh arm_Lower_Right_Mesh;
    //public Mesh hand_Left_Mesh;
    //public Mesh hand_Right_Mesh;

    //[Header("Lower")]
    //public Mesh cuisess_Mesh;
    //public Mesh knee_Left_Mesh;
    //public Mesh knee_Right_Mesh;
    //public Mesh leg_Left_Mesh;
    //public Mesh leg_Right_Mesh;

    public GameObject mainWeapon;
    public GameObject subWeapon;

    private void Start()
    {
        if (isServer) return;

        rootTransform = transform.GetChild(2).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        main_weapon_parent = FindHandPart("Chr_MainWeapon");
        Helmet = FindCustumePart("Chr_Helmet");
        HelmProp = FindCustumePart("Chr_HelmetAttachment");
        Cuirass = FindCustumePart("Chr_Cuirass");
        Viel = FindCustumePart("Chr_Viel");
        Shoulder_Left = FindCustumePart("Chr_ShoulderAttachLeft");
        Shoulder_Right = FindCustumePart("Chr_ShoulderAttachRight");
        Arm_Upper_Left = FindCustumePart("Chr_ArmUpperLeft");
        Arm_Upper_Right = FindCustumePart("Chr_ArmUpperRight");
        Arm_Lower_Left = FindCustumePart("Chr_ArmLowerLeft");
        Arm_Lower_Right = FindCustumePart("Chr_ArmLowerRight");
        Hand_Left = FindCustumePart("Chr_HandLeft");
        Hand_Right = FindCustumePart("Chr_HandRight");
        Cuisess = FindCustumePart("Chr_Cuisses");
        Knee_Right = FindCustumePart("Chr_KneeAttachLeft");
        Knee_Left = FindCustumePart("Chr_KneeAttachRight");
        Leg_Left = FindCustumePart("Chr_LegLeft");
        Leg_Right = FindCustumePart("Chr_LegRight");

        mainWeapon = null;
        subWeapon = null;
    }

    SkinnedMeshRenderer FindCustumePart(string _name)
    {
        foreach (SkinnedMeshRenderer t in rootTransform)
        {
            if (t.gameObject.name == _name)
            {
                return t;
            }
        }
        return null;
    }

    Transform FindHandPart(string _name)
    {
        foreach (Transform t in transform.GetChild(0).GetComponentsInChildren<Transform>())
        {
            if (t.gameObject.name == _name)
            {
                return t;
            }
        }
        return null;
    }

    public void SetCustume()
    {
        if(Player.PlayerScriptable != null)
            SetEquipmentAppearance(Player.PlayerScriptable.equipeditemList);
    }

    [ClientRpc]
    public void RpcSetEquipmentAppearance(string[] _item)
    {
        SetEquipmentAppearance(_item);
    }

    public void SetEquipmentAppearance(string[] _item)
    {
        for (int i = 0; i < _item.Length; i++)
        {
            switch (i)
            {
                case (int)SuitParts.Main_Weapon:
                    if (mainWeapon != null)
                    {
                        Destroy(mainWeapon);
                        mainWeapon = null;
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Sub_Weapon:
                    if (subWeapon != null)
                        subWeapon = null;
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Helm:
                    Helmet.sharedMesh = null;
                    HelmProp.sharedMesh = null;
                    Player.PlayerAppearance.OnCloseHead(false);
                    if (isLocalPlayer)
                    {
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.HelmetAttachment].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Helmet].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.OnCloseHead(false);
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Cuirass:
                    Cuirass.sharedMesh = null;
                    Shoulder_Left.sharedMesh = null;
                    Shoulder_Right.sharedMesh = null;
                    Arm_Upper_Left.sharedMesh = null;
                    Arm_Upper_Right.sharedMesh = null;
                    Player.PlayerAppearance.OnCloseCuirass(false);
                    if (isLocalPlayer)
                    {
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Cuirass].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.ShoulderAttach_Left].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.ShoulderAttach_Right].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Upper_Left].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Upper_Right].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.OnCloseCuirass(false);
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Gauntlets:
                    Arm_Lower_Left.sharedMesh = null;
                    Arm_Lower_Right.sharedMesh = null;
                    Hand_Left.sharedMesh = null;
                    Hand_Right.sharedMesh = null;
                    Player.PlayerAppearance.OnCloseGauntlets(false);
                    if (isLocalPlayer)
                    {
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Lower_Left].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Lower_Right].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Hand_Left].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Hand_Right].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.OnCloseGauntlets(false);
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Viel:
                    Viel.sharedMesh = null;
                    if (isLocalPlayer)
                    {
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Viel].sharedMesh = null;
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Cuisses:
                    Cuisess.sharedMesh = null;
                    Knee_Left.sharedMesh = null;
                    Knee_Right.sharedMesh = null;
                    Player.PlayerAppearance.Hips.sharedMesh = Player.PlayerAppearance.hips_Mesh;
                    if (isLocalPlayer)
                    {
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.KneeAttach_Left].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.KneeAttach_Right].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Cuisses].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Hips].gameObject.SetActive(true);
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
                case (int)SuitParts.Greaves:
                    Leg_Left.sharedMesh = null;
                    Leg_Right.sharedMesh = null;
                    Player.PlayerAppearance.Leg_Left.sharedMesh = Player.PlayerAppearance.leg_Left_Mesh;
                    Player.PlayerAppearance.Leg_Right.sharedMesh = Player.PlayerAppearance.leg_Right_Mesh;
                    if (isLocalPlayer)
                    {
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Leg_Left].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Leg_Right].sharedMesh = null;
                        PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Leg_Left].gameObject.SetActive(true);
                        PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Leg_Right].gameObject.SetActive(true);
                    }
                    if (_item[i] != null)
                        SetEquipmentAppearance(OutlanderDB.singleton.GetItemScriptable(_item[i]), i);
                    break;
            }
        }
    }

    private void SetEquipmentAppearance(ItemScriptable _item, int i)
    {
        if (_item.mainType == Type.Equipment && _item.equipmentMesh != null)
        {
            switch (i)
            {
                case (int)SuitParts.Helm:
                    foreach (Mesh tmp in _item.equipmentMesh)
                    {
                        switch (tmp.name.Split("_")[1])
                        {
                            case "Head":
                            case "HeadCoverings":
                                Helmet.sharedMesh = tmp;
                                if(isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Helmet].sharedMesh = tmp;
                                if (_item.itemName.Split(' ')[1] == "Archer" || _item.itemName.Split(' ')[1] == "Knight")
                                {
                                    Player.PlayerAppearance.OnCloseHead(true);
                                    if (isLocalPlayer)
                                        PlayerManagers.Instance.characterCreator.OnCloseHead(true);
                                }
                                else
                                {
                                    Player.PlayerAppearance.OnCloseHead(false);
                                    if (isLocalPlayer)
                                        PlayerManagers.Instance.characterCreator.OnCloseHead(false);
                                }
                                break;
                            case "HelmetAttachment":
                                HelmProp.sharedMesh = tmp;
                                if (isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.HelmetAttachment].sharedMesh = tmp;
                                break;
                        }
                    }
                    break;
                case (int)SuitParts.Cuirass:
                    foreach (Mesh tmp in _item.equipmentMesh)
                    {
                        switch (tmp.name.Split("_")[1])
                        {
                            case "Torso":
                                Cuirass.sharedMesh = tmp;
                                Player.PlayerAppearance.Torso.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Cuirass].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Torso].gameObject.SetActive(false);
                                }
                                break;
                            case "ShoulderAttachLeft":
                                Shoulder_Left.sharedMesh = tmp;
                                if (isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.ShoulderAttach_Left].sharedMesh = tmp;
                                break;
                            case "ShoulderAttachRight":
                                Shoulder_Right.sharedMesh = tmp;
                                if (isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.ShoulderAttach_Right].sharedMesh = tmp;
                                break;
                            case "ArmUpperLeft":
                                Arm_Upper_Left.sharedMesh = tmp;
                                Player.PlayerAppearance.Arm_Upper_Left.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Upper_Left].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Arm_Upper_Left].gameObject.SetActive(false);
                                }
                                break;
                            case "ArmUpperRight":
                                Arm_Upper_Right.sharedMesh = tmp;
                                Player.PlayerAppearance.Arm_Upper_Right.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Upper_Right].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Arm_Upper_Right].gameObject.SetActive(false);
                                }
                                break;
                        }
                    }
                    break;
                case (int)SuitParts.Gauntlets:
                    foreach (Mesh tmp in _item.equipmentMesh)
                    {
                        switch (tmp.name.Split("_")[1])
                        {
                            case "ArmLowerLeft":
                                Arm_Lower_Left.sharedMesh = tmp;
                                Player.PlayerAppearance.Arm_Lower_Left.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Lower_Left].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Arm_Lower_Left].gameObject.SetActive(false);
                                }
                                break;
                            case "ArmLowerRight":
                                Arm_Lower_Right.sharedMesh = tmp;
                                Player.PlayerAppearance.Arm_Lower_Right.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Arm_Lower_Right].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Arm_Lower_Right].gameObject.SetActive(false);
                                }
                                break;
                            case "HandLeft":
                                Hand_Left.sharedMesh = tmp;
                                Player.PlayerAppearance.Hand_Left.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Hand_Left].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Hand_Left].gameObject.SetActive(false);
                                }
                                break;
                            case "HandRight":
                                Hand_Right.sharedMesh = tmp;
                                Player.PlayerAppearance.Hand_Right.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Hand_Right].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Hand_Right].gameObject.SetActive(false);
                                }
                                break;
                        }
                    }
                    break;
                case (int)SuitParts.Viel:
                    foreach (Mesh tmp in _item.equipmentMesh)
                    {
                        switch (tmp.name.Split("_")[1])
                        {
                            case "BackAttachment":
                                Viel.sharedMesh = tmp;
                                if (isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Viel].sharedMesh = tmp;
                                break;
                        }
                    }
                    break;
                case (int)SuitParts.Cuisses:
                    foreach (Mesh tmp in _item.equipmentMesh)
                    {
                        switch (tmp.name.Split("_")[1])
                        {
                            case "Hips":
                                Cuisess.sharedMesh = tmp;
                                Player.PlayerAppearance.Hips.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Cuisses].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Hips].gameObject.SetActive(false);
                                }
                                break;
                            case "KneeAttackLeft":
                                Knee_Left.sharedMesh = tmp;
                                if (isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.KneeAttach_Left].sharedMesh = tmp;
                                break;
                            case "KneeAttackRight":
                                Knee_Right.sharedMesh = tmp;
                                if (isLocalPlayer)
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.KneeAttach_Right].sharedMesh = tmp;
                                break;
                        }
                    }
                    break;
                case (int)SuitParts.Greaves:
                    foreach (Mesh tmp in _item.equipmentMesh)
                    {
                        switch (tmp.name.Split("_")[1])
                        {
                            case "LegLeft":
                                Leg_Left.sharedMesh = tmp;
                                Player.PlayerAppearance.Leg_Left.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Leg_Left].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Leg_Left].gameObject.SetActive(false);
                                }
                                break;
                            case "LegRight":
                                Leg_Right.sharedMesh = tmp;
                                Player.PlayerAppearance.Leg_Right.sharedMesh = null;
                                if (isLocalPlayer)
                                {
                                    PlayerManagers.Instance.characterCreator.suit[(int)BodySuits.Leg_Right].sharedMesh = tmp;
                                    PlayerManagers.Instance.characterCreator.body[(int)BodyParts.Leg_Right].gameObject.SetActive(false);
                                }
                                break;
                        }
                    }
                    break;
            }
        }
        else if (_item.mainType == Type.MainWeapon)
        {
            switch (i)
            {
                case (int)SuitParts.Main_Weapon:
                    mainWeapon = Instantiate(Resources.Load<GameObject>($"Prefab/Weapon/{_item.item_type.ToString()}/{_item.name}"), _item.weaponData.weaponPosition, Quaternion.Euler(_item.weaponData.weaponRotation.x, _item.weaponData.weaponRotation.y, _item.weaponData.weaponRotation.z), main_weapon_parent);
                    mainWeapon.transform.localPosition = _item.weaponData.weaponPosition;
                    mainWeapon.transform.localEulerAngles = _item.weaponData.weaponRotation;
                    break;
                case (int)SuitParts.Sub_Weapon:
                    subWeapon = Resources.Load<GameObject>($"Prefab/Weapon/{_item.item_type.ToString()}/{_item.name}");
                    break;
            }
            if(mainWeapon!= null)
            {
                Player.MovementStateMachine.SetWeaponVisible();
                /*if (Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.CLIMB
                    || Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.CROUCH 
                    || Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.DODGE
                    || Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.SWIM)
                    mainWeapon.SetActive(false);
                else
                    mainWeapon.SetActive(true);*/
            }
        }
    }
}
