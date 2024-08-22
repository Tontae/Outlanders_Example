using JetBrains.Annotations;
using Outlander.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppearanceData", menuName = "OutlanderDatabase/AppearanceData", order = 1)]

public class AppearanceScriptable : ScriptableObject
{
    [Header("MALE")]
    public List<Mesh> maleFace = new List<Mesh>();
    public List<Mesh> maleEyebrow = new List<Mesh>();
    public List<Mesh> maleTorso = new List<Mesh>();
    public List<Mesh> maleArmUpperLeft = new List<Mesh>();
    public List<Mesh> maleArmUpperRight = new List<Mesh>();
    public List<Mesh> maleArmLowerLeft = new List<Mesh>();
    public List<Mesh> maleArmLowerRight = new List<Mesh>();
    public List<Mesh> maleHandLeft = new List<Mesh>();
    public List<Mesh> maleHandRight = new List<Mesh>();
    public List<Mesh> maleHips = new List<Mesh>();
    public List<Mesh> maleLegLeft = new List<Mesh>();
    public List<Mesh> maleLegRight = new List<Mesh>();

    [Header("FEMALE")]
    public List<Mesh> femaleFace = new List<Mesh>();
    public List<Mesh> femaleEyebrow = new List<Mesh>();
    public List<Mesh> femaleTorso = new List<Mesh>();
    public List<Mesh> femaleArmUpperLeft = new List<Mesh>();
    public List<Mesh> femaleArmUpperRight = new List<Mesh>();
    public List<Mesh> femaleArmLowerLeft = new List<Mesh>();
    public List<Mesh> femaleArmLowerRight = new List<Mesh>();
    public List<Mesh> femaleHandLeft = new List<Mesh>();
    public List<Mesh> femaleHandRight = new List<Mesh>();
    public List<Mesh> femaleHips = new List<Mesh>();
    public List<Mesh> femaleLegLeft = new List<Mesh>();
    public List<Mesh> femaleLegRight = new List<Mesh>();

    [Header("BOTH")]
    public List<Mesh> hair = new List<Mesh>();
    public List<Mesh> beard = new List<Mesh>();
}
