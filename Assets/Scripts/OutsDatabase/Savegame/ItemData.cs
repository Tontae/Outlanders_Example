using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipmentManager;

[System.Serializable]
public class ItemData
{
    public string itemId;
    public string itemName;
    public string description;
    public Type type;
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
    public List<ItemStatus> Status;

    public ItemData(ItemScriptable data)
    {
        itemId = data.itemId;
        itemName = data.itemName;
        description = data.description;
        type = data.mainType;
        item_type = data.item_type;
        tier = data.tier;
        pairWeapon = data.pairWeapon;
        spriteName = data.spriteName;
        weight = data.weight;
        element = data.element;
        maxCapacity = data.maxCapacity;
        isMultipleItem = data.isMultipleItem;
        buyPrice = data.buyPrice;
        sellPrice = data.sellPrice;
        isPurchaseable = data.isPurchaseable;
        Status = data.Status;
    }
}
