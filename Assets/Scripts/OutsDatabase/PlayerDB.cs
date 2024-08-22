using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

[Serializable]
public class PlayerDB : MonoBehaviour
{
    [SerializeField] private List<PlayerScriptable> datas;

    private Dictionary<string, PlayerScriptable> idWithPlayer;

    public void CreateDataIndex()
    {
        idWithPlayer = new Dictionary<string, PlayerScriptable>();
    }

    public void SyncDatabase(List<PlayerScriptable> datas)
    {
        this.datas = datas;

        for (int i = 0; i < datas.Count; i++)
        {
            if(!idWithPlayer.ContainsKey(datas[i].id))
            {
                idWithPlayer.Add(datas[i].id, datas[i]);
            }
            else
            {
                idWithPlayer[datas[i].id] = datas[i];
            }
        }
    }

    public List<PlayerScriptable> LoadData()
    {
        return datas;
    }

    public Dictionary<string, PlayerScriptable> LoadDataIndex()
    {
        return idWithPlayer;
    }

    public void SaveDataIndex(Dictionary<string, PlayerScriptable> datas)
    {
        this.idWithPlayer = datas;
    }

    // Create data with index
    public void CreateData(PlayerScriptable data)
    {
        datas.Add(data);
        idWithPlayer.Add(data.id, data);
    }

    // Read data with index
    public PlayerScriptable ReadData(string id)
    {
        if (idWithPlayer.ContainsKey(id)) { return idWithPlayer[id]; }
        else { return null; }
    }

    public PlayerScriptable ReadDataByConn(NetworkConnectionToClient conn)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].conn == conn.connectionId)
            {
                return datas[i];
            }
        }
        return null;
    }

    // Update data with index
    public void UpdateData(string id, PlayerScriptable data)
    {
        if(idWithPlayer.ContainsKey(id))
        {
            idWithPlayer[id] = data;


            for (int i = 0; i < datas.Count; i++)
            {
                if(datas[i].id == id)
                {
                    datas[i] = data;
                    break;
                }
            }
        }
    }

    // Delete data with index
    public void DeleteData(string id)
    {
        if (idWithPlayer.ContainsKey(id))
        {
            idWithPlayer.Remove(id);

            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].id == id)
                {
                    datas.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void ClearData()
    {
        datas.Clear();
        idWithPlayer.Clear();
    }
}
