using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Persistence/InGameProgress")]
public class InGameProgress : ScriptableObject
{
    public PlayerData player;
    public Dictionary<string, Dictionary<string, ObjectState>> scenes = new();

    public void Load(SavedProgress savedProgress)
    {
        player = savedProgress.player;
        scenes = savedProgress.scenes;
    }

    public void Clear()
    {
        player = new();
        scenes = new();
    }
}
