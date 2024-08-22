using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class JSONPlayerData
{
    [JsonProperty("data")] public Data data;
}

[Serializable]
public class JSONPlayer
{
    [JsonProperty("data")] public Data data;
    [JsonProperty("message")] public string message;
}

[Serializable]
public class Data
{
    [JsonProperty("unlock_icon_btr")] public IconBTR icon;
    [JsonProperty("wallet_address")] public string walletAddress;
    [JsonProperty("level")] public int level;
    [JsonProperty("experience")] public float experience;
    [JsonProperty("is_active")] public bool isActive;
    [JsonProperty("_id")] public string id;
    [JsonProperty("username")] public string username;
    [JsonProperty("role")] public string role;
    [JsonProperty("appearance_btr")] public Appearance appearance;
    //[JsonProperty("treasury")] public Treasury treasury;
    //[JsonProperty("proficiency_btr")] public Proficiency proficiency;
}

[Serializable]
public class JSONPlayerHistory
{
    [JsonProperty("status")] public bool isStatus;
    [JsonProperty("message")] public string message;
    [JsonProperty("data")] public HistoryData data;
    public class HistoryData
    {
        [JsonProperty("is_active")] public bool isActive;
        [JsonProperty("room_id")] public MatchDetail matchDetail;
        [JsonProperty("player_id")] public playerMyData playerData;
        [JsonProperty("player_kill")] public int playerKill;
        [JsonProperty("monster_kill")] public int monsterKill;
        [JsonProperty("miniboss_kill")] public int moniBossKill;
        [JsonProperty("boss_kill")] public int BossKill;
        [JsonProperty("score")] public int score;
        [JsonProperty("coin")] public int coin;
        [JsonProperty("match_duration")] public float duration;
        [JsonProperty("last_number_survival")] public int surviveNumber;

        public class MatchDetail
        {
            [JsonProperty("room_id")] public string matchID;
            [JsonProperty("max_player")] public int maxPlayer;
            [JsonProperty("min_player")] public int minPlayer;
            [JsonProperty("player_data")] public List<PlayerMatchData> playerMatchData;
            public class PlayerMatchData
            {
                [JsonProperty("party_code")] public string partyCode;
                [JsonProperty("player_list")] public List<PlayerPartyData> playerList;
                public class PlayerPartyData
                {
                    [JsonProperty("player_id")] public string playerID;
                    [JsonProperty("username")] public string playerName;
                    [JsonProperty("status")] public string status;
                }
            }
        }
        public class playerMyData
        {
            [JsonProperty("wallet_address")] public string wallet;
            [JsonProperty("username")] public string username;
            [JsonProperty("email")] public string email;
        }
    }
}

[Serializable]
public class JSONPlayerSound
{
    [JsonProperty("status")] public bool status;
    [JsonProperty("data")] public JSONUpdateSound soundData;
    [JsonProperty("message")] public string message;
}

[Serializable]
public class IconBTR
{
    [JsonProperty("icon")] public List<IconData> icon;
    [JsonProperty("border")] public List<IconData> border;
    [JsonProperty("title")] public List<IconData> title;
    [JsonProperty("emotion")] public List<IconData> emotion;
    [JsonProperty("sticker")] public List<IconData> sticker;
}

[Serializable]
public class IconData
{
    [JsonProperty("_id")] public string id;
    [JsonProperty("is_active")] public bool isActive;
    [JsonProperty("name")] public string name;
    [JsonProperty("type")] public string type;
    [JsonProperty("img_url")] public string url;
}

[Serializable]
public class Inventory
{
    [JsonProperty("money")] public Currency currency;
    [JsonProperty("max_amount")] public int maxAmount;
    [JsonProperty("max_weight")] public float maxWeight;
    [JsonProperty("item")] public List<JSONItem> items;
}

[Serializable]
public class InventoryBox
{
    [JsonProperty("max_amount")] public int maxAmount;
    [JsonProperty("item")] public List<JSONItem> items;
}

[Serializable]
public class Currency
{
    [JsonProperty("gold")] public int gold;
    [JsonProperty("silver")] public int silver;
    [JsonProperty("bronze")] public int bronze;
}

[Serializable]
public class PlayerStatus
{
    [JsonProperty("str")] public int STR;
    [JsonProperty("agi")] public int AGI;
    [JsonProperty("vit")] public int VIT;
    [JsonProperty("dex")] public int DEX;
    [JsonProperty("int")] public int INT;
    [JsonProperty("luk")] public int LUK;
    [JsonProperty("point")] public int point;
    [JsonProperty("used_point")] public int usedPoint;
    [JsonProperty("rebirth")] public int rebirth;
}

[Serializable]
public class Appearance
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
    [JsonProperty("skin_color")] public AppearanceDetail skin_color;
    [JsonProperty("weapon")] public AppearanceDetail weapon;
    [JsonProperty("gender")] public int gender;
}

public class AppearanceDetail
{
    [JsonProperty("appearance_id")] public string id;
    [JsonProperty("color")] public string color;
}

