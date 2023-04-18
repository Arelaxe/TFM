using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerInput input;

    private Vector2 inputMovement;
    private Vector2 prevInputMovement;
    private bool horzDir;

    [SerializeField]
    private PlayerConstants constants;
    public PlayerConstants Constants { get => constants; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        input = GetComponent<PlayerInput>();

        InitInputActions();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InitInputActions()
    {
        InputAction moveAction = input.actions[constants.ActionMove];
        moveAction.performed += (ctx) => { inputMovement = ctx.ReadValue<Vector2>(); };
        moveAction.canceled += (ctx) => { inputMovement = Vector2.zero; rb.velocity = inputMovement; };
    }

    private void Move()
    {
        if (inputMovement.x != 0f || inputMovement.y != 0f)
        {
            CheckDirection();

            if (horzDir)
            {
                rb.velocity = new Vector2(Mathf.Sign(inputMovement.x) * constants.Speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, Mathf.Sign(inputMovement.y) * constants.Speed);
            }
        }

        prevInputMovement = inputMovement;
    }

    private void CheckDirection()
    {
        if (input.currentControlScheme == "Gamepad")
        {
            if (Mathf.Abs(inputMovement.x) > Mathf.Abs(inputMovement.y))
            {
                horzDir = true;
            }
            else if (Mathf.Abs(inputMovement.y) > Mathf.Abs(inputMovement.x))
            {
                horzDir = false;
            }
        }
        else
        {
            if (prevInputMovement.x == 0f && inputMovement.x != 0f || inputMovement.x != 0f && inputMovement.y == 0f)
            {
                horzDir = true;
            }
            else if (prevInputMovement.y == 0f && inputMovement.y != 0f || inputMovement.y != 0f && inputMovement.x == 0f)
            {
                horzDir = false;
            }
        }
    }
}
