using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    List<GameObject> children = new List<GameObject>(10);
    public Text zero;

    int current = 0;

    public void Init()
    {
        children.Add(zero.gameObject);
        for (int i = 1; i <= 9; ++i)
        {
            Text nt = Instantiate(zero);
            nt.text = i.ToString();            
            nt.gameObject.transform.SetParent(transform, false); //parent = transform;
            nt.gameObject.transform.position = zero.transform.position;
            nt.gameObject.transform.localScale = v0;
            children.Add(nt.gameObject);
            // nt.gameObject.SetActive(false);
        }
    }

    static Vector3 v1 = Vector3.one, v0 = Vector3.zero;

    public void UpdateView(int next)
    {
        if (next == current) { return; }

        children[current].transform.localScale = v0;
        children[next].transform.localScale = v1;
        //children[current].SetActive(false);
        //children[next].SetActive(true);

        current = next;
    }
}
