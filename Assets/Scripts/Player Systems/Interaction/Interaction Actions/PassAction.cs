using UnityEngine;

public class PassAction : Action
{
    [SerializeField]
    private string destinationScene;

    [SerializeField]
    private int destinationPassage;

    [SerializeField]
    private bool reverseLookingAt;

    public override void Execute()
    {
        SceneLoadManager.Instance.LoadScene(destinationScene, destinationPassage, reverseLookingAt);
    }
}
