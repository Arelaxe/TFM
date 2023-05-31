using UnityEngine;

public class PickUpAction : Action
{
    [SerializeField]
    private Item item;

    public override void Execute()
    {
        InventoryController inventoryController = PlayerManager.Instance.GetInventoryController();
        inventoryController.AddItem(PlayerManager.Instance.SelectedCharacterOne, item);
        
        InteractionController interactor = PlayerManager.Instance.GetInteractionController();
        interactor.DestroyInteractions();

        Destroy(gameObject);
    }
}
