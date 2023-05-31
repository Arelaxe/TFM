using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
    [SerializeField]
    private TextMeshProUGUI m_SpeakerText;
    [SerializeField]
    private TextMeshProUGUI m_DialogueText;

    [SerializeField]
    private RectTransform m_ChoicesBoxTransform;
    [SerializeField]
    private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    [SerializeField] UIParams uiParams;

    private bool m_ListenToInput = false;
    private DialogueNode m_NextNode = null;

    private bool dialogEnded = false;
    private string currentText;

    private void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        gameObject.SetActive(false);
        //m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    private void Update()
    {
        if (m_ListenToInput && Input.GetButtonDown("Submit"))
        {
            if (dialogEnded){
                m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
            }
            else{
                dialogEnded = true;
                m_DialogueText.text =  currentText;
            }
        }
    }

    private void OnDialogueNodeStart(DialogueNode node)
    {
        gameObject.SetActive(true);
        currentText = node.DialogueLine.Text;
        StartCoroutine(ShowLine(currentText));
        m_SpeakerText.text = node.DialogueLine.Speaker.CharacterName;
        m_SpeakerText.color = node.DialogueLine.Speaker.Color;
        node.Accept(this);
    }

    private IEnumerator ShowLine(string line)
    {
        dialogEnded = false;
        m_DialogueText.text= string.Empty;

        foreach(char c in line){
            if (!dialogEnded){
                m_DialogueText.text += c;
                yield return new WaitForSecondsRealtime(uiParams.DialogSpeed);
            }
        }

        dialogEnded = true;
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        m_NextNode = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";
/*
        foreach (Transform child in m_ChoicesBoxTransform)
        {
            Destroy(child.gameObject);
        }*/

        gameObject.SetActive(false);
        //m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    public void Visit(BasicDialogueNode node)
    {
        m_ListenToInput = true;
        m_NextNode = node.NextNode;
    }

    public void Visit(ChoiceDialogueNode node)
    {
        //m_ChoicesBoxTransform.gameObject.SetActive(true);

        foreach (DialogueChoice choice in node.Choices)
        {
           // UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
           // newChoice.Choice = choice;
        }
    }
}