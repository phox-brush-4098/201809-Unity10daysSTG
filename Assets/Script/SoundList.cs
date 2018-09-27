using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    public List<AudioSource> list;

    public static SoundList gi { get; private set; }

    private void Awake()
    {
        gi = this;
    }

    private void OnDestroy()
    {
        gi = null;
    }
}
