using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextView : MonoBehaviour
{
    int cnt = 0;
    public int wait, div;
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
    }
}
