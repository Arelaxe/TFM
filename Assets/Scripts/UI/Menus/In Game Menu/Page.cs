using UnityEngine.EventSystems;
using UnityEngine;
using System;

public abstract class Page : MonoBehaviour, IEventSystemHandler
{
    [SerializeField]
    protected RectTransform contentPanel;

    public event Action<Page> OnShow, OnHide;

    public virtual void Show()
    {
        OnShow?.Invoke(this);
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        OnHide?.Invoke(this);
        gameObject.SetActive(false);
    }
}
