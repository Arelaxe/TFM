using UnityEngine.InputSystem;
using UnityEngine;

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
        GameObject[] popups = GameObject.FindGameObjectsWithTag("TemporalPopups"); 
        DialogueSequencer dialogueSequencer = PlayerManager.Instance.GetDialogueInstigator().GetDialogueSequencer();

        for (int i = 0; i < popups.Length; i++){
            Destroy(popups[i]);
        }

        dialogueSequencer.EndDialogue(dialogueSequencer.GetCurrentDialogue());
        SceneLoadManager.Instance.LoadScene("IntroScene");
    }

}
