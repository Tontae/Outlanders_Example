using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTracker", menuName = "CreateDataTracker")]
public class DataTrackerScriptable : ScriptableObject
{
    [Serializable]
    public class DataTracker
    {
        public string id;
        public string dataType;
        public object value;
    }

    public List<DataTracker> dataTrackers = new List<DataTracker>();
}