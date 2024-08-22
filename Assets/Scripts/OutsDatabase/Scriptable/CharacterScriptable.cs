using Mirror;
using Outlander.Character;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "OutlanderDatabase/PlayerData", order = 2)]
public class PlayerScriptable : ScriptableObject
{
    public string walletAddress;
    public string id;
    public string token;
    public int conn;
    public string username;
    public string role;
    public bool isActive;
    public int level;
    public float experience;
    public int gender;
    public CharacterAppearance[] appearance;
    public string[] equipeditemList;
    public CharacterInventory inventory;
	public List<CharacterProficiency> proficiency;
}

[Serializable]
public class CharacterLocation
{
    public string mapName;
    public float posX;
    public float posY;
    public float posZ;
}

[Serializable]
public class CharacterAppearance
{
    public string id;
    public string color;
}

[Serializable]
public class CharacterStatus
{
    public int STR;
    public int AGI;
    public int VIT;
    public int DEX;
    public int INT;
    public int LUK;
    public int point;
    public int usedPoint;
    public int rebirth;
    public string id;
}

[Serializable]
public class CharacterInventory
{
    public int maxAmount;
    public float maxWeight;
    public string id;
    public List<CharacterItem> items;
    public CharacterCurrency characterCurrency;
}

[Serializable]
public class CharacterStorage
{
    public int maxAmount;
    public string id;
    public List<CharacterItem> items;
}

[Serializable]
public class CharacterItem
{
    public string id;
    public int quantities;
    public string id_gen;
    public int enhance;
    public int durable;
    public string id_backend_gen;
    public List<string> skillList;
}

[Serializable]
public class CharacterCurrency
{
    public int bronze;
    public int silver;
    public int gold;
}

[Serializable]
public class CharacterAchievement
{
    public string achievementId;
    public int inProgress;
    public bool isComplete;
    public List<CharacterAchievementData> achievementData;
}

[Serializable]
public class CharacterAchievementData
{
    public int number;
    public bool isSuccess;
    public string name;
    public string objectType;
    public string action;
    public string mapId;
    public string mapName;
}

[Serializable]
public class CharacterSkill
{
    public string skillName;
    public int coolDown;
}

[Serializable]
public class CharacterProficiency
{
    public string className;
    public int level;
    public float exp;
}

public class CharacterQuest
{
    public string questId;
    public string questName;
    public bool complete;
}

[Serializable]
public class CharacterFriend
{
    public string username;
    public string id;
    public string status;
}