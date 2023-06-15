using System;

[Serializable]
public class PlayerData
{
    public CharacterData selectedCharacter;
    public CharacterData unselectedCharacter;
    public bool selectedCharacterOne;
    public bool grouped;

    public void Save(string selectedCharacterScene, string unselectedCharacterScene)
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        selectedCharacterOne = dualCharacterController.SelectedCharacterOne;
        grouped = dualCharacterController.Grouped;

        selectedCharacter.Save(selectedCharacterScene, dualCharacterController.GetCharacter(true).transform.position, dualCharacterController.GetCharacterLookingAt(true));

        unselectedCharacterScene = GetUnselectedCharacterScene(selectedCharacterScene, unselectedCharacterScene);
        unselectedCharacter.Save(unselectedCharacterScene, dualCharacterController.GetCharacter(false).transform.position, dualCharacterController.GetCharacterLookingAt(false));
    }

    private string GetUnselectedCharacterScene(string selectedCharacterScene, string unselectedCharacterScene)
    {
        string result = unselectedCharacterScene;
        if (grouped || !grouped && unselectedCharacterScene == null)
        {
            result = selectedCharacterScene;
        }
        return result;
    }
}
