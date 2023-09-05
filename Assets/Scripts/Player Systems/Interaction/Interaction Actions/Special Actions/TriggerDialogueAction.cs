using UnityEngine;
using UnityEngine.Events;

public class TriggerDialogueAction : Action
{
    [SerializeField] UnityEvent dialogueRequestEvent;

    public override void Execute()
    {
        dialogueRequestEvent.Invoke();
    }
}
