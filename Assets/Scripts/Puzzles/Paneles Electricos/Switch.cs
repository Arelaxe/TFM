using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    public Boolean active;
    private Button btn;
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

    public void endPanel(Boolean allActive){
        if (allActive){
            Debug.Log("Panel Completado");
        }
    }
}
