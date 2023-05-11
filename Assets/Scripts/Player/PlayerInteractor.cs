using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class PlayerInteractor : MonoBehaviour
{
    private PlayerController playerController;
    private InventoryController inventoryController;

    private PlayerInput input;
    private InputAction interactAction;

    private Vector2 position;
    private Vector2 size;

    private readonly Collider2D[] colliders = new Collider2D[3];
    private int foundInteractables;

    [Header("UI Elements")]
    [SerializeField]
    private RectTransform interactionCanvas;
    [Space]
    [SerializeField]
    private GameObject interactionBtn;
    [SerializeField]
    private float bottomOffset;
    [SerializeField]
    private float spacing;
    private List<GameObject> interactions = new();

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        inventoryController = GetComponent<InventoryController>();
        InitInputActions();
    }

    void Update()
    {
        SetInteractionArea();
        foundInteractables = Physics2D.OverlapBoxNonAlloc(position, size, 0, colliders, LayerMask.GetMask(GlobalConstants.LayerInteractable));

        if (foundInteractables > 0)
        {
            Interactable interactable = colliders[0].GetComponent<Interactable>();
            if (interactable != null && interactions.Count == 0)
            {
                InstantiateInteractions(interactable);
            }
        }
    }

    private void InitInputActions()
    {
        input = playerController.GetComponent<PlayerInput>();
        interactAction = input.actions[PlayerConstants.ActionInteract];
    }

    private void SetInteractionArea()
    {
        Tuple<bool, bool> lookingAt = playerController.GetLookingAt();
        BoxCollider2D playerCol = playerController.GetPlayerCollider();
        if (lookingAt.Item1)
        {
            position = new Vector2(playerCol.bounds.center.x, lookingAt.Item2 ? playerCol.bounds.max.y : playerCol.bounds.min.y);
            size = new Vector2(playerCol.size.x, playerController.PlayerParams.InteractionRange);
        }
        else
        {
            position = new Vector2(lookingAt.Item2 ? playerCol.bounds.max.x : playerCol.bounds.min.x, playerCol.bounds.center.y);
            size = new Vector2(playerController.PlayerParams.InteractionRange, playerCol.size.y);
        }
    }

    private void InstantiateInteractions(Interactable interactable)
    {
        int shown = 0;
        for (int i = 0; i < interactable.Interactions.Length; i++)
        {
            Interaction interaction = interactable.Interactions[i];

            if (interaction.IsAvailable)
            {
                GameObject goButton = Instantiate(interactionBtn);
                goButton.transform.SetParent(interactionCanvas, false);

                SpriteRenderer spriteRenderer = playerController.GetPlayerSpriteRenderer();
                goButton.transform.position = new Vector3(spriteRenderer.bounds.center.x, spriteRenderer.bounds.max.y + bottomOffset + spacing * shown, goButton.transform.position.z);
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
        tempButton.onClick.AddListener(() => PerformInteraction(interaction.Action));

        if (IsNotInteractable(interaction))
        {
            tempButton.interactable = false;
        }
        goButton.SetActive(true);
    }

    private bool IsNotInteractable(Interaction interaction)
    {
        bool isCharacter1 = playerController.IsCharacter1(false);

        bool hackingConstraint = interaction.Type.Equals(Interaction.ActionType.Hacking) && !isCharacter1;
        bool spiritualConstraint = interaction.Type.Equals(Interaction.ActionType.Spiritual) && isCharacter1;
        bool itemConstraint = interaction.RequiredItem != null && !inventoryController.HasCharacterItem(isCharacter1, interaction.RequiredItem);

        bool pickUpConstraint = false;
        if (interaction.Action is PickUpAction)
        {
            pickUpConstraint = inventoryController.IsCharacterInventoryFull(isCharacter1);
        }

        return hackingConstraint || spiritualConstraint || itemConstraint || pickUpConstraint;
    }

    private void PerformInteraction(Action action)
    {
        if (action)
        {
            action.Execute(playerController);
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
}
