using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlgBehavior : MonoBehaviour
{
    public static Vector3 vecBuff = Vector3.zero;

    public GameObject go { private set; get; }
    public Transform trans { private set; get; }

    private void Awake()
    {
        go = gameObject;
        trans = transform;

        go.SetActive(false);
    }

    public void Init() { go.SetActive(true); InitMain(); /*InitEndMain();*/ }
    public bool Run() { RunMain(); /*RunEndMain();*/ return go.activeSelf; }
    public void Disable() { DisableMain(); /*DisableEndMain();*/ go.SetActive(false); }

    protected virtual void InitMain() { }
    protected virtual void RunMain() { }
    protected virtual void DisableMain() { }

    //protected virtual void InitEndMain() { }
    //protected virtual void RunEndMain() { }
    //protected virtual void DisableEndMain() { }

    // 実際の画面よりは広め、zの値が0の想定
    public void DisableIfOutOfSScreen()
    {
        Vector3 pos = trans.position;
        if (Mathf.Abs(pos.x) > 3.9f || pos.y > 7.3f || pos.y < -5f) { Disable(); }
    }
}
