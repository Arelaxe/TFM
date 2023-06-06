using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject player;
    public bool SelectedCharacterOne { get => GetDualCharacterController().SelectedCharacterOne; }
    public bool Grouped { get => GetDualCharacterController().Grouped; }

    protected override void LoadData()
    {
        player = GameObject.Find("Player");
    }

    public DualCharacterController GetDualCharacterController()
    {
        return player.GetComponent<DualCharacterController>();
    }

    public InteractionController GetInteractionController()
    {
        return player.GetComponent<InteractionController>();
    }

    public InventoryController GetInventoryController()
    {
        return player.GetComponent<InventoryController>();
    }

    public DocumentationController GetDocumentationController()
    {
        return player.GetComponent<DocumentationController>();
    }

}