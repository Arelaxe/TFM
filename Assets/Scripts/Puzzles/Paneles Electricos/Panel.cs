using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    private string code;
    private string panelInteraction;
    private string lockedInteraction;
    private Interactable panelInteractable;
    private Interactable lockedInteractable;
    public Switch[] switches;
    public ElectricInput eI;
    public Switch onBtn;
    [SerializeField]
    public AudioClip buttonSound;
    [SerializeField]
    public AudioClip switchSound;
    bool btnControl = true; 

        private void Awake()
    {
        ControlPlayerActions(false);
    }
    void Update()
    {
        bool allActive = true;
        for (int i = 0; i < switches.Length; i++){
            if(switches[i].getActive() == false){
                allActive = false;
            }
        }
        if(allActive && btnControl && eI.active){
            btnControl = false;
            onBtn.changeColor();
        }
        if(!allActive && !btnControl){
            btnControl = true;
            onBtn.changeColor();
        }
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
    public void activateCorners(){
        switches[0].changeColor();
        switches[3].changeColor();
        switches[12].changeColor();
        switches[15].changeColor();
        switches[1].changeColor(); //el boton que pulsamos.
    }
    public void activateDiagonal1(){
        switches[3].changeColor();
        switches[6].changeColor();
        switches[9].changeColor();
        switches[12].changeColor();
        switches[2].changeColor(); //el boton que pulsamos.
    }
    public void activateDiagonal2(){
        switches[0].changeColor();
        switches[5].changeColor();
        switches[10].changeColor();
        switches[15].changeColor();
        switches[4].changeColor(); //el boton que pulsamos.
    }
    public void activateCenter(){
        switches[5].changeColor();
        switches[6].changeColor();
        switches[9].changeColor();
        switches[10].changeColor();
        switches[14].changeColor(); //el boton que pulsamos.
    }
    public void activateDoubleChange(){
        switches[6].changeColor(); //el boton que pulsamos.
        switches[9].changeColor(); //el boton que pulsamos.
    }
    public void activateDoubleChange2(){
        switches[5].changeColor(); //el boton que pulsamos.
        switches[10].changeColor(); //el boton que pulsamos.
    }

    public void endBtn(){
        if(onBtn.getActive()){
            onBtn.endPanel(onBtn.getActive());
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1);

        Interaction unlock = panelInteractable.GetInteraction(panelInteraction);
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
}
