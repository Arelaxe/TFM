using UnityEngine.Events;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent dialogueRequestEvent;
    private void OnTriggerEnter2D(Collider2D other) {
        dialogueRequestEvent.Invoke();
        Destroy(gameObject);
    }
}
