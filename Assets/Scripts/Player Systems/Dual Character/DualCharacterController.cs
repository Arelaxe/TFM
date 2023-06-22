using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Cinemachine;
using System;
using System.Collections;

public class DualCharacterController: MonoBehaviour
{
    // General
    private PlayerInput input;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    
    [SerializeField]
    private PlayerParams playerParams;

    // Movement
    private bool canMove = true;
    private bool canFollowerMove = true;
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

    private bool selectedCharacterOne = true;
    private bool canSwitch = true;

    private InputAction splitAction;
    [SerializeField]
    private Slider groupSlider;

    private bool grouped = true;
    private bool prevGrouped = true;
    private bool groupedError = false;
    private Vector3 followerVelocity = Vector3.zero;

    // Camera
    [SerializeField]
    private CinemachineStateDrivenCamera stateDrivenCamera;
    private float defaultTransitionTime;

    private void Awake()
    {
        InitInputActions();
        InitSelectedPlayer();
        InitNavAgents();
        InitAnimators();
        InitCamera();
    }

    private void Update()
    {
        CheckSplit();
        UpdateSplitBar();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
        if (canFollowerMove)
        {
            Follow();
        }
    }

    // Initialization
    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();

        InputAction moveAction = input.actions[PlayerConstants.ActionMove];
        moveAction.performed += (ctx) => { inputMovement = ctx.ReadValue<Vector2>().normalized; };
        moveAction.canceled += (ctx) => { inputMovement = Vector2.zero; };

