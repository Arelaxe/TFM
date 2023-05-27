using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InventoryPage : MonoBehaviour, IEventSystemHandler
{
    [SerializeField]
    private InventoryItem inventoryItem;

    [SerializeField]
    private InventoryDescription inventoryDescription;

    [SerializeField]
    private RectTransform contentPanel1;

    [SerializeField]
    private RectTransform contentPanel2;

    [SerializeField]
    private MouseFollower mouseFollower;

    private List<InventoryItem> items1 = new();
    private List<InventoryItem> items2 = new();

    private int currentDraggedItemIndex = -1;
    private int currentDraggedInventoryIndex = -1;

    public event Action<int, int> OnDescriptionRequested, OnStartDragging;
    public event Action<int, int, int, int> OnSwapItems, OnSwitchInventory;

    private void Awake()
    {
        mouseFollower.Toggle(false);
        inventoryDescription.ResetDescription();
    }

    public void InitInventoriesUI(int inventorySize1, int inventorySize2)
    {
        InitInventoryUI(inventorySize1, contentPanel1, items1);
        InitInventoryUI(inventorySize2, contentPanel2, items2);
    }

    private void InitInventoryUI(int inventorySize, RectTransform contentPanel, List<InventoryItem> itemList)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem item = Instantiate(inventoryItem, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(contentPanel);
            item.transform.localScale = new Vector3(1, 1, 1);
            itemList.Add(item);

            item.OnItemSelected += HandleItemSelection;
            item.OnItemSubmit += HandleSwitch;

            item.OnItemClicked += HandleItemSelection;
            item.OnItemBeginDrag += HandleBeginDrag;            
            item.OnItemEndDrag += HandleEndDrag;
            item.OnItemDroppedOn += HandleSwap;
        }
    }

    public void LoadItems(Inventory inventory)
    {
        List<InventoryItem> items = inventory.Index == 1 ? ref items1 : ref items2;

        for (int i = 0; i < inventory.GetItems().Count; i++)
        {
            items[i].SetData(inventory.GetItems()[i].ItemImage);
        }
    }

    public void UpdateItems(Inventory inventory)
    {
        List<InventoryItem> items = inventory.Index == 1 ? ref items1 : ref items2;
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

    public void UpdateSelected(int inventoryIndex, int itemIndex, Item item)
    {
        if (item)
        {
            inventoryDescription.SetDescription(item.ItemImage, item.Name, item.Description);
            if (inventoryIndex == 1)
            {
                items1[itemIndex].Select();
            }
            else
            {
                items2[itemIndex].Select();
            }
        }
        else
        {
            inventoryDescription.ResetDescription();
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
    }

    public void Show()
    {
        gameObject.SetActive(true);
        inventoryDescription.ResetDescription();
        SelectFirstItemAvailable();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectFirstItemAvailable()
    {
        InventoryItem item = null;
        if (PlayerManager.Instance.selectedCharacterOne)
        {
            if (!items1[0].Empty)
            {
                item = items1[0];
            }
            else if (PlayerManager.Instance.GetDualCharacterController().Grouped && !items2[0].Empty)
            {
                item = items2[0];
            }
        }
        else
        {
            if (!items2[0].Empty)
            {
                item = items2[0];
            }
            else if (PlayerManager.Instance.GetDualCharacterController().Grouped && !items1[0].Empty)
            {
                item = items1[0];
            }
        }

        if (item)
        {
            item.Select();
        }
    }

    public void EnableContentPanel(bool forCharacter1)
    {
        CanvasGroup canvasGroup = GetCharacterContentPanel(forCharacter1).GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        SetItemsInteractableStatus(forCharacter1, true);
    }

    public void DisableContentPanel(bool forCharacter1)
    {
        CanvasGroup canvasGroup = GetCharacterContentPanel(forCharacter1).GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        SetItemsInteractableStatus(forCharacter1, false);
    }

    protected void SetItemsInteractableStatus(bool forCharacter1, bool interactable)
    {
        foreach (var item in GetCharacterItems(forCharacter1))
        {
            item.interactable = interactable;
        }
    }

    // Event handlers

    private void HandleItemSelection(InventoryItem item)
    {
        if (items1.Contains(item))
        {
            OnDescriptionRequested?.Invoke(1, items1.IndexOf(item));
        }
        else
        {
            OnDescriptionRequested?.Invoke(2, items2.IndexOf(item));
        }
    }

    private void HandleBeginDrag(InventoryItem item)
    {
        HandleItemSelection(item);
        if (items1.Contains(item))
        {
            currentDraggedItemIndex = items1.IndexOf(item);
            currentDraggedInventoryIndex = 1;
            OnStartDragging?.Invoke(1, items1.IndexOf(item));
        }
        else
        {
            currentDraggedItemIndex = items2.IndexOf(item);
            currentDraggedInventoryIndex = 2;
            OnStartDragging?.Invoke(2, items2.IndexOf(item));
        }
    }

    private void HandleEndDrag(InventoryItem item)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(InventoryItem item)
    {
        int droppedOnInventoryIndex = items1.Contains(item) ? 1 : 2;
        int droppedOnItemIndex = droppedOnInventoryIndex == 1 ? items1.IndexOf(item) : items2.IndexOf(item);

        if (currentDraggedInventoryIndex == droppedOnInventoryIndex 
            || currentDraggedInventoryIndex != droppedOnInventoryIndex && PlayerManager.Instance.GetDualCharacterController().Grouped)
        {
            OnSwapItems?.Invoke(currentDraggedInventoryIndex, currentDraggedItemIndex, droppedOnInventoryIndex, droppedOnItemIndex);
        }
    }

    private void HandleSwitch(InventoryItem item)
    {
        int currentSubmitInventoryIndex = items1.Contains(item) ? 1 : 2;
        int switchInventoryIndex = items1.Contains(item) ? 2 : 1;

        int currentSubmitItemIndex = switchInventoryIndex == 1 ? items2.IndexOf(item) : items1.IndexOf(item);
        int switchItemIndex = switchInventoryIndex == 1 ? items1.Count - 1 : items2.Count - 1;

        if (PlayerManager.Instance.GetDualCharacterController().Grouped)
        {
            OnSwitchInventory?.Invoke(currentSubmitInventoryIndex, currentSubmitItemIndex, switchInventoryIndex, switchItemIndex);
        }
    }
    
    // Auxiliar methods

    private RectTransform GetCharacterContentPanel(bool characterOne)
    {
        return characterOne ? contentPanel1 : contentPanel2;
    }

    private List<InventoryItem> GetCharacterItems(bool characterOne)
    {
        return characterOne ? items1 : items2;
    }
}
