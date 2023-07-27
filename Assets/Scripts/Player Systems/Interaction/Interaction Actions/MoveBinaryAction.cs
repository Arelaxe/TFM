using UnityEngine;

public class MoveBinaryAction : Action
{
    [SerializeField]
    private BinaryMovable movable;

    public override void Execute()
    {
        movable.Move();
    }
}
