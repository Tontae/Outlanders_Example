using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "UserScoreData", menuName = "OutlanderDatabase/UserScoreData", order = 3)]

public class UserScoreScriptable : ScriptableObject
{
    public string username;
    public int sum_score;
    public int rank;
}
