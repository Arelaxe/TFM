using UnityEngine;
using UnityEngine.Events;

public class TriggereableDialogue : MonoBehaviour
{
    [SerializeField] UnityEvent dialogueRequestEvent;

    private void OnTriggerEnter2D(Collider2D other) {
        dialogueRequestEvent.Invoke();
        gameObject.SetActive(false);
    }
}
