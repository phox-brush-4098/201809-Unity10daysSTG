using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    int cnt = 0;
    int tmp = 0;
    float tf = -2.5f;
    int waitingEnemy = 0;   // 作成待機
    int currentPointer = -1, curCnt = 0, curInterval = 0;
    bool bossFlag = false;
    public static bool isClear = false;

    private void Awake()
    {
        isClear = false;
    }

    public void StartBoss()
    {
        if (bossFlag) { return; }
        bossFlag = true;
        cnt = -240;
        currentPointer = -1;
        curCnt = 0;
        bossStartText.gameObject.SetActive(true);
        waitingEnemy = 0;
    }

    public Text text, bossStartText;

    void GameClear()
    {
        text.gameObject.SetActive(true);
    }

    void Pat2()
    {
        if (currentPointer == 3)
        {
            if (curCnt == 45)
            {
                GameClear();
            }
            if (curCnt > 120)
            {
                GameClear();
                isClear = true;
                GameManager.SetGameOver();
            }
            ++curCnt;
            return;
        }

        const int currentPtrMax = 4;
        if (waitingEnemy + EnemyBurn.AliveCnt <= 0)
        {
            ++currentPointer;

            switch (currentPointer % currentPtrMax)
            {
            case 0: SetPat(2, 7); break;
            case 1: SetPat(2, 128); break;
            case 2: SetPat(2, 31); break;
            }

            curCnt = 0;
        }

        //
        // static Vector3 bossscale = new Vector3(1.5f, 1.5f, 1.5f);

        if (curCnt % curInterval == 0 && waitingEnemy > 0)
        {
            EnemyBurn ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
            // ene.SetModel(1);
            MoveAddPos mp = null;
            MoveAddPosStop mp2 = null;
            MoveAddPosExt mp3 = null;
            EBul03 eb03 = null;
            EBul05 eb05 = null;
            EBul06 eb06 = null;

            // 敵作成 HP1200程度が全力1秒漏れなしショット, 1セット4発で80
            switch (currentPointer % currentPtrMax)
            {
            case 0:
                tmpVec.Set(0f, -8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(10000, 5, 6, 360);
                mp2 = MoveAddPosStop.GP.GetSleep();
                mp2.Settings(120, 3600);
                mp2.SetSpdDeg(12f * 0.02f, 90f);
                mp2.SetSpd2Deg(2.5f * 0.02f, 90f);
                ene.AddScript(mp2);
                eb05 = EBul05.GP.GetSleep();
                ene.AddScript(eb05);

                for (int i = 0; i < 3; ++i)
                {
                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(-1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(600, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(17f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 5, 90, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(600, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(17f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 5, 90, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    waitingEnemy -= 2;
                }
                break;

            case 1:
                tmpVec.Set(1f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(30, 3, 1, 45);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(30f * 0.02f, -92f);
                ene.AddScript(mp);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-1f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(30, 3, 1, 45);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(30f * 0.02f, -88f);
                ene.AddScript(mp);

                --waitingEnemy;
                break;

            case 2:
                tmpVec.Set(0f, -8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(8000, 5, 6, 360);
                mp2 = MoveAddPosStop.GP.GetSleep();
                mp2.Settings(120, 3600);
                mp2.SetSpdDeg(12f * 0.02f, 90f);
                mp2.SetSpd2Deg(2.5f * 0.02f, 90f);
                ene.AddScript(mp2);
                eb06 = EBul06.GP.GetSleep();
                ene.AddScript(eb06);

                for (int i = 0; i < 5; ++i)
                {
                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(-1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(800, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(17f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(800, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(17f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    waitingEnemy -= 2;
                }

                for (int i = 0; i < 5; ++i)
                {
                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    //  ene.SetModel(1);
                    tmpVec.Set(-1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(600, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(25f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(600, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(25f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    waitingEnemy -= 2;
                }

                for (int i = 0; i < 5; ++i)
                {
                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(-1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(400, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(32f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(400, 3, 3, 360);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(32f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 10, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    waitingEnemy -= 2;
                }
                break;
            }

            --waitingEnemy;
        }

        ++curCnt;
    }

    void SetPat(int interval, int enemyNum)
    {
        curInterval = interval;
        waitingEnemy = enemyNum;
    }

    Vector3 tmpVec = Vector3.zero;

    void Pat1()
    {
        const int currentPtrMax = 19;
        if (waitingEnemy + EnemyBurn.AliveCnt <= 0)
        {
            ++currentPointer;

            switch (currentPointer % currentPtrMax)
            {
            case 0: case 1: SetPat(8, 15); break;
            case 2: case 3: SetPat(6, 18); break;
            case 4: SetPat(150, 6); break;
            case 5: SetPat(1, 1); break;
            case 6: SetPat(15, 24); break;
            case 7: SetPat(5, 64); break;
            case 8: case 10: SetPat(4, 10); break;
            case 9: case 11: SetPat(4, 10); break;
            case 12: SetPat(50, 20); break;
            case 13: SetPat(90, 6); break;
            case 14: SetPat(25, 15); break;
            case 15: SetPat(3, 24); break;
            case 16: SetPat(3, 64);  break;
            case 17: SetPat(1, 11); break;
            case 18: SetPat(3, 48); break;
            }
            
            curCnt = 0;
        }

        //
        if (curCnt % curInterval == 0 && waitingEnemy > 0)
        {
            EnemyBurn ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
            // ene.SetModel(1);
            MoveAddPos mp = null;
            MoveAddPosStop mp2 = null;
            MoveAddPosExt mp3 = null;
            EBul01 eb01 = null;
            EBul02 eb02 = null;
            EBul03 eb03 = null;
            EBul04 eb04 = null;

            // 敵作成 HP1200程度が全力1秒漏れなしショット, 1セット4発で80
            switch (currentPointer % currentPtrMax)
            {
            case 0:case 8: case 10:
                tmpVec.Set(6f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(80, 3, 1, 90);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(8f * 0.02f, -90f);
                ene.AddScript(mp);
                break;

            case 1:case 9: case 11:
                tmpVec.Set(-6f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(80, 3, 1, 90);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(8f * 0.02f, -90f);
                ene.AddScript(mp);
                break;

            case 2:
                tmpVec.Set(10f, 4f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(120, 3, 2, 120);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(12f * 0.02f, 180f);
                ene.AddScript(mp);
                break;

            case 3:
                tmpVec.Set(-10f, 4f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(120, 3, 2, 120);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(12f * 0.02f, 0f);
                ene.AddScript(mp);
                break;

            case 4:
                tmpVec.Set(4f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(2300, 3, 5, 300);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(2.35f * 0.02f, -105f);
                ene.AddScript(mp);
                eb01 = EBul01.GP.GetSleep();
                eb01.Settings(60, 3, true, 60f, 8f);
                ene.AddScript(eb01);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-4f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(2300, 3, 5, 300);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(2.35f * 0.02f, -75f);
                ene.AddScript(mp);
                eb01 = EBul01.GP.GetSleep();
                eb01.Settings(60, 3, true, 60f, 8f);
                ene.AddScript(eb01);

                --waitingEnemy;
                break;

            case 5:
                tmpVec.Set(0f, -8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(6000, 5, 5, 270);
                mp2 = MoveAddPosStop.GP.GetSleep();
                mp2.Settings(120, 300);
                mp2.SetSpdDeg(12f * 0.02f, 90f);
                mp2.SetSpd2Deg(2.5f * 0.02f, 90f);
                ene.AddScript(mp2);
                eb02 = EBul02.GP.GetSleep();
                ene.AddScript(eb02);
                break;

            case 6:
                tmpVec.Set(10f, 5.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(300, 3, 3, 240);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(2.5f * 0.02f, -160f);
                ene.AddScript(mp);
                eb01 = EBul01.GP.GetSleep();
                eb01.Settings(100, 3, true, 60f, 8f);
                ene.AddScript(eb01);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-10f, 5.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(300, 3, 3, 240);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(2.5f * 0.02f, -20f);
                ene.AddScript(mp);
                eb01 = EBul01.GP.GetSleep();
                eb01.Settings(100, 3, true, 60f, 8f);
                ene.AddScript(eb01);

                --waitingEnemy;
                break;

            case 7: case 15:
                tmpVec.Set(Random.Range(-6.5f, 6.5f), 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(90, 3, 1, 360);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(5f * 0.02f, Random.Range(-115f, -65f));
                ene.AddScript(mp);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(60, 3, 120, 6, 0.8f, 360, 12f);
                ene.AddScript(eb03);                
                break;

            case 12:
                tmpVec.Set(9, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(90, 3, 2, 200);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(5f * 0.02f, -110);
                ene.AddScript(mp);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 5, 150, 5, 0.8f, 360, 10f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(4f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(90, 3, 2, 200);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(5f * 0.02f, -100);
                ene.AddScript(mp);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 5, 150, 5, 0.8f, 360, 10f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-4f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(90, 3, 2, 200);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(5f * 0.02f, -80);
                ene.AddScript(mp);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 5, 150, 5, 0.8f, 360, 10f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-9f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(90, 3, 2, 200);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(5f * 0.02f, -70);
                ene.AddScript(mp);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 5, 150, 5, 0.8f, 360, 10f);
                ene.AddScript(eb03);

                waitingEnemy -= 3;
                break;

            case 13:
                tmpVec.Set(0, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(1300, 3, 2, 200);
                mp3 = MoveAddPosExt.GP.GetSleep();
                mp3.Settings(0.975f, 60);
                mp3.SetSpdDeg(7f * 0.02f, -90);
                ene.AddScript(mp3);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 7, 200, 10, 0.6f, 360, 13f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(7f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(1000, 3, 5, 270);
                mp3 = MoveAddPosExt.GP.GetSleep();
                mp3.Settings(0.975f, 60);
                mp3.SetSpdDeg(9f * 0.02f, -100);
                ene.AddScript(mp3);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 6, 200, 10, 0.6f, 360, 13f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-7f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(1000, 3, 5, 270);
                mp3 = MoveAddPosExt.GP.GetSleep();
                mp3.Settings(0.975f, 60);
                mp3.SetSpdDeg(9f * 0.02f, -80);
                ene.AddScript(mp3);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(70, 6, 200, 10, 0.6f, 360, 13f);
                ene.AddScript(eb03);

                waitingEnemy -= 2;
                break;

            case 14:
                tmpVec.Set(0, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(300, 3, 3, 200);
                mp3 = MoveAddPosExt.GP.GetSleep();
                mp3.Settings(0.985f, 60);
                mp3.SetSpdDeg(10f * 0.02f, -90);
                ene.AddScript(mp3);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(50, 7, 200, 10, 0.6f, 360, 11f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(7f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(200, 3, 2, 240);
                mp3 = MoveAddPosExt.GP.GetSleep();
                mp3.Settings(0.985f, 60);
                mp3.SetSpdDeg(9f * 0.02f, -100);
                ene.AddScript(mp3);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(50, 6, 200, 10, 0.6f, 360, 11f);
                ene.AddScript(eb03);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-7f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(200, 3, 2, 240);
                mp3 = MoveAddPosExt.GP.GetSleep();
                mp3.Settings(0.985f, 60);
                mp3.SetSpdDeg(9f * 0.02f, -80);
                ene.AddScript(mp3);
                eb03 = EBul03.GP.GetSleep();
                eb03.Settings(50, 6, 200, 10, 0.6f, 360, 11f);
                ene.AddScript(eb03);

                waitingEnemy -= 2;
                break;

            case 16: case 18:
                tmpVec.Set(1f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(30, 3, 1, 45);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(30f * 0.02f, -92f);
                ene.AddScript(mp);

                ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                // ene.SetModel(1);
                tmpVec.Set(-1f, 8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(30, 3, 1, 45);
                mp = MoveAddPos.GP.GetSleep();
                mp.SetSpdDeg(30f * 0.02f, -88f);
                ene.AddScript(mp);

                --waitingEnemy;
                break;

            case 17:
                tmpVec.Set(0f, -8.5f, 0f);
                ene.trans.position = tmpVec;
                ene.Settings(5000, 5, 5, 210);
                mp2 = MoveAddPosStop.GP.GetSleep();
                mp2.Settings(90, 600);
                mp2.SetSpdDeg(13f * 0.02f, 90f);
                mp2.SetSpd2Deg(2.5f * 0.02f, 90f);
                ene.AddScript(mp2);
                eb04 = EBul04.GP.GetSleep();
                ene.AddScript(eb04);

                for (int i = 0; i < 5; ++i)
                {
                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(-1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(500, 3, 2, 300);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(9f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 15, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    ene = EnemyManager.gi.GetObject(1) as EnemyBurn;
                    // ene.SetModel(1);
                    tmpVec.Set(1.75f * i, 8.5f, 0f);
                    ene.trans.position = tmpVec;
                    ene.Settings(500, 3, 2, 300);
                    mp3 = MoveAddPosExt.GP.GetSleep();
                    mp3.Settings(0.95f, 120);
                    mp3.SetSpdDeg(9f * 0.02f, -90);
                    ene.AddScript(mp3);
                    eb03 = EBul03.GP.GetSleep();
                    eb03.Settings(50, 3, 120, 15, 0.4f, 360, 14f);
                    ene.AddScript(eb03);

                    waitingEnemy -= 2;
                }

                break;
            }

            --waitingEnemy;
        }
        ++curCnt;
    }

    public void Run()
    {
        if (!bossFlag) { Pat1(); }
        else
        {
            if (cnt > 0)
            {
                Pat2();
            }
            else
            {
                // Player.plTempe = 0f;
                Player.fieldTemp = 0f;
                Player.timeDamageRaise = 0;
            }
        }

        //
        ++cnt;
    }

    public void BGProc()
    {

    }
}
