using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class InventoryController : MonoBehaviour
{
    private PlayerInput input;
    private InputAction inventoryAction;

    private PlayerController playerController;

    [SerializeField]
    private InventoryPage page;

    [SerializeField]
    private Inventory inventory1;
    
    [SerializeField]
    private Inventory inventory2;

    public void Start()
    {
        playerController = GetComponent<PlayerController>();

        InitInputActions();
        InitPage();
    }

    void Update()
    {
        if (inventoryAction.triggered)
        {
            if (!page.isActiveAndEnabled)
            {
                page.LoadItems(inventory1, inventory2);
                page.Show();
            }
            else
            {
                page.Hide();
            }
        }
    }

    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        inventoryAction = input.actions[PlayerConstants.ActionInventory];
    }

    private void InitPage()
    {
        if (inventory2)
        {
            page.InitInventoriesUI(inventory1.Size, inventory2.Size);
        }
        else if (inventory1)
        {
            page.InitInventoriesUI(inventory1.Size, 0);
        }
        else
        {
            page.InitInventoriesUI(0, 0);
        }

        page.OnDescriptionRequested += HandleDescriptionRequest;
        page.OnSwapItems += HandleSwapItems;
        page.OnStartDragging += HandleDragging;
    }

    public bool HasCharacterItem(bool isCharacter1, Item item)
    {
        bool hasItem;

        bool hasItemChar1 = inventory1.GetItems().Contains(item);
        bool hasItemChar2 = inventory2.GetItems().Contains(item);

        if (playerController.AreGrouped())
        {
            hasItem = hasItemChar1 || hasItemChar2;
        }
        else
        {
            hasItem = isCharacter1 ? hasItemChar1 : hasItemChar2;
        }

        return hasItem;
    }

    public bool IsCharacterInventoryFull(bool isCharacter1)
    {
        bool isFull;

        bool isChar1Full = inventory1.isFull;
        bool isChar2Full = inventory2.isFull;

        if (playerController.AreGrouped())
        {
            isFull = isChar1Full && isChar2Full;
        }
        else
        {
            isFull = isCharacter1 ? isChar1Full : isChar2Full;
        }

        return isFull;
    }

    public void AddItem(bool isCharacter1, Item item)
    {
        if (isCharacter1)
        {
            AddItemToCharacter(!inventory1.isFull, item);
        }
        else
        {
            AddItemToCharacter(inventory2.isFull, item);
        }
    }

    public void AddItemToCharacter(bool isCharacter1, Item item)
    {
        if (isCharacter1)
        {
            inventory1.AddItem(item);
        }
        else
        {
            inventory2.AddItem(item);
        }
        page.UpdateItems(inventory1, inventory2);
    }

    public void RemoveItem(Item item)
    {
        inventory1.GetItems().Remove(item);
        inventory2.GetItems().Remove(item);
        page.UpdateItems(inventory1, inventory2);
    }

    public void UpdateItemPanelsForSwitch(bool isCharacter1, bool grouped)
    {
        if (!grouped)
        {
            page.EnableContentPanel(isCharacter1);
            page.DisableContentPanel(!isCharacter1);
        }
    }

    public void UpdateItemPanelsForGrouping(bool isCharacter1, bool grouped)
    {
        if (grouped)
        {
            page.EnableContentPanel(!isCharacter1);
        }
        else
        {
            page.DisableContentPanel(!isCharacter1);
        }
    }

    // Page Event Handlers

    private void HandleDescriptionRequest(int inventoryIndex, int itemIndex)
    {
        Inventory inventory = inventoryIndex == 1 ? inventory1 : inventory2;
        if (itemIndex < inventory.GetItems().Count)
        {
            Item item = inventory.GetItems()[itemIndex];
            page.UpdateDescription(inventoryIndex, itemIndex, item);
        }
    }

    private void HandleSwapItems(int draggedItemInventoryIndex, int draggedItemIndex, int droppedOnInventoryIndex, int droppedOnItemIndex)
    {
        Tuple<Item, int> dragItemResult;
        if (draggedItemInventoryIndex == droppedOnInventoryIndex)
        {
            if (draggedItemInventoryIndex == 1)
            {
                dragItemResult = SwapItems(inventory1, inventory1, draggedItemIndex, droppedOnItemIndex);
            }
            else
            {
                dragItemResult = SwapItems(inventory2, inventory2, draggedItemIndex, droppedOnItemIndex);
            }
        }
        else
        {
            if (draggedItemInventoryIndex == 1)
            {
                dragItemResult = SwapItems(inventory1, inventory2, draggedItemIndex, droppedOnItemIndex);
            }
            else
            {
                dragItemResult = SwapItems(inventory2, inventory1, draggedItemIndex, droppedOnItemIndex);
            }
        }

        page.UpdateItems(inventory1, inventory2);
        page.UpdateDescription(droppedOnInventoryIndex, dragItemResult.Item2, dragItemResult.Item1);
    }

    private Tuple<Item, int> SwapItems(Inventory draggedInventory, Inventory droppedOnInventory, int draggedItemIndex, int droppedOnItemIndex)
    {
        Item draggedItem = draggedInventory.GetItems()[draggedItemIndex];
        int selectedIndex = draggedItemIndex;

        if (droppedOnItemIndex < droppedOnInventory.GetItems().Count)
        {
            Item droppedOnItem = droppedOnInventory.GetItems()[droppedOnItemIndex];

            droppedOnInventory.GetItems()[droppedOnItemIndex] = draggedItem;
            draggedInventory.GetItems()[draggedItemIndex] = droppedOnItem;

            selectedIndex = droppedOnItemIndex;
        }
        else if (draggedInventory != droppedOnInventory)
        {
            draggedInventory.GetItems().RemoveAt(draggedItemIndex);
            droppedOnInventory.AddItem(draggedItem);

            selectedIndex = droppedOnInventory.GetItems().IndexOf(draggedItem);
        }

        return Tuple.Create(draggedItem, selectedIndex);
    }

    private void HandleDragging(int inventoryIndex, int itemIndex)
    {
        Inventory inventory = inventoryIndex == 1 ? inventory1 : inventory2;
        if (itemIndex < inventory.GetItems().Count)
        {
            Item item = inventory.GetItems()[itemIndex];
            page.SetDraggedItem(item.ItemImage);
        }
    }
}
