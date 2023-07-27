using UnityEngine;
using UnityEngine.Events;

public class TalkAction : Action
{
    [SerializeField] UnityEvent dialogueRequestEvent;

    public override void Execute()
    {
        dialogueRequestEvent.Invoke();

        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        interactionController.SetInteractivity(false);
        interactionController.DestroyInteractions();
    }
}