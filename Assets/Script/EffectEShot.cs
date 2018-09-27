using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEShot : Effect
{
    protected override sealed void InitMain()
    {
        trans.localScale = v1;
        trans.Rotate(0f, 0f, Random.Range(0f, 360f));
        viewObjTrans.Rotate(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        var tmpSpd = Random.Range(6f, 12f) * 0.02f;
        var tmpAng = Random.Range(-Mathf.PI, Mathf.PI);
        spd.Set(tmpSpd * Mathf.Cos(tmpAng), tmpSpd * Mathf.Sin(tmpAng), 0f);
        rot.Set(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        q = Quaternion.Euler(rot);
    }

    protected override sealed void RunMain()
    {
        trans.position = trans.position + spd;
        spd *= 0.95f;

        // viewObjTrans.Rotate(rot);
        viewObjTrans.localRotation = viewObjTrans.localRotation * q;
        // rot *= 0.985f;

        var ext = trans.localScale;
        ext *= 0.9f;
        trans.localScale = ext;
        if (ext.x < 0.05f) { Disable(); }
    }
}
