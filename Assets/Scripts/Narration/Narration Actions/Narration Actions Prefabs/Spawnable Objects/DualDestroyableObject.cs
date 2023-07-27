using UnityEngine.Events;
using UnityEngine;

public class DualDestroyableObject : MonoBehaviour
{
    private GameObject otherObject;
    [SerializeField] private UnityEvent dialogueRequestEvent;
    private bool triggered;
    
    private void OnTriggerEnter2D(Collider2D other) {
        triggered = true;

        if (otherObject.GetComponent<DualDestroyableObject>().GetTriggered()){
            dialogueRequestEvent.Invoke();
            Destroy(otherObject.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        triggered = false;
    }

    public bool GetTriggered(){
        return triggered;
    }

    public void SetOtherObject(GameObject obj){
        otherObject = obj;
    }
}
