using System.Collections.Generic;
using System;

[Serializable]
public class SavedProgress
{
    public PlayerData player;
    public Dictionary<string, Dictionary<string, ObjectState>> scenes;
}
