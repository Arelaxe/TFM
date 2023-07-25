using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminateObject : NarrationAction
{
    [SerializeField]
    protected SpriteRenderer objectToIllum;

    public override void Execute()
    {
        objectToIllum.color = Color.cyan;
    }

    public override void EndAction()
    {
        throw new System.NotImplementedException();
    }
}
