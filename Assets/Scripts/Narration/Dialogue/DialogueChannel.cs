using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Dialogue Channel")]
public class DialogueChannel : ScriptableObject
{
    public delegate void DialogueCallback(Dialogue dialogue);
    public DialogueCallback OnDialogueRequested;
    public DialogueCallback OnDialogueStart;
    public DialogueCallback OnDialogueEnd;

    public delegate void DialogueNodeCallback(DialogueNode node);
    public DialogueNodeCallback OnDialogueNodeRequested;
    public DialogueNodeCallback OnDialogueNodeStart;
    public DialogueNodeCallback OnDialogueNodeEnd;
    public DialogueNode currentNode;

    public void RaiseRequestDialogue(Dialogue dialogue)
    {
        currentNode = dialogue.FirstNode;
        OnDialogueRequested?.Invoke(dialogue);
    }

    public void RaiseDialogueStart(Dialogue dialogue)
    {
        currentNode = dialogue.FirstNode;
        OnDialogueStart?.Invoke(dialogue);
    }

    public void RaiseDialogueEnd(Dialogue dialogue)
    {
        currentNode = dialogue.FirstNode;
        OnDialogueEnd?.Invoke(dialogue);
    }

    public void RaiseRequestDialogueNode(DialogueNode node)
    {
        currentNode = node;
        OnDialogueNodeRequested?.Invoke(node);
    }

    public void RaiseDialogueNodeStart(DialogueNode node)
    {
        currentNode = node;
        OnDialogueNodeStart?.Invoke(node);
    }

    public void RaiseDialogueNodeEnd(DialogueNode node)
    {
        currentNode = node;
        OnDialogueNodeEnd?.Invoke(node);
    }
}