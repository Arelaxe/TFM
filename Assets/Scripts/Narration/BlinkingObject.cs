using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    private bool canBlink;

    // Start is called before the first frame update
    void Start()
    {
        canBlink = true;
        PlayerManager.Instance.StartCoroutine(Blink(1.0f));
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    IEnumerator Blink( float delayBetweenBlinks)
    {
        while(canBlink){
            gameObject.SetActive(!gameObject.activeSelf);
            yield return new WaitForSeconds( delayBetweenBlinks );
        }
    }

    private void OnDestroy(){
        canBlink = false;
        PlayerManager.Instance.StopCoroutine("Blink");
    }
}
