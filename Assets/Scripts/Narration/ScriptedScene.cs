using UnityEngine.Events;
using UnityEngine;

public class ScriptedScene : MonoBehaviour
{
    [SerializeField] UnityEvent dialogueRequestEvent;
    
    private bool init = false;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!init){
            if (!SceneLoadManager.Instance.Loading){
                dialogueRequestEvent.Invoke();
                init = true;
            }
        }
        
    }
}
