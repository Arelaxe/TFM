using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private NavMeshAgent navAgent;
    private Animator animator;

    private Tuple<bool, bool> lookingAt = Tuple.Create(true, false);
    private bool settingLookingAt;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.updatePosition = false;

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        AnimMovement(); 
    }

    private void AnimMovement()
    {
        if (!settingLookingAt)
        {
            Vector2 velocity = !navAgent.enabled ? rb.velocity : navAgent.velocity;

            lookingAt = CalculateLookingAt(velocity, lookingAt.Item1, lookingAt.Item2);

            animator.SetInteger(PlayerConstants.AnimParamVelocity, (int)velocity.sqrMagnitude);
            animator.SetBool(PlayerConstants.AnimParamVerticalMovement, lookingAt.Item1);
            animator.SetBool(PlayerConstants.AnimParamPositiveMovement, lookingAt.Item2);
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

    public Tuple<bool, bool> GetCharacterLookingAt()
    {
        return lookingAt;
    }

    public void SetCharacterLookingAt(Tuple<bool, bool> lookingAt)
    {
        this.lookingAt = lookingAt;
        StartCoroutine(SetLookingAt(lookingAt));
    }

    private IEnumerator SetLookingAt(Tuple<bool, bool> lookingAt)
    {
        settingLookingAt = true;

        animator.SetInteger(PlayerConstants.AnimParamVelocity, 1);
        animator.SetBool(PlayerConstants.AnimParamVerticalMovement, lookingAt.Item1);
        animator.SetBool(PlayerConstants.AnimParamPositiveMovement, lookingAt.Item2);
        
        yield return null;

        settingLookingAt = false;
    }

}