        splitAction = input.actions[PlayerConstants.ActionSplit];
        splitAction.performed += SwitchCharacterAndGrouping;
    }

    private void InitSelectedPlayer()
    {
        rb = GetCharacter(true).GetComponent<Rigidbody2D>();
        col = GetCharacter(true).GetComponent<BoxCollider2D>();
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

        navAgent1.enabled = !selectedCharacterOne;
        navAgent2.enabled = selectedCharacterOne;
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

    private void InitCamera()
    {
        defaultTransitionTime = stateDrivenCamera.m_DefaultBlend.m_Time;
    }

    // Movement

    private void Move()
    {
        rb.velocity = inputMovement * playerParams.Speed;
        SetMoveAnimParams(rb.velocity, selectedCharacterOne ? animator1 : animator2, false);

        if (rb.velocity.sqrMagnitude != 0)
        {
            PlayerManager.Instance.GetInteractionController().DestroyInteractions();
        }
    }

    private void Follow()
    {
        if (grouped)
        {
            if (selectedCharacterOne)
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
        characterAnimator.SetBool(PlayerConstants.AnimParamVerticalMovement, IsCharacterOne(isFollower) ? lookingAt1.Item1 : lookingAt2.Item1);
        characterAnimator.SetBool(PlayerConstants.AnimParamPositiveMovement, IsCharacterOne(isFollower) ? lookingAt1.Item2 : lookingAt2.Item2);
    }

    private void SetLookingAt(bool isFollower)
    {
        Vector2 velocity = rb.velocity;
        if (isFollower)
        {
            velocity = IsCharacterOne(isFollower) ? navAgent1.velocity : navAgent2.velocity;
        }

        bool verticalMovement = IsCharacterOne(isFollower) ? lookingAt1.Item1 : lookingAt2.Item1;
        bool positiveMovement = IsCharacterOne(isFollower) ? lookingAt1.Item2 : lookingAt2.Item2;

        Tuple<bool, bool> currentLookingAt = CalculateLookingAt(velocity, verticalMovement, positiveMovement);
        if (IsCharacterOne(isFollower))
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

    public void SetCharacterLookingAt(bool selected, Tuple<bool, bool> lookingAt)
    {
        Animator animator;
        if (selected)
        {
            animator = selectedCharacterOne ? ref animator1 : ref animator2;
        }
        else
        {
            animator = selectedCharacterOne ? ref animator2 : ref animator1;
        }
        animator.SetInteger(PlayerConstants.AnimParamVelocity, 1);
        animator.SetBool(PlayerConstants.AnimParamVerticalMovement, lookingAt.Item1);
        animator.SetBool(PlayerConstants.AnimParamPositiveMovement, lookingAt.Item2);

        StartCoroutine(DisableAnimVelocity(animator));
    }

    private IEnumerator DisableAnimVelocity(Animator animator)
    {
        yield return null;
        animator.SetInteger(PlayerConstants.AnimParamVelocity, 0);
    }

    // Switch and group

    private void SwitchCharacterAndGrouping(InputAction.CallbackContext context)
    {
        if (context.interaction is PressInteraction)
        {
            if (canSwitch)
            {
                if (!SceneLoadManager.Instance.LoadSceneOnSwitch)
                {
                    SwitchCharacter();
                }
                else
                {
                    StartCoroutine(SceneLoadManager.Instance.LoadSceneSwitchCouroutine());
                }
            }
        }
        else if (context.interaction is HoldInteraction)
        {
            if (grouped || CanGroup())
            {
                SwitchGrouping();
            }
        }
    }

    public void SwitchCharacter()
    {
        SwitchSelectedCharacter();
        rb.velocity = Vector2.zero;
        InitSelectedPlayer();
        InitNavAgents();

        Animator cameraAnimator = stateDrivenCamera.GetComponent<Animator>();
        if (selectedCharacterOne)
        {
            cameraAnimator.Play(PlayerConstants.CameraStateRyo);
        }
        else
        {
            cameraAnimator.Play(PlayerConstants.CameraStateShinen);
        }

        PlayerManager.Instance.GetInteractionController().DestroyInteractions();
        PlayerManager.Instance.GetInventoryController().UpdateItemPanelsForSwitch(selectedCharacterOne, grouped);
    }

    public void SwitchGrouping()
    {
        grouped = !grouped;

        if (grouped)
        {
            SceneLoadManager.Instance.ResetFollowerInSceneData();
        }

        GetUnselectedCharacterAgent().enabled = grouped;

        PlayerManager.Instance.GetInventoryController().UpdateItemPanelsForGrouping(selectedCharacterOne, grouped);
    }

    private bool CanGroup()
    {
        bool canGroup = false;
        int layer = GetCharacter(true).layer == GlobalConstants.LayerIntTerrenal ? GlobalConstants.LayerIntSpiritual : GlobalConstants.LayerIntTerrenal;
        LayerMask layerMask = LayerMask.GetMask(LayerMask.LayerToName(layer));

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GetCharacter(true).transform.position, playerParams.GroupingMaxDistance, layerMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag(GlobalConstants.TagPlayer))
            {
                canGroup = true;
                break;
            }
            i++;
        }

        return canGroup && !groupedError;
    }

    private void CheckSplit()
    {
        if (GetUnselectedCharacterAgent().pathStatus.Equals(NavMeshPathStatus.PathPartial))
        {
            SwitchGrouping();
        }
    }

    private void UpdateSplitBar()
    {
        if (!groupedError)
        {
            if (splitAction.inProgress && splitAction.GetTimeoutCompletionPercentage() > 0.1 && splitAction.GetTimeoutCompletionPercentage() < 1)
            {
                groupSlider.gameObject.SetActive(true);
                groupSlider.value = splitAction.GetTimeoutCompletionPercentage();
                prevGrouped = grouped;
            }
            else if (groupSlider.gameObject.activeSelf && groupSlider.value > 0)
            {
                if (splitAction.GetTimeoutCompletionPercentage() == 1)
                {
                    StartCoroutine(CheckGroupError());
                }
                else
                {
                    ResetGroupSlider();
                }
            }
        }
    }

    private IEnumerator CheckGroupError()
    {
        if (!prevGrouped && !grouped)
        {
            groupedError = true;
            groupSlider.fillRect.GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
        }
        ResetGroupSlider();
    } 

    private void ResetGroupSlider()
    {
        groupSlider.fillRect.GetComponent<Image>().color = Color.white;
        groupSlider.value = 0;
        groupSlider.gameObject.SetActive(false);
        groupedError = false;
    }

    // Auxiliar methods
    public PlayerParams Params { get => playerParams; }
    public BoxCollider2D Collider { get => col; }
    public bool SelectedCharacterOne { get => selectedCharacterOne; }
    public bool Grouped { get => grouped; }

    public GameObject GetCharacter(bool selected)
    {
        GameObject character;
        if (selected)
        {
            character = selectedCharacterOne ? character1 : character2;
        }
        else
        {
            character = selectedCharacterOne ? character2 : character1;
        }
        return character;
    }

    public bool IsCharacterActive(bool selected)
    {
        GameObject character = GetCharacter(selected);
        return character.GetComponent<SpriteRenderer>().enabled && character.GetComponent<Collider2D>().enabled;
    }

    public void SetCharacterActive(bool selected, bool active)
    {
        GameObject character = GetCharacter(selected);
        character.GetComponent<SpriteRenderer>().enabled = active;
        character.GetComponent<Collider2D>().enabled = active;
    }

    public Tuple<bool, bool> GetCharacterLookingAt(bool selected)
    {
        Tuple<bool, bool> lookingAt;
        if (selected)
        {
            lookingAt = selectedCharacterOne ? lookingAt1 : lookingAt2;
        }
        else
        {
            lookingAt = selectedCharacterOne ? lookingAt2 : lookingAt1;
        }
        return lookingAt;
    }

    public bool IsCharacterOne(bool isFollower)
    {
        return isFollower ? !selectedCharacterOne : selectedCharacterOne;
    }

    private NavMeshAgent GetUnselectedCharacterAgent()
    {
        return selectedCharacterOne ? navAgent2 : navAgent1;
    }

    // Flags

    public void SwitchSelectedCharacter()
    {
        selectedCharacterOne = !selectedCharacterOne;
    }

    public void SetCharacterMobility(bool selected, bool canMove)
    {
        if (selected)
        {
            this.canMove = canMove;
            if (!canMove)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            canFollowerMove = canMove;
            GetUnselectedCharacterAgent().enabled = canMove;
        }
    }

    public void SetMobility(bool canMove)
    {
        SetCharacterMobility(true, canMove);
        SetCharacterMobility(false, canMove);
    }

    public void SetSwitchAvailability(bool canSwitch)
    {
        this.canSwitch = canSwitch;
    }
    
    public void SetCameraTransitionTime(bool insta)
    {
        CinemachineBlendDefinition blend = stateDrivenCamera.m_DefaultBlend;
        blend.m_Time = insta ? 0 : defaultTransitionTime;
        stateDrivenCamera.m_DefaultBlend = blend;
    }

}
