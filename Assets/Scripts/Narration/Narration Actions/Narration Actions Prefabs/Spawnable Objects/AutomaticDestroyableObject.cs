using UnityEngine.Events;
using UnityEngine;

public class AutomaticDestroyableObject : MonoBehaviour
{
    [SerializeField]
    private UnityEvent dialogueRequestEvent;
    private void OnDestroy() {
        dialogueRequestEvent.Invoke();
    }
}
