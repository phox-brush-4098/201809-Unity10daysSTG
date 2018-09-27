using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager gi { get; private set; }

    // List<List<GameObject>> viewObjPool;
    List<Pool> pools;
    public List<Enemy> enemyPrefab;
    public List<GameObject> viewObjPrefab;

    private void Awake()
    {
        gi = this;
    }

    private void OnDestroy()
    {
        gi = null;
    }

    public void AllocEO(int arg, int num) { pools[arg].Allocate(num); }
    //public void AllocVO(int arg, int num)
    //{
    //    for (int i = viewObjPool[arg].Count; i < arg; ++i)
    //    {
    //        viewObjPool[arg].Add(Instantiate(viewObjPrefab[arg]));
    //        (viewObjPool[arg])[i].SetActive(false);
    //    }
    //}

    public void Init()
    {
        pools = new List<Pool>(enemyPrefab.Count);

        for (int i = 0; i < enemyPrefab.Count; ++i)
        {
            pools.Add(gameObject.AddComponent<Pool>());
            enemyPrefab[i].gameObject.SetActive(true);
            pools[i].prefab = enemyPrefab[i];
        }

        //viewObjPool = new List<List<GameObject>>(viewObjPrefab.Count);

        //for (int i = 0; i < viewObjPrefab.Count; ++i)
        //{
        //    viewObjPool.Add(new List<GameObject>());
        //}
    }

    //public GameObject GetModel(int index)
    //{
    //    if (viewObjPool[index].Count > 0)
    //    {
    //        var obj = (viewObjPool[index])[0];
    //        viewObjPool[index].RemoveAt(0);
    //        if (obj == null) { return Instantiate(viewObjPrefab[index]); } obj.SetActive(true);
    //        return obj;
    //    }
    //    return Instantiate(viewObjPrefab[index]);
    //}

    //public GameObject GetModel(int index, Vector3 arg)
    //{
    //    var r = GetModel(index);
    //    r.transform.position = arg;
    //    return r;
    //}

    //public void ReturnModel(int arg, GameObject obj)
    //{
    //    if (obj != null)
    //    {
    //        obj.transform.parent = transform;
    //        obj.transform.localRotation = viewObjPrefab[arg].transform.localRotation;
    //        obj.SetActive(false);
    //    }
        
    //    viewObjPool[arg].Add(obj);
    //}

    public void Run()
    {
        for (int i = 0; i < pools.Count; ++i)
        {
            pools[i].Run();
        }
    }

    public Enemy GetObject(int index)
    {
        return pools[index].GetSleep() as Enemy;
    }

    public Enemy GetObject(int index, Vector3 arg)
    {
        return pools[index].GetSleepPos(arg) as Enemy;
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
