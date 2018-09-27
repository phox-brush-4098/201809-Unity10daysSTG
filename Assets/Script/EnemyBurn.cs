using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBurn : Enemy
{
    public int HP, score, corpseNum, corpseAlive;
    public bool isAlive;

    public static int AliveCnt { get; protected set; }    
    public static void InitAliveCnt() { AliveCnt = 0; }
    int cnt;

    protected override void InitMain()
    {
        trans.localRotation = zeroQ;
        isAlive = true;
        score = 0;
        ++AliveCnt;
        HP = corpseNum = corpseAlive = 0;
        cnt = 0;
    }

    public void SetHP(int arg) { HP = arg; }
    public void SetScore(int arg) { score = arg; }
    public void SetHpAndScore(int argHP, int scoreExt) { HP = argHP; score = argHP * scoreExt; }

    public void Settings(int argHP, int argScoreExt, int argCorpseNum, int argCorpseAlive)
    {
        SetHpAndScore(argHP, argScoreExt);
        corpseNum = argCorpseNum;
        corpseAlive = argCorpseAlive;
    }

    protected override void RunMain()
    {
        for (int i = 0; i < es.Count; ++i) { es[i].Run(); }

        var pos = trans.position;
        if (pos.x < -11f || pos.x > 11f || pos.y > 9.5f || pos.y < -9.5f) { Disable(); }

        if (cnt % 3 == 0) { --corpseAlive; }

        ++cnt;
    }

    protected override void DisableMain()
    {
        for (int i = 0; i < es.Count; ++i) { es[i].Disable(); es[i].RS(); }
        es.Clear();
        //if (modelIndex >= 0) { EnemyManager.gi.ReturnModel(modelIndex, view); }
        //view = null;
        --AliveCnt;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 一応同じフレームに多重破壊されるの防止
        if (!isAlive) { return; }

        //switch (other.gameObject.tag)
        //{
        //case "plMS01": HP -= 20; break;
        //case "plSW01": HP -= 80; break;
        //}
        HP -= 20;

        if (HP <= 0)
        {
            var p = trans.position + effTrans;
            for (int i = 0; i < 12; ++i)
            {
                var e = EffectPool.gi.GetObject(1, p);
                e.SpdExt(Random.Range(1f, 2f));
            }
            ScoreManager.AddScoreEnemy(score);            
            isAlive = false;

            if (corpseAlive > 6)
            {
                for (int i = 0; i < corpseNum; ++i)
                {
                    CorpseManager.gi.GetObject(0, corpseAlive, p);
                }
            }

            SoundList.gi.list[0].transform.position = trans.position;
            SoundList.gi.list[0].Play();

            Disable();
        }
    }

    // 継続ダメージがあるもののみ
    private void OnTriggerStay(Collider other)
    {
        if (!isAlive) { return; }

        //switch (other.gameObject.tag)
        //{
        //case "plSW01": HP -= 14; break;
        //}
        HP -= 18;

        if (HP <= 0)
        {
            var p = trans.position + effTrans;
            for (int i = 0; i < 12; ++i)
            {
                var e = EffectPool.gi.GetObject(1, p);
                e.SpdExt(Random.Range(1f, 1.75f));
            }
            ScoreManager.AddScoreEnemy(score);            
            isAlive = false;
            if (corpseAlive > 6)
            {
                for (int i = 0; i < corpseNum; ++i)
                {
                    CorpseManager.gi.GetObject(0, corpseAlive, p);
                }
            }
            SoundList.gi.list[0].transform.position = trans.position;
            SoundList.gi.list[0].Play();

            Disable();
        }
    }
}

public class EBul01 : EnemyScript
{
    public int interval;
    public int bulWay;
    public bool aimPlayer;
    public float angleRange;
    public float angleAdd = 0f;
    public float bulSpd;
    int cnt = 0;

    public void Settings(int argInterval, int argWay, bool isAimPlayer, float argAngleRange, float argBulSpd)
    {
        interval = argInterval;
        bulWay = argWay;
        aimPlayer = isAimPlayer;
        angleRange = argAngleRange;
        bulSpd = argBulSpd;
    }

    public static void ShotWay(float angleAdd, bool isAim, Vector3 bulMakePos, float Range, int way, float shotSpd/*内部で0.02乗算が自動で行われる*/)
    {
        float pAim = angleAdd;
        if (isAim)
        {
            Vector2 v2 = Player.GetBefPos();
            pAim = Mathf.Rad2Deg * Mathf.Atan2(v2.y - bulMakePos.y, v2.x - bulMakePos.x) + angleAdd;
        }

        float rot = Range / way;
        if (way % 2 == 0) { pAim -= rot * (way / 2 - 0.5f); }
        else { pAim -= rot * (way / 2); }

        shotSpd *= 0.02f;

        for (int i = 0; i < way; ++i)
        {
            var ene = EnemyManager.gi.GetObject(0, bulMakePos);
            // ene.SetModel(0);
            ene.trans.Rotate(0f, 0f, pAim);
            var move = MoveAddPos.GP.GetSleep();
            move.SetSpdDeg(shotSpd, pAim);
            ene.AddScript(move);
            pAim += rot;
        }
    }

    protected override sealed void RunMain()
    {
        ++cnt;

        if (cnt % interval == 0)
        {
            ShotWay(angleAdd, aimPlayer, trans.position, angleRange, bulWay, bulSpd);
        }
    }

    public static void InitStaticPool() { GP = new SCPool<EBul01>(); }
    public static SCPool<EBul01> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); cnt = 0; interval = 1; }
}

public class EBul02 : EnemyScript
{
    int cnt = 0;

