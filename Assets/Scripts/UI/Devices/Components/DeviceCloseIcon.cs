public class DeviceCloseIcon : DeviceIcon
{
    public override void Execute()
    {
        SceneLoadManager.Instance.ReturnFromAdditiveScene();
    }
}
