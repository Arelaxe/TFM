using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData
{
    public CharacterData selectedCharacter = new();
    public CharacterData unselectedCharacter = new();
    public bool selectedCharacterOne;
    public bool grouped;

    public List<int> itemsOne = new();
    public List<int> itemsTwo = new();
    public List<int> documents = new();

    public void Save()
    {
        string selectedCharacterScene = SceneManager.GetActiveScene().name;

        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        selectedCharacterOne = dualCharacterController.SelectedCharacterOne;
        grouped = dualCharacterController.Grouped;

        selectedCharacter.Save(selectedCharacterScene, dualCharacterController.GetCharacter(true).transform.position, dualCharacterController.GetCharacterLookingAt(true));
        unselectedCharacter.Save(GetUnselectedCharacterScene(selectedCharacterScene), dualCharacterController.GetCharacter(false).transform.position, dualCharacterController.GetCharacterLookingAt(false));

        SaveItems(itemsOne, PlayerManager.Instance.GetInventoryController().InventoryOne.GetItems());
        SaveItems(itemsTwo, PlayerManager.Instance.GetInventoryController().InventoryTwo.GetItems());
        SaveItems(documents, PlayerManager.Instance.GetDocumentationController().Documents.GetItems());
    }

    private string GetUnselectedCharacterScene(string selectedCharacterScene)
    {
        string unselectedCharacterScene = SceneLoadManager.Instance.UnselectedScene;
        if (grouped || !grouped && unselectedCharacterScene == null)
        {
            unselectedCharacterScene = selectedCharacterScene;
        }
        return unselectedCharacterScene;
    }

    private void SaveItems(List<int> itemsIds, List<Item> items)
    {
        itemsIds.Clear();
        foreach (Item item in items)
        {
            itemsIds.Add(item.ID);
        }
    }
}
