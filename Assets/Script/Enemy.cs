using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : FlgBehavior
{
    public List<EnemyScript> es = new List<EnemyScript>(4);
    public GameObject view;

    protected int modelIndex;

    protected static Quaternion zeroQ = Quaternion.identity;

    protected override void InitMain()
    {
        trans.localRotation = zeroQ;
    }

    public void AddScript(EnemyScript arg)
    {
        es.Add(arg);
        arg.trans = trans;
    }

    //public void SetModel(int index)
    //{
    //    //modelIndex = index;
    //    //view = EnemyManager.gi.GetModel(index, trans.position);
    //    //view.transform.parent = trans;
    //}
    
    protected override void RunMain()
    {
        for (int i = 0; i < es.Count; ++i) { es[i].Run(); }

        var pos = trans.position;
        if (pos.x < -11f || pos.x > 11f || pos.y > 7.8f || pos.y < -8f) { Disable(); }
    }

    protected override void DisableMain()
    {
        for (int i = 0; i < es.Count; ++i) { es[i].Disable(); es[i].RS(); }
        es.Clear();
        // if (modelIndex >= 0) { EnemyManager.gi.ReturnModel(modelIndex, view); }
        // view = null;
    }

    protected static Vector3 effTrans = new Vector3(0f, 0f, 1f);

    private void OnTriggerEnter(Collider other)
    {
        // 敵弾用
        for (int i = 0; i < 4; ++i)
        {
            EffectPool.gi.GetObject(1, trans.position + effTrans);
        }

        ScoreManager.AddScore(1);
        Disable();
    }
}

public class SCPool<T> where T : EnemyScript, new()
{
    List<T> pool = new List<T>();
    public T GetSleep()
    {
        if (pool.Count > 0)
        {
            var r = pool[0];
            pool.RemoveAt(0);
            return r;
        }

        return new T();
    }

    // poolの格納数がして位置になるまで、なので貸し出しているものは含まず
    public void Allocate(int arg)
    {
        for (int i = pool.Count; i < arg; ++i)
        {
            pool.Add(new T());
        }
    }
    public void ReturnScript(T arg) { pool.Add(arg); }

    //public void Run()
    //{
    //    for (int i = 0; i < pool.Count; ++i)
    //    {
    //        pool[i].Run();
    //    }
    //}
}

public class MoveAddPos : EnemyScript
{
    public Vector3 spd;

    protected override sealed void RunMain()
    {
        trans.position = trans.position + spd;
    }

    public static void InitStaticPool() { GP = new SCPool<MoveAddPos>(); }
    public static SCPool<MoveAddPos> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); spd = v0; }
    public void SetSpdRad(float spdValue, float rad) { spd.Set(spdValue * Mathf.Cos(rad), spdValue * Mathf.Sin(rad), 0f); }
    public void SetSpdDeg(float spdValue, float rad) { SetSpdRad(spdValue, Mathf.Deg2Rad * rad); }
}

public class MoveAddPosExt : EnemyScript
{
    public Vector3 spd;
    public float extRate;
    public int extTime;
    int cnt = 0;

    public void Settings(float aExtRate, int aExtTime)
    {
        extRate = aExtRate;
        extTime = aExtTime;
    }

    protected override sealed void RunMain()
    {
        trans.position = trans.position + spd;

        if (cnt < extTime) { spd *= extRate; }
        ++cnt;
    }

    public static void InitStaticPool() { GP = new SCPool<MoveAddPosExt>(); }
    public static SCPool<MoveAddPosExt> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); spd = v0; extRate = 1f; cnt = 0; }
    public void SetSpdRad(float spdValue, float rad) { spd.Set(spdValue * Mathf.Cos(rad), spdValue * Mathf.Sin(rad), 0f); }
    public void SetSpdDeg(float spdValue, float rad) { SetSpdRad(spdValue, Mathf.Deg2Rad * rad); }
}

public class MoveAddPosStop : EnemyScript
{
    public Vector3 spd;
    public Vector3 spd2;
    int state = 0;
    int cCnt = 0;
    public int time1, time2;

    public void Settings(int t1, int t2)
    {
        time1 = t1;
        time2 = t2;
    }

    protected override sealed void RunMain()
    {
        switch(state)
        {
        case 0:
            trans.position = trans.position + spd * ((float)(time1 - cCnt) / time1);
            if (cCnt >= time1) { cCnt = 0; state = 1; }
            break;

        case 1:
            if (cCnt >= time2) { cCnt = 0; state = 2; }
            break;

        case 2:
            trans.position = trans.position + spd2;
            break;
        }

        ++cCnt;
    }

    public static void InitStaticPool() { GP = new SCPool<MoveAddPosStop>(); }
    public static SCPool<MoveAddPosStop> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); spd = v0; state = 0; cCnt = 0; }
    public void SetSpdRad(float spdValue, float rad) { spd.Set(spdValue * Mathf.Cos(rad), spdValue * Mathf.Sin(rad), 0f); }
    public void SetSpdDeg(float spdValue, float rad) { SetSpdRad(spdValue, Mathf.Deg2Rad * rad); }
    public void SetSpd2Rad(float spdValue, float rad) { spd2.Set(spdValue * Mathf.Cos(rad), spdValue * Mathf.Sin(rad), 0f); }
    public void SetSpd2Deg(float spdValue, float rad) { SetSpd2Rad(spdValue, Mathf.Deg2Rad * rad); }
}

public class MoveTranslate : EnemyScript
{
    public Vector3 spd;

    protected override sealed void RunMain()
    {
        trans.Translate(spd);
    }

    public static void InitStaticPool() { GP = new SCPool<MoveTranslate>(); }
    public static SCPool<MoveTranslate> GP { get; private set; }
    public override void RS() {  GP.ReturnScript(this); spd = v0; }
}

public class MoveTranslateExt : EnemyScript
{
    public Vector3 spd, spdAdd;
    public float ext = 1f, saExt = 1f;

    protected override sealed void RunMain()
    {
        trans.Translate(spd);
        spd *= ext;
        spd += spdAdd;
        spdAdd *= saExt;
    }

    public static void InitStaticPool() { GP = new SCPool<MoveTranslateExt>(); }
    public static SCPool<MoveTranslateExt> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); spd = spdAdd = v0; ext = saExt = 1f; }
}

public class EnemyScript
{
    public Transform trans;

    protected virtual void DisableMain() { }
    public void Disable()
    {
        trans = null;
        DisableMain();
    }

    protected virtual void RunMain() { }
    public void Run()
    {
        RunMain();
    }
    
    public virtual void RS() { }

    public static Vector3 v1 = Vector3.one, v0 = Vector3.zero;
}
