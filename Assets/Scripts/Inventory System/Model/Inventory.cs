using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<Item> items;
    [field: SerializeField]
    public int Size { get; set; }
    private bool isFull => items.Count == Size;

    public void AddItem(Item item)
    {
        if (!isFull)
        {
            items.Add(item);
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }
}