using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : NarrationAction
{
    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField] Transform position;
    
    public override void Execute() { }

    public override void EndAction()
    {
        GameObject clone = Instantiate(objectToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
