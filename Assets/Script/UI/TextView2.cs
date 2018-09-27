using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextView2 : MonoBehaviour
{
    int cnt = 0;
    public int wait, div, startVanish;
    public Text text;
    public string message;

    // Update is called once per frame
    void Update()
    {
        ++cnt;

        if (cnt > wait)
        {
            int tmp = cnt - wait;

            if (message.Length >= tmp / div)
            {
                text.text = message.Substring(0, tmp / div);
            }
        }

        if (startVanish < cnt)
        {
            var sc = transform.localScale;
            sc.x *= 0.9f;
            sc.y *= 0.85f;
            transform.localScale = sc;
            if (sc.x < 0.0001f) { gameObject.SetActive(false); }
        }
    }
}
