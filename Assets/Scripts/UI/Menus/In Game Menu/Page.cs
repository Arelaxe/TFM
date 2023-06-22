using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class Page : MonoBehaviour, IEventSystemHandler
{
    [SerializeField]
    protected RectTransform contentPanel;

    [SerializeField] 
    protected GameObject m_DialogPanel;

    protected bool dialogueMode = false;

    public event Action<Page> OnShow, OnHide;

    public virtual void Show()
    {
        OnShow?.Invoke(this);
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        if (!dialogueMode){
            OnHide?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void ShowDialogueMode(DialogueChannel channel){
        m_DialogPanel.SetActive(false);
        Show();
        dialogueMode = true;
        PlayerManager.Instance.GetInventoryController().SetChannel(channel);
    }
}
