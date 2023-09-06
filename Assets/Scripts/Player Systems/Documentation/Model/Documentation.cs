using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Documentation")]
public class Documentation : ScriptableObject
{
    [SerializeField]
    private List<Item> documents;

    public bool IsEmpty => documents.Count == 0;

    public void AddItem(Item item)
    {
        if (!documents.Contains(item))
        {
            documents.Add(item);
        }
    }
    public void AddItem(int index, Item item)
    {
        if (!documents.Contains(item))
        {
            documents.Insert(index, item);
        }
    }

    public List<Item> GetItems()
    {
        return documents;
    }

    public void Clear()
    {
        documents.Clear();
    }
}