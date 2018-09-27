using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public List<ScoreText> texts;
    public Text extText;
    public ScoreLowView a;

    static int score;
    static bool flag;

    int currentExt;

    public static int GetCubeScore() { return score; }

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

    public static void AddScore(int arg)
    {
        score += arg;
        flag = true;
    }
    
    public void UpdateView()
    {
        if (flag)
        {
            int tmpSocre = score;
            int tmpExt = 0;
            while (tmpSocre > 999)
            {
                tmpSocre /= 10;
                ++tmpExt;
            }

            if (tmpExt != currentExt)
            {
                extText.text = tmpExt.ToString();
                currentExt = tmpExt;
            }

            texts[0].UpdateView(tmpSocre / 100 % 10);
            texts[1].UpdateView(tmpSocre / 10 % 10);
            texts[2].UpdateView(tmpSocre % 10);

            flag = false;

            a.UpdateView(score);
        }
    }
}
