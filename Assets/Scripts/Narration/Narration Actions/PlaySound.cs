using UnityEngine;

public class PlaySound : NarrationAction
{
    [SerializeField]
    private AudioClip sound;
    public override void Execute() { 
        base.Execute();
        SoundManager.Instance.PlayEffectOneShot(sound);
    }

    public override void EndAction()
    {
        base.EndAction();
        SoundManager.Instance.StopSound();
    }
}
