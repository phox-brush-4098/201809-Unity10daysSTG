using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : FlgBehavior
{
    int cnt;
    public int aliveTime;
    public Transform viewObjTrans;
    public Collider col;
    protected Vector3 spd;
    protected static Vector3 rot;
    protected Quaternion q;

    protected override void InitMain()
    {
        cnt = 0;
        trans.localScale = Effect.v1;
        var tmpSpd = Random.Range(0f, 5f) * 0.02f;
        var tmpAng = Random.Range(-Mathf.PI, Mathf.PI);
        spd.Set(tmpSpd * Mathf.Cos(tmpAng), tmpSpd * Mathf.Sin(tmpAng), 0f);
        rot.Set(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        q = Quaternion.Euler(rot);
    }

    public void SetAliveTime(int arg)
    {
        aliveTime = arg;
    }

    protected static Vector3 effTrans = new Vector3(0f, 0f, 1f);
    Vector3 sc = Vector3.zero;

    protected override sealed void RunMain()
    {
        ++cnt;
        float f = (float)(aliveTime - cnt + 2) / aliveTime; 
        sc.Set(f, f, f);
        trans.localScale = sc;
        trans.position = trans.position + spd;

        // viewObjTrans.Rotate(rot);
        viewObjTrans.localRotation = viewObjTrans.localRotation * q;

        if (cnt % 3 == 0) { col.enabled = true; }
        else { col.enabled = false; }

        if (cnt >= aliveTime)
        {
            Disable();
            // for (int i = 0; i < 4; ++i) { EffectPool.gi.GetObject(1, trans.position + effTrans); }
        }
    }
}
