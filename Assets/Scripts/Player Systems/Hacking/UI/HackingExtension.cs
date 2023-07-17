using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HackingExtension : MonoBehaviour
{
    [SerializeField]
    private Image item;
    [SerializeField]
    private TextMeshProUGUI document;

    public void SetItem(Image icon)
    {
        item.enabled = true;
        item.sprite = icon.sprite;
    }

    public void SetDocument(string title)
    {
        document.text = title;
    }

    public void ResetData()
    {
        item.sprite = null;
        item.enabled = false;
        document.text = "";
    }

    public void OpenMiniGame()
    {
        SceneManager.LoadScene("HackingMinigame", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
    }
}
