using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryController : PageController
{
    [SerializeField]
    private InventoryPage page;
    
    [SerializeField]
    private Inventory inventory1;
    
    [SerializeField]
    private Inventory inventory2;

    private Item selectedItem;
    private DialogueChannel channel;

    public override Page Page => page;

    public override string MenuAction => PlayerConstants.ActionInventory;

    protected override void InitPage()
    {
        page.InitUI(inventory1.Size, inventory2.Size);
        page.OnDescriptionRequested += HandleDescriptionRequest;
        page.OnSwitchInventory += HandleSwitchInventory;
        page.OnSwapItems += HandleSwapItems;
        page.OnStartDragging += HandleDragging;
    }

    public override void LoadData()
    {
        page.LoadData(inventory1);
        page.LoadData(inventory2);
    }

    public bool HasCharacterItem(bool isCharacter1, Item item)
    {
        bool hasItem;

        bool hasItemChar1 = inventory1.GetItems().Contains(item);
        bool hasItemChar2 = inventory2.GetItems().Contains(item);

        if (PlayerManager.Instance.Grouped)
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

        bool isChar1Full = inventory1.IsFull;
        bool isChar2Full = inventory2.IsFull;

        if (PlayerManager.Instance.Grouped)
        {
            isFull = isChar1Full && isChar2Full;
        }
        else
        {
            isFull = isCharacter1 ? isChar1Full : isChar2Full;
        }

        return isFull;
    }

    public void Clear()
    {
        inventory1.Clear();
        inventory2.Clear();
    }

    public void AddItem(bool isCharacter1, Item item)
    {
        if (isCharacter1)
        {
            AddItemToCharacter(!inventory1.IsFull, item);
        }
        else
        {
            AddItemToCharacter(inventory2.IsFull, item);
        }
    }

    public void AddItemToCharacter(bool isCharacter1, Item item)
    {
        if (isCharacter1)
        {
            inventory1.AddItem(item);
            page.UpdateData(inventory1);
        }
        else
        {
            inventory2.AddItem(item);
            page.UpdateData(inventory2);
        }
    }

    public void RemoveItem(Item item)
    {
        inventory1.GetItems().Remove(item);
        inventory2.GetItems().Remove(item);
        page.UpdateData(inventory1);
        page.UpdateData(inventory2);
    }

    public void UpdateItemPanelsForSwitch(bool isCharacter1, bool grouped)
    {
        if (!grouped)
        {
            page.EnableContentPanel(isCharacter1);
            page.DisableContentPanel(!isCharacter1);
            page.SelectFirstAvailable();
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
        Item item = null;
        if (itemIndex < inventory.GetItems().Count)
        {
            item = inventory.GetItems()[itemIndex];
        }

        selectedItem = item;
        page.UpdateSelected(inventoryIndex, itemIndex, item);
    }

    private void HandleSwapItems(int draggedItemInventoryIndex, int draggedItemIndex, int droppedOnInventoryIndex, int droppedOnItemIndex)
    {
        MoveItems(draggedItemInventoryIndex, draggedItemIndex, droppedOnInventoryIndex, droppedOnItemIndex);
        
        Inventory droppedOnInventory = droppedOnInventoryIndex == 1 ? inventory1 : inventory2;
        if (droppedOnItemIndex >= droppedOnInventory.GetItems().Count)
        {
            droppedOnItemIndex = droppedOnInventory.GetItems().Count - 1;
        }

        HandleDescriptionRequest(droppedOnInventoryIndex, droppedOnItemIndex);
    }

    private void HandleSwitchInventory(int draggedItemInventoryIndex, int draggedItemIndex, int droppedOnInventoryIndex, int droppedOnItemIndex)
    {
        MoveItems(draggedItemInventoryIndex, draggedItemIndex, droppedOnInventoryIndex, droppedOnItemIndex);
        HandleDescriptionRequest(draggedItemInventoryIndex, draggedItemIndex);
    }

    private void MoveItems(int draggedItemInventoryIndex, int draggedItemIndex, int droppedOnInventoryIndex, int droppedOnItemIndex)
    {
        if (draggedItemInventoryIndex == droppedOnInventoryIndex)
        {
            Inventory inventory = draggedItemInventoryIndex == 1 ? ref inventory1 : ref inventory2;
            MoveItems(inventory, inventory, draggedItemIndex, droppedOnItemIndex);

            page.UpdateData(inventory);
        }
        else
        {
            Inventory draggedInventory = draggedItemInventoryIndex == 1 ? ref inventory1 : ref inventory2;
            Inventory droppedOnInventory = draggedItemInventoryIndex == 1 ? ref inventory2 : ref inventory1;
            MoveItems(draggedInventory, droppedOnInventory, draggedItemIndex, droppedOnItemIndex);

            page.UpdateData(draggedInventory);
            page.UpdateData(droppedOnInventory);
        }
    }

    private void MoveItems(Inventory draggedInventory, Inventory droppedOnInventory, int draggedItemIndex, int droppedOnItemIndex)
    {
        Item draggedItem = draggedInventory.GetItems()[draggedItemIndex];

        if (droppedOnItemIndex < droppedOnInventory.GetItems().Count)
        {
            Item droppedOnItem = droppedOnInventory.GetItems()[droppedOnItemIndex];

            droppedOnInventory.GetItems()[droppedOnItemIndex] = draggedItem;
            draggedInventory.GetItems()[draggedItemIndex] = droppedOnItem;
        }
        else
        {
            draggedInventory.GetItems().RemoveAt(draggedItemIndex);
            droppedOnInventory.AddItem(draggedItem);
        }
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

    public Inventory InventoryOne { get => inventory1; }
    public Inventory InventoryTwo { get => inventory2; }
    public Item SelectedItem { get => selectedItem; }
    public DialogueChannel Channel { get => channel; }
    public void SetChannel(DialogueChannel dialogueChannel) { 
        channel = dialogueChannel; 
    }
}
