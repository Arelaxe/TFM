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
        if (inventoryAction.WasPressedThisFrame())
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

    private void HandleSwapItems(int draggedItemInventoryIndex, int draggedItemIndex, int droppedOnItemIndex)
    {
        Tuple<Item, int> dragItemResult;
        if (draggedItemInventoryIndex == 1)
        {
            dragItemResult = SwapItems(inventory1, inventory2, draggedItemIndex, droppedOnItemIndex);
        }
        else
        {
            dragItemResult = SwapItems(inventory2, inventory1, draggedItemIndex, droppedOnItemIndex);
        }

        page.UpdateItems(inventory1, inventory2);
        page.UpdateDescription(draggedItemInventoryIndex == 1 ? 2 : 1, dragItemResult.Item2, dragItemResult.Item1);
    }

    private Tuple<Item, int> SwapItems(Inventory draggedInventory, Inventory droppedOnInventory, int draggedItemIndex, int droppedOnItemIndex)
    {
        Item draggedItem = draggedInventory.GetItems()[draggedItemIndex];

        if (droppedOnItemIndex < droppedOnInventory.GetItems().Count)
        {
            droppedOnInventory.AddItem(draggedItem);
            draggedInventory.AddItem(droppedOnInventory.GetItems()[droppedOnItemIndex]);

            droppedOnInventory.GetItems().RemoveAt(droppedOnItemIndex);
            draggedInventory.GetItems().RemoveAt(draggedItemIndex);
        }
        else
        {
            droppedOnInventory.AddItem(draggedItem);
            draggedInventory.GetItems().RemoveAt(draggedItemIndex);
        }

        return Tuple.Create<Item, int>(draggedItem, droppedOnInventory.GetItems().IndexOf(draggedItem));
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
