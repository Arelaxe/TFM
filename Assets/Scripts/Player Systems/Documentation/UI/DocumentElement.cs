using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DocumentElement : Selectable, IPointerClickHandler, ISubmitHandler
{
    [SerializeField]
    private TMP_Text title;

    public event Action<DocumentElement> OnElementSelect, OnElementSubmit, OnElementDoubleSubmit;

    private bool submit = false;

    public void SetData(string title)
    {
        this.title.text = title;
    }

    public TMP_Text GetTitle()
    {
        return title;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        submit = false;
        OnElementSelect?.Invoke(this);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        submit = false;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!submit)
        {
            submit = true;
            OnElementSubmit?.Invoke(this);
        }
        else
        {
            submit = false;
            OnElementDoubleSubmit?.Invoke(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.clickCount == 1)
            {
                OnElementSubmit?.Invoke(this);
            }
            else if (eventData.clickCount > 1)
            {
                OnElementDoubleSubmit?.Invoke(this);
            }
        }
    }

}
