using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerController: MonoBehaviour
{
    // General
    private PlayerInput input;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private SpriteRenderer spriteRenderer;

    private PlayerInteractor interactor;
    
    [SerializeField]
    private PlayerParams playerParams;

    // Movement
    private Vector2 inputMovement;
    private Tuple<bool, bool> lookingAt1 = Tuple.Create(true, false);
    private Tuple<bool, bool> lookingAt2 = Tuple.Create(true, false);

    // Group and switch
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
    private Vector3 followerVelocity = Vector3.zero;

    // Camera
    [SerializeField]
    private Animator cameraAnimator;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(GlobalConstants.LayerIntTerrenal, GlobalConstants.LayerIntSpiritual, true); // TODO extraer en clase de inicialización
    }

    void Start()
    {
        InitInputActions();
        InitSelectedPlayer();
        InitNavAgents();
        InitAnimators();
        interactor = GetComponent<PlayerInteractor>();
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

    // Initialization
    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();

        InputAction moveAction = input.actions[PlayerConstants.ActionMove];
        moveAction.performed += (ctx) => { inputMovement = ctx.ReadValue<Vector2>().normalized; };
        moveAction.canceled += (ctx) => { inputMovement = Vector2.zero; };

        InputAction splitAction = input.actions[PlayerConstants.ActionSplit];
        splitAction.performed += SwitchCharacterAndGrouping;
    }

    private void InitSelectedPlayer()
    {
        rb = GetSelectedCharacter().GetComponent<Rigidbody2D>();
        col = GetSelectedCharacter().GetComponent<BoxCollider2D>();
        spriteRenderer = GetSelectedCharacter().GetComponent<SpriteRenderer>();
    }

    private void InitNavAgents()
    {
        if (!navAgent1)
        {
            navAgent1 = character1.GetComponent<NavMeshAgent>();
            InitNavAgent(navAgent1);
        }
        if (!navAgent2)
        {
            navAgent2 = character2.GetComponent<NavMeshAgent>();
            InitNavAgent(navAgent2);
        }

        navAgent1.enabled = !selectedCharacter1;
        navAgent2.enabled = selectedCharacter1;
    }

    private void InitNavAgent(NavMeshAgent navAgent)
    {
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.updatePosition = false;
    }

    private void InitAnimators()
    {
        animator1 = character1.GetComponent<Animator>();
        animator2 = character2.GetComponent<Animator>();
    }

    // Movement

    private void Move()
    {
        rb.velocity = inputMovement * playerParams.Speed;
        SetMoveAnimParams(rb.velocity, selectedCharacter1 ? animator1 : animator2, false);

        if (rb.velocity.sqrMagnitude != 0)
        {
            interactor.DestroyInteractions();
        }
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
            followerAgent.speed = playerParams.Speed;
        }
        followerAgent.SetDestination(leader.transform.position);
        follower.transform.position = Vector3.SmoothDamp(follower.transform.position, followerAgent.nextPosition, ref followerVelocity, 0.1f);

        SetMoveAnimParams(followerAgent.velocity, followerAnimator, true);
    }

    private void SetMoveAnimParams(Vector2 velocity, Animator characterAnimator, bool isFollower)
    {
        SetLookingAt(isFollower);
        characterAnimator.SetInteger(PlayerConstants.AnimParamVelocity, (int)velocity.sqrMagnitude);
        characterAnimator.SetBool(PlayerConstants.AnimParamVerticalMovement, IsCharacter1(isFollower) ? lookingAt1.Item1 : lookingAt2.Item1);
        characterAnimator.SetBool(PlayerConstants.AnimParamPositiveMovement, IsCharacter1(isFollower) ? lookingAt1.Item2 : lookingAt2.Item2);
    }

    private void SetLookingAt(bool isFollower)
    {
      Vector2 velocity = rb.velocity;
        if (isFollower)
        {
            velocity = IsCharacter1(isFollower) ? navAgent1.velocity : navAgent2.velocity;
        }

        bool verticalMovement = IsCharacter1(isFollower) ? lookingAt1.Item1 : lookingAt2.Item1;
        bool positiveMovement = IsCharacter1(isFollower) ? lookingAt1.Item2 : lookingAt2.Item2;

        Tuple<bool, bool> currentLookingAt = CalculateLookingAt(velocity, verticalMovement, positiveMovement);
        if (IsCharacter1(isFollower))
        {
            lookingAt1 = currentLookingAt;
        }
        else
        {
            lookingAt2 = currentLookingAt;
        }
    }

    private Tuple<bool, bool> CalculateLookingAt(Vector2 velocity, bool currentVerticalMovement, bool currentPositiveMovement)
    {
        int velocitySqr = (int)velocity.sqrMagnitude;
        if (velocitySqr > 0)
        {
            currentVerticalMovement = Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x);
            if (currentVerticalMovement)
            {
                currentPositiveMovement = velocity.y > 0;
            }
            else
            {
                currentPositiveMovement = velocity.x > 0;
            }
        }
        return Tuple.Create(currentVerticalMovement, currentPositiveMovement);
    }

    // Switch and group

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
            cameraAnimator.Play(PlayerConstants.CameraStateRyo);
        }
        else
        {
            cameraAnimator.Play(PlayerConstants.CameraStateShinen);
        }

        interactor.DestroyInteractions();
    }

    private void SwitchGrouping()
    {
        grouped = !grouped;
        GetUnselectedCharacterAgent().enabled = grouped;
    }

    private bool CanGroup()
    {
        bool canGroup = false;
        int layer = GetSelectedCharacter().layer == GlobalConstants.LayerIntTerrenal ? GlobalConstants.LayerIntSpiritual : GlobalConstants.LayerIntTerrenal;
        LayerMask layerMask = LayerMask.GetMask(LayerMask.LayerToName(layer));

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GetSelectedCharacter().transform.position, playerParams.GroupingMaxDistance, layerMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == GlobalConstants.TagPlayer)
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

    // Auxiliar methods

    public GameObject GetSelectedCharacter()
    {
        return selectedCharacter1 ? character1 : character2;
    }

    private NavMeshAgent GetUnselectedCharacterAgent()
    {
        return selectedCharacter1 ? navAgent2 : navAgent1;
    }

    public bool IsCharacter1(bool isFollower)
    {
        return isFollower ? !selectedCharacter1 : selectedCharacter1;
    }

    public Tuple<bool, bool> GetLookingAt()
    {
        return selectedCharacter1 ? lookingAt1 : lookingAt2;
    }

    public BoxCollider2D GetPlayerCollider()
    {
        return col;
    }

    public SpriteRenderer GetPlayerSpriteRenderer()
    {
        return spriteRenderer;
    }

    public bool AreGrouped()
    {
        return grouped;
    }

    public PlayerParams PlayerParams { get => playerParams; }
}
