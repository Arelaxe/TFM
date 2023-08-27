using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Numpad : MonoBehaviour
{
    private string code;
    private string numpadInteraction;
    private string lockedInteraction;
    private Interactable numpadInteractable;
    private Interactable lockedInteractable;

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

    private bool startText = true;

    private void Awake()
    {
        ControlPlayerActions(false);
        btn1.GetComponent<Button>().Select();
    }

    private void ControlPlayerActions(bool close)
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetCharacterMobility(true, close);
        dualCharacterController.SetSwitchAvailability(close);

        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        interactionController.SetInteractivity(close);
        interactionController.DestroyInteractions();

        PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(close);
    }

    public void B1 (){
        WriteCharHolder("1");
    }
    public void B2 (){
        WriteCharHolder("2");
    }
    public void B3 (){
        WriteCharHolder("3");
    }
    public void B4 (){
        WriteCharHolder("4");
    }
    public void B5 (){
        WriteCharHolder("5");
    }
    public void B6 (){
        WriteCharHolder("6");
    }
    public void B7 (){
        WriteCharHolder("7");
    }
    public void B8 (){
        WriteCharHolder("8");
    }
    public void B9 (){
        WriteCharHolder("9");
    }
    public void B0 (){
        WriteCharHolder("0");
    }
    public void BClear (){
        charHolder.text = "INSERTA PIN";
    }
    public void BEnter (){
        if(charHolder.text == code)
        {
            charHolder.text = "ACTIVADO";
            StartCoroutine(Unlock());
        }
        else
        {
            charHolder.text = "PIN ERRONEO";
        }
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1);

        Interaction unlock = numpadInteractable.GetInteraction(numpadInteraction);
        if (unlock != null)
        {
            unlock.SetAvailable(false);
        }

        Interaction locked = lockedInteractable.GetInteraction(lockedInteraction);
        if (locked != null)
        {
            locked.SetBlocked(false);
        }

        Close();
    }

    public void Close()
    {
        ControlPlayerActions(true);
        PlayerManager.Instance.GetInGameMenuController().DestroyAdditionalUI();
    }

    private void WriteCharHolder(string value){
        if (startText)
        {
            charHolder.text = "";
            startText = false;
        }
        if (charHolder.text.Length < 4)
        {
            charHolder.text += value;
        }
        else if (charHolder.text.Length >= 4)
        {
            charHolder.text = "";
            charHolder.text += value;
        }
    }

    public void Init(string code, string numpadInteraction, Interactable numpadInteractable, string lockedInteraction, Interactable lockedInteractable)
    {
        this.code = code;
        this.numpadInteraction = numpadInteraction;
        this.numpadInteractable = numpadInteractable;
        this.lockedInteraction = lockedInteraction;
        this.lockedInteractable = lockedInteractable;
    }

}
