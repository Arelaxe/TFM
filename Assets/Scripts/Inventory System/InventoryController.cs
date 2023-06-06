using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private PlayerInput input;
    private InputAction inventoryAction;

    [SerializeField]
    private InventoryPage page;

    [SerializeField]
    private Inventory inventory1;
    
    [SerializeField]
    private Inventory inventory2;

    public void Start()
    {
        InitInputActions();
        InitPage();
    }

    void Update()
    {
        if (inventoryAction.triggered)
        {
            DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
            InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

            if (!page.isActiveAndEnabled)
            {
                DocumentationController documentationController = PlayerManager.Instance.GetDocumentationController();
                if (documentationController.IsActive())
                {
                    documentationController.Hide();
                }

                playerController.SetMobility(false);
                interactionController.SetInteractivity(false);
                interactionController.DestroyInteractions();

                page.LoadItems(inventory1);
                page.LoadItems(inventory2);
                page.Show();
            }
            else
            {
                playerController.SetMobility(true);
                interactionController.SetInteractivity(true);
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
        page.InitInventoriesUI(inventory1.Size, inventory2.Size);
        page.OnDescriptionRequested += HandleDescriptionRequest;
        page.OnSwitchInventory += HandleSwitchInventory;
        page.OnSwapItems += HandleSwapItems;
        page.OnStartDragging += HandleDragging;
    }

    public bool IsActive()
    {
        return page.isActiveAndEnabled;
    }

    public void Hide()
    {
        page.Hide();
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

        bool isChar1Full = inventory1.isFull;
        bool isChar2Full = inventory2.isFull;

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
            page.UpdateItems(inventory1);
        }
        else
        {
            inventory2.AddItem(item);
            page.UpdateItems(inventory2);
        }
    }

    public void RemoveItem(Item item)
    {
        inventory1.GetItems().Remove(item);
        inventory2.GetItems().Remove(item);
        page.UpdateItems(inventory1);
        page.UpdateItems(inventory2);
    }

    public void UpdateItemPanelsForSwitch(bool isCharacter1, bool grouped)
    {
        if (!grouped)
        {
            page.EnableContentPanel(isCharacter1);
            page.DisableContentPanel(!isCharacter1);
            page.SelectFirstItemAvailable();
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

            page.UpdateItems(inventory);
        }
        else
        {
            Inventory draggedInventory = draggedItemInventoryIndex == 1 ? ref inventory1 : ref inventory2;
            Inventory droppedOnInventory = draggedItemInventoryIndex == 1 ? ref inventory2 : ref inventory1;
            MoveItems(draggedInventory, droppedOnInventory, draggedItemIndex, droppedOnItemIndex);

            page.UpdateItems(draggedInventory);
            page.UpdateItems(droppedOnInventory);
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
}
