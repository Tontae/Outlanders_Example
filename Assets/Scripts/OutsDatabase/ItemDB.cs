using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ItemDB : MonoBehaviour
{
    [SerializeField] private List<ItemScriptable> datas;

    private Dictionary<string, ItemScriptable> datasWithIndex;

    public void CreateDataIndex()
    {
        datasWithIndex = new Dictionary<string, ItemScriptable>();

      
        string databasePath = "BATTLEROYALItemScriptable";
        Object[] allItemInDatabase = Resources.LoadAll(databasePath, typeof(ItemScriptable));

        foreach (var item in allItemInDatabase)
        {
            ItemScriptable itemObject = (ItemScriptable)item;
            datas.Add(itemObject);
            datasWithIndex.Add(itemObject.itemId.ToString(), itemObject);
        }
    }

    public void SyncDatabase(List<ItemScriptable> datas)
    {
        this.datas = datas;

        for (int i = 0; i < datas.Count; i++)
        {
            if (!datasWithIndex.ContainsKey(datas[i].itemId.ToString()))
            {
                datasWithIndex.Add(datas[i].itemId.ToString(), datas[i]);
            }
            else
            {
                datasWithIndex[datas[i].itemId.ToString()] = datas[i];
            }
        }
    }

    public List<ItemScriptable> LoadData()
    {
        return datas;
    }

    public Dictionary<string, ItemScriptable> LoadDataIndex()
    {
        return datasWithIndex;
    }

    public void SaveDataIndex(Dictionary<string, ItemScriptable> datas)
    {
        this.datasWithIndex = datas;
    }

    // Create data with index
    public void CreateData(ItemScriptable data)
    {
        datas.Add(data);
        datasWithIndex.Add(data.itemId.ToString(), data);
    }

    // Read data with index
    public ItemScriptable ReadData(string itemId)
    {
        if (datasWithIndex.ContainsKey(itemId)) { return datasWithIndex[itemId]; }
        else { return null; }
    }

    // Update data with index
    public void UpdateData(string itemId, ItemScriptable data)
    {
        if (datasWithIndex.ContainsKey(itemId))
        {
            datasWithIndex[itemId] = data;

            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].itemId.ToString() == itemId)
                {
                    datas[i] = data;
                    break;
                }
            }
        }
    }

    // Delete data with index
    public void DeleteData(string itemId)
    {
        if (datasWithIndex.ContainsKey(itemId))
        {
            datasWithIndex.Remove(itemId);

            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].itemId.ToString() == itemId)
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
        datasWithIndex.Clear();
    }
}
