using System.Collections;
using UnityEngine;

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
        PlayerManager.Instance.GetInteractionController().SetInteractivity(false);
        PlayerManager.Instance.GetInteractionController().DestroyInteractions();
        PlayerManager.Instance.GetDualCharacterController().GetCharacter(true).transform.position = target.position;
        yield return null;
        PlayerManager.Instance.GetInteractionController().SetInteractivity(true);
    }
}
