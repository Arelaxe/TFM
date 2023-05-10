using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    private Canvas canvas;
    private InventoryItem item;

    [SerializeField]
    private PlayerInput input;
    private InputAction mousePositionAction;

    public void Awake()
    {
        canvas = transform.parent.GetComponent<Canvas>();
        item = GetComponentInChildren<InventoryItem>();
        mousePositionAction = input.actions[PlayerConstants.ActionMousePos];
    }

    public void SetData(Sprite sprite)
    {
        item.SetData(sprite);
    }

    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, mousePositionAction.ReadValue<Vector2>(), canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool active)
    {
        gameObject.SetActive(active);
    }
}
