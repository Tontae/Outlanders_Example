using System.Collections;
using System.Collections.Generic;
using Mirror;
using Outlander.Player;
using UnityEngine;

public static class CustomReadWriteData
{
    public static void WriteDataTracker(this NetworkWriter writer, DataTrackerScriptable.DataTracker dataTracker)
    {
        writer.WriteString(dataTracker.id);
        writer.WriteString(dataTracker.dataType);

        switch (dataTracker.value)
        {
            case int intValue:
                Debug.Log($"{dataTracker.id} => {intValue}");
                writer.WriteInt(intValue);
                break;
            case float floatValue:
                Debug.Log($"{dataTracker.id} => {floatValue}");
                writer.WriteFloat(floatValue);
                break;
            case bool boolValue:
                Debug.Log($"{dataTracker.id} => {boolValue}");
                writer.WriteBool(boolValue);
                break;
            //case Dictionary<string, float> dictValue:
            //    foreach (var temp1 in dictValue)
            //        Debug.Log($"{dataTracker.id} => {temp1.Key} : {temp1.Value}");
            //    break;
            //case Dictionary<WeaponManager.WeaponType, KillDamage> dictValue:
            //    foreach (var temp1 in dictValue)
            //        Debug.Log($"{temp.Key} => {temp1.Key} : totalDamage={temp1.Value.totalDamage} , totalKill={temp1.Value.killCount}");
            //    break;
            //case Dictionary<string, Dictionary<WeaponManager.WeaponType, KillDamage>> dictValue:
            //    foreach (var temp1 in dictValue)
            //        foreach (var temp2 in temp1.Value)
            //            Debug.Log($"{temp.Key} => ({temp1.Key},{temp2.Key}) : totalDamage={temp2.Value.totalDamage} , totalKill={temp2.Value.killCount}");
            //    break;
            case Dictionary<Tier, int> dictValue:
                {
                    List<Tier> ltier = new List<Tier>();
                    List<int> lint = new List<int>();
                    foreach (var temp1 in dictValue)
                    {
                        Debug.Log($"{dataTracker.id} => {temp1.Key} : {temp1.Value}");
                        ltier.Add(temp1.Key);
                        lint.Add(temp1.Value);
                    }
                    writer.WriteList(ltier);
                    writer.WriteList(lint);
                }
                break;
            case Dictionary<string, int> dictValue:
                {
                    List<string> lstr = new List<string>();
                    List<int> lint = new List<int>();
                    foreach (var temp1 in dictValue)
                    {
                        Debug.Log($"{dataTracker.id} => {temp1.Key} : {temp1.Value}");
                        lstr.Add(temp1.Key);
                        lint.Add(temp1.Value);
                    }
                    writer.WriteList(lstr);
                    writer.WriteList(lint);
                }
                break;
        }
    }

    public static DataTrackerScriptable.DataTracker ReadDataTracker(this NetworkReader reader)
    {
        string id = reader.ReadString();
        string dataType = reader.ReadString();

        switch (dataType)
        {

            case "System.Int32":
                int intValue = reader.ReadInt();
                //Debug.Log($"{id} => {intValue}");
                return new DataTrackerScriptable.DataTracker { id = id, dataType = dataType, value = intValue };
            case "System.Single":
                float floatValue = reader.ReadFloat();
                //Debug.Log($"{id} => {floatValue}");
                return new DataTrackerScriptable.DataTracker { id = id, dataType = dataType, value = floatValue };
            case "System.Boolean":
                bool boolValue = reader.ReadBool();
                //Debug.Log($"{id} => {boolValue}");
                return new DataTrackerScriptable.DataTracker { id = id, dataType = dataType, value = boolValue };
            case "System.Collections.Generic.Dictionary`2[Tier,System.Int32]":
                { 
                    List<Tier> ltier = reader.ReadList<Tier>();
                    List<int> lint = reader.ReadList<int>();
                    Dictionary<Tier, int> dictValue = new Dictionary<Tier, int>();
                    for (int i = 0; i < ltier.Count; i++)
                    {
                        dictValue.Add(ltier[i], lint[i]);
                        //Debug.Log($"{id} => {lstr[i]} : {lint[i]}");
                    }
                
                return new DataTrackerScriptable.DataTracker { id = id, dataType = dataType, value = dictValue };
                }
            case "System.Collections.Generic.Dictionary`2[System.String,System.Int32]":
                {
                    List<string> lstr = reader.ReadList<string>();
                    List<int> lint = reader.ReadList<int>();
                    Dictionary<string, int> dictValue = new Dictionary<string, int>();
                    for (int i = 0; i < lstr.Count; i++)
                    {
                        dictValue.Add(lstr[i], lint[i]);
                        //Debug.Log($"{id} => {lstr[i]} : {lint[i]}");
                    }
                    return new DataTrackerScriptable.DataTracker { id = id, dataType = dataType, value = dictValue };
                }
            default:
                return new DataTrackerScriptable.DataTracker { id = id, dataType = dataType, value = null };
        }
    }

