using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScene : MonoBehaviour
{
    [SerializeField]
    private AudioClip musicClip;

    public AudioClip MusicClip { get => musicClip; }
}
