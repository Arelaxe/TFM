using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Persistence/Progress")]
public class Progress : ScriptableObject
{
    public Dictionary<string, Dictionary<string, ObjectState>> sceneRootDynamicObjects = new();
}
