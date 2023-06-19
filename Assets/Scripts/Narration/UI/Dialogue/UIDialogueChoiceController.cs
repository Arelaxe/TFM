using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Choice;
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueNode m_ChoiceNextNode;
    private bool m_InventoryOption = false;

    public DialogueChoice Choice
    {
        set
        {
            m_Choice.text = value.ChoicePreview;
            m_ChoiceNextNode = value.ChoiceNode;
        }
    }

    public TextMeshProUGUI getChoice() { return m_Choice; }
    public void setInventoryOption(bool invOpt) { m_InventoryOption = invOpt; }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (m_InventoryOption){
            InventoryController invController = PlayerManager.Instance.GetInventoryController();
            invController.LoadData();
            invController.Page.ShowDialogueMode(m_DialogueChannel);
        }
        else{
            m_DialogueChannel.RaiseRequestDialogueNode(m_ChoiceNextNode);
        }
    }
}