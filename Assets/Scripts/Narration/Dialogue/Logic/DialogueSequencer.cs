using UnityEngine;

public class DialogueException : System.Exception
{
    public DialogueException(string message)
        : base(message)
    {
    }
}

public class DialogueSequencer
{
    public delegate void DialogueCallback(Dialogue dialogue);
    public delegate void DialogueNodeCallback(DialogueNode node);

    public DialogueCallback OnDialogueStart;
    public DialogueCallback OnDialogueRequested;
    public DialogueCallback OnDialogueEnd;
    public DialogueNodeCallback OnDialogueNodeStart;
    public DialogueNodeCallback OnDialogueNodeEnd;

    private Dialogue m_CurrentDialogue;
    private DialogueNode m_CurrentNode;

    public void StartDialogue(Dialogue dialogue)
    {
        if (m_CurrentDialogue == null)
        {
            m_CurrentDialogue = dialogue;
            OnDialogueStart?.Invoke(m_CurrentDialogue);
            StartDialogueNode(dialogue.FirstNode);
        }
        else
        {   
            throw new DialogueException("Can't start a dialogue when another is already running.");
        }
    }

    public void EndDialogue(Dialogue dialogue)
    {
        if (m_CurrentDialogue == dialogue)
        {
            StopDialogueNode(m_CurrentNode);
            OnDialogueEnd?.Invoke(m_CurrentDialogue);
            m_CurrentDialogue = null;
        }
        else
        {
            throw new DialogueException("Trying to stop a dialogue that ins't running.");
        }
    }

    private bool CanStartNode(DialogueNode node)
    {
        return true;
        //return (m_CurrentNode == null || node == null || m_CurrentNode.CanBeFollowedByNode(node) || );
    }

    public void StartDialogueNode(DialogueNode node)
    {   
        if (CanStartNode(node))
        {
            if (m_CurrentNode != null){
                StopDialogueNode(m_CurrentNode);
            }

            m_CurrentNode = node;

            if (m_CurrentNode != null)
            {
                if (m_CurrentDialogue.LimitedOptions && m_CurrentDialogue.Options == 0){
                    m_CurrentNode = m_CurrentDialogue.ConfluenceNode;
                    m_CurrentDialogue.DecreaseOptions();
                }
                OnDialogueNodeStart?.Invoke(m_CurrentNode);
            }
            else
            {
                EndDialogue(m_CurrentDialogue);
            }
        }
        else
        {
            throw new DialogueException("Failed to start dialogue node.");
        }
    }

    private void StopDialogueNode(DialogueNode node)
    {
        if (m_CurrentNode == node)
        {
            if (m_CurrentNode != null){
                if (m_CurrentNode.EndsChoiceOption){
                    m_CurrentDialogue.DecreaseOptions();
                }
            }

            OnDialogueNodeEnd?.Invoke(m_CurrentNode);
            m_CurrentNode = null;
        }
        else
        {
            throw new DialogueException("Trying to stop a dialogue node that ins't running.");
        }
    }

    public Dialogue GetCurrentDialogue(){
        return m_CurrentDialogue;
    }
}