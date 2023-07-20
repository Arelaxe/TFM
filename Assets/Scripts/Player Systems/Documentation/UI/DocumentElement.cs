using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DocumentElement : Selectable, IPointerClickHandler, ISubmitHandler
{
    [SerializeField]
    private TMP_Text title;

    public event Action<DocumentElement> OnElementSelect, OnElementSubmit;

    public void SetData(string title)
    {
        this.title.text = title;
    }

    public TMP_Text GetTitle()
    {
        return title;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnElementSubmit?.Invoke(this);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnElementSelect?.Invoke(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnElementSubmit?.Invoke(this);
    }
    
}
