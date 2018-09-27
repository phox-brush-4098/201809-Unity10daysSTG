using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    List<Pool> pools;
    public List<Effect> effList;

    public static EffectPool gi { get; private set; }

    private void Awake()
    {
        gi = this;
    }

    private void OnDestroy()
    {
        //Debug.Log(pools[0].GetActiveCnt() + pools[0].GetSleepCnt());
        //Debug.Log(pools[0].GetActiveCnt() + pools[1].GetSleepCnt());
        gi = null;
    }

    public void Init()
    {
        pools = new List<Pool>(effList.Count);
        for (int i = 0; i < effList.Count; ++i)
        {
            pools.Add(gameObject.AddComponent<Pool>());
            effList[i].gameObject.SetActive(true);
            pools[i].prefab = effList[i];
        }
    }

    public void Run()
    {
        for (int i = 0; i < pools.Count; ++i)
        {
            pools[i].Run();
        }
    }

    public void Allocate(int index, int count)
    {
        pools[index].Allocate(count);
    }

    public Effect GetObject(int index)
    {
        return pools[index].GetSleep() as Effect;
    }

    public Effect GetObject(int index, Vector3 arg)
    {
        return pools[index].GetSleepPos(arg) as Effect;
    }

    public void AllAwake(int arg)
    {
        pools[arg].AllAwake();
    }

    public void AllSleep(int arg)
    {
        pools[arg].AllSleep();
    }
}
