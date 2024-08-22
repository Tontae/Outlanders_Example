using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;
using System.Net;

#region Account

public partial struct TokenMsg : NetworkMessage
{
    public string token;
    public string matchID;
}

public partial struct LoginSuccessMsg : NetworkMessage
{
    public string id;
}

public partial struct InstantiatePlayerMsg : NetworkMessage
{
    public string playerID;
    public string matchID;
}
public partial struct DestroyPlayerMsg : NetworkMessage
{
    public bool isNewMatch;
}

#endregion

#region Player Data

public partial struct PlayerProficiencyMsg : NetworkMessage
{
    public string id;
    //public UpdateProficiency proficiencies;
    public JSONUpdateProficiency proficiency;
}

public partial struct PlayerEquipmentMsg : NetworkMessage
{
    public string id;
    public string[] equipment;
}

public partial struct PlayerItemStatusMsg : NetworkMessage
{
    public string id;
    public JSONUpdateItemStatus itemStatus;
}

public partial struct PlayerAchievementMsg : NetworkMessage
{
    public string id;
    public string achievementId;
    public string objectType;
    public string action;
    public string name;
    public string location;
    public int number;
}

#endregion

#region Send Player data to Client

public partial struct SendPlayerDatabaseMsg : NetworkMessage
{
    public bool isLogin;
    public PlayerScriptable playerScriptable;
}

public partial struct PlayerExitMatchMessage : NetworkMessage
{
    public string playerId;
}

#endregion

public partial struct CharacterLoadMsg : NetworkMessage
{
    public string id;
    public string username;
    public string spawnName;
}

#region Inventory

public partial struct InventoryMsg : NetworkMessage
{
    public string id;
    public JSONCollectItem jsonCollectItem;
    public JSONUseItem jsonUseItem;
}

#endregion

#region Essential

public partial struct ChatMessage : NetworkMessage
{
    public string username;
    public string message;
}

#endregion

#region Network Discovery

public partial struct ServerResponseMsg : NetworkMessage
{
    public int port;
    public int playerAmount;
}

#endregion