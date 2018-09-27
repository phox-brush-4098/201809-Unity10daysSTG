using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlBullet : FlgBehavior
{
    public bool nextDead;

    int cnt, value;

    protected override void InitMain()
    {
        nextDead = false;
        cnt = 0;
        value = 7;
    }

    protected override void RunMain()
    {
        trans.Translate(0f, 36f * 0.02f, 0f);
        if (nextDead) { Disable(); return; }
        if (trans.position.y > 8.5f) { Disable(); return; }

        ++cnt;
        if (cnt % 2 == 0 && value > 0) { --value; }
    }

    static Vector3 effPosAdd = new Vector3(0f, 0.7f, 1f);

    private void OnTriggerEnter(Collider other)
    {
        value += 1;
        nextDead = true;
        Player.AddCharge(value);
        for (int i = 0, n = value / 3 + 1; i < n; ++i) { EffectPool.gi.GetObject(0, trans.position + effPosAdd); }

        ScoreManager.AddScore(value);
        CubeManager.AddScore(value);

        Player.fieldTemp -= value / 80f;
        Player.plTempe -= value / 240f;

        SoundList.gi.list[3].transform.position = trans.position;
        SoundList.gi.list[3].Play();
    }
}
