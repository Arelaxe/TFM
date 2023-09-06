using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : Singleton<ItemDataManager>
{
    private Dictionary<string, Item> dataDictionary;

    protected override void LoadData()
    {
        LoadFromResources();
    }

    private void LoadFromResources()
    {
        dataDictionary = new();
        Inventory[] inventories = Resources.LoadAll<Inventory>(GlobalConstants.ResourcesBaseDataFolder + "/" + GlobalConstants.ResourcesItemDataFolder);
        foreach (Inventory inventory in inventories)
        {
            foreach (Item item in inventory.GetItems())
            {
                dataDictionary.TryAdd(item.Name, item);
            }
        }
    }

    public Item Get(string id)
    {
        Item item = null;
        if (dataDictionary.ContainsKey(id))
        {
            item = dataDictionary[id];
        }
        return item;
    }

}
