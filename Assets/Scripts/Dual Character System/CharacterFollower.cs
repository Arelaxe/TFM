using UnityEngine;

public class CharacterFollower : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        GameObject selectedCharacter = PlayerManager.Instance.GetDualCharacterController().GetSelectedCharacter();
        rectTransform.position = new Vector3(selectedCharacter.transform.position.x, selectedCharacter.transform.position.y, rectTransform.position.z);
    }
}
