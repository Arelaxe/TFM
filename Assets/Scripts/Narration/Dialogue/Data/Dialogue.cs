using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    private DialogueNode m_FirstNode;
    [SerializeField]
    private bool m_LimitedOptions;
    [SerializeField]
    private int m_Options;
    [SerializeField]
    private DialogueNode m_ConfluenceNode;

    public DialogueNode FirstNode => m_FirstNode;
    public bool LimitedOptions => m_LimitedOptions;
    public int Options => m_Options;
    public DialogueNode ConfluenceNode => m_ConfluenceNode;

    public void DecreaseOptions(){
        m_Options--;
    }
}