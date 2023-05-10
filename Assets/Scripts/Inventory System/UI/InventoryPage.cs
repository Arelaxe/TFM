using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private InventoryItem itemUIPrefab;

    [SerializeField]
    private InventoryDescription inventoryDescription;

    [SerializeField]
    private RectTransform contentPanel1;

    [SerializeField]
    private RectTransform contentPanel2;

    [SerializeField]
    private MouseFollower mouseFollower;

    private List<InventoryItem> items1 = new List<InventoryItem>();
    private List<InventoryItem> items2 = new List<InventoryItem>();

    private int currentDraggedItemIndex = -1;
    private int currentDraggedItemInventoryIndex = -1;

    public event Action<int, int> OnDescriptionRequested, OnStartDragging;
    public event Action<int, int, int> OnSwapItems;

    private void Awake()
    {
        Hide();
        mouseFollower.Toggle(false);
        inventoryDescription.ResetDescription();
    }

    public void InitInventoriesUI(int inventorySize1, int inventorySize2)
    {
        if (inventorySize1 > 0)
        {
            InitInventoryUI(inventorySize1, contentPanel1, items1);
        }
        if (inventorySize2 > 0)
        {
            InitInventoryUI(inventorySize2, contentPanel2, items2);
        }
    }

    private void InitInventoryUI(int inventorySize, RectTransform contentPanel, List<InventoryItem> itemList)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem item = Instantiate(itemUIPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(contentPanel);
            item.transform.localScale = new Vector3(1, 1, 1);
            itemList.Add(item);

            item.OnItemClicked += HandleItemSelection;
            item.OnItemBeginDrag += HandleBeginDrag;            
            item.OnItemEndDrag += HandleEndDrag;
            item.OnItemDroppedOn += HandleSwap;
        }
    }

    public void LoadItems(Inventory inventory1, Inventory inventory2)
    {
        LoadItems(inventory1, items1);
        LoadItems(inventory2, items2);
    }

    protected void LoadItems(Inventory inventory, List<InventoryItem> items)
    {
        if (inventory)
        {
            for (int i = 0; i < inventory.GetItems().Count; i++)
            {
                items[i].SetData(inventory.GetItems()[i].ItemImage);
            }
        }
    }

    public void UpdateItems(Inventory inventory1, Inventory inventory2)
    {
        UpdateItems(inventory1, items1);
        UpdateItems(inventory2, items2);
    }

    protected void UpdateItems(Inventory inventory, List<InventoryItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (i < inventory.GetItems().Count)
            {
                items[i].SetData(inventory.GetItems()[i].ItemImage);
            }
            else
            {
                items[i].ResetData();
            }
        }
    }

    public void UpdateDescription(int inventoryIndex, int itemIndex, Item item)
    {
        inventoryDescription.SetDescription(item.ItemImage, item.Name, item.Description);
        DeselectAll();
        if (inventoryIndex == 1)
        {
            items1[itemIndex].Select();
        }
        else
        {
            items2[itemIndex].Select();
        }
    }

    public void SetDraggedItem(Sprite sprite)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentDraggedItemIndex = -1;
    }

    private void DeselectAll()
    {
        foreach (var item in items1)
        {
            item.Deselect();
        }

        foreach (var item in items2)
        {
            item.Deselect();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        inventoryDescription.ResetDescription();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Events

    private void HandleItemSelection(InventoryItem item)
    {
        if (items2.Contains(item))
        {
            OnDescriptionRequested?.Invoke(2, items2.IndexOf(item));
        }
        else
        {
            OnDescriptionRequested?.Invoke(1, items1.IndexOf(item));
        }
    }

    private void HandleBeginDrag(InventoryItem item)
    {
        HandleItemSelection(item);
        if (items2.Contains(item))
        {
            currentDraggedItemIndex = items2.IndexOf(item);
            currentDraggedItemInventoryIndex = 2;
            OnStartDragging?.Invoke(2, items2.IndexOf(item));
        }
        else
        {
            currentDraggedItemIndex = items1.IndexOf(item);
            currentDraggedItemInventoryIndex = 1;
            OnStartDragging?.Invoke(1, items1.IndexOf(item));
        }
    }

    private void HandleEndDrag(InventoryItem item)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(InventoryItem item)
    {
        HandleItemSelection(item);
        if (playerController.AreGrouped())
        {
            if (items1.Contains(item) && currentDraggedItemInventoryIndex != 1)
            {
                OnSwapItems?.Invoke(currentDraggedItemInventoryIndex, currentDraggedItemIndex, items1.IndexOf(item));
            }
            else if (items2.Contains(item) && currentDraggedItemInventoryIndex != 2)
            {
                OnSwapItems?.Invoke(currentDraggedItemInventoryIndex, currentDraggedItemIndex, items2.IndexOf(item));
            }
        }
    }
}
