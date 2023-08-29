using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField]
    private Item redFuse;
    [SerializeField]
    private Item purpleFuse;
    [SerializeField]
    private GameObject redFuseGO;
    [SerializeField]
    private GameObject purpleFuseGO;
    private string panelInteraction;
    private Interactable panelInteractable;
    public Switch[] switches;
    public ElectricInput eI;
    public Drag[] drag;
    public Switch onBtn;
    [SerializeField]
    public AudioClip buttonSound;
    private GameObject doorToDestroy;
    private List<int> array;
    bool btnControl = true; 

    private void Awake()
    {
        
        ControlPlayerActions(false);
        if(PlayerManager.Instance.GetInventoryController().HasCharacterItem(PlayerManager.Instance.SelectedCharacterOne, redFuse)){
            redFuseGO.SetActive(true);
        }
        if(PlayerManager.Instance.GetInventoryController().HasCharacterItem(PlayerManager.Instance.SelectedCharacterOne, purpleFuse)){
            purpleFuseGO.SetActive(true);
        }
        
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
        switches[array[0]].changeColor(); //el boton que pulsamos.
    }
    public void activateDiagonal1(){
        switches[3].changeColor();
        switches[6].changeColor();
        switches[9].changeColor();
        switches[12].changeColor();
        switches[array[1]].changeColor(); //el boton que pulsamos.
    }
    public void activateDiagonal2(){
        switches[0].changeColor();
        switches[5].changeColor();
        switches[10].changeColor();
        switches[15].changeColor();
        switches[array[2]].changeColor(); //el boton que pulsamos.
    }
    public void activateCenter(){
        switches[5].changeColor();
        switches[6].changeColor();
        switches[9].changeColor();
        switches[10].changeColor();
        switches[array[3]].changeColor(); //el boton que pulsamos.
    }
    public void activateDoubleChange(){
        switches[array[4]].changeColor(); //el boton que pulsamos.
        switches[0].changeColor(); 
        switches[3].changeColor(); 
    }
    public void activateDoubleChange2(){
        switches[array[5]].changeColor(); //el boton que pulsamos.
        switches[12].changeColor(); 
        switches[15].changeColor(); 
    }
    public void activateDoubleChange3(){
        switches[array[6]].changeColor(); //el boton que pulsamos.
        switches[6].changeColor(); 
        switches[10].changeColor(); 
    }
        public void activateDoubleChange4(){
        switches[array[7]].changeColor(); //el boton que pulsamos.
        switches[5].changeColor(); 
        switches[9].changeColor(); 
    }

    public void endBtn(){
        if(onBtn.getActive()){
            onBtn.endPanel(onBtn.getActive());
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        SoundManager.Instance.PlayEffectOneShot(buttonSound);
        yield return new WaitForSeconds(1);

        Interaction unlock = panelInteractable.GetInteraction(panelInteraction);
        if (unlock != null)
        {
            unlock.SetAvailable(false);
        }
        Destroy(doorToDestroy);
        Close();
    }

    public void Close()
    {
        ControlPlayerActions(true);
        PlayerManager.Instance.GetInGameMenuController().DestroyAdditionalUI();
    }

    public void Init(string tagFuse, string panelInteraction, Interactable panelInteractable, List<int> array, Sprite onFuseImage, GameObject doorToDestroy)
    {
        eI.tag = tagFuse;
        eI.setOn(onFuseImage);
        this.panelInteraction = panelInteraction;
        this.panelInteractable = panelInteractable;
        this.doorToDestroy = doorToDestroy;
        this.array = array;
        
        for (int i = 0; i < drag.Length; i++)
        {
            drag[i].setCanvas(transform.parent.transform.parent.GetComponent<Canvas>());
        }

        for (int i = 0; i < switches.Length; i++)
        {
            if(i == array[0]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateCorners);
            }
            else if(i == array[1]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateDiagonal1);
            }
            else if(i == array[2]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateDiagonal2);
            }
            else if(i == array[3]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateCenter);
            }
            else if(i == array[4]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateDoubleChange);
            }
            else if(i == array[5]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateDoubleChange2);
            }
            else if(i == array[6]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateDoubleChange3);
            }
            else if(i == array[7]){
                switches[i].GetComponent<Button>().onClick.AddListener(activateDoubleChange4);
            }
            else{
                switches[i].GetComponent<Button>().onClick.AddListener(switches[i].changeColor);
            }
        }
    }
}
