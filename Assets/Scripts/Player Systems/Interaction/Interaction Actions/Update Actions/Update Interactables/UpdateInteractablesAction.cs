using UnityEngine;

public class UpdateInteractablesAction : Action
{
    [SerializeField]
    private InteractableUpdate[] interactablesUpdates;

    public override void Execute()
    {
        foreach (InteractableUpdate interactableUpdate in interactablesUpdates)
        {
            foreach (int index in interactableUpdate.availableInteractions)
            {
                interactableUpdate.interactable.Interactions[index].SetAvailable(true);
            }

            foreach (int index in interactableUpdate.notAvailableInteractions)
            {
                interactableUpdate.interactable.Interactions[index].SetAvailable(false);
            }

            foreach (int index in interactableUpdate.blockedInteractions)
            {
                interactableUpdate.interactable.Interactions[index].SetBlocked(true);
            }

            foreach (int index in interactableUpdate.notBlockedInteractions)
            {
                interactableUpdate.interactable.Interactions[index].SetBlocked(false);
            }
        }
    }
}