        //public static void WriteDatabaseData(this NetworkWriter writer, DatabaseData database)
        //{
        //    NetworkIdentity accountIdentity = database.accountDB.GetComponent<NetworkIdentity>();
        //    writer.WriteNetworkIdentity(accountIdentity);

        //    NetworkIdentity characterIdentity = database.characterDB.GetComponent<NetworkIdentity>();
        //    writer.WriteNetworkIdentity(characterIdentity);
        //}

        //public static DatabaseData ReadDatabaseData(this NetworkReader reader)
        //{

        //    NetworkIdentity accountIdentity = reader.ReadNetworkIdentity();
        //    AccountDB accountDB = accountIdentity != null ? accountIdentity.GetComponent<AccountDB>() : null;

        //    NetworkIdentity characterIdentity = reader.ReadNetworkIdentity();
        //    CharacterDB characterDB = accountIdentity != null ? characterIdentity.GetComponent<CharacterDB>() : null;

        //    return new DatabaseData
        //    {
        //        accountDB = accountDB,
        //        characterDB = characterDB
        //    };
        //}

        /*public static void WriteAccounDB(this NetworkWriter writer, AccountDB account)
        {
            NetworkIdentity networkIdentity = account.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);
        }

        public static AccountDB ReadAccountDB(this NetworkReader reader)
        {
            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            AccountDB accountDB = networkIdentity != null ? networkIdentity.GetComponent<AccountDB>() : null;

            return accountDB;
        }

        public static void WriteCharacterDB(this NetworkWriter writer, CharacterDB character)
        {
            NetworkIdentity networkIdentity = character.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);
        }

        public static CharacterDB ReadCharacterDB(this NetworkReader reader)
        {
            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            CharacterDB characterDB = networkIdentity != null ? networkIdentity.GetComponent<CharacterDB>() : null;

            return characterDB;
        }

        public static void WriteCharacterItem(this NetworkWriter writer, CharacterItem characterItem)
        {
            writer.WriteString(characterItem.id);
            writer.WriteInt(characterItem.quantities);
            writer.WriteString(characterItem.id_gen);
            writer.WriteInt(characterItem.durable);
            writer.WriteInt(characterItem.enhance);
            writer.WriteString(characterItem.id_backend_gen);
        }

        public static CharacterItem ReadCharacterItem(this NetworkReader reader)
        {
            string id = reader.ReadString();
            int qty = reader.ReadInt();
            string id_gen = reader.ReadString();
            int durable = reader.ReadInt();
            int enhance = reader.ReadInt();
            string id_backend_gen = reader.ReadString();

            return new CharacterItem { id = id, quantities = qty, id_gen = id_gen, durable = durable, enhance = enhance, id_backend_gen = id_backend_gen };
        }

        public static void WriteCharacterEquipment(this NetworkWriter writer, CharacterEquipment characterEquipment)
        {
            writer.WriteString(characterEquipment.slotId);
            writer.WriteString(characterEquipment.itemId);
            writer.WriteString(characterEquipment.name);
        }

        public static CharacterEquipment ReadCharacterEquipment(this NetworkReader reader)
        {
            string slot_id = reader.ReadString();
            string item_id = reader.ReadString();
            string name = reader.ReadString();

            return new CharacterEquipment { slotId = slot_id, itemId = item_id, name = name};
        }*/

        //public static void WriteCharacterCurrency(this NetworkWriter writer, CharacterCurrency characterCurrency)
        //{
        //    writer.WriteInt(characterCurrency.bronze);
        //    writer.WriteInt(characterCurrency.silver);
        //    writer.WriteInt(characterCurrency.gold);
        //}

        //public static CharacterCurrency ReadCharacterCurrency(this NetworkReader reader)
        //{
        //    int bronze = reader.ReadInt();
        //    int silver = reader.ReadInt();
        //    int gold = reader.ReadInt();

        //    return new CharacterCurrency { bronze = bronze, silver = silver, gold = gold };
        //}
    }
