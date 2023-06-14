using System.Collections.Generic;
using UnityEngine;

public class ObjectStateUtils : MonoBehaviour
{
    public static Dictionary<string, ObjectState> SaveObjects(GameObject rootObject)
    {
        Dictionary<string, ObjectState> objectStates = new();

        foreach (DynamicObject dynamicObject in rootObject.GetComponentsInChildren<DynamicObject>())
        {
            dynamicObject.objectState.Save(dynamicObject.gameObject);
            objectStates[dynamicObject.objectState.guid] = dynamicObject.objectState;
        }

        return objectStates;
    }

    public static void LoadObjects(GameObject rootObject, Dictionary<string, ObjectState> objectStates)
    {
        if (objectStates.Count > 0)
        {
            foreach (DynamicObject dynamicObject in rootObject.GetComponentsInChildren<DynamicObject>())
            {
                string objectStateGuid = dynamicObject.objectState.guid;
                if (objectStates.ContainsKey(objectStateGuid))
                {
                    ObjectState savedObjectState = objectStates[objectStateGuid];
                    dynamicObject.gameObject.transform.position = savedObjectState.objectPosition;
                    dynamicObject.Load(savedObjectState);
                }
                else
                {
                    Destroy(dynamicObject.gameObject);
                }
            }
        }
    }
}
