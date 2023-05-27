using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public bool selectedCharacterOne = true;

    public GameObject player;

    protected override void LoadData()
    {
        player = GameObject.Find("Player");
    }

    public void SwitchSelectedCharacter()
    {
        selectedCharacterOne = !selectedCharacterOne;
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

}