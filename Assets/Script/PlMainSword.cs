using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlMainSword : FlgBehavior
{
    // static Vector3 addPos = new Vector3(0f, -0.75f, 0f);
    Vector3 spd = Vector3.one;
    float spdAddValue;
    float scExt;
    float rot;
    int cnt;

    bool currentFrSE;

    static Vector3 defExt = new Vector3(0.2f, 0.2f, 0.2f);

    protected override void InitMain()
    {
        cnt = 0;
        trans.localScale = defExt;
        spd.Set(0f, -15f * 0.02f, 0f);
        spdAddValue = 6.5f * 0.02f;
        scExt = 1f;
    }

    public void SetRotValue(float arg) { rot = arg; }

    protected override void RunMain()
    {
        currentFrSE = true;

       var sc = trans.localScale;
        if (cnt < 8)
        {
            sc *= (1.4f - cnt * 0.05f);
        }
        else
        {
            sc *= scExt;
            scExt -= cnt * 0.000008f;
            if (sc.x < 0.05f) { gameObject.SetActive(false); return; }
            else if (sc.x < 0.2f) { scExt *= 0.85f; }
            else if (sc.x < 0.4f) { scExt *= 0.9f; }
            else if (sc.x < 0.6f) { scExt *= 0.975f; }            
        }
        ++cnt;
        trans.localScale = sc;

        trans.Rotate(0f, 0f, rot);

        float f = -cnt * 0.005f;

        trans.position = trans.position + spd;
        sc.Set(0f, spdAddValue, 0f);
        spd += sc;
        spdAddValue *= 0.95f + f;
        spd *= 0.9125f + f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player.AddCharge(4);
        Player.fieldTemp -= 4 / 100f;
        Player.plTempe -= 4 / 50f;
        CubeManager.AddScore(4);
        for (int i = 0; i < 2; ++i) { EffectPool.gi.GetObject(0, other.transform.position); }

        if (currentFrSE)
        {
            SoundList.gi.list[3].transform.position = trans.position;
            SoundList.gi.list[3].Play();
            currentFrSE = false;
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        // corpseが相手だと点数が低くなる
        if (other.gameObject.layer == 14)
        {
            Player.AddCharge(6);
            CubeManager.AddScore(6);
            Player.fieldTemp -= 6 / 520f;
            Player.plTempe -= 6 / 250f;
            /*for (int i = 0; i < 1; ++i)*/
            if (Random.Range(0, 160) == 0) { EffectPool.gi.GetObject(0, other.transform.position).SetSmall(); }
        }
        else
        {
            CubeManager.AddScore(5);
            Player.AddCharge(5);
            Player.fieldTemp -= 5 / 180f;
            Player.plTempe -= 5 / 90f;
            if (Random.Range(0, 16) == 0) { EffectPool.gi.GetObject(0, other.transform.position).SetSmallA(); }
        }

        if (currentFrSE)
        {
            SoundList.gi.list[3].transform.position = trans.position;
            SoundList.gi.list[3].Play();
            currentFrSE = false;
        }
    }
}
