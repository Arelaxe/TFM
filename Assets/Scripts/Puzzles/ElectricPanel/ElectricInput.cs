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

    [SerializeField]
    public AudioClip pieceSound;

    [SerializeField]
    public Button firstSwitch;

    void Start()
    {
        img = GetComponent<Image>();
        active = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        TrySetFuse(other.gameObject);
    }

    public void setOn (Sprite onImage){
        on = onImage;
    }

    public void TrySetFuse(GameObject fuse)
    {
        if (fuse.CompareTag(tag))
        {
            SoundManager.Instance.PlayEffectOneShot(pieceSound);
            Destroy(fuse);
            img.sprite = on;
            active = true;
            firstSwitch.Select();
        }
    }
}
