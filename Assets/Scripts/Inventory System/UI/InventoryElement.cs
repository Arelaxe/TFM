using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryElement : Selectable, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, ISubmitHandler
{
    [SerializeField]
    private Image icon;

    public event Action<InventoryElement> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemSelected, OnItemSubmit;
    private bool empty = true;

    public void SetData(Sprite sprite)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = sprite;
        empty = false;
    }

    public void ResetData()
    {
        icon.gameObject.SetActive(false);
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
