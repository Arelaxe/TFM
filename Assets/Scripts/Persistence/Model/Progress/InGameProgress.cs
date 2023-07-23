using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Persistence/InGameProgress")]
public class InGameProgress : ScriptableObject
{
    public bool newGame = false;
    public PlayerData player;
    public Dictionary<string, Dictionary<string, ObjectState>> scenes = new();
    public Dictionary<string, string> keyActions = new();

    public void Load(SavedProgress savedProgress)
    {
        player = savedProgress.player;
        scenes = savedProgress.scenes;
        keyActions = savedProgress.keyActions;
    }

    public void Copy(InGameProgress inGameProgress)
    {
        InGameProgress copy = Instantiate(inGameProgress);
        newGame = copy.newGame;
        player = copy.player;
        scenes = copy.scenes;
        keyActions = copy.keyActions;
    }

    public void Clear()
    {
        player = new();
        scenes = new();
        keyActions = new();
    }
}
