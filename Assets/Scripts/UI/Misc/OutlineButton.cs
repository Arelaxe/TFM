using UnityEngine;
using UnityEngine.UI;

public class OutlineButton : MonoBehaviour
{
    [SerializeField]
    private Image outline;

    public void Select()
    {
        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }
}
