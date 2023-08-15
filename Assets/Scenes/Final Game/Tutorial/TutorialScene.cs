using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScene : ScriptedScene
{
    private PlayerInput input;
    private InputAction skipAction;

    void Start(){
        input = PlayerManager.Instance.GetDualCharacterController().GetComponent<PlayerInput>();
        skipAction = input.actions[PlayerConstants.ActionSkip];
        skipAction.performed += SkipTutorial;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void SkipTutorial(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene"){
            GameObject[] popups = GameObject.FindGameObjectsWithTag("TemporalPopups"); 
            DialogueSequencer dialogueSequencer = PlayerManager.Instance.GetDialogueInstigator().GetDialogueSequencer();

            for (int i = 0; i < popups.Length; i++){
                Destroy(popups[i]);
            }

            if (dialogueSequencer.GetCurrentDialogue() != null){
                dialogueSequencer.EndDialogue(dialogueSequencer.GetCurrentDialogue());
            }

            if (!PlayerManager.Instance.GetDualCharacterController().Grouped){
                PlayerManager.Instance.GetDualCharacterController().SwitchGrouping();
            }

            SceneLoadManager.Instance.LoadScene("IntroScene");
        }
    }

}
