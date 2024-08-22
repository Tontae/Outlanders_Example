using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class DatabaseData
{
    public List<PlayerScriptable> playerScriptable = new List<PlayerScriptable>();
    public List<ItemScriptable> itemScriptables = new List<ItemScriptable>();
    public List<Sprite> itemSprites = new List<Sprite>();
}

[RequireComponent(typeof(PlayerDB))]
[RequireComponent(typeof(ItemDB))]
[RequireComponent(typeof(ItemSpriteDB))]
public class OutlanderDB : MonoBehaviour
{
    // singleton for easier accesss
    public static OutlanderDB singleton;

    // Data Table
    private PlayerDB playerDB;
    private ItemDB itemDB;
    private ItemSpriteDB itemSpriteDB;
    // Struct
    [SerializeField] private DatabaseData databaseData;

    private void Awake()
    {
        // initialize singleton
        if (singleton == null) singleton = this;
    }

    #region Database
    public void CreateDatabase()
    {
        Debug.Log($"[OutsDB] Create database");

        playerDB = GetComponent<PlayerDB>();
        itemDB = GetComponent<ItemDB>();
        itemSpriteDB = GetComponent<ItemSpriteDB>();

        playerDB.CreateDataIndex();
        itemDB.CreateDataIndex();
        itemSpriteDB.CreateDataIndex();
    }

    public void UpdateDatabase()
    {
        //Debug.Log($"[OutsDB] Update database");
        databaseData.playerScriptable = playerDB.LoadData();
        databaseData.itemScriptables = itemDB.LoadData();
        databaseData.itemSprites = itemSpriteDB.LoadData();
    }

    public DatabaseData LoadDatabase()
    {
        //Debug.Log($"[OutsDB] Load database");
        return databaseData;
    }

    #endregion

    #region Network Message
    public void OnSyncPlayerDatabase(SendPlayerDatabaseMsg msg)
    {
        if (msg.isLogin)
        {
            databaseData.playerScriptable.Add(msg.playerScriptable);
            playerDB.SyncDatabase(databaseData.playerScriptable);
        }
        else
        {
            UpdatePlayer(msg.playerScriptable);
            UpdateDatabase();
        }
    }

    public void OnSyncDisconnect(NetworkConnectionToClient conn)
    {
        if (singleton.GetPlayerByConn(conn) == null) return;
        PlayerScriptable player = singleton.GetPlayerByConn(conn);

        playerDB.DeleteData(player.id);

        databaseData.playerScriptable.Remove(player);

        Debug.Log($"[OutsDB] Clear database of player : {player}");
    }

    public void OnClearAccountStruck(string id)
    {
        playerDB.DeleteData(id);

        PlayerScriptable character = singleton.GetPlayer(id);

        databaseData.playerScriptable.Remove(character);
    }

    public void ClientClearDB()
    {
        Debug.Log("[OutsDB] Clear Database");
        playerDB.ClearData();
        itemDB.ClearData();
        itemSpriteDB.ClearData();
    }
    #endregion

    #region AccountDB
    public void CreatePlayer(PlayerScriptable data)
    {
        playerDB.CreateData(data);
    }

    public PlayerScriptable GetPlayer(string id)
    {
        return playerDB.ReadData(id);
    }

    public PlayerScriptable GetPlayerByConn(NetworkConnectionToClient conn)
    {
        return playerDB.ReadDataByConn(conn);
    }

    public void UpdatePlayer(PlayerScriptable account)
    {
        playerDB.UpdateData(account.id, account);
    }

    public bool IsAccountIdOnline(string id)
    {
        Dictionary<string, PlayerScriptable> character = playerDB.LoadDataIndex();

        foreach (var data in character)
        {
            if (data.Value.id == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsAccountEmailOnline(string id)
    {
        Dictionary<string, PlayerScriptable> accounts = playerDB.LoadDataIndex();

        foreach(var data in accounts)
        {
            if(data.Value.id == id)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region ItemDB
    public List<ItemScriptable> GetItemList()
    {
        return itemDB.LoadData();
    }

    public ItemScriptable GetItemScriptable(string Id)
    {
        return itemDB.ReadData(Id);
    }
    #endregion

    #region ItemSpriteDB
    public List<Sprite> GetItemSpriteList()
    {
        return itemSpriteDB.LoadData();
    }

    public Sprite GetSpritebyName(string spriteName)
    {
        return itemSpriteDB.ReadData(spriteName);
    }
    #endregion

    #region AchievementDB

    //public int GetIndexByAchievementId(string id)
    //{
    //    return characterDB.GetAchieveIndex(id);
    //}

    #endregion
}
