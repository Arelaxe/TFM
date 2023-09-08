using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Linq;

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
    [SerializeField] GameObject pauseDialogue;

    [SerializeField] private int soundRatio;
    [SerializeField] Item michelangeloDocument;

    private bool m_ListenToInput = false;
    private DialogueNode m_NextNode = null;
    private PlayerInput input;
    private InputAction interactAction;

    private bool dialogEnded = false;
    private bool choiceDialog = false;
    private string currentText;

    private void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        input = PlayerManager.Instance.GetDualCharacterController().GetComponent<PlayerInput>();
        interactAction = input.actions[PlayerConstants.ActionInteract];
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    private void Update()
    {
        if (m_ListenToInput && interactAction.triggered && !pauseDialogue.activeSelf)
        {
            if (dialogEnded){
                if (!choiceDialog){
                    m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
                }
            }
            else{
                dialogEnded = true;
                m_DialogueText.text = currentText;
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
        int charIndex = 0;

        foreach(char c in line){
            if (!dialogEnded){
                m_DialogueText.text += c;
                charIndex++;

                if (charIndex % soundRatio == 0){
                    SoundManager.Instance.PlayEffectOneShot(m_DialogueChannel.currentNode.DialogueLine.Speaker.Voice);
                }
                
                yield return new WaitForSecondsRealtime(uiParams.DialogSpeed);
            }
        }

        dialogEnded = true;
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        if (node != null){
            if (node.GetType().ToString() == "ActionDialogueNode"){
                ActionDialogueNode copyNode = (ActionDialogueNode) node;
                copyNode.m_Action.GetComponent<NarrationAction>().EndAction();
            }
        }

        m_NextNode = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxTransform)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    public void Visit(ActionDialogueNode node)
    {
        choiceDialog = false;
        m_ListenToInput = true;
        m_NextNode = node.NextNode;
    }

    public void Visit(BasicDialogueNode node)
    {
        choiceDialog = false;
        m_ListenToInput = true;
        m_NextNode = node.NextNode;
    }

    public void Visit(ChoiceDialogueNode node)
    {
        choiceDialog = true;
        m_ListenToInput = true;
        m_ChoicesBoxTransform.gameObject.SetActive(true);

        foreach (DialogueChoice choice in node.Choices)
        {
            if (choice.InitiallyAvailable || 
                (choice.ChoicePreview == "Preguntar por el sótano 2" && SceneLoadManager.Instance.GetKeyAction(KeyActions.TalkedToNPC) == "completed")
                || choice.ChoicePreview == "Preguntar sobre el proyecto Michelangelo" && PlayerManager.Instance.GetDocumentationController().Documents.GetItems().Contains(michelangeloDocument)){
                UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
                newChoice.Choice = choice;
            }
        }

        if (node.InventoryChoices.Length > 0){
            UIDialogueChoiceController opt = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
            opt.getChoice().text = "Abrir Inventario";
            opt.setInventoryOption(true);
        }
    }
}