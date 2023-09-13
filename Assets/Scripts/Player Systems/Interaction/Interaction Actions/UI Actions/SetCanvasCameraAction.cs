using UnityEngine;

public class SetCanvasCameraAction : Action
{
    [SerializeField]
    private Canvas canvas;

    public override void Execute()
    {
        canvas.worldCamera = Camera.main;
    }
}
