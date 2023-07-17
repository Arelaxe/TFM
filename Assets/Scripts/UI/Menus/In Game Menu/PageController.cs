using UnityEngine.InputSystem;
using UnityEngine;

public abstract class PageController : MonoBehaviour
{
    private PlayerInput input;
    private InputAction menuAction;
    private InputAction backAction;

    public abstract Page Page
    {
        get;
    }

    public abstract string MenuAction
    {
        get;
    }

    protected virtual void Start()
    {
        InitInputActions();
        InitPage();
    }

    protected virtual void Update()
    {
        if (!SceneLoadManager.Instance.Paused && !SceneLoadManager.Instance.Loading)
        {
<<<<<<< HEAD
            if (menuAction.triggered && !Page.DialoguePanel.activeSelf && !Page.OtherPage.activeSelf)
=======
            if (menuAction.triggered && PlayerManager.Instance.GetInGameMenuController().SwitchPageAvailable)
>>>>>>> develop
            {
                if (!Page.isActiveAndEnabled)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
            if (backAction.triggered && Page.isActiveAndEnabled)
            {
                Hide();
            }
        }
    }

    protected void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        menuAction = input.actions[MenuAction];
        backAction = input.actions[PlayerConstants.ActionCancel];
    }

    public void Show()
    {
        DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

        playerController.SetCharacterMobility(true, false);
        interactionController.SetInteractivity(false);
        interactionController.DestroyInteractions();

        LoadData();
        Page.Show();
    }

    protected void Hide()
    {
        DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

        playerController.SetCharacterMobility(true, true);
        interactionController.SetInteractivity(true);

        HidePage();
    }

    public void HidePage()
    {
        Page.Hide();
    }

    protected abstract void InitPage();
    public abstract void LoadData();
}