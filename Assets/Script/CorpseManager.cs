using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseManager : MonoBehaviour
{
    List<Pool> pools;
    public List<Corpse> effList;

    public static CorpseManager gi { get; private set; }

    private void Awake()
    {
        gi = this;
    }

    private void OnDestroy()
    {
        gi = null;
    }

    public void Init()
    {
        pools = new List<Pool>(effList.Count);
        for (int i = 0; i < effList.Count; ++i)
        {
            pools.Add(gameObject.AddComponent<Pool>());
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

    public Corpse GetObject(int index, int aliveTime)
    {
        var r = pools[index].GetSleep() as Corpse;
        r.SetAliveTime(aliveTime);
        return r;
    }

    public Corpse GetObject(int index, int aliveTime, Vector3 arg)
    {
        var r = pools[index].GetSleepPos(arg) as Corpse;
        r.SetAliveTime(aliveTime);
        return r;
    }
}
