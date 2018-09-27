using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : FlgBehavior
{
    public static Vector3 tmp = Vector3.zero;
    public static Vector3 v0 = Vector3.zero;
    public static Vector3 v1 = Vector3.one;
    public Transform viewObjTrans;

    protected Vector3 spd;
    protected static Vector3 rot;
    protected Quaternion q;

    protected override void InitMain()
    {
        trans.localScale = v1;
        // trans.Rotate(0f, 0f, Random.Range(0f, 360f));
        viewObjTrans.Rotate(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        var tmpSpd = Random.Range(6f, 19f) * 0.02f;
        var tmpAng = Random.Range(-Mathf.PI, Mathf.PI);
        spd.Set(tmpSpd * Mathf.Cos(tmpAng), tmpSpd * Mathf.Sin(tmpAng), 0f);
        rot.Set(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        q = Quaternion.Euler(rot);
    }

    public void SpdExt(float arg) { spd *= arg; }
    public void SetSmall()
    {
        var ext = trans.localScale;
        ext *= 0.5f;
        trans.localScale = ext;
        spd *= 1.5f;
    }

    public void SetSmallA()
    {
        var ext = trans.localScale;
        ext *= 0.8f;
        spd *= 1.3f;
        trans.localScale = ext;
    }


    protected override void RunMain()
    {
        trans.position = trans.position + spd;
        spd *= 0.925f;

        // viewObjTrans.Rotate(rot);
        viewObjTrans.localRotation = viewObjTrans.localRotation * q;
        // rot *= 0.985f;

        var ext = trans.localScale;
        ext *= 0.875f;
        trans.localScale = ext;
        if (ext.x < 0.05f) { Disable(); }

        var pos = trans.position;
        if (pos.x < -11f || pos.x > 11f || pos.y > 7.8f || pos.y < -8f) { Disable(); }
    }
}
