using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTemporaryObjects : NarrationAction
{
    public override void Execute() { 
        base.Execute();
        GameObject[] temporary = GameObject.FindGameObjectsWithTag("Temporary"); 

        for (int i = 0; i < temporary.Length; i++){
            Destroy(temporary[i]);
        }
    }

    public override void EndAction() { 
        base.EndAction();
    }
}
