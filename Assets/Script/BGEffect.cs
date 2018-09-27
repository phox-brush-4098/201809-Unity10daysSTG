using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGEffect : FlgBehavior
{
    public static Vector3 tmp = Vector3.zero;
    public static Vector3 v1 = Vector3.one;

    protected Vector3 spd;
    protected Vector3 rot;

    // int cnt = 0;

    protected override void InitMain()
    {
        var pos = trans.position;
        pos.Set(Random.Range(-11f, 11f), 18f, Random.Range(4f, 15f));
        spd.Set(Random.Range(-6f, 6f) * 0.02f, Random.Range(-6f, -18f) * 0.02f, Random.Range(-0.25f, 2f) * 0.02f);
        trans.position = pos;
        rot.Set(Random.Range(-11f, 11f), Random.Range(-11f, 11f), Random.Range(-11f, 11f));
    }

    protected override void RunMain()
    {
        trans.position = trans.position + spd;
        trans.Rotate(rot);

        if (trans.position.y < -18f) { Disable(); }
    }
}
