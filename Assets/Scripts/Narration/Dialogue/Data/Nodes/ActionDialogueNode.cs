using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Node/Action")]
public class ActionDialogueNode : DialogueNode
{
    [SerializeField]
    private DialogueNode m_NextNode;
    public GameObject m_Action;
    public DialogueNode NextNode => m_NextNode;


    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return m_NextNode == node;
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        m_Action.GetComponent<NarrationAction>().Execute();
        visitor.Visit(this);
    }
}
