using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class JSONInventory
{
    [JsonProperty("status")] public bool status;
    [JsonProperty("message")] public string message;
    [JsonProperty("data")] public InventoryData data;
}

[Serializable]
public class JSONInventoryBox
{
    [JsonProperty("status")] public bool status;
    [JsonProperty("message")] public string message;
    [JsonProperty("data")] public InventoryBoxData data;
}

[Serializable]
public class InventoryData
{
    [JsonProperty("money")] public Currency currency;
    [JsonProperty("max_amount")] public int maxAmount;
    [JsonProperty("max_weight")] public float maxWeight;
    [JsonProperty("created_at")] public DateTime createdAt;
    [JsonProperty("updated_at")] public DateTime updatedAt;
    [JsonProperty("_id")] public string id;
    [JsonProperty("item")] public List<JSONItem> items;
    [JsonProperty("__v")] public string v;
    [JsonProperty("player_id")] public string playerId;
}

[Serializable]
public class InventoryBoxData
{
    [JsonProperty("max_amount")] public int maxAmount;
    [JsonProperty("is_active")] public bool isActive;
    [JsonProperty("created_at")] public DateTime createdAt;
    [JsonProperty("updated_at")] public DateTime updatedAt;
    [JsonProperty("_id")] public string id;
    [JsonProperty("item")] public List<JSONItem> items;
    [JsonProperty("__v")] public string v;
    [JsonProperty("player_id")] public string playerId;
}

[Serializable]
public class JSONItem
{
    [JsonProperty("qty")] public int qty;
    [JsonProperty("id_gen")] public string idGen;
    [JsonProperty("model_id")] public string modelId;
    [JsonProperty("durable")] public int durable;
    [JsonProperty("enhancepoint")] public int enchancePoint;
    [JsonProperty("_id")] public string id;
    [JsonProperty("item_id")] public string itemId;
}

[Serializable]
public class JSONCollectItem
{
    [JsonProperty("item")] public List <CollectItem> item;
}

public class CollectItem
{
    [JsonProperty("model_id")] public string modelId;
    [JsonProperty("item_id")] public string id_backgen;
    [JsonProperty("id_gen")] public string id_gen;
    [JsonProperty("qty")] public float qty;
    [JsonProperty("durable")] public float durable;
    [JsonProperty("enhancepoint")] public float enhancePoint;
    [JsonProperty("sub_status")] public SubStatus subStatus;
}

[Serializable]
public class SubStatus
{
    [JsonProperty("type")] public string type;
    [JsonProperty("value")] public int value;
}

[Serializable]
public class JSONUseItem
{
    [JsonProperty("item")] public List<UseItem> item;
}

public class UseItem
{
    [JsonProperty("model_id")] public string modelId;
    [JsonProperty("id_gen")] public string id_gen;
    [JsonProperty("qty")] public float qty;
}

[Serializable]
public class JSONMoveItem
{
    [JsonProperty("item")] public List<MoveItem> item;
}

public class MoveItem
{
    [JsonProperty("item_id")] public string itemId;
    [JsonProperty("qty")] public float qty;
}

[Serializable]
public class JSONUpdateItemStatus
{
    [JsonProperty("item")] public List<UpdateItemStatus> items;
}

[Serializable]
public class UpdateItemStatus
{
    [JsonProperty("id_gen")] public string idGen;
    [JsonProperty("durable")] public int durable;
    [JsonProperty("enhancepoint")] public int enhancePoint;
}