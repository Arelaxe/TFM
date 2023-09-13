using System.Collections;
using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    private GameObject parent;
    private GameObject target;
    
    [SerializeField]
    private float parentSeparation = 0.3f;
    [SerializeField]
    private float siblingsSeparation = 0.25f;
    private float separation;

    [SerializeField]
    private float defaultSpeedFactor = 4f;
    [SerializeField]
    private float acceleration = 0.1f;
    [SerializeField]
    private float endSpeed = 40f;
    private float speedFactor;

    private Vector3 newPosition;
    private bool reached = false;

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        if (target)
        {
            if (parent.GetComponent<EnergyFlowController>().CanMove)
            {
                Vector3 targetPosition = new(target.transform.position.x, target.transform.position.y - separation, target.transform.position.z);
                newPosition = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * GetSpeed());

                if (!reached && Mathf.Abs(target.transform.position.y - newPosition.y) > separation)
                {
                    speedFactor += acceleration;
                }
                else
                {
                    reached = true;
                    speedFactor = defaultSpeedFactor;
                }
            }
            else
            {
                newPosition = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * endSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            transform.position = newPosition;
        }
    }

    public void SetTarget(GameObject parent, GameObject target)
    {
        this.parent = parent;
        this.target = target;

        separation = parent.Equals(target) ? parentSeparation : siblingsSeparation;
    }

    private float GetSpeed()
    {
        return parent.GetComponent<Rigidbody2D>().velocity.magnitude * speedFactor;
    }
}
