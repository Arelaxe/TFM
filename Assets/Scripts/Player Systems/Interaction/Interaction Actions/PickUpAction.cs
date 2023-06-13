using UnityEngine;

public class PickUpAction : Action
{
    [SerializeField]
    private Item item;

    public override void Execute()
    {
        if (item.Type.Equals(Item.ItemType.Basic))
        {
            InventoryController inventoryController = PlayerManager.Instance.GetInventoryController();
            inventoryController.AddItem(PlayerManager.Instance.SelectedCharacterOne, item);
        }
        else if (item.Type.Equals(Item.ItemType.Document))
        {
            DocumentationController documentationController = PlayerManager.Instance.GetDocumentationController();
            documentationController.Add(item);
        }
        
        InteractionController interactor = PlayerManager.Instance.GetInteractionController();
        interactor.DestroyInteractions();

        Destroy(gameObject);
    }
}
