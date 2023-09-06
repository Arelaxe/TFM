using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private List<Page> pages;

    [SerializeField]
    private GameObject hackingExtension;

    private GameObject additionalUI;

    private PlayerInput input;
    private InputAction backAction;
    private bool switchPageAvailable = true;

    private int openPage = -1;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        backAction = input.actions[PlayerConstants.ActionCancel];
        InitPageEvents();
    }

    private void Update()
    {
        InputSystemUtils.ControlCursor(input);
        if (!SceneLoadManager.Instance.Loading)
        {
            if (backAction.triggered)
            {
                if (additionalUI)
                {
                    DestroyAdditionalUI();
                }
                if (SceneLoadManager.Instance.InAdditive)
                {
                    SceneLoadManager.Instance.ReturnFromAdditiveScene();
                }
            }
        }
    }

    private void InitPageEvents()
    {
        foreach (var page in pages)
        {
            page.OnShow += HandleShow;
            page.OnHide += HandleHide;
        }
    }

    // Event handlers

    private void HandleShow(Page page)
    {
        if (openPage != -1)
        {
            pages[openPage].Hide();
        }
        openPage = pages.IndexOf(page);
    }

    private void HandleHide(Page page)
    {
        if (openPage == pages.IndexOf(page))
        {
            openPage = -1;
        }
    }

    public void Hide()
    {
        pages[openPage].Hide();
        openPage = -1;
        HideHackingExtension();
    }

    public bool SwitchPageAvailable { get => switchPageAvailable; }
    public void SetSwitchPageAvailability(bool switchPageAvailable)
    {
        this.switchPageAvailable = switchPageAvailable;
    }

    public void ShowHackingExtension(string hackingScene, GameObject interactable)
    {
        hackingExtension.GetComponent<HackingExtension>().ResetData(hackingScene, interactable);
        hackingExtension.SetActive(true);
    }

    public void HideHackingExtension()
    {
        hackingExtension.SetActive(false);
    }

    public HackingExtension GetHackingExtension()
    {
        HackingExtension extension = null;
        if (hackingExtension.activeSelf)
        {
            extension = hackingExtension.GetComponent<HackingExtension>();
        }
        return extension;
    }

    public void AddAdditionalUI(GameObject panel)
    {
        panel.GetComponent<RectTransform>().SetParent(rectTransform, false);
        panel.GetComponent<RectTransform>().position = rectTransform.position;
        additionalUI = panel;
        ControlPlayerActions(false);
    }

    public void DestroyAdditionalUI()
    {
        Destroy(additionalUI);
        additionalUI = null;
        ControlPlayerActions(true);
    }

    private void ControlPlayerActions(bool close)
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetCharacterMobility(true, close);
        dualCharacterController.SetSwitchAvailability(close);

        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        interactionController.SetInteractivity(close);
        interactionController.DestroyInteractions();

        PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(close);
    }
}
