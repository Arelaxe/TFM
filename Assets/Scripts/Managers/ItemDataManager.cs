using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : Singleton<ItemDataManager>
{
    private Dictionary<int, Item> dataDictionary;

    protected override void LoadData()
    {
        LoadFromResources();
    }

    private void LoadFromResources()
    {
        dataDictionary = new();
        Item[] items = Resources.LoadAll<Item>(GlobalConstants.ResourcesBaseDataFolder + "\\" + GlobalConstants.ResourcesItemDataFolder);
        foreach (Item item in items)
        {
            dataDictionary.TryAdd(item.ID, item);
        }
    }

    public Item Get(int id)
    {
        Item item = null;
        if (dataDictionary.ContainsKey(id))
        {
            item = dataDictionary[id];
        }
        return item;
    }

}
