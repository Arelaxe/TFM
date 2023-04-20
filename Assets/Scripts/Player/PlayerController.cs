using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController: MonoBehaviour
{
    // General
    private PlayerInput input;
    private Rigidbody2D rb;
    private Collider2D col;
    
    [SerializeField]
    private PlayerConstants constants;

    // Movement
    private Vector2 inputMovement;

    // Split
    [SerializeField]
    private GameObject character1;
    private NavMeshAgent navAgent1;
    [SerializeField]
    private GameObject character2;
    private NavMeshAgent navAgent2;

    private bool selectedCharacter1 = true;
    private bool grouped = true;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true); // TODO extraer en clase de inicialización
    }

    void Start()
    {
        InitInputActions();
        InitSelectedPlayer();
        InitNavAgents();
    }

    private void Update()
    {
        CheckSplit();
        Follow();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();

        InputAction moveAction = input.actions[constants.ActionMove];
        moveAction.performed += (ctx) => { inputMovement = ctx.ReadValue<Vector2>().normalized; };
        moveAction.canceled += (ctx) => { inputMovement = Vector2.zero; };

        InputAction splitAction = input.actions[constants.ActionSplit];
        splitAction.performed += SwitchCharacterAndGrouping;
    }

    private void InitSelectedPlayer()
    {
        rb = GetSelectedCharacter().GetComponent<Rigidbody2D>();
        col = GetSelectedCharacter().GetComponent<Collider2D>();
    }

    private void InitNavAgents()
    {
        if (!navAgent1)
        {
            navAgent1 = character1.GetComponent<NavMeshAgent>();
            navAgent1.updateRotation = false;
            navAgent1.updateUpAxis = false;
        }
        if (!navAgent2)
        {
            navAgent2 = character2.GetComponent<NavMeshAgent>();
            navAgent2.updateRotation = false;
            navAgent2.updateUpAxis = false;
        }

        navAgent1.enabled = !selectedCharacter1;
        navAgent2.enabled = selectedCharacter1;
    }

    private void Move()
    {
        rb.velocity = inputMovement * constants.Speed;
    }

    private void Follow()
    {
        if (grouped)
        {
            if (selectedCharacter1)
            {
                navAgent2.SetDestination(character1.transform.position);
            }
            else
            {
                navAgent1.SetDestination(character2.transform.position);
            }
        }
    }

    private void SwitchCharacterAndGrouping(InputAction.CallbackContext context)
    {
        if (context.interaction is PressInteraction)
        {
            SwitchCharacter();
        }
        else if (context.interaction is HoldInteraction)
        {
            if (grouped || CanGroup())
            {
                SwitchGrouping();
            }
        }
    }

    private void SwitchCharacter()
    {
        selectedCharacter1 = !selectedCharacter1;
        rb.velocity = Vector2.zero;
        InitSelectedPlayer();
        InitNavAgents();
    }

    private void SwitchGrouping()
    {
        grouped = !grouped;
        GetUnselectedCharacterAgent().enabled = grouped;
    }

    private bool CanGroup()
    {
        bool canGroup = false;
        LayerMask layerMask = LayerMask.GetMask(LayerMask.LayerToName(GetSelectedCharacter().layer == 6 ? 7 : 6)); // TODO extraer en scriptable object general

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GetSelectedCharacter().transform.position, constants.GroupingMaxDistance, layerMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player") // TODO extraer en scriptable object general
            {
                canGroup = true;
                break;
            }
            i++;
        }

        return canGroup;
    }

    private void CheckSplit()
    {
        if (GetUnselectedCharacterAgent().pathStatus.Equals(NavMeshPathStatus.PathPartial))
        {
            SwitchGrouping();
        }
    }

    private GameObject GetSelectedCharacter()
    {
        return selectedCharacter1 ? character1 : character2;
    }

    private NavMeshAgent GetUnselectedCharacterAgent()
    {
        return selectedCharacter1 ? navAgent2 : navAgent1;
    }

    public PlayerConstants Constants { get => constants; }
}
