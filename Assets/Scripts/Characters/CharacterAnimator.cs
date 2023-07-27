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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.updatePosition = false;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimMovement();
    }

    private void AnimMovement()
    {
        Vector2 velocity = !navAgent.enabled ? rb.velocity : navAgent.velocity;

        lookingAt = CalculateLookingAt(velocity, lookingAt.Item1, lookingAt.Item2);

        animator.SetInteger(PlayerConstants.AnimParamVelocity, (int)velocity.sqrMagnitude);
        animator.SetBool(PlayerConstants.AnimParamVerticalMovement, lookingAt.Item1);
        animator.SetBool(PlayerConstants.AnimParamPositiveMovement, lookingAt.Item2);
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
}
