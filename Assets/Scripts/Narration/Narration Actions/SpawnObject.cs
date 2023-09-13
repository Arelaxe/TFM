using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : NarrationAction
{
    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField] 
    private int positionX;
    [SerializeField] 
    private int positionY;
    [SerializeField] 
    private int positionZ;
    
    public override void Execute() { 
        base.Execute();
    }

    public override void EndAction() { 
        base.EndAction();
        GameObject clone = Instantiate(objectToSpawn, new Vector3(positionX, positionY, positionZ), Quaternion.identity);
    }
}
