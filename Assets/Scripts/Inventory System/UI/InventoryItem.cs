using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private Image borderImage;

    public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag;
    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

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

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

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
}
