using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : Selectable, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, ISubmitHandler
{
    [SerializeField]
    private Image itemImage;

    public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemSelected, OnItemSubmit;
    private bool empty = true;

    public void SetData(Sprite sprite)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        empty = false;
    }

    public void ResetData()
    {
        itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnItemSelected?.Invoke(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!empty)
        {
            OnItemSubmit?.Invoke(this);
        }
    }

    // Mouse events

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!empty)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!empty)
        {
            OnItemBeginDrag?.Invoke(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Mandatory for OnBeginDrag
    }

    public bool Empty { get => empty; }
}
