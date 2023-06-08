using UnityEngine.InputSystem;
using UnityEngine;

public abstract class PageController : MonoBehaviour
{
    private PlayerInput input;
    private InputAction menuAction;

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
        if (menuAction.triggered)
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
    }

    protected void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        menuAction = input.actions[MenuAction];
    }

    protected void Show()
    {
        DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

        playerController.SetMobility(false);
        interactionController.SetInteractivity(false);
        interactionController.DestroyInteractions();

        LoadData();
        Page.Show();
    }

    protected void Hide()
    {
        DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

        playerController.SetMobility(true);
        interactionController.SetInteractivity(true);

        HidePage();
    }

    public void HidePage()
    {
        Page.Hide();
    }

    protected abstract void InitPage();
    protected abstract void LoadData();
    
}
