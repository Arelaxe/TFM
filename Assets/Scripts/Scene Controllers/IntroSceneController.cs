using UnityEngine;

public class IntroSceneController : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        PlayerManager.Instance.GetHUDController().DisableHUD();
    }
}
