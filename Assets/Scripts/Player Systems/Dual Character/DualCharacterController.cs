using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Cinemachine;
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

    // Group and switch
    [SerializeField]
    private GameObject character1;
    private NavMeshAgent navAgent1;
    [SerializeField]
    private GameObject character2;
    private NavMeshAgent navAgent2;

    private bool selectedCharacterOne = true;
    private bool canSwitch = true;

    private InputAction splitAction;
    [SerializeField]
    private Slider groupSlider;

    private bool grouped = true;
    private bool prevGrouped = true;
    private bool groupedError = false;
    private Vector3 followerVelocity = Vector3.zero;

    [Header("Camera Management")]
    [SerializeField]
    private CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField]
    private CinemachineVirtualCamera additiveCamera;
    private float defaultTransitionTime;

    [Header("Dialog Management")]
    [SerializeField]
    private GameObject dialoguePanel;

    private void Awake()
    {
        InitInputActions();
        InitSelectedPlayer();
        InitNavAgents();
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
        navAgent1 = character1.GetComponent<NavMeshAgent>();
        navAgent2 = character2.GetComponent<NavMeshAgent>();
        EnableNavAgents();
    }

    private void EnableNavAgents()
    {
        navAgent1.enabled = !selectedCharacterOne;
        navAgent2.enabled = selectedCharacterOne;
    }

    private void InitCamera()
    {
        defaultTransitionTime = stateDrivenCamera.m_DefaultBlend.m_Time;
    }

    // Movement

    private void Move()
    {
        rb.velocity = inputMovement * playerParams.Speed;
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
                FollowMovement(character1, character2, navAgent2);
            }
            else
            {
                FollowMovement(character2, character1, navAgent1);
            }
        }
    }

    private void FollowMovement(GameObject leader, GameObject follower, NavMeshAgent followerAgent)
    {
        if (followerAgent.speed == 0)
        {
            followerAgent.speed = playerParams.Speed;
        }

        if (followerAgent.isOnNavMesh){
            followerAgent.SetDestination(leader.transform.position);
            follower.transform.position = Vector3.SmoothDamp(follower.transform.position, followerAgent.nextPosition, ref followerVelocity, 0.1f);
        }
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
                    StartCoroutine(SceneLoadManager.Instance.LoadSceneSwitchCoroutine());
                }
            }
        }
        else if (context.interaction is HoldInteraction)
        {
            if ((grouped || CanGroup()) && !dialoguePanel.activeSelf)
            {
                SwitchGrouping();
            }
        }
    }

    public void SwitchCharacter()
    {
        rb.velocity = Vector2.zero;

        SwitchSelectedCharacter();
        InitSelectedPlayer();
        EnableNavAgents();

        SwitchToCharacterCamera();

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
        if (!groupedError && !dialoguePanel.activeSelf)
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

    public void SwitchToAdditiveCamera()
    {
        additiveCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
        stateDrivenCamera.GetComponent<Animator>().Play(PlayerConstants.CameraStateAdditive);
    }

    public void SwitchToCharacterCamera()
    {
        Animator cameraAnimator = stateDrivenCamera.GetComponent<Animator>();
        if (selectedCharacterOne)
        {
            cameraAnimator.Play(PlayerConstants.CameraStateRyo);
        }
        else
        {
            cameraAnimator.Play(PlayerConstants.CameraStateShinen);
        }
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

    public CharacterAnimator GetCharacterAnimator(bool selected)
    {
        return GetCharacter(selected).GetComponent<CharacterAnimator>();
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

    public CinemachineVirtualCamera GetAdditiveCamera()
    {
        return additiveCamera;
    }
}
