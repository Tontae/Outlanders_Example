using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class JSONLogin
{
    [JsonProperty("email")] public string email;
    [JsonProperty("password")] public string password;
}

[Serializable]
public class JSONToken
{
    [JsonProperty("status")] public bool status;
    [JsonProperty("message")] public string message;
    [JsonProperty("token")] public Token token;
    [JsonProperty("refresh_token")] public string refreshToken;
}

[Serializable]
public class Token
{
    [JsonProperty("token")] public string token;
    [JsonProperty("expires_in")] public int expiresIn;
}