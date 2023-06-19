using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    [SerializeField]
    private string m_ChoicePreview;
    [SerializeField]
    private DialogueNode m_ChoiceNode;
    [SerializeField]
    private bool m_InitiallyAvailable;

    public string ChoicePreview => m_ChoicePreview;
    public DialogueNode ChoiceNode => m_ChoiceNode;
    public bool InitiallyAvailable => m_InitiallyAvailable;
}

[Serializable]
public class DialogueInventoryChoice
{
    [SerializeField]
    private Item m_Item;
    [SerializeField]
    private DialogueNode m_ChoiceNode;

    public Item Item => m_Item;
    public DialogueNode ChoiceNode => m_ChoiceNode;
}


[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Node/Choice")]
public class ChoiceDialogueNode : DialogueNode
{
    [SerializeField]
    private DialogueChoice[] m_Choices;
    [SerializeField]
    private DialogueInventoryChoice[] m_InventoryChoices;
    [SerializeField]
    private DialogueNode m_DefaultInventoryChoice;

    public DialogueChoice[] Choices => m_Choices;
    public DialogueInventoryChoice[] InventoryChoices => m_InventoryChoices;
    public DialogueNode DefaultInventoryChoice => m_DefaultInventoryChoice;

    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return m_Choices.Any(x => x.ChoiceNode == node);
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}