    protected override sealed void RunMain()
    {
        ++cnt;

        if (cnt < 130) { return; }

        if (cnt % 60 == 0)
        {
            EBul01.ShotWay(Random.Range(0f, 15f), false, trans.position, 360f, 16, 5f);
            EBul01.ShotWay(0, true, trans.position, 28f, 5, 10f);
            EBul01.ShotWay(0, true, trans.position, 28f, 5, 9f);
            EBul01.ShotWay(0, true, trans.position, 28f, 5, 8f);
            EBul01.ShotWay(0, true, trans.position, 28f, 5, 7f);
        }
    }

    public static void InitStaticPool() { GP = new SCPool<EBul02>(); }
    public static SCPool<EBul02> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); cnt = 0; }
}

public class EBul03 : EnemyScript
{
    int cnt = 0;

    public int wait, way, interval, loopWay;
    public float lWaySub, angleRange,sSpd;

    public void Settings(int aWait, int aWay, int aInterval, int aLoopWay, float aLWaySub, float aAngleRange, float aSSpd)
    {
        wait = aWait;
        way = aWay;
        interval = aInterval;
        loopWay = aLoopWay;
        lWaySub = aLWaySub;
        angleRange = aAngleRange;
        sSpd = aSSpd;
    }

    protected override sealed void RunMain()
    {
        ++cnt;

        if (cnt < wait) { return; }

        if ((cnt - wait) % interval == 0)
        {
            for (int i = 0; i < loopWay; ++i)
            {
                EBul01.ShotWay(0, true, trans.position, angleRange, way, sSpd - i * lWaySub);
            }
        }
    }

    public static void InitStaticPool() { GP = new SCPool<EBul03>(); }
    public static SCPool<EBul03> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); cnt = 0; }
}

public class EBul04 : EnemyScript
{
    int cnt = 0, c2 = 0;
    float ext = 1f;

    protected override sealed void RunMain()
    {
        ++cnt;

        float f = c2 % 2 == 0 ? 0: Random.Range(-360f, 0f);
        if (Random.Range(0, 1000) < 70) { ext *= -1f; }

        if (cnt < 150) { return; }

        if (cnt % 16 == 0)
        {
            EBul01.ShotWay(cnt / 16 * 8 * ext % 360 + f, false, trans.position, 360, 18, 14f);
            EBul01.ShotWay(cnt / 16 * 8 * ext % 360 + f, false, trans.position, 360, 18, 13.25f);
            EBul01.ShotWay(cnt / 916 * 8 * ext % 360 + f, false, trans.position, 360, 18, 12.5f);

            ++c2;
        }

        if ((cnt - 150) % 120 == 0)
        {
            EBul01.ShotWay(-4, true, trans.position, 360, 7, 12);
            EBul01.ShotWay(-2, true, trans.position, 360, 7, 13);
            EBul01.ShotWay(0, true, trans.position, 360, 7, 14);
            EBul01.ShotWay(2, true, trans.position, 360, 7, 13);
            EBul01.ShotWay(4, true, trans.position, 360, 7, 12);
        }
    }

    public static void InitStaticPool() { GP = new SCPool<EBul04>(); }
    public static SCPool<EBul04> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); cnt = 0; }
}

public class EBul05 : EnemyScript
{
    int cnt = 0;

    protected override sealed void RunMain()
    {
        ++cnt;

        if (cnt < 150) { return; }
        
        if ((cnt - 150) % 30 == 0)
        {
            EBul01.ShotWay(-6, true, trans.position, 360, 15, 9);
            EBul01.ShotWay(-4, true, trans.position, 360, 15, 10);
            EBul01.ShotWay(-2, true, trans.position, 360, 15, 11);
            EBul01.ShotWay(0, true, trans.position, 360, 15, 12);
            EBul01.ShotWay(2, true, trans.position, 360, 15, 11);
            EBul01.ShotWay(4, true, trans.position, 360, 15, 10);
            EBul01.ShotWay(6, true, trans.position, 360, 15, 9);
        }
    }

    public static void InitStaticPool() { GP = new SCPool<EBul05>(); }
    public static SCPool<EBul05> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); cnt = 0; }
}

public class EBul06 : EnemyScript
{
    int cnt = 0;
    float ext = 1f;

    protected override sealed void RunMain()
    {
        ++cnt;

        if (Random.Range(0, 1000) < 70) { ext *= -1f; }

        if (cnt < 150) { return; }

        //if (cnt % 30 == 0)
        //{
        //    
        //    EBul01.ShotWay(f, false, trans.position, 360, 12, 14f);
        //    EBul01.ShotWay(f, false, trans.position, 360, 12, 13f);
        //    EBul01.ShotWay(f, false, trans.position, 360, 12, 12f);
        //}
        float f = Random.Range(-180f, 180f);
        
        if ((cnt - 150) % 22 == 0)
        {
            bool b = ((cnt - 150) / 22) % 2 == 0;
            if (b) { f = 0; }
            EBul01.ShotWay(-4 + f, b, trans.position, 360, 17, 13);
            EBul01.ShotWay(-2 + f, b, trans.position, 360, 17, 14);
            EBul01.ShotWay(0 + f, b, trans.position, 360, 17, 15);
            EBul01.ShotWay(2 + f, b, trans.position, 360, 17, 14);
            EBul01.ShotWay(4 + f, b, trans.position, 360, 17, 13);
        }
    }

    public static void InitStaticPool() { GP = new SCPool<EBul06>(); }
    public static SCPool<EBul06> GP { get; private set; }
    public override void RS() { GP.ReturnScript(this); cnt = 0; }
}

