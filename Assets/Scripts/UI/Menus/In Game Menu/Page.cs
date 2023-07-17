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

    [SerializeField] 
    protected GameObject m_OtherPage;

    [SerializeField]
    private GameObject buttons;

    protected bool dialogueMode = false;

    public event Action<Page> OnShow, OnHide;

    public virtual void Show()
    {
        OnShow?.Invoke(this);
        gameObject.SetActive(true);
        buttons.SetActive(false);
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
        buttons.SetActive(true);
        dialogueMode = true;
        PlayerManager.Instance.GetInventoryController().SetChannel(channel);
    }

    public bool DialogueMode { get { return dialogueMode; } set { dialogueMode = value; } }
    public GameObject DialoguePanel { get => m_DialogPanel; }

    public GameObject OtherPage { get => m_OtherPage; }
}
