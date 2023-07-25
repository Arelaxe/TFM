using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsNotDestroyable : NarrationAction
{
    [SerializeField]
    protected GameObject object1;
    [SerializeField]
    protected GameObject object2;
    [SerializeField] 
    protected int position1X;
    [SerializeField] 
    protected int position1Y;
    [SerializeField] 
    protected int position1Z;
    [SerializeField] 
    protected int position2X;
    [SerializeField] 
    protected int position2Y;
    [SerializeField] 
    protected int position2Z;
    
    public override void Execute() { 
        base.Execute();
        GameObject clone1 = Instantiate(object1, new Vector3(position1X, position1Y, position1Z), Quaternion.identity);
        GameObject clone2 = Instantiate(object2, new Vector3(position2X, position2Y, position2Z), Quaternion.identity);
    }

    public override void EndAction() {
        base.EndAction();
    }
}