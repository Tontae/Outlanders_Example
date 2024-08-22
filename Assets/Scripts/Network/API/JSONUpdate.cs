using System;
using System.Collections.Generic;
using Newtonsoft.Json;
public class JSONUpdate{}
public class JSONUpdatePlayerData
{
    [JsonProperty("wallet_address")] public string walletAddress;
    [JsonProperty("level")] public int level;
    [JsonProperty("experience")] public float experience;
    [JsonProperty("is_active")] public bool isActive;
    [JsonProperty("id")] public string id;
    [JsonProperty("username")] public string username;
    [JsonProperty("role")] public string role;
    [JsonProperty("point")] public int point;
    [JsonProperty("money")] public int money;
}

public class JSONUpdateAppearance
{
    [JsonProperty("skin")] public AppearanceDetail skin;
    [JsonProperty("hair")] public AppearanceDetail hair;
    [JsonProperty("face")] public AppearanceDetail face;
    [JsonProperty("beard")] public AppearanceDetail beard;
    [JsonProperty("eyebrows")] public AppearanceDetail eyebrows;
    [JsonProperty("hat")] public AppearanceDetail hat;
    [JsonProperty("shirt")] public AppearanceDetail shirt;
    [JsonProperty("trousers")] public AppearanceDetail trousers;
    [JsonProperty("gloves")] public AppearanceDetail gloves;
    [JsonProperty("shoes")] public AppearanceDetail shoes;
    [JsonProperty("gender")] public int gender;
}

[Serializable]
public class JSONUpdatePositon
{
    [JsonProperty("x")] public float posX;
    [JsonProperty("y")] public float posY;
    [JsonProperty("z")] public float posZ;
    [JsonProperty("map_name")] public string mapName;
}

[Serializable]
public class JSONUpdateAchievement
{
    [JsonProperty("achievement_id")] public string achievementId;
    [JsonProperty("object_type")] public string objectType;
    [JsonProperty("action")] public string action;
    [JsonProperty("name")] public string name;
    [JsonProperty("location")] public string location;
    [JsonProperty("number")] public int number;
}

//[Serializable]
//public class JSONUpdateEquipment
//{
//    [JsonProperty("equipment")] public List<Equipment> equipment;
//}

//[Serializable]
//public class Equipment
//{
//    [JsonProperty("slot")] public string slot;
//    [JsonProperty("item_id")] public string itemId;
//}

[Serializable]
public class JSONUpdateTreasurySkin
{
    [JsonProperty("type")] public string type;
    [JsonProperty("model_id")] public string modelId;
}

[Serializable]
public class JSONUpdateEquipSkin
{
    [JsonProperty("type")] public string type;
    [JsonProperty("skin_id")] public string skinId;
}

[Serializable]
public class JSONUpdateHistory
{
    [JsonProperty("room_id")] public string roomID;
    [JsonProperty("player_id")] public string playerID;
    [JsonProperty("player_kill")] public int playerKill;
    [JsonProperty("monster_kill")] public int monsterKill;
    [JsonProperty("miniboss_kill")] public int miniBossKill;
    [JsonProperty("boss_kill")] public int bossKill;
    [JsonProperty("score")] public int score;
    [JsonProperty("coin")] public int coin;
    [JsonProperty("match_duration")] public float duration;
    [JsonProperty("last_number_survival")] public int surviveNumber;
    [JsonProperty("level_exp")] public int level_exp;
    [JsonProperty("rank_exp")] public int rank_exp;
    [JsonProperty("proficiency_exp")] public List<Proficiency> proficiency_exp;
    [JsonProperty("deal_damage")] public float deal_damage;
    [JsonProperty("collect_item")] public int collect_item;
    [JsonProperty("craft_item")] public int craft_item;
    [JsonProperty("use_health_potion")] public int use_health_potion;
    [JsonProperty("use_mana_potion")] public int use_mana_potion;
    [JsonProperty("god_slayer")] public int god_slayer;
    [JsonProperty("traveler")] public float traveler;
    [JsonProperty("wardog")] public int wardog;
    [JsonProperty("not_fear_the_reaper")] public int not_fear_the_reaper;
}

[Serializable]
public class Proficiency
{
    [JsonProperty("weapon_type")] public string weapon;
    [JsonProperty("exp")] public int exp;
}

[Serializable]
public class JSONUpdateSound
{
    [JsonProperty("master")] public float master;
    [JsonProperty("music")] public float bgm;
    [JsonProperty("sfx")] public float sfx;
}

[Serializable]
public class JSONUpdateSkill
{
    [JsonProperty("skill_name")] public string skillName;
    [JsonProperty("cool_down")] public int coolDown;
}

[Serializable]
public class JSONUpdateProficiency
{
    [JsonProperty("class_name")] public string className;
    [JsonProperty("level")] public int level;
    [JsonProperty("exp")] public float exp;
    //[JsonProperty("class")] public List<UpdateProficiency> playerClass;
}

[Serializable]
public class UpdateProficiency
{
    //[JsonProperty("class")] public List<JSONUpdateProficiency> playerClass;
    [JsonProperty("class")] public JSONUpdateProficiency playerProficiency;
    //[JsonProperty("class_name")] public string className;
    //[JsonProperty("level")] public int level;
    //[JsonProperty("exp")] public float exp;
}

[Serializable]
public class JSONUpdateQuest
{
    [JsonProperty("quest_name")] public string questName;
    [JsonProperty("complete")] public bool complete;
}
