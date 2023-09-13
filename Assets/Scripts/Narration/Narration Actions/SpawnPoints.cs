using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : NarrationAction
{
    [SerializeField]
    private GameObject object1;
    [SerializeField]
    private GameObject object2;
    [SerializeField] 
    private int position1X;
    [SerializeField] 
    private int position1Y;
    [SerializeField] 
    private int position1Z;
    [SerializeField] 
    private int position2X;
    [SerializeField] 
    private int position2Y;
    [SerializeField] 
    private int position2Z;
    
    public override void Execute() { 
        base.Execute();
        GameObject clone1 = Instantiate(object1, new Vector3(position1X, position1Y, position1Z), Quaternion.identity);
        GameObject clone2 = Instantiate(object2, new Vector3(position2X, position2Y, position2Z), Quaternion.identity);

        clone1.GetComponent<DualDestroyableObject>().SetOtherObject(clone2);
        clone2.GetComponent<DualDestroyableObject>().SetOtherObject(clone1);
    }

    public override void EndAction() {
        base.EndAction();
    }
}
