using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class PersistenceUtils
{
    public static string SAVE_PATH = Application.persistentDataPath + "\\save.json";

    public static void ClearSave()
    {
        File.Delete(SAVE_PATH);
    }
    public static void Save()
    {
        SceneLoadManager.Instance.SaveSceneProgress();
        SceneLoadManager.Instance.Progress.player.Save();
        string json = JsonConvert.SerializeObject(SceneLoadManager.Instance.Progress, Formatting.Indented);
        File.WriteAllText(SAVE_PATH, json);
    }

    public static SavedProgress Load()
    {
        SavedProgress savedProgress = null;
        try
        {
            string json = File.ReadAllText(SAVE_PATH);
            savedProgress = JsonConvert.DeserializeObject<SavedProgress>(json);
        }
        catch (FileNotFoundException)
        {
        }
        return savedProgress;
    }

    public static Dictionary<string, ObjectState> SaveObjects(GameObject rootObject)
    {
        Dictionary<string, ObjectState> objectStates = new();

        foreach (DynamicObject dynamicObject in rootObject.GetComponentsInChildren<DynamicObject>())
        {
            dynamicObject.State.Save(dynamicObject.gameObject);
            objectStates[dynamicObject.Guid] = dynamicObject.State;
        }

        return objectStates;
    }

    public static void LoadObjects(GameObject rootObject, Dictionary<string, ObjectState> objectStates)
    {
        if (objectStates.Count > 0)
        {
            foreach (DynamicObject dynamicObject in rootObject.GetComponentsInChildren<DynamicObject>())
            {
                string objectStateGuid = dynamicObject.Guid;
                if (objectStates.ContainsKey(objectStateGuid))
                {
                    ObjectState savedObjectState = objectStates[objectStateGuid];
                    dynamicObject.gameObject.transform.position = savedObjectState.Position;
                    dynamicObject.Load(savedObjectState);
                }
                else
                {
                    UnityEngine.Object.Destroy(dynamicObject.gameObject);
                }
            }
        }
    }

    public static T Get<T>(object obj)
    {
        T result;
        try
        {
            result = (T)obj;
        }
        catch (InvalidCastException)
        {
            result = JsonConvert.DeserializeObject<T>(obj.ToString());
        }
        return result;
    }

    public static List<T> GetList<T>(object list)
    {
        List<T> result;
        try
        {
            result = (List<T>)list;
        }
        catch (InvalidCastException)
        {
            result = JsonConvert.DeserializeObject<List<T>>(list.ToString());
        }
        return result;
    }
}
