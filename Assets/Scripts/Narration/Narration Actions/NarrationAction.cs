using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationAction : MonoBehaviour
{
    [SerializeField]
    protected bool opensPopup;
    [SerializeField]
    protected bool closesPopups;
    [SerializeField]
    protected bool turnOffSwitchPageAvailability;
    [SerializeField]
    protected GameObject popupToOpen;

    public virtual void Execute(){
        if (turnOffSwitchPageAvailability){
            PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(false);
        }

        if (closesPopups){
            GameObject[] popups = GameObject.FindGameObjectsWithTag("TemporalPopups"); 

            for (int i = 0; i < popups.Length; i++){
                Destroy(popups[i]);
            }
        }
    }

    public virtual void EndAction(){
        if (opensPopup){
            GameObject popup = Instantiate(popupToOpen, new Vector3(0, 0, 0), Quaternion.identity);
            popup.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        if (turnOffSwitchPageAvailability){
            PlayerManager.Instance.GetInGameMenuController().SetSwitchPageAvailability(true);
        }
    }
}