public class Treasury
{
    [JsonProperty("sword_skin")] public List<TreasuryDetail> swordSkin;
    [JsonProperty("spear_skin")] public List<TreasuryDetail> spearSkin;
    [JsonProperty("axe_skin")] public List<TreasuryDetail> axeSkin;
    [JsonProperty("book_skin")] public List<TreasuryDetail> bookSkin;
    [JsonProperty("helmet_skin")] public List<TreasuryDetail> helmetSkin;
    [JsonProperty("cuirass_skin")] public List<TreasuryDetail> cuirassSkin;
    [JsonProperty("cuisses_skin")] public List<TreasuryDetail> cuissesSkin;
    [JsonProperty("gauntlets_skin")] public List<TreasuryDetail> gauntletsSkin;
    [JsonProperty("greaves_skin")] public List<TreasuryDetail> greavesSkin;
    [JsonProperty("veil_skin")] public List<TreasuryDetail> veilSkin;
    [JsonProperty("hair_skin")] public List<TreasuryDetail> hairSkin;
    [JsonProperty("eyebrows_skin")] public List<TreasuryDetail> eyebrowsSkin;
    [JsonProperty("beard_skin")] public List<TreasuryDetail> beardSkin;
    [JsonProperty("face_skin")] public List<TreasuryDetail> faceSkin;
    [JsonProperty("shoes_skin")] public List<TreasuryDetail> shoesSkin;
    [JsonProperty("gloves_skin")] public List<TreasuryDetail> glovesSkin;
    [JsonProperty("trousers_skin")] public List<TreasuryDetail> trousersSkin;
    [JsonProperty("shirt_skin")] public List<TreasuryDetail> shirtSkin;
    [JsonProperty("hat_skin")] public List<TreasuryDetail> hatSkin;
}

[Serializable]
public class TreasuryDetail
{
    [JsonProperty("equip")] public bool isEquip;
    [JsonProperty("_id")] public string id;
    [JsonProperty("skin_id")] public string skin_id;
}

public class History
{
    [JsonProperty("player_kill")] public int playerKill;
    [JsonProperty("monster_kill")] public int monsterKill;
    [JsonProperty("miniboss_kill")] public int miniBossKill;
    [JsonProperty("boss_kill")] public int bossKill;
    [JsonProperty("sum_score")] public int sumScore;
    [JsonProperty("sum_coin")] public int coin;
    [JsonProperty("match_duration")] public float duration;
    [JsonProperty("last_number_survival")] public int surviveNumber;
    [JsonProperty("max_score")] public int maxScore;
    [JsonProperty("min_score")] public int minScore;
    [JsonProperty("match_played")] public int matchPlayed;
    [JsonProperty("win_count")] public int winCount;
}

#region Mobile

[Serializable]
public class PlayerEffect
{
    [JsonProperty("_id")] public string id;
    [JsonProperty("list")] public List<object> effect;
}


[Serializable]
public class PlayerEquipment
{
    [JsonProperty("slot")] public string slotId;
    [JsonProperty("item_id")] public string itemId;
    [JsonProperty("name")] public string itemName;
}

[Serializable]
public class PlayerCurrency
{
    [JsonProperty("bronze")] public int bronze;
    [JsonProperty("silver")] public int silver;
    [JsonProperty("gold")]   public int gold;
}

[Serializable]
public class PlayerPosition
{
    [JsonProperty("x")] public float posX;
    [JsonProperty("y")] public float posY;
    [JsonProperty("z")] public float posZ;
    [JsonProperty("map_name")] public string mapName;
}

[Serializable]
public class PlayerSkill
{
    [JsonProperty("skill")] public List<CharacterSkill> skill;
}

[Serializable]
public class PlayerClass
{
    [JsonProperty("class")] public List<PlayerProficiency> playerClass;
}

[Serializable]
public class PlayerProficiency
{
    [JsonProperty("_id")] public string _id;
    [JsonProperty("class_id")] public string class_id;
    [JsonProperty("class_name")] public string className;
    [JsonProperty("level")] public int level;
    [JsonProperty("exp")] public float exp;
    //[JsonProperty("time_stamp")] public string time_stamp;
}

[Serializable]
public class PlayerQuest
{
    [JsonProperty("list")] public List<Quest> quest;
}

public class Quest
{
    [JsonProperty("_id")] public string _id;
    [JsonProperty("quest_id")] public string questId;
    [JsonProperty("quest_name")] public string questName;
    [JsonProperty("complete")] public bool complete;
}

#region Achievement
[Serializable]
public class JSONPlayerAchievement
{
    [JsonProperty("data")] public AchieveData data;
    [JsonProperty("message")] public string message;
}

[Serializable]
public class AchieveData
{
    [JsonProperty("is_active")] public bool isActive;
    [JsonProperty("created_at")] public DateTime createAt;
    [JsonProperty("updated_at")] public DateTime updateAt;
    [JsonProperty("_id")] public string dataId;
    [JsonProperty("achievement")] public List<Achievement> achievement;
    //[JsonProperty("__v")] public int v;
    [JsonProperty("player_id")] public string playerId;
}

[Serializable]
public class PlayerAchievement
{
    [JsonProperty("achievement")] public List<Achievement> achievementList;
}

[Serializable]
public class Achievement
{
    [JsonProperty("in_progress")] public int inProgress;
    [JsonProperty("is_complete")] public bool isComplete;
    [JsonProperty("_id")] public string id;
    [JsonProperty("achievement_id")] public string achievementId;
    [JsonProperty("data_progress")] public List<AchievementData> achievementdata;
}

[Serializable]
public class AchievementData
{
    [JsonProperty("success")] public bool success;
    [JsonProperty("_id")] public string id;
    [JsonProperty("name")] public string name;
    [JsonProperty("number")] public int number;
    [JsonProperty("object_type")] public string objectType;
    [JsonProperty("action")] public string action;
    [JsonProperty("location")] public string mapId;
    [JsonProperty("location_name")] public string mapName;
}
#endregion

#endregion