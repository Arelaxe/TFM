using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TalkAction : Action
{
    [SerializeField] UnityEvent dialogueRequestEvent;

    public override void Execute()
    {
        dialogueRequestEvent.Invoke();
    }
}