using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class UIParams : ScriptableObject
{
    [Header("DialogSpeed")]
    [SerializeField]
    private float dialogSpeed;

    public float DialogSpeed { get => dialogSpeed; }
}
