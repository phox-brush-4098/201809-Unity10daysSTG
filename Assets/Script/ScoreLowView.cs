using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLowView : MonoBehaviour
{
    Transform trans;

    public float div;
    public float mod;

    private void Awake()
    {
        trans = transform;
    }

    public void UpdateView(float arg)
    {
        var tmp =  trans.localScale;
        tmp.x = (arg % mod) / (float)(mod);
        trans.localScale = tmp;
    }
}
