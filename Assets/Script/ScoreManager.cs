using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public List<ScoreText> texts;
    public Text extText;
    public ScoreLowView a, b;

    static long score;
    static bool flag;

    int currentExt;

    private void Awake()
    {
        score = 0;
        flag = true;
        currentExt = 0;
    }

    private void Start()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            texts[i].Init();
        }
    }

    public static long GetScore() { return score; }

    // そのまま加算
    public static void AddScore(int arg)
    {
        score += arg;
        flag = true;
    }

    // 倍率その他を計算
    public static void AddScoreEnemy(int arg)
    {
        score += (arg + CubeManager.GetCubeScore() / 50);
        flag = true;
    }
    
    public void UpdateView()
    {
        if (flag)
        {
            long tmpSocre = score;
            int tmpExt = 0;
            while (tmpSocre > 99999)
            {
                tmpSocre /= 10;
                ++tmpExt;
            }

            if (tmpExt != currentExt)
            {
                if (tmpExt < 10) { extText.text = tmpExt.ToString(); }
                else
                {
                    switch (tmpExt)
                    {
                    case 10: extText.text = "A"; break;
                    case 11: extText.text = "B"; break;
                    case 12: extText.text = "C"; break;
                    case 13: extText.text = "D"; break;
                    case 14: extText.text = "E"; break;
                    case 15: extText.text = "F"; break;
                    case 16: extText.text = "G"; break;
                    case 17: extText.text = "H"; break;
                    case 18: extText.text = "I"; break;
                    case 19: extText.text = "J"; break;
                    default: extText.text = "-"; break;
                    }
                }
                currentExt = tmpExt;
            }

            texts[0].UpdateView((int)(tmpSocre / 10000));
            texts[1].UpdateView((int)(tmpSocre / 1000 % 10));
            texts[2].UpdateView((int)(tmpSocre / 100 % 10));
            texts[3].UpdateView((int)(tmpSocre / 10 % 10));
            texts[4].UpdateView((int)(tmpSocre % 10));

            flag = false;

            a.UpdateView(score);
            b.UpdateView(score);
        }
    }
}
