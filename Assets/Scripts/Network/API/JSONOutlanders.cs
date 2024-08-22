using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class JSONOutlanders {}

public class JSONRequestBadWordFilter
{
    [JsonProperty("message")] public string message;
}

public class JSONResponsesBadWordFilter
{
    [JsonProperty("status")] public bool status;
    [JsonProperty("message")] public string message;
}