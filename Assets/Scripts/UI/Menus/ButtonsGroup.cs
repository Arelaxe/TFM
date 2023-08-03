using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonsGroup : MonoBehaviour
{
    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            SoundManager.Instance.AddSounds(button);
        }

    }
}
