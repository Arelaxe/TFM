using System.Collections;
using UnityEngine;
using TMPro;

public class TalkAction : Action
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField, TextArea(4,6)] private string[] dialogLines;
    [SerializeField] UIParams uiParams;
    [SerializeField] GameObject player;
    private bool didDialogStart;
    private int lineIndex;
    public override void Execute()
    {
        if (!didDialogStart){
            StartDialog();
        }
        else if (dialogText.text == dialogLines[lineIndex]){
            NextDialogLine();
        }
        else{
            StopAllCoroutines();
            dialogText.text = dialogLines[lineIndex];
        }
    }

    private void StartDialog(){
        didDialogStart = true;
        dialogPanel.SetActive(true);
        lineIndex = 0;
        StartCoroutine(ShowLine());
        //player
    }

    private void NextDialogLine(){
        lineIndex ++;

        if (lineIndex < dialogLines.Length){
            StartCoroutine(ShowLine());
        }
        else{
            didDialogStart = false;
            dialogPanel.SetActive(false);
        }
    }

    private IEnumerator ShowLine()
    {
        dialogText.text = string.Empty;

        foreach(char c in dialogLines[lineIndex]){
            dialogText.text += c;
            yield return new WaitForSecondsRealtime(uiParams.DialogSpeed);
        }
    }
}