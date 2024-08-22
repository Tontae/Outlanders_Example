using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ItemSpriteDB : MonoBehaviour
{
    [SerializeField] private List<Sprite> itemSpriteList;

    private Dictionary<string, Sprite> itemSpriteWithIndex;

    public void CreateDataIndex()
    {
        itemSpriteWithIndex = new Dictionary<string, Sprite>();

        string spriteDataPath = "ItemSprite";
        Object[] allItemInDatabase = Resources.LoadAll(spriteDataPath, typeof(Sprite));

        foreach (var item in allItemInDatabase)
        {
            Sprite itemObject = (Sprite)item;
            itemSpriteList.Add(itemObject);
            itemSpriteWithIndex.Add(itemObject.name.ToString(), itemObject);

        }
    }

    public void SyncDatabase(List<Sprite> itemSpriteList)
    {
        this.itemSpriteList = itemSpriteList;

        for (int i = 0; i < itemSpriteList.Count; i++)
        {
            if (!itemSpriteWithIndex.ContainsKey(itemSpriteList[i].name.ToString()))
            {
                itemSpriteWithIndex.Add(itemSpriteList[i].name.ToString(), itemSpriteList[i]);
            }
            else
            {
                itemSpriteWithIndex[itemSpriteList[i].name.ToString()] = itemSpriteList[i];
            }
        }
    }

    public List<Sprite> LoadData()
    {
        return itemSpriteList;
    }

    public Dictionary<string, Sprite> LoadDataIndex()
    {
        return itemSpriteWithIndex;
    }

    // Create data with index
    public void CreateData(Sprite itemSprite)
    {
        itemSpriteList.Add(itemSprite);
        itemSpriteWithIndex.Add(itemSprite.name.ToString(), itemSprite);
    }

    // Read data with index
    public Sprite ReadData(string itemId)
    {
        if (itemSpriteWithIndex.ContainsKey(itemId)) { return itemSpriteWithIndex[itemId]; }
        else { return null; }
    }

    // Update data with index
    public void UpdateData(string spriteName, Sprite data)
    {
        if (itemSpriteWithIndex.ContainsKey(spriteName))
        {
            itemSpriteWithIndex[spriteName] = data;

            for (int i = 0; i < itemSpriteList.Count; i++)
            {
                if (itemSpriteList[i].name.ToString() == spriteName)
                {
                    itemSpriteList[i] = data;
                    break;
                }
            }
        }
    }

    // Delete data with index
    public void DeleteData(string spriteName)
    {
        if (itemSpriteWithIndex.ContainsKey(spriteName))
        {
            itemSpriteWithIndex.Remove(spriteName);

            for (int i = 0; i < itemSpriteList.Count; i++)
            {
                if (itemSpriteList[i].name.ToString() == spriteName)
                {
                    itemSpriteList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void ClearData()
    {
        itemSpriteList.Clear();
        itemSpriteWithIndex.Clear();
    }
}
