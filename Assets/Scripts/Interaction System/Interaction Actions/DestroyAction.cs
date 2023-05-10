using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAction : Action
{
    public override void Execute(PlayerController playerController)
    {
        Destroy(gameObject);
    }
}
