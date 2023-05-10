using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAction : Action
{
    [SerializeField]
    private Item item;

    public override void Execute(PlayerController playerController)
    {
        InventoryController inventoryController = playerController.GetComponent<InventoryController>();
        inventoryController.AddItemToCharacter(playerController.IsCharacter1(false), item);
        PlayerInteractor interactor = playerController.GetComponent<PlayerInteractor>();
        interactor.DestroyInteractions();
        Destroy(gameObject);
    }
}
