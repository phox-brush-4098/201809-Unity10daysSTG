using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGEffectF : FlgBehavior
{
    public static Vector3 tmp = Vector3.zero;
    public static Vector3 v1 = Vector3.one;

    protected Vector3 spd;
    
    protected override void InitMain()
    {
        var pos = trans.position;
        var f = Random.Range(0, 1) == 0 ? -1f : 1f;
        pos.Set(Random.Range(-11f, 11f), Random.Range(12f, 11f) * f, 60f);
        spd.Set(0f, Random.Range(0f, 2f) * 0.02f * f, Random.Range(-10f, -20f) * 0.02f);
        trans.position = pos;
    }

    protected override void RunMain()
    {
        trans.position = trans.position + spd;

        if (trans.position.z < -20f) { Disable(); }
    }
}
