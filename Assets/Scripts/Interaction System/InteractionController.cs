using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class InteractionController : MonoBehaviour
{
    private Vector2 position;
    private Vector2 size;

    private bool canInteract = true;
    private readonly Collider2D[] colliders = new Collider2D[3];
    private int foundInteractables;

    [Header("UI Elements")]
    [SerializeField]
    private RectTransform interactionCanvas;
    [Space]
    [SerializeField]
    private GameObject interactionBtn;
    [SerializeField]
    private float spacing;
    private List<GameObject> interactions = new();

    void Update()
    {
        SetInteractionArea();
        foundInteractables = Physics2D.OverlapBoxNonAlloc(position, size, 0, colliders, LayerMask.GetMask(GlobalConstants.LayerInteractable));

        if (foundInteractables > 0)
        {
            Interactable interactable = colliders[0].GetComponent<Interactable>();
            if (interactable != null && interactions.Count == 0 && canInteract)
            {
                InstantiateInteractions(interactable);
            }
        }
    }

    private void SetInteractionArea()
    {
        DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
        Tuple<bool, bool> lookingAt = playerController.GetLookingAt();
        BoxCollider2D playerCol = playerController.Collider;
        if (lookingAt.Item1)
        {
            position = new Vector2(playerCol.bounds.center.x, lookingAt.Item2 ? playerCol.bounds.max.y : playerCol.bounds.min.y);
            size = new Vector2(playerCol.size.x, playerController.Params.InteractionRange);
        }
        else
        {
            position = new Vector2(lookingAt.Item2 ? playerCol.bounds.max.x : playerCol.bounds.min.x, playerCol.bounds.center.y);
            size = new Vector2(playerController.Params.InteractionRange, playerCol.size.y);
        }
    }

    private void InstantiateInteractions(Interactable interactable)
    {
        CreateButtonList(interactable);

        SelectLastInteractableButton();

        foreach (var interaction in interactions)
        {
            interaction.SetActive(true);
        }
    }

    private void CreateButtonList(Interactable interactable)
    {
        int shown = 0;
        for (int i = 0; i < interactable.Interactions.Length; i++)
        {
            Interaction interaction = interactable.Interactions[i];

            if (interaction.IsAvailable)
            {
                GameObject goButton = Instantiate(interactionBtn);
                goButton.transform.SetParent(interactionCanvas, false);
                goButton.transform.position = new Vector3(interactionCanvas.position.x, interactionCanvas.position.y + spacing * shown, goButton.transform.position.z);
                goButton.transform.localScale = new Vector3(1, 1, 1);

                CustomButton(interaction, goButton);

                interactions.Add(goButton);
                shown++;
            }
        }
    }

    private void CustomButton(Interaction interaction, GameObject goButton)
    {
        Button tempButton = goButton.GetComponent<Button>();

        string text = interaction.Name;
        if (interaction.RequiredItem)
        {
            text = text + " (" + interaction.RequiredItem.Name + ")";
        }

        tempButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
        tempButton.onClick.AddListener(() => PerformInteraction(interaction));

        if (IsNotInteractable(interaction))
        {
            tempButton.interactable = false;
        }
    }

    private void SelectLastInteractableButton()
    {
        Button interactableButton = null;
        foreach (var interaction in interactions)
        {
            Button tempButton = interaction.GetComponent<Button>();
            if (tempButton.interactable)
            {
                interactableButton = tempButton;
            }
        }
        if (interactableButton)
        {
            interactableButton.Select();
        }
    }

    private bool IsNotInteractable(Interaction interaction)
    {
        InventoryController inventoryController = PlayerManager.Instance.GetInventoryController();

        bool selectedCharacterOne = PlayerManager.Instance.selectedCharacterOne;

        bool hackingConstraint = interaction.Type.Equals(Interaction.ActionType.Hacking) && !selectedCharacterOne;
        bool spiritualConstraint = interaction.Type.Equals(Interaction.ActionType.Spiritual) && selectedCharacterOne;
        bool itemConstraint = interaction.RequiredItem != null && !inventoryController.HasCharacterItem(selectedCharacterOne, interaction.RequiredItem);

        bool pickUpConstraint = false;
        if (interaction.Action is PickUpAction)
        {
            pickUpConstraint = inventoryController.IsCharacterInventoryFull(selectedCharacterOne);
        }

        return hackingConstraint || spiritualConstraint || itemConstraint || pickUpConstraint;
    }

    private void PerformInteraction(Interaction interaction)
    {
        Action action = interaction.Action;
        if (action)
        {
            if (interaction.RequiredItem)
            {
                PlayerManager.Instance.GetInventoryController().RemoveItem(interaction.RequiredItem);
            }
            action.Execute();
        }
    }

    public void DestroyInteractions()
    {
        if (interactions.Count != 0) {
            foreach (GameObject btn in interactions)
            {
                Destroy(btn);
            }
            interactions.Clear();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position, size);
    }

    // Auxiliar methods

    public void SetInteractivity(bool canInteract)
    {
        this.canInteract = canInteract;
    }
}
