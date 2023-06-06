using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Documentation")]
public class Documentation : ScriptableObject
{
    [SerializeField]
    private List<Item> documents;

    public void AddDocument(Item item)
    {
        documents.Add(item);
    }
    public void AddDocument(int index, Item item)
    {
        documents.Insert(index, item);
    }

    public List<Item> GetDocuments()
    {
        return documents;
    }
}