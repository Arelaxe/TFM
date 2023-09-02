using UnityEngine;

public class OpenNumpad : Action
{
    [SerializeField]
    private GameObject numpad;

    [Header("Initialization")]
    [SerializeField]
    private string code;
    [SerializeField]
    private string numpadInteraction;
    [SerializeField]
    private string lockedInteraction;
    [SerializeField]
    private Interactable numpadInteractable;
    [SerializeField]
    private Interactable lockedInteractable;

    public override void Execute()
    {
        GameObject newNumpad = Instantiate(numpad);
        newNumpad.GetComponent<Numpad>().Init(code, numpadInteraction, numpadInteractable, lockedInteraction, lockedInteractable);

        PlayerManager.Instance.GetInGameMenuController().AddAdditionalUI(newNumpad);
    }

}
