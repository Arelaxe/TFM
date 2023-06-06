using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DocumentItem : Selectable, IPointerClickHandler, ISubmitHandler
{
    [SerializeField]
    private TMP_Text documentName;

    public event Action<DocumentItem> OnDocumentSelect, OnDocumentSubmit;

    public void SetData(string name)
    {
        documentName.text = name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnDocumentSubmit?.Invoke(this);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnDocumentSelect?.Invoke(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnDocumentSubmit?.Invoke(this);
    }
    
}
