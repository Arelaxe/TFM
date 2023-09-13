using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryElement : Selectable, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, ISubmitHandler
{
    [SerializeField]
    private Image icon;

    public event Action<InventoryElement> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemSelected, OnItemDoubleSubmit;
    private bool empty = true;
    private bool submit = false;

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

    public Image GetIcon()
    {
        return icon;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        submit = false;
        SoundManager.Instance.PlaySelectedButton();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        submit = false;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!empty)
        {
            SoundManager.Instance.PlayClickedButton();
            if (!submit)
            {
                submit = true;
                OnItemSelected?.Invoke(this);
            }
            else
            {
                submit = false;
                OnItemDoubleSubmit?.Invoke(this);
            }  
        }
    }

    // Mouse events

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!empty)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                SoundManager.Instance.PlayClickedButton();
                if (eventData.clickCount == 1)
                {
                    OnItemClicked?.Invoke(this);
                }
                else if (eventData.clickCount > 1)
                {
                    OnItemDoubleSubmit?.Invoke(this);
                }
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
