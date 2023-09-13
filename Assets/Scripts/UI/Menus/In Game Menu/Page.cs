using UnityEngine.EventSystems;
using UnityEngine;
using System;

public abstract class Page : MonoBehaviour, IEventSystemHandler
{
    [Header("General")]
    [SerializeField]
    protected RectTransform contentPanel;

    [SerializeField] 
    protected GameObject m_DialogPanel;

    [SerializeField]
    private GameObject buttons;

    [Header("Audio")]
    [SerializeField]
    private AudioClip openSound;
    [SerializeField]
    private AudioClip closeSound;

    protected bool dialogueMode = false;

    public event Action<Page> OnShow, OnHide;

    public virtual void Show()
    {
        SoundManager.Instance.PlayEffectOneShot(openSound);
        OnShow?.Invoke(this);
        gameObject.SetActive(true);
        if (buttons){
            buttons.SetActive(false);
        }
    }

    public virtual void Hide()
    {
        if (!dialogueMode){
            SoundManager.Instance.PlayEffect(closeSound);
            OnHide?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void ShowDialogueMode(DialogueChannel channel){
        PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(false);
        m_DialogPanel.SetActive(false);
        Show();
        buttons.SetActive(true);
        dialogueMode = true;
        PlayerManager.Instance.GetInventoryController().SetChannel(channel);
    }

    public bool DialogueMode { get { return dialogueMode; } set { dialogueMode = value; } }
    public GameObject DialoguePanel { get => m_DialogPanel; }
}
