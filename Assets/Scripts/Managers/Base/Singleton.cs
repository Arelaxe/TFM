using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new();
                    obj.name = typeof(T).Name;
                    obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        LoadData();
    }

    protected abstract void LoadData();

}