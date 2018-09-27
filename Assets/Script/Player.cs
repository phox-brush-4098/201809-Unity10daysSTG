using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : FlgBehavior
{
    const float spd = 12f;
    public Transform viewObjTrans;
    public Transform viewObjTrans2;

    public Pool bulPool, mSwordPool;
    public ScoreText ch1, ch2;
    public ScoreText plT1, plT2, plT3;
    public ScoreText fiT1, fiT2, fiT3;
    public ScoreLowView chargeBar;

    public PlayerHitCallBack hitChild;

    float yRot = 0f;

    int MainShotCnt = 0;
    static int charge = 0;
    const int chargeLevel = 10000;
    const int chargeMax = 50000;
    int burning = 0;
    public static int timeDamageRaise = 0;

    public static float plTempe, fieldTemp;
    protected static Vector3 effTrans = new Vector3(0f, 0f, 0.5f);
    static Vector3 beforePos;
    public static Vector3 GetBefPos() { return beforePos; }
    public void SetBefPosToCurrentPos() { beforePos = trans.position; }
    public void ResetDamageRaise() { timeDamageRaise = 0; }
    public static bool bossFrag;

    protected override void InitMain()
    {
        yRot = 0f;
        charge = chargeLevel * 3;
        bulPool.Allocate(16);
        mSwordPool.Allocate(2);
        bossFrag = false;
        hitChild.Avoid(90);

        //
        ch1.Init();
        ch2.Init();

        plT1.Init();
        plT2.Init();
        plT3.Init();

        fiT1.Init();
        fiT2.Init();
        fiT3.Init();

        plTempe = 0f;
        fieldTemp = 0f;
        timeDamageRaise = 0;
    }

    public static void AddCharge(int arg) { charge += arg; if (charge > chargeMax) { charge = chargeMax; } }

    static Vector3 DeadScale = new Vector3(0.05f, 0.05f, 0.05f);
    static Vector3 BurnScaleAdd = new Vector3(0.02f, 0.02f, 0.02f);

    public void HitCallBack(float value)
    {
        plTempe += (value + fieldTemp / 10f) * (1f + timeDamageRaise / 3600f);
        fieldTemp += 8f;
        fieldTemp *= 1.3333333333f;
        burning = 52;
        SoundList.gi.list[1].transform.position = trans.position;
        SoundList.gi.list[1].Play();
        for (int i = 0; i < 128; ++i)
        {
            var obj = EffectPool.gi.GetObject(0, trans.position);
            obj.SpdExt(Random.Range(0.75f, 3.25f));
        }

        trans.localScale = DeadScale;
    }

    int beforeCaution = 0;

    protected override void RunMain()
    {
        viewObjTrans2.Rotate(9f, 0f, 0f);
        Move();
        MoveSP();
        MoveLimit();
        Atack();

        hitChild.Run();

        if (burning > 0)
        {
            --burning;
            if (trans.localScale.x < 1f) { trans.localScale = trans.localScale + BurnScaleAdd; if (trans.localScale.x > 1f) { trans.localScale = Effect.v1; } }
            var p = trans.position + effTrans;
            for (int i = 0; i < 3; ++i)
            {
                var obj = EffectPool.gi.GetObject(1, p);
                obj.SpdExt(Random.Range(1.25f, 2.5f));
            }
        }

        // charge
        AddCharge(30);

        ch1.UpdateView(charge / chargeLevel);
        ch2.UpdateView(charge * 10 / chargeLevel % 10);
        chargeBar.UpdateView(charge % chargeLevel / 100f);

        // Life(temerature)
        float timePenalty = 1f + (float)timeDamageRaise * (1f / 2400f);        // 時間経過で被ダメージ量が増加
        plTempe += fieldTemp / 600f * timePenalty;   // 場の気温が最高だと10秒で死ぬ
        fieldTemp += (100f / 1800f) * (0.65f + timePenalty * 0.35f);     // 何も起こらないと30秒で最大になる

        if (plTempe >= 80f)
        {
            if (beforeCaution <= 0)
            {
                SoundList.gi.list[4].transform.position = trans.position;
                SoundList.gi.list[4].Play();
            }
            beforeCaution = 15;
        }
        else
        {
            --beforeCaution;
        }

        if (Stage.isClear) { plTempe = 0f; }
        if (GameManager.GetIsGameOver() && !Stage.isClear)
        {
            plTempe = 99.925f;
        }
        if (plTempe >= 99.925f)
        {
            plTempe = 99.925f;            
            burning = 200;
            if (CubeManager.GetCubeScore() > 99999 && !bossFrag)
            {
                if (!GameManager.GetIsGameOver())
                {
                    SoundList.gi.list[1].transform.position = trans.position;
                    SoundList.gi.list[1].Play();
                }
                bossFrag = true;
                plTempe = 0f;
                fieldTemp = 0f;
                GameManager.StartBoss();
                // GameManager.SetGameOver();    // ボスが実装できたらコメントにしてください
            }
            else
            {
                if (!GameManager.GetIsGameOver())
                {
                    SoundList.gi.list[1].transform.position = trans.position;
                    SoundList.gi.list[1].Play();
                }
                GameManager.SetGameOver();
            }      
        }
        else if (plTempe < 0f) { plTempe = 0f; }
        if (fieldTemp >= 99.925f) { fieldTemp = 99.925f; }
        else if (fieldTemp < 0f) { fieldTemp = 0f; }

        plT1.UpdateView(Mathf.FloorToInt(plTempe / 10f));
        plT2.UpdateView(Mathf.FloorToInt(plTempe) % 10);
        plT3.UpdateView(Mathf.FloorToInt(plTempe * 10f) % 10);

        fiT1.UpdateView(Mathf.FloorToInt(fieldTemp / 10f));
        fiT2.UpdateView(Mathf.FloorToInt(fieldTemp) % 10);
        fiT3.UpdateView(Mathf.FloorToInt(fieldTemp * 10f) % 10);

        //
        bulPool.Run();
        mSwordPool.Run();

        ++timeDamageRaise;
    }

    bool swRev = false;

    void Atack()
    {
        // MShot
        if (MainShotCnt > 0) { --MainShotCnt; }
        if (MainShotCnt == 0 && (Input.touchCount > 0 || Input.GetKey(KeyCode.Z)) && bulPool.GetSleepCnt() >= 4)
        {
            MainShotCnt = 3;
            Vector3 pos = trans.position;
            pos.y += 0.7f;
            pos.x += 0.6f;
            for (int i = 0; i < 4; ++i)
            {
                FlgBehavior obj = bulPool.GetSleepPos(pos);
                obj.trans.rotation = Quaternion.Euler(0f, 0f, 1.5f * i - 2.25f);
                pos.x -= 0.4f;
            }

            SoundList.gi.list[2].transform.position = trans.position;
            SoundList.gi.list[2].Play();
        }

        // MSword
        if (((Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.X)) 
            && charge > chargeLevel && mSwordPool.GetSleepCnt() > 0)
        {
            var obj = mSwordPool.GetSleepPos(trans.position);
            (obj as PlMainSword).SetRotValue(9f * (swRev ? 1f : -1f));
            swRev = !swRev;
            hitChild.Avoid(4);
            charge -= chargeLevel;
        }
    }

    void Move()
    {
        if (yRot != 0f) { yRot = yRot * 0.825f; }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            trans.Translate(0f, spd * 0.02f, 0f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            trans.Translate(0f, -spd * 0.02f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            trans.Translate(spd * 0.02f, 0f, 0f);
            yRot -= 3.5f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            trans.Translate(-spd * 0.02f, 0f, 0f);
            yRot += 3.5f;
        }

        Quaternion q = viewObjTrans.transform.rotation;
        q.y = yRot * Mathf.PI / 180f;
        viewObjTrans.transform.rotation = q;        
    }

    void MoveLimit()
    {
        float rightLimit = 7.5f;
        float leftLimit = -rightLimit;
        float upLimit = 5.8f;
        float downLimit = -7f;

        Vector3 pos;
        if (trans.position.y > upLimit) { pos = trans.position; pos.y = upLimit; trans.position = pos; }
        if (trans.position.y < downLimit) { pos = trans.position; pos.y = downLimit; trans.position = pos; }
        if (trans.position.x > rightLimit) { pos = trans.position; pos.x = rightLimit; trans.position = pos; }
        if (trans.position.x < leftLimit) { pos = trans.position; pos.x = leftLimit; trans.position = pos; }
    }

    void MoveSP()
    {
        if (Input.touchCount < 1) { return; }

        Touch t = Input.GetTouch(0);
        Vector2 move = t.deltaPosition;
        move *= 16f / Screen.height;

        if (t.phase != TouchPhase.Moved) { return; }

        trans.Translate(move.x, move.y, 0f);
    }
}
