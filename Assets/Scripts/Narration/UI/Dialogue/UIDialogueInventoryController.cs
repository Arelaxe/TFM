using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDialogueInventoryController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Text;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("hey");
    }
}
