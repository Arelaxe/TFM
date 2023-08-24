using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField]
    private string m_CharacterName;
    [SerializeField]
    private Color m_Color;
    [SerializeField]
    private AudioClip m_Voice;

    public string CharacterName => m_CharacterName;
    public Color Color => m_Color;
    public AudioClip Voice => m_Voice;
}