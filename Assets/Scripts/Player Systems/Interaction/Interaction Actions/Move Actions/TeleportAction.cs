using System.Collections;
using UnityEngine;
using Cinemachine;

public class TeleportAction : Action
{
    [SerializeField]
    private Transform target;

    public override void Execute()
    {
        if (PlayerManager.Instance.Grouped)
        {
            PlayerManager.Instance.GetDualCharacterController().SwitchGrouping();
        }

        StartCoroutine(Teleport());
    }

    private IEnumerator Teleport()
    {
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        interactionController.SetInteractivity(false);
        interactionController.DestroyInteractions();

        PlayerManager.Instance.GetDualCharacterController().GetCharacter(true).transform.position = target.position;

        yield return null;
        interactionController.SetInteractivity(true);
    }
}
