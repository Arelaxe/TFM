using UnityEngine.Events;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] UnityEvent dialogueRequestEvent;
    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
        dialogueRequestEvent.Invoke();
    }
}
