using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class ListenToInput : NarrationAction
{
    [SerializeField]
    private string action;
    [SerializeField] UnityEvent dialogueRequestEvent;
    private bool listening;
    private PlayerInput input;

    public override void Execute(){ }

    public override void EndAction()
    {
        input = PlayerManager.Instance.GetDualCharacterController().GetComponent<PlayerInput>();
        listening = true;
        PlayerManager.Instance.StartCoroutine(Listen());
    }

    IEnumerator Listen()
    {
        while( listening )
        {
            if (input.actions[action].triggered){
                listening = false;
                PlayerManager.Instance.StopCoroutine("Listen");
                dialogueRequestEvent.Invoke();
            }

            yield return new WaitForSeconds( 0.001f );
        }
    }
}
