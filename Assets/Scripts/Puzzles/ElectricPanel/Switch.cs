using System;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    public Boolean active;
    private Button btn;

    [SerializeField]
    private AudioClip press;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        if (!active){
            btn.image.sprite = off;
        } else {
            btn.image.sprite = on;   
        }
    }
    // Update is called once per frame
     public void changeColor(){
        if (press)
        {
            SoundManager.Instance.PlayEffectOneShot(press);
        }
        if (active){
            btn.image.sprite = off;
            active = false;
        } else {
            btn.image.sprite = on;
            active = true;
        }
    }
    public Boolean getActive(){
        return active;
    }
}
