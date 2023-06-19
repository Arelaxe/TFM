using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameObject player;

    public bool SelectedCharacterOne { get => GetDualCharacterController().SelectedCharacterOne; }
    public bool Grouped { get => GetDualCharacterController().Grouped; }

    protected override void LoadData()
    {
        GameObject playerUtils = Instantiate(SceneLoadManager.Instance.PlayerUtils, Vector3.zero, Quaternion.identity);

        int childCount = playerUtils.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = playerUtils.transform.GetChild(0);
            child.parent = transform;
            
            if (child.gameObject.CompareTag(GlobalConstants.TagPlayer))
            {
                player = child.gameObject;
            }
        }

        Destroy(playerUtils);
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