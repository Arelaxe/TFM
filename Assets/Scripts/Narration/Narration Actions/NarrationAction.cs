using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NarrationAction : MonoBehaviour
{
    public abstract void Execute();

    public abstract void EndAction();
}
