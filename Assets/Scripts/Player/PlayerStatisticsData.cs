using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerStatisticsData : PlayerElements
    {
        public DataTrackerScriptable trackerScriptable;
        private Dictionary<string, DataTrackerScriptable.DataTracker> dictDataTracker = new Dictionary<string, DataTrackerScriptable.DataTracker>();

        public struct KillDamage
        {
            public int killCount;
            public float totalDamage;
        }

        public struct ClientCompassData
        {
            public int healthPotionUseCount;
            public int manaPotionUseCount;
            public int skillUseCount;
            public int craftItemCount;
            public List<Tier> craftItemNames;
            public List<int> craftItemCounts;
            public int openChestCount;
            public List<string> pickupNames;
            public List<int> pickupCounts;
            public int receiveCoin;
            public int coinUsed;
            public int buyItemCount;
            public List<string> craftMaterialsUsedNames;
            public List<int> craftMaterialsUsedCounts;
        }

        public struct DamageName
        {
            public float time;
            public string name;
            public float damage;
        }

        // Server-side
        public int PlayerKillCount 
        { 
            get 
            { 
                return (int)GetData("playerKillCount", 0); 
            } 
            set 
            {
                ModifyData("playerKillCount", value);
                if (isLocalPlayer)
                    Player.PlayerUIManager.UpdateKillCount(value);
                
            } 
        }
        public int PlayerRank { get => (int)GetData("playerRank", 0); set => ModifyData("playerRank", value); }
        public float PlayerSurviveTime { get => (float)GetData("playerSurviveTime", 0f); set => ModifyData("playerSurviveTime", value); }
        public int MonsterKillCount { get => (int)GetData("monsterKillCount", 0); set => ModifyData("monsterKillCount", value); }
        public int MiniBossKillCount { get => (int)GetData("miniBossKillCount",0); set => ModifyData("miniBossKillCount", value); }
        public int BossKillCount { get => (int)GetData("bossKillCount", 0); set => ModifyData("bossKillCount", value); }
        public int TotalMonsterKill { get => (int)GetData("totalMonsterKill", 0); set => ModifyData("totalMonsterKill", value); }
        public float TotalPlayerDamage { get => (float)GetData("totalPlayerDamage", 0f); set => ModifyData("totalPlayerDamage", value); }
        public float MinDamage { get => (float)GetData("minDamage", float.MaxValue); set => ModifyData("minDamage", value); }
        public float MaxDamage { get => (float)GetData("maxDamage", 0f); set => ModifyData("maxDamage", value); }
        public int CritCount { get => (int)GetData("critCount", 0); set => ModifyData("critCount", value); }
        public int BlockCount { get => (int)GetData("blockCount", 0); set => ModifyData("blockCount", value); }
        public float TotalPlayerDamageReceive { get => (float)GetData("totalPlayerDamageReceive", 0f); set => ModifyData("totalPlayerDamageReceive", value); }
        public bool EndMatchNormal { get => (bool)GetData("endMatchNormal", false); set => ModifyData("endMatchNormal", value); }
        public bool IsKillBounty { get => (bool)GetData("isKillBounty", false); set => ModifyData("isKillBounty", value); }
        public bool IsKillFirst { get => (bool)GetData("isKillFirst", false); set => ModifyData("isKillFirst", value); }
        public bool IsGodSlayer { get => (bool)GetData("isGodSlayer", false); set => ModifyData("isGodSlayer", value); }
        public bool IsSurviveBounty { get => (bool)GetData("isSurviveBounty", false); set => ModifyData("isSurviveBounty", value); }
        public Dictionary<string, float> PlayerDamageReceive { get => (Dictionary<string, float>)GetData("playerDamageReceive", new Dictionary<string, float>()); set => ModifyData("playerDamageReceive", value); }
        public Dictionary<WeaponManager.WeaponType, KillDamage> PlayerKillDamageData { get => (Dictionary<WeaponManager.WeaponType, KillDamage>)GetData("playerKillDamageData", new Dictionary<WeaponManager.WeaponType, KillDamage>()); set => ModifyData("playerKillDamageData", value); }

        public Dictionary<WeaponManager.WeaponType, KillDamage> MonsKillDamageData { get => (Dictionary<WeaponManager.WeaponType, KillDamage>)GetData("monsKillDamageData", new Dictionary<WeaponManager.WeaponType, KillDamage>()); set => ModifyData("monsKillDamageData", value); }
        public Dictionary<WeaponManager.WeaponType, KillDamage> MiniBossKillDamageData { get => (Dictionary<WeaponManager.WeaponType, KillDamage>)GetData("miniBossKillDamageData", new Dictionary<WeaponManager.WeaponType, KillDamage>()); set => ModifyData("miniBossKillDamageData", value); }
        public Dictionary<WeaponManager.WeaponType, KillDamage> BossKillDamageData { get => (Dictionary<WeaponManager.WeaponType, KillDamage>)GetData("bossKillDamageData", new Dictionary<WeaponManager.WeaponType, KillDamage>()); set => ModifyData("bossKillDamageData", value); }

        public Dictionary<string, Dictionary<WeaponManager.WeaponType, KillDamage>> MonsterKillDamageData { get => (Dictionary<string, Dictionary<WeaponManager.WeaponType, KillDamage>>)GetData("monsterKillDamageData", new Dictionary<string, Dictionary<WeaponManager.WeaponType, KillDamage>>()); set => ModifyData("monsterKillDamageData", value); }
        public List<DamageName> DamageNames { get => (List<DamageName>)GetData("damageNames", new List<DamageName>()); set => ModifyData("damageNames", value); }


        // Client-side
        public int HealthPotionUseCount { get => (int)GetData("healthPotionUseCount", 0); set => ModifyData("healthPotionUseCount", value); }
        public int ManaPotionUseCount { get => (int)GetData("manaPotionUseCount", 0); set => ModifyData("manaPotionUseCount", value); }
        public int SkillUseCount { get => (int)GetData("skillUseCount", 0); set => ModifyData("skillUseCount", value); }
        public Dictionary<Tier, int> CraftItemCountDict { get => (Dictionary<Tier, int>)GetData("craftItemCountDict", new Dictionary<Tier, int>()); set => ModifyData("craftItemCountDict", value); }
        public int OpenChestCount { get => (int)GetData("openChestCount", 0); set => ModifyData("openChestCount", value); }
        public Dictionary<string, int> PickupItemCount { get => (Dictionary<string, int>)GetData("pickupItemCount", new Dictionary<string, int>()); set => ModifyData("pickupItemCount", value); }
        public Dictionary<string, int> ObtainItemCount { get => (Dictionary<string, int>)GetData("obtainItemCount", new Dictionary<string, int>()); set => ModifyData("obtainItemCount", value); }
        public int ReceiveCoin { get => (int)GetData("receiveCoin", 0); set => ModifyData("receiveCoin", value); }
        public int CoinUsed { get => (int)GetData("coinUsed", 0); set => ModifyData("coinUsed", value); }
        public int BuyItemCount { get => (int)GetData("buyItemCount", 0); set => ModifyData("buyItemCount", value); }
        public Dictionary<string, int> CraftMaterialsUsedDict { get => (Dictionary<string, int>)GetData("craftMaterialsUsedDict", new Dictionary<string, int>()); set => ModifyData("craftMaterialsUsedDict", value); }
        public float TravelDistance { get => (float)GetData("travelDistance", 0f); set => ModifyData("travelDistance", value); }


        private void Start()
        {
            if(isServer || isLocalPlayer)
                SetDataFromScriptable();
        }

        private void SetDataFromScriptable()
        {
            DataTrackerScriptable temp = Instantiate(trackerScriptable);
            foreach (var entry in temp.dataTrackers)
            {
                dictDataTracker.Add(entry.id, entry);
            }
        }

        public void UpdatePlayerDoDamage(string targetName, WeaponManager.WeaponType weaponType, bool isCrit, float damage, bool isBoss, bool isMiniBoss)
        {
            TotalPlayerDamage += damage;
            if (targetName.Equals("Player") || targetName.Equals("Bot"))
            {
                PlayerKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });

                KillDamage temp = PlayerKillDamageData[weaponType];
                temp.totalDamage += damage;
            }
            else
            {
                if (isBoss)
                {
                    BossKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });
                    KillDamage temps = BossKillDamageData[weaponType];
                    temps.totalDamage += damage;
                }
                else if (isMiniBoss)
                {
                    MiniBossKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });
                    KillDamage temps = MiniBossKillDamageData[weaponType];
                    temps.totalDamage += damage;
                }
                else
                {
                    MonsKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });
                    KillDamage temps = MonsKillDamageData[weaponType];
                    temps.totalDamage += damage;
                }

                MonsterKillDamageData.TryAdd(targetName, CreateDictDataForMonster());

                KillDamage temp = MonsterKillDamageData[targetName][weaponType];
                temp.totalDamage += damage;
                
            }

            if (isCrit)
                CritCount += 1;
            if (MinDamage > damage)
                MinDamage = damage;
            if (MaxDamage < damage)
                MaxDamage = damage;
        }

        public void UpdatePlayerIsDamaged(string enemyName, float damage, bool isWeaponAction)
        {
            TotalPlayerDamageReceive += damage;
            if (!PlayerDamageReceive.TryAdd(enemyName, damage))
                PlayerDamageReceive[enemyName] += damage;
            if (isWeaponAction)
                BlockCount += 1;
        }

        public void UpdatePlayerKill(WeaponManager.WeaponType weaponType, bool isBounty, bool isFirst, bool isGodSlayer)
        {
            PlayerKillCount += 1;

            PlayerKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });

            KillDamage temp = PlayerKillDamageData[weaponType];
            temp.killCount += 1;

            if (isBounty)
                IsKillBounty = true;
            if (isFirst)
                IsKillFirst = true;
            if (isGodSlayer)
                IsGodSlayer = true;
        }

        public void UpdateMonsterKill(string name, bool isMiniBoss, bool isBoss, WeaponManager.WeaponType weaponType)
        {
            TotalMonsterKill += 1;
            if (isBoss)
            {
                BossKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0,totalDamage = 0});
                KillDamage temps = BossKillDamageData[weaponType];
                temps.killCount += 1;
                BossKillCount += 1;
            }
            else if (isMiniBoss)
            {
                MiniBossKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });
                KillDamage temps = MiniBossKillDamageData[weaponType];
                temps.killCount += 1;
                MiniBossKillCount += 1;
            }
            else
            {
                MonsKillDamageData.TryAdd(weaponType, new KillDamage { killCount = 0, totalDamage = 0 });
                KillDamage temps = MonsKillDamageData[weaponType];
                temps.killCount += 1;
                MonsterKillCount += 1;
            }

            MonsterKillDamageData.TryAdd(name, CreateDictDataForMonster());

            KillDamage temp = MonsterKillDamageData[name][weaponType];
            temp.killCount += 1;
        }

        private Dictionary<WeaponManager.WeaponType, KillDamage> CreateDictDataForMonster()
        {
            Dictionary<WeaponManager.WeaponType, KillDamage> results = new Dictionary<WeaponManager.WeaponType, KillDamage>();
            foreach(WeaponManager.WeaponType temp in (WeaponManager.WeaponType[])Enum.GetValues(typeof(WeaponManager.WeaponType)))
                results.Add(temp, new KillDamage { killCount = 0, totalDamage = 0 });
            return results;
        }

        private object GetData(string id, object initValue)
        {
            return dictDataTracker[id].value == null ? dictDataTracker[id].value = initValue : dictDataTracker[id].value;
        }

        private void ModifyData(string id, object newValue)
        {
            if (dictDataTracker.TryGetValue(id, out var data))
            {
                dictDataTracker[id].value = newValue;
                //switch (data.value) 
                //{
                //    case int intValue:
                //        Debug.Log($"{id} => {intValue}");
                //        break;
                //    case float floatValue:
                //        Debug.Log($"{id} => {floatValue}");
                //        break;
                //    case bool boolValue:
                //        Debug.Log($"{id} => {boolValue}");
                //        break;
                //    case Dictionary<string, float> dictValue:
                //        foreach (var temp in dictValue)
                //            Debug.Log($"{id} => {temp.Key} : {temp.Value}");
                //        break;
                //    case Dictionary<WeaponManager.WeaponType, KillDamage> dictValue:
                //        foreach (var temp in dictValue)
                //            Debug.Log($"{id} => {temp.Key} : totalDamage={temp.Value.totalDamage} , totalKill={temp.Value.killCount}");
                //        break;
                //    case Dictionary<string, Dictionary<WeaponManager.WeaponType, KillDamage>> dictValue:
                //        foreach (var temp in dictValue)
                //            foreach (var temp2 in temp.Value)
                //                Debug.Log($"{id} => ({temp.Key},{temp2.Key}) : totalDamage={temp2.Value.totalDamage} , totalKill={temp2.Value.killCount}");
                //        break;
                //    case Dictionary<string, int> dictValue:
                //        foreach (var temp in dictValue)
                //            Debug.Log($"{id} => {temp.Key} : {temp.Value}");
                //        break;
                //}
            }
        }

        private bool IsNumeric(object value)
        {
            return value is int || value is float;
        }

        public void ClientPrepareCountData()
        {
            TravelDistance = Player.MovementStateMachine.DistanceTravel;

            List<DataTrackerScriptable.DataTracker> compressData = new List<DataTrackerScriptable.DataTracker>();

            foreach (var temp in dictDataTracker)
                if (temp.Value.value != null)
                    compressData.Add(temp.Value);

            CmdSendClientCountData(compressData);
        }

        [Command]
        private void CmdSendClientCountData(List<DataTrackerScriptable.DataTracker> compressData)
        {
            foreach (var data in compressData)
            {
                dictDataTracker[data.id].value = data.value;
                switch (data.value)
                {
                    case int intValue:
                        Debug.Log($"{data.id} => {intValue}");
                        break;
                    case float floatValue:
                        Debug.Log($"{data.id} => {floatValue}");
                        break;
                    case bool boolValue:
                        Debug.Log($"{data.id} => {boolValue}");
                        break;
                    case Dictionary<string, float> dictValue:
                        foreach (var temp in dictValue)
                            Debug.Log($"{data.id} => {temp.Key} : {temp.Value}");
                        break;
                    case Dictionary<WeaponManager.WeaponType, KillDamage> dictValue:
                        foreach (var temp in dictValue)
                            Debug.Log($"{data.id} => {temp.Key} : totalDamage={temp.Value.totalDamage} , totalKill={temp.Value.killCount}");
                        break;
                    case Dictionary<string, Dictionary<WeaponManager.WeaponType, KillDamage>> dictValue:
                        foreach (var temp in dictValue)
                            foreach (var temp2 in temp.Value)
                                Debug.Log($"{data.id} => ({temp.Key},{temp2.Key}) : totalDamage={temp2.Value.totalDamage} , totalKill={temp2.Value.killCount}");
                        break;
                    case Dictionary<string, int> dictValue:
                        foreach (var temp in dictValue)
                            Debug.Log($"{data.id} => {temp.Key} : {temp.Value}");
                        break;
                    case Dictionary<Tier, int> dictValue:
                        foreach (var temp in dictValue)
                            Debug.Log($"{data.id} => {temp.Key} : {temp.Value}");
                        break;
                }
            }
        }
    }
}
