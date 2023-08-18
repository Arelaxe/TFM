using UnityEngine;

public abstract class DialogueNode : ScriptableObject
{
    [SerializeField]
    private NarrationLine m_DialogueLine;
    [SerializeField]
    private bool m_EndsChoiceOption;

    public NarrationLine DialogueLine => m_DialogueLine;
    public bool EndsChoiceOption => m_EndsChoiceOption;

    public abstract bool CanBeFollowedByNode(DialogueNode node);
    public abstract void Accept(DialogueNodeVisitor visitor);
}