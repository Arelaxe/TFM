using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyPad : MonoBehaviour
{
    public TMP_InputField charHolder;
    public GameObject btn1;
    public GameObject btn2;
    public GameObject btn3;
    public GameObject btn4;
    public GameObject btn5;
    public GameObject btn6;
    public GameObject btn7;
    public GameObject btn8;
    public GameObject btn9;
    public GameObject btn0;
    public GameObject btnClr;
    public GameObject btnEnt;

    public bool startText = true;

    public void b1 (){
        writeCharHolder("1");
    }
    public void b2 (){
        writeCharHolder("2");
    }
    public void b3 (){
        writeCharHolder("3");
    }
    public void b4 (){
        writeCharHolder("4");
    }
    public void b5 (){
        writeCharHolder("5");
    }
    public void b6 (){
        writeCharHolder("6");
    }
    public void b7 (){
        writeCharHolder("7");
    }
    public void b8 (){
        writeCharHolder("8");
    }
    public void b9 (){
        writeCharHolder("9");
    }
    public void b0 (){
        writeCharHolder("0");
    }
    public void bClr (){
        charHolder.text = "INSERTA PIN";
    }
    public void bEntr (){
        if(charHolder.text == "1234"){
           charHolder.text = "ACTIVADO";
        }
        else {
             charHolder.text = "PIN ERRONEO";
        }
    }

    void writeCharHolder(string value){
        if (startText){
            charHolder.text = "";
            startText = false;
        }
        if (charHolder.text.Length < 4){
            charHolder.text = charHolder.text + value;
        }else if (charHolder.text.Length >= 4){
            charHolder.text = "";
            charHolder.text = charHolder.text + value;
        }
    }
}
