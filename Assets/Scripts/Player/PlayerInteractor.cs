using UnityEngine;
using System;

public class PlayerInteractor : MonoBehaviour
{
    private PlayerController playerController;
    private Vector2 position;
    private Vector2 size;

    private readonly Collider2D[] cols = new Collider2D[3];
    [SerializeField]
    private int interactablesFound;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        SetPositionAndSize();
        interactablesFound = Physics2D.OverlapBoxNonAlloc(position, size, 0, cols, LayerMask.GetMask(GlobalConstants.LayerInteractable));
    }

    private void SetPositionAndSize()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position, size);
    }
}
