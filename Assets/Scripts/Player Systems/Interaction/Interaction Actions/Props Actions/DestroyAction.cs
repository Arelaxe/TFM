using UnityEngine;

public class DestroyAction : Action
{
    [SerializeField]
    private GameObject destroyable;

    public override void Execute()
    {
        if (destroyable)
        {
            Destroy(destroyable);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
