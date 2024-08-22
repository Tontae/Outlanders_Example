using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

[Serializable]
public class JSONMatch
{
    [JsonProperty("status")] public bool isStatus;
    [JsonProperty("message")] public string message;
    [JsonProperty("data")] public MatchData data;

    [Serializable]
    public class MatchData
    {
        [JsonProperty("game_mode")] public string gameMode;
        [JsonProperty("is_active")] public bool isActive;
        [JsonProperty("created_at")] public string createdAt;
        [JsonProperty("updated_at")] public string updatedAt;
        [JsonProperty("room_id")] public string matchID;
        [JsonProperty("max_player")] public int maxPlayer;
        [JsonProperty("min_player")] public int minPlayer;
        [JsonProperty("map")] public string map;
        [JsonProperty("status")] public string status;
        [JsonProperty("port")] public int port;
        [JsonProperty("start_at")] public string startAt;
        [JsonProperty("player_data")] public List<JSONPlayerHistory.HistoryData.MatchDetail.PlayerMatchData> playerMatchData;
        [JsonProperty("count_player")] public int countPlayer;
    }
}

[Serializable]
public class JSONMatchList
{
    [JsonProperty("message")] public string message;
    [JsonProperty("match")] public List<MatchListData> matchList;

    [Serializable]
    public class MatchListData
    {
        [JsonProperty("match_id")] public string matchID;
        [JsonProperty("match_port")] public int port;
    }
}

[Serializable]
public class JSONMatchEncryption
{
    [JsonProperty("data")] public string data;
}

[Serializable]
public class JSONRefreshToken
{
    [JsonProperty("refreshToken")] public string token;
}

[Serializable]
public class JSONCreateMatch
{
    [JsonProperty("room_id")] public string roomID;
    [JsonProperty("max_player")] public int maxPlayer;
    [JsonProperty("min_player")] public int minPlayer;
    [JsonProperty("game_mode")] public string gameMode;
    [JsonProperty("map")] public string map;
    [JsonProperty("play_mode")] public string playMode;
}

public class JSONUpdateMatchStatus
{
    [JsonProperty("room_id")] public string roomID;
    [JsonProperty("status")] public string status;
    [JsonProperty("apiKey")] public string key;
}

public class JSONClientUpdateMatchPlayer
{
    [JsonProperty("room_id")] public string roomID;
    [JsonProperty("player_id")] public string playerID;
    [JsonProperty("type")] public string type;
}

public class JSONServerUpdateMatchPlayer
{
    [JsonProperty("room_id")] public string roomID;
    [JsonProperty("player_id")] public string playerID;
    [JsonProperty("type")] public string type;
    [JsonProperty("apiKey")] public string key;
}

public class JSONGetMatchDetail
{
    [JsonProperty("room_id")] public string roomID;
    [JsonProperty("apiKey")] public string key;
}


