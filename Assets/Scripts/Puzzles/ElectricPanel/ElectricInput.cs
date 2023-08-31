using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricInput : MonoBehaviour
{
    private Sprite on;
    public Sprite off;
    public bool active;
    private Image img;
    private ElectricInput eI;
    [SerializeField]
    public AudioClip pieceSound;
    void Start()
    {
        eI = GetComponent<ElectricInput>();
        img = GetComponent<Image>();
        active = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Entra el Collider");
        if (other.gameObject.tag == eI.gameObject.tag){
            SoundManager.Instance.PlayEffectOneShot(pieceSound);
            Destroy(other.gameObject);
            img.sprite = on; 
            active = true;
        }  
    }

    public void setOn (Sprite onImage){
        on = onImage;
    }
}
