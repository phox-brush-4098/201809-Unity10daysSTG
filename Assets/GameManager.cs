using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player pl;
    public ScoreManager scManager;
    public CubeManager cManager;
    public EffectPool ep;
    public FadeCanvas fc;
    public Stage stage;
    public CorpseManager crManager;
    public Pool bgEff/*, bgEff2*/;
    public MonoBehaviour postProcessingV2;
    public GameObject bgm;

    // FPSCounter fps = new FPSCounter();
    // public Image fpsView;

    static bool InitStatic = true;

    public bool useRanking;
    bool beforeIsBoss = false;

    private void Awake()
    {
        if (!StartScrScript.useEffect) { postProcessingV2.enabled = false; }
        Camera.main.depthTextureMode |= DepthTextureMode.Depth;

        EnemyBurn.InitAliveCnt();

        isGameOver = false;
        isBossStage = false;
        fc.gameObject.SetActive(true);

        if (InitStatic)
        {
            MoveTranslate.InitStaticPool();
            // MoveTranslate.GP.Allocate(32);

            MoveAddPos.InitStaticPool();
            MoveAddPos.GP.Allocate(512);
            MoveAddPosStop.InitStaticPool();
            MoveAddPosStop.GP.Allocate(32);
            MoveAddPosExt.InitStaticPool();
            MoveAddPosExt.GP.Allocate(32);

            EBul01.InitStaticPool();
            EBul01.GP.Allocate(32);
            EBul02.InitStaticPool();
            EBul02.GP.Allocate(4);
            EBul03.InitStaticPool();
            EBul03.GP.Allocate(32);
            EBul04.InitStaticPool();
            EBul04.GP.Allocate(1);
            EBul05.InitStaticPool();
            EBul05.GP.Allocate(1);
            EBul06.InitStaticPool();
            EBul06.GP.Allocate(1);
            InitStatic = false;
        }

        memSpaceAlloc = new int[8192];
    }

    void BGProc()
    {
        while (Random.Range(0, 240) < 15 + Mathf.Min(cnt / 90, 160)) { bgEff.GetSleep(); }
        // while (Random.Range(0, 240) < 60) { bgEff2.GetSleep(); }
        bgEff.Run();
        // bgEff2.Run();
    }

    int[] memSpaceAlloc;

    // Use this for initialization
    void Start()
    {
        pl.Init();
        ep.Init();
        ep.Allocate(0, 2048);
        ep.Allocate(1, 512);
        EnemyManager.gi.Init();
        EnemyManager.gi.AllocEO(0, 512);
        EnemyManager.gi.AllocEO(1, 64);
        // EnemyManager.gi.AllocVO(0, 512);
        // EnemyManager.gi.AllocVO(1, 64);
        crManager.Init();
        crManager.Allocate(0, 256);
        bgEff.Allocate(256);
        EnemyBurn.InitAliveCnt();

        for (int i = 0; i < 900; ++i) { BGProc(); }

        //
        memSpaceAlloc = null;
        System.GC.Collect(2);
        System.GC.WaitForPendingFinalizers();

        // Shader.WarmupAllShaders();
    }

    int cnt = 0, goCnt = 0, gowait = 0;

    static bool isGameOver, isBossStage;
    static bool isScoreBoardProcEnd = false;

    public static void SetGameOver() { isGameOver = true; }
    public static bool GetIsGameOver() { return isGameOver; }

    public static void StartBoss()
    {
        isBossStage = true;
    }

    int fframe = 0;

    void Update()
    {
        if (fframe == 0)
        {
            ep.AllAwake(0);
            ep.AllAwake(1);
            EnemyManager.gi.AllAwake(0);
            for (int i = 0; i < 16; ++i) { ep.Run(); }
            ++fframe;
        }
        else if (fframe == 1)
        {
            ++fframe;
        }
        else if (fframe == 2)
        {
            ep.AllSleep(0);
            ep.AllSleep(1);
            var v0 = Vector3.zero;
            for (int i = 0; i < 2048; ++i) { ep.GetObject(0, v0).SetSmall(); }
            for (int i = 0; i < 512; ++i) { ep.GetObject(1, v0).SetSmall(); }

            Enemy ene;
            EnemyManager.gi.AllSleep(0);
            for (int i = 0; i < 512; ++i)
            {
                ene = EnemyManager.gi.GetObject(0, v0);
                // ene.SetModel(0);
            }
            ep.Run();
            ++fframe;
        }
        else if (fframe == 3)
        {
            ep.AllSleep(0);
            ep.AllSleep(1);
            var v0 = Vector3.zero;
            for (int i = 0; i < 2048; ++i) { ep.GetObject(0, v0).SetSmall(); }
            for (int i = 0; i < 512; ++i) { ep.GetObject(1, v0).SetSmall(); }

            Enemy ene;
            EnemyManager.gi.AllSleep(0);
            for (int i = 0; i < 512; ++i)
            {
                ene = EnemyManager.gi.GetObject(0, v0);
                // ene.SetModel(0);
            }
            ep.Run();
            ++fframe;
        }
        else if (fframe == 4)
        {
            ep.AllSleep(0);
            ep.AllSleep(1);
            EnemyManager.gi.AllSleep(0);
            EnemyManager.gi.AllSleep(1);
            ++fframe;
            bgm.SetActive(true);
        }
        //fps.Update();
        //var sc = fpsView.transform.localScale;
        //sc.x = (fps.GetFPS() - 50) / 10f * 0.75f;
        //if (sc.x <= 0f) { sc.x = 0f; }
        //fpsView.transform.localScale = sc;        

        if (isGameOver)
        {
            if (gowait < 24)
            {
                pl.trans.localScale = Effect.v0;
                ++gowait;
                for (int i = 0; i < 12; ++i)
                {
                    var obj = EffectPool.gi.GetObject(0, pl.trans.position);
                    obj.SpdExt(Random.Range(1f, 2.25f));
                }
                goto goWaitNormalLoop;
            }

            scManager.UpdateView();
            cManager.UpdateView();

            if (!isScoreBoardProcEnd && useRanking)
            {
                isScoreBoardProcEnd = true;
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(ScoreManager.GetScore());

                goCnt = 1;
            }

            if (useRanking)
            {
                if (naichilab.RankingSceneManager.windowClosed)
                {
                    useRanking = false;
                }
            }
            else
            {
                if (goCnt == 60)
                {
                    fc.isFadeIn = false;
                    fc.Time = 60;
                    fc.StartProc();
                }
                if (goCnt == 120)
                {
                    SceneManager.LoadScene("title");
                }

                ++goCnt;
            }

            return;
        }

        goWaitNormalLoop:

        ++cnt;

        //
        BGProc();

        if (cnt < 10) { return; }

        if (isBossStage && !beforeIsBoss)
        {
            beforeIsBoss = true;
            stage.StartBoss();
        }

        pl.SetBefPosToCurrentPos();
        stage.Run();
        EnemyManager.gi.Run();
        ep.Run();
        crManager.Run();
        pl.Run();        

        //
        scManager.UpdateView();
        cManager.UpdateView();

        //
        Physics.SyncTransforms();
        Physics.Simulate(0.01666666f);
    }
}

// http://tasogare-games.hatenablog.jp/entry/20150609/1433779228
//public class FPSCounter
//{
//    int frameCount = 0;
//    float prevTime = 0.0f;
//    float currentFPS = 0f;

//    public float GetFPS() { return currentFPS; }

//    public void Update()
//    {
//        ++frameCount;
//        float rt = Time.realtimeSinceStartup;
//        float time = rt - prevTime;

//        if (time >= 0.333333f)
//        {
//            currentFPS = frameCount / time;

//            frameCount = 0;
//            prevTime = rt;
//        }
//    }
//}

//// http://baba-s.hatenablog.com/entry/2017/12/20/000200
//public class FPSCounter
//{
//    private float m_updateInterval = 0.25f;

//    private float m_accum;
//    private int m_frames;
//    private float m_timeleft;
//    private float m_fps;

//    public float GetFPS() { return m_fps; }

//    public void Update()
//    {
//        m_timeleft -= Time.deltaTime;
//        m_accum += Time.timeScale / Time.deltaTime;
//        m_frames++;

//        if (0 < m_timeleft) return;

//        m_fps = m_accum / m_frames;
//        m_timeleft = m_updateInterval;
//        m_accum = 0;
//        m_frames = 0;
//    }
//}