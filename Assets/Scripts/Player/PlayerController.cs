using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController: MonoBehaviour
{
    // General
    private PlayerInput input;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    
    [SerializeField]
    private PlayerConstants constants;

    // Movement
    private Vector2 inputMovement;

    // Split
    [SerializeField]
    private GameObject character1;
    private NavMeshAgent navAgent1;
    private Animator animator1;
    [SerializeField]
    private GameObject character2;
    private NavMeshAgent navAgent2;
    private Animator animator2;

    private bool selectedCharacter1 = true;
    private bool grouped = true;
    private Vector3 followVelocity = Vector3.zero;

    // Camera
    [SerializeField]
    private Animator cameraAnimator;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true); // TODO extraer en clase de inicialización
    }

    void Start()
    {
        InitInputActions();
        InitSelectedPlayer();
        InitNavAgents();
        InitAnimators();
    }

    private void Update()
    {
        CheckSplit();
    }

    private void FixedUpdate()
    {
        Move();
        Follow();
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
        col = GetSelectedCharacter().GetComponent<BoxCollider2D>();
    }

    private void InitNavAgents()
    {
        if (!navAgent1)
        {
            navAgent1 = character1.GetComponent<NavMeshAgent>();
            navAgent1.updateRotation = false;
            navAgent1.updateUpAxis = false;
            navAgent1.updatePosition = false;
        }
        if (!navAgent2)
        {
            navAgent2 = character2.GetComponent<NavMeshAgent>();
            navAgent2.updateRotation = false;
            navAgent2.updateUpAxis = false;
            navAgent2.updatePosition = false;
        }

        navAgent1.enabled = !selectedCharacter1;
        navAgent2.enabled = selectedCharacter1;
    }

    private void InitAnimators()
    {
        animator1 = character1.GetComponent<Animator>();
        animator2 = character2.GetComponent<Animator>();
    }

    private void Move()
    {
        rb.velocity = inputMovement * constants.Speed;
        SetMoveAnimParams(rb.velocity, selectedCharacter1 ? animator1 : animator2);
    }

    private void Follow()
    {
        if (grouped)
        {
            if (selectedCharacter1)
            {
                FollowMovement(character1, character2, navAgent2, animator2);
            }
            else
            {
                FollowMovement(character2, character1, navAgent1, animator1);
            }
        }
    }

    private void FollowMovement(GameObject leader, GameObject follower, NavMeshAgent followerAgent, Animator followerAnimator)
    {
        if (followerAgent.speed == 0)
        {
            followerAgent.speed = constants.Speed;
        }
        followerAgent.SetDestination(leader.transform.position);
        follower.transform.position = Vector3.SmoothDamp(follower.transform.position, followerAgent.nextPosition, ref followVelocity, 0.1f);

        SetMoveAnimParams(followerAgent.velocity, followerAnimator);
    }

    private void SetMoveAnimParams(Vector2 velocity, Animator characterAnimator)
    {
        int velocitySqr = (int) velocity.sqrMagnitude;
        characterAnimator.SetInteger(constants.AnimParamVelocity, velocitySqr);

        if (velocitySqr > 0)
        {
            bool verticalMovement = Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x);
            bool positiveMovement;
            if (verticalMovement)
            {
                positiveMovement = velocity.y > 0;
            }
            else
            {
                positiveMovement = velocity.x > 0;
            }

            characterAnimator.SetBool(constants.AnimParamVerticalMovement, verticalMovement);
            characterAnimator.SetBool(constants.AnimParamPositiveMovement, positiveMovement);
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

        if (selectedCharacter1)
        {
            cameraAnimator.Play(constants.CameraStateRyo);
        }
        else
        {
            cameraAnimator.Play(constants.CameraStateShinen);
        }
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
