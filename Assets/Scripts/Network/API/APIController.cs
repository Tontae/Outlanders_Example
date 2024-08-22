using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;

public static class APIController
{
    #region Player

    public static IEnumerator SendLoginData(JSONLogin login, Action<JSONToken> callback) //, Action<JSONPlayer> callback
    {
        string json = JsonConvert.SerializeObject(login);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "login", "POST");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONToken token = new JSONToken();
        token = JsonConvert.DeserializeObject<JSONToken>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendLoginData): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text);
            callback(null);
        }
        else
        {
            Debug.Log($"[Backend] {token.message}");
            callback(token);
        }

    }

    public static IEnumerator SendRefreshToken(JSONRefreshToken oldToken, Action<JSONToken> callback) //, Action<JSONPlayer> callback
    {
        string json = JsonConvert.SerializeObject(oldToken);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "refresh-token", "POST");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONToken token = new JSONToken();
        token = JsonConvert.DeserializeObject<JSONToken>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendLoginData): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text);
            callback(null);
        }
        else
        {
            Debug.Log($"[Backend] {token.message} : {token.token.token}");
            callback(token);
        }
    }

    public static IEnumerator SendGetPlayerData(string login, Action<JSONPlayerData> callback) //, Action<JSONPlayer> callback
    {
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "profile/", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + login);

        yield return request.SendWebRequest();

        JSONPlayerData player = new JSONPlayerData();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendGetPlayerData): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text);
            callback(null);
        }
        else
        {
            player = JsonConvert.DeserializeObject<JSONPlayerData>(request.downloadHandler.text);
            Debug.Log($"[Backend] Player : {player.data.username} is got data");
            callback(player);
        }
    }

    public static IEnumerator SendPlayerHistory(JSONUpdateHistory history,PlayerScriptable account)
    {
        string json = JsonConvert.SerializeObject(history);
        //string json = JsonUtility.ToJson(player);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "history/add", "POST");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(Encryption(json));
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + account.token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONPlayerHistory data = new JSONPlayerHistory();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendPlayerHistory): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at update history player : {account.id}");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONPlayerHistory>(request.downloadHandler.text);
            Debug.Log($"[Backend] Account : {account.id} is {data.message} data");
        }
    }

    public static IEnumerator GetPlayerSound(string token, Action<JSONPlayerSound> callback)
    {
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "sound/player", "GET");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        JSONPlayerSound data = new JSONPlayerSound();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (GetPlayerSound): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at get player sound.");
            callback(null);
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONPlayerSound>(request.downloadHandler.text);
            Debug.Log($"[Backend] Account is got sound data.");
            callback(data);
        }
    }

    public static IEnumerator UpdatePlayerSound(JSONUpdateSound setting, string token, Action<bool> callback)
    {
        string json = JsonConvert.SerializeObject(setting);
        //string json = JsonUtility.ToJson(player);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "sound/player/update", "PUT");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (UpdatePlayerSound): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at update sound.");
            callback(false);
        }
        else
        {
            Debug.Log($"[Backend] Sound setting is updated.");
            callback(true);
        }
    }

    #endregion

    #region Match

    public static IEnumerator SendCreateMatch(JSONCreateMatch match, string token, Action<JSONMatch> callBack)
    {
        string json = JsonConvert.SerializeObject(match);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/create", "POST");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(Encryption(json));
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONMatch data = new JSONMatch();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendCreateMatch): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at create match.");
            callBack(null);
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatch>(request.downloadHandler.text);
            Debug.Log($"[Backend] Match : {data.data} is Created | Status is : {data.data.status}");
            callBack(data);
        }
    }

    public static IEnumerator SendMatchUpdateStatus(JSONUpdateMatchStatus match)
    {
        string json = JsonConvert.SerializeObject(match);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/update", "PUT");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(Encryption(json));
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONMatch data = new JSONMatch();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendMatchUpdateStatus): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at update match status : {match.status}.");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatch>(request.downloadHandler.text);
            Debug.Log($"[Backend] Match : {match.roomID} | Status : {match.status}");
        }
    }

    public static IEnumerator ClientSendMatchUpdatePlayer(string token,JSONClientUpdateMatchPlayer match)
    {
        string json = JsonConvert.SerializeObject(match);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/player/update", "PUT");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(Encryption(json));
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONMatch data = new JSONMatch();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendMatchUpdatePlayer): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" by Player : {match.playerID} at update match player by client.");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatch>(request.downloadHandler.text);
            Debug.Log($"[Backend] Match : {match.roomID} | Player {match.playerID} is {match.type} the Match");
        }
    }

    public static IEnumerator ServerSendMatchUpdatePlayer(JSONServerUpdateMatchPlayer match)
    {
        string json = JsonConvert.SerializeObject(match);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/update-player", "PUT");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(Encryption(json));
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONMatch data = new JSONMatch();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendMatchUpdatePlayer): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" by Player : {match.playerID} at update match player by server.");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatch>(request.downloadHandler.text);
            Debug.Log($"[Backend] Match : {match.roomID} | Player {match.playerID} is {match.type} the Match");
        }
    }

    public static IEnumerator ClientSendGetMatchDetail(TokenMsg player,Action<JSONMatch> callback)
    {
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/get-room/" + player.matchID, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + player.token);

        yield return request.SendWebRequest();

        JSONMatch data = new JSONMatch();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendMatchUpdateStatus): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at client get room : {player.matchID} detail.");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatch>(request.downloadHandler.text);
            Debug.Log($"[Backend] Match : {data.data.matchID} | Status : {data.data.status}");
        }
        callback(data);
    }

    public static IEnumerator ServerSendGetMatchDetail(JSONGetMatchDetail content, Action<JSONMatch> callback)
    {
        string json = JsonConvert.SerializeObject(content);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/detail", "POST");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(Encryption(json));
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONMatch data = new JSONMatch();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendMatchUpdateStatus): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at server get room : {content.roomID} detail.");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatch>(request.downloadHandler.text);
            Debug.Log($"[Backend] Match : {data.data.matchID} got data.");
            callback(data);
        }
    }

    public static IEnumerator GetMatchList(string token,Action<JSONMatchList> callback)
    {
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "game-room/availability", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        JSONMatchList data = new JSONMatchList();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (SendMatchUpdateStatus): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text + $" at get room list.");
        }
        else
        {
            data = JsonConvert.DeserializeObject<JSONMatchList>(request.downloadHandler.text);
        }
        callback(data);
    }
    #endregion

    #region Other

    public static IEnumerator BadWordFilter(JSONRequestBadWordFilter content, Action<JSONResponsesBadWordFilter> callBack)
    {
        string json = JsonConvert.SerializeObject(content);
        UnityWebRequest request = new UnityWebRequest(UIManagers.Instance.uiNetwork.api + "message/filter", "POST");

        byte[] jsonByte = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonByte);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        JSONResponsesBadWordFilter data = new JSONResponsesBadWordFilter();
        data = JsonConvert.DeserializeObject<JSONResponsesBadWordFilter>(request.downloadHandler.text);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[Backend] Error While Sending (BadWordFilter): " + request.error);
            Debug.Log("[Backend] Error Log : " + request.downloadHandler.text);
            callBack(null);
        }
        else
        {
            callBack(data);
        }
    }

    #endregion

    #region Encryption

    public static string Encryption(string json)
    {
        var encryptionResult = SimpleAESEncryption.Encrypt(json);
        JSONMatchEncryption encryp = new JSONMatchEncryption() { data = encryptionResult.EncryptedText };
        return JsonConvert.SerializeObject(encryp);
    }

    #endregion
}
