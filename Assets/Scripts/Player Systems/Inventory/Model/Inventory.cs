using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private int inventoryIndex;
    public int Index { get => inventoryIndex; }

    [SerializeField]
    private List<Item> items;

    [field: SerializeField]
    public int Size { get; set; }
    public bool IsFull => items.Count == Size;
    public bool IsEmpty => items.Count == 0;

    public void AddItem(Item item)
    {
        if (!IsFull && !items.Contains(item))
        {
            items.Add(item);
        }
    }
    public void AddItem(int index, Item item)
    {
        if (!IsFull && !items.Contains(item))
        {
            items.Insert(index, item);
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void Clear()
    {
        items.Clear();
    }
}