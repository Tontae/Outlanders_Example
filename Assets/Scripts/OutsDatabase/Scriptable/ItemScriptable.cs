using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ItemEffecttype
{
    none,
    increaseHP,
    increaseMP,
    increaseWeight,
    increaseRuneSlot
}

public enum StatusType
{
    STR,
    AGI,
    VIT,
    INT,
    DEX,
    LUK,

    ATK,
    MATK,
    DEF,
    MR,
    CRIT,

    MHP,
    MMP,
    ATKSPD,
    MOVSPD,
    CRITDAMAGE
}

public enum Type
{
    none,
    Coin,
    Ingredient,
    Material,
    Food,
    Rune,
    MainWeapon,
    SubWeapon,
    Equipment,
    Consumable,
    Skill,
    Bag
}

public enum ItemType
{
    None,
    Axe,
    Bow,
    Lance,
    Shield,
    Sword,
    Cuirass,
    Cuisses,
    FullHelm,
    Gauntlets,
    Greaves,
    Helm,
    Viel,
    Rune,
    Material,
    ATK,
    DEF,
    HP,
    AGI,
    CRIT,
    ALL,
    Consumable,
    Bag
}

[CreateAssetMenu(fileName = "ItemData", menuName = "OutlanderDatabase/ItemData", order = 3)]
public class ItemScriptable : ScriptableObject
{

    public string itemId;
    public string itemName;
    public string description;
    //public string type;
    public Type mainType;
    // public string item_type;
    public ItemType item_type;
    public Tier tier;
    public string pairWeapon;
    public string spriteName;
    public float weight;
    public string element;
    public int maxCapacity;
    public bool isMultipleItem;
    public int buyPrice;
    public int sellPrice;
    public bool isPurchaseable;
    public bool isEquipable;
    public bool isInteractable;

    public List<ItemStatus> Status;
    public List<RequireMatt> craftsMatsList;
    public CookingState cookingState;
    public List<ItemEffect> itemEffectList;
    public List<SkillScriptable> Skill;
    [HideInInspector]
    public List<Mesh> equipmentMesh;
    [HideInInspector][SerializeField]
    public WeaponData weaponData;
}

[System.Serializable]
public class ItemStatus
{
    public StatusType statusKeys;
    public float value;
}

[System.Serializable]
public class CookingState
{
    public float rawTime;
    public float welldoneTime;
    public float overCookedTime;

}

[System.Serializable]
public class RequireMatt
{
    public string itemId;
    public int qty;
}

[System.Serializable]
public class ItemEffect
{
    public ItemEffecttype itemEffect;
    public float value;
}

[System.Serializable]
public class WeaponData
{
    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
}

public enum Tier
{
    Common = 0,
    UnCommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
}

#if UNITY_EDITOR

[CustomEditor(typeof(ItemScriptable))]
public class ItemScriptable_Editor : Editor
{
    ItemScriptable script;

    SerializedProperty meshListProperty;
    SerializedProperty weaponProperty;
    SerializedProperty PosProperty;
    SerializedProperty RotProperty;

    private void OnEnable()
    {
        script = (ItemScriptable)target;
        meshListProperty = serializedObject.FindProperty(nameof(script.equipmentMesh));
        weaponProperty = serializedObject.FindProperty(nameof(script.weaponData));;
        PosProperty = weaponProperty.FindPropertyRelative(nameof(script.weaponData.weaponPosition));
        RotProperty = weaponProperty.FindPropertyRelative(nameof(script.weaponData.weaponRotation));
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        if (script.mainType == Type.Equipment)
        {
            EditorGUILayout.PropertyField(meshListProperty,true);
        }
        else if (script.mainType == Type.MainWeapon)
        {
            EditorGUILayout.PropertyField(weaponProperty, true);
            EditorGUILayout.PropertyField(PosProperty, true);
            EditorGUILayout.PropertyField(RotProperty, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}

#endif