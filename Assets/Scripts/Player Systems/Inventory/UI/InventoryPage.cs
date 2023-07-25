using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InventoryPage : Page
{
    [SerializeField]
    private InventoryElement element;

    [SerializeField]
    private InventoryDescription description;

    [SerializeField]
    private RectTransform contentPanel2;

    [SerializeField]
    private MouseFollower mouseFollower;
/*
    [SerializeField] 
    private GameObject m_DialogPanel;*/

    private List<InventoryElement> elementList = new();
    private List<InventoryElement> elementList2 = new();

    private int currentDraggedItemIndex = -1;
    private int currentDraggedInventoryIndex = -1;

    public event Action<int, int> OnDescriptionRequested, OnStartDragging;
    public event Action<int, int, int, int> OnSwapItems, OnSwitchInventory;

    private void Awake()
    {
        mouseFollower.Toggle(false);
        description.ResetDescription();
    }

    public void InitUI(int inventorySize, int inventorySize2)
    {
        InitUIElementList(inventorySize, contentPanel, elementList);
        InitUIElementList(inventorySize2, contentPanel2, elementList2);
    }

    private void InitUIElementList(int inventorySize, RectTransform contentPanel, List<InventoryElement> elementList)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InitUIElement(contentPanel, elementList);
        }
    }

    private void InitUIElement(RectTransform contentPanel, List<InventoryElement> elementList)
    {
        InventoryElement item = Instantiate(element, Vector3.zero, Quaternion.identity, contentPanel);
        item.transform.localPosition = new Vector3(item.transform.position.x, item.transform.position.y, 0);
        item.transform.localScale = new Vector3(1, 1, 1);
        elementList.Add(item);

        item.OnItemSelected += HandleSelect;
        item.OnItemDoubleSubmit += HandleDoubleSubmit;

        item.OnItemClicked += HandleSelect;
        item.OnItemBeginDrag += HandleBeginDrag;
        item.OnItemEndDrag += HandleEndDrag;
        item.OnItemDroppedOn += HandleSwap;
    }

    public void LoadData(Inventory inventory)
    {
        List<InventoryElement> items = inventory.Index == 1 ? ref elementList : ref elementList2;

        for (int i = 0; i < inventory.GetItems().Count; i++)
        {
            items[i].SetData(inventory.GetItems()[i].ItemImage);
        }
    }

    public void UpdateData(Inventory inventory)
    {
        List<InventoryElement> items = inventory.Index == 1 ? ref elementList : ref elementList2;
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
            description.SetDescription(item.ItemImage, item.Name, item.Description);
            if (inventoryIndex == 1)
            {
                elementList[itemIndex].Select();
            }
            else
            {
                elementList2[itemIndex].Select();
            }
        }
        else
        {
            description.ResetDescription();
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

    public override void Show()
    {
        base.Show();
        description.ResetDescription();
        SelectFirstAvailable();
    }

    public override void Hide()
    {
        base.Hide();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectFirstAvailable()
    {
        InventoryElement item = null;
        if (PlayerManager.Instance.SelectedCharacterOne)
        {
            if (!elementList[0].Empty)
            {
                item = elementList[0];
            }
            else if (PlayerManager.Instance.Grouped && !elementList2[0].Empty)
            {
                item = elementList2[0];
            }
        }
        else
        {
            if (!elementList2[0].Empty)
            {
                item = elementList2[0];
            }
            else if (PlayerManager.Instance.Grouped && !elementList[0].Empty)
            {
                item = elementList[0];
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

    private void HandleSelect(InventoryElement item)
    {
        if (elementList.Contains(item))
        { 
            OnDescriptionRequested?.Invoke(1, elementList.IndexOf(item));
        }
        else
        {
            OnDescriptionRequested?.Invoke(2, elementList2.IndexOf(item));
        }
    }

    private void ItemShowDialog(){
        m_DialogPanel.SetActive(false);
        dialogueMode = false;
        InventoryController invController = PlayerManager.Instance.GetInventoryController();
        ChoiceDialogueNode choiceNode = (ChoiceDialogueNode) invController.Channel.currentNode;
        DialogueInventoryChoice[] invChoices = choiceNode.InventoryChoices;
        bool foundItem = false;

        for (int i = 0; i < invChoices.Length && !foundItem; i++){
            if (invChoices[i].Item.ID == invController.SelectedItem.ID){
                invController.Channel.RaiseRequestDialogueNode(invChoices[i].ChoiceNode);
                foundItem = true;
            }
        }

        if (!foundItem){
            invController.Channel.RaiseRequestDialogueNode(choiceNode.DefaultInventoryChoice);
        }

        base.Hide();
    }

    private void HandleBeginDrag(InventoryElement item)
    {
        HandleSelect(item);
        if (elementList.Contains(item))
        {
            currentDraggedItemIndex = elementList.IndexOf(item);
            currentDraggedInventoryIndex = 1;
            OnStartDragging?.Invoke(1, elementList.IndexOf(item));
        }
        else
        {
            currentDraggedItemIndex = elementList2.IndexOf(item);
            currentDraggedInventoryIndex = 2;
            OnStartDragging?.Invoke(2, elementList2.IndexOf(item));
        }
    }

    private void HandleEndDrag(InventoryElement item)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(InventoryElement item)
    {
        int droppedOnInventoryIndex = elementList.Contains(item) ? 1 : 2;
        int droppedOnItemIndex = droppedOnInventoryIndex == 1 ? elementList.IndexOf(item) : elementList2.IndexOf(item);

        if (currentDraggedInventoryIndex == droppedOnInventoryIndex 
            || currentDraggedInventoryIndex != droppedOnInventoryIndex && PlayerManager.Instance.Grouped)
        {
            OnSwapItems?.Invoke(currentDraggedInventoryIndex, currentDraggedItemIndex, droppedOnInventoryIndex, droppedOnItemIndex);
        }
    }

    private void HandleDoubleSubmit(InventoryElement item)
    {
        HackingExtension extension = PlayerManager.Instance.GetInGameMenuController().GetHackingExtension();
        if (extension)
        {
            extension.SetItem(PlayerManager.Instance.GetInventoryController().SelectedItem);
            PlayerManager.Instance.GetDocumentationController().Show();
        }
        else
        {
            int currentSubmitInventoryIndex = elementList.Contains(item) ? 1 : 2;
            int switchInventoryIndex = elementList.Contains(item) ? 2 : 1;

            int currentSubmitItemIndex = switchInventoryIndex == 1 ? elementList2.IndexOf(item) : elementList.IndexOf(item);
            int switchItemIndex = switchInventoryIndex == 1 ? elementList.Count - 1 : elementList2.Count - 1;

            if (PlayerManager.Instance.Grouped)
            {
                OnSwitchInventory?.Invoke(currentSubmitInventoryIndex, currentSubmitItemIndex, switchInventoryIndex, switchItemIndex);
            }
        }
    }
    
    // Buttons

    public void OnExit(){
        DialoguePanel.SetActive(true);
        DialogueMode = false;
        PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(true);
        Hide();
    }

    public void OnShowObject(){
        DialoguePanel.SetActive(false);
        dialogueMode = false;
        InventoryController invController = PlayerManager.Instance.GetInventoryController();
        ChoiceDialogueNode choiceNode = (ChoiceDialogueNode) invController.Channel.currentNode;
        DialogueInventoryChoice[] invChoices = choiceNode.InventoryChoices;
        bool foundItem = false;

        for (int i = 0; i < invChoices.Length && !foundItem; i++){
            if (invChoices[i].Item.ID == invController.SelectedItem.ID){
                invController.Channel.RaiseRequestDialogueNode(invChoices[i].ChoiceNode);
                foundItem = true;
            }
        }

        if (!foundItem){
            invController.Channel.RaiseRequestDialogueNode(choiceNode.DefaultInventoryChoice);
        }

        PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(true);
        base.Hide();
    }

    // Auxiliar methods

    private RectTransform GetCharacterContentPanel(bool characterOne)
    {
        return characterOne ? contentPanel : contentPanel2;
    }

    private List<InventoryElement> GetCharacterItems(bool characterOne)
    {
        return characterOne ? elementList : elementList2;
    }
}
