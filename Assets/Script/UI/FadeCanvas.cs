using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeCanvas : MonoBehaviour
{
    public Image img;
    int Cnt = 0;
    public int Time;
    public bool isFadeIn;   // trueだとだんだん薄くなる
    public bool autoStart = true;

    private void Awake()
    {
        if (autoStart)
        {
            StartProc();
        }
    }

    public void StartProc()
    {
        gameObject.SetActive(true);
        if (isFadeIn) { Cnt = Time; }
        else { Cnt = 0; }
    }

    void Update()
    {
        // フェードアウトの時はオブジェクトを無効化しない
        if (Cnt < 0/* || Cnt > Time*/) { gameObject.SetActive(false); return; }

        var c = img.color;
        c.a = (float)Cnt / Time;
        img.color = c;

        Cnt += (isFadeIn ? -1 : 1);
    }
}
