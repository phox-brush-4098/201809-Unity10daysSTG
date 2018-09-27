using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class Pool : MonoBehaviour
{
    SimpleList<FlgBehavior> activeList, sleepList;
    public FlgBehavior prefab;

    private void Awake()
    {
        activeList = new SimpleList<FlgBehavior>();
        sleepList = new SimpleList<FlgBehavior>();
    }

    public void ClearList() { activeList.Clear(); sleepList.Clear(); }

    public void Run()
    {
        var it = activeList.GetForward();

        for (int i = 0, n = activeList.Count; i < n; ++i)
        {
            if (!it.Ref().Run()) { sleepList.Add(it.Ref()); activeList.Delete(ref it); }
            else { it.Next(); }
        }
    }

    public void AllAwake()
    {
        var it = sleepList.GetForward();

        for (int i = 0, n = sleepList.Count; i < n; ++i)
        {
            activeList.Add(it.Ref());
            it.Ref().Init();
            sleepList.Delete(ref it);
        }
    }

    public void AllSleep()
    {
        var it = activeList.GetForward();

        for (int i = 0, n = activeList.Count; i < n; ++i)
        {
            sleepList.Add(it.Ref());
            it.Ref().Disable();
            activeList.Delete(ref it);
        }
    }

    public FlgBehavior GetSleep()
    {
        FlgBehavior result = null;
        if (sleepList.Count > 0)
        {
            result = sleepList.GetForwardRef();
            activeList.Add(result);
            sleepList.DeleteForward();
        }
        else if (prefab != null) { activeList.Add(result = Instantiate(prefab)); }

        if (result != null) { result.Init(); }

        return result;
    }

    public FlgBehavior GetSleepPos(Vector3 pos)
    {
        FlgBehavior result = null;
        if (sleepList.Count > 0)
        {
            var it = sleepList.GetForwardRef();
            result = it;
            activeList.Add(result);
            sleepList.DeleteForward();
        }
        else if (prefab != null) { activeList.Add(result = Instantiate(prefab)); }
        result.trans.position = pos;
        if (result != null) { result.Init(); }

        return result;
    }

    public void Allocate(int allocCount)
    {
        // FlgBehavior f;
        for (int i = 0; i < allocCount; ++i) { sleepList.Add(/*f = */Instantiate(prefab)); }
        activeList.SetctorStackAllocate(allocCount);
        for (int i = 0; i < allocCount; ++i) { GetSleep(); }
        AllSleep();
    }

    public int GetActiveCnt() { return activeList.Count; }
    public int GetSleepCnt() { return sleepList.Count; }
}

public class SimpleListSector<T> where T : class
{
    public SimpleListSector<T> before;
    public SimpleListSector<T> next;
    public T obj;

    public void SetDefault() { before = next = null; }

    public void AddNext(SimpleListSector<T> arg) { next = arg; }
    public void AddBefore(SimpleListSector<T> arg) { before = arg; }
}


public struct SimpleListIterator<T> where T : class
{
    public SimpleListSector<T> Obj;
    public void Before() { Obj = Obj.before; }
    public void Next() { Obj = Obj.next; }
    public SimpleListSector<T> Delete_ReplaceNext()
    {
        if (Obj.next != null) { Obj.next.before = Obj.before; }
        if (Obj.before != null) { Obj.before.next = Obj.next; }
        var result = Obj;
        Obj = Obj.next;
        return result;
    }

    public T Ref() { return Obj.obj; }

    public bool ExistNext() { if (Obj == null) { return false; } return Obj.next != null; }
}

public struct SimpleStack<T> where T : class
{
    public SimpleListSector<T> forward;

    public SimpleListSector<T> Get()
    {
        if (forward == null) { return null; }
        SimpleListSector<T> result = forward;
        forward = forward.next;
        return result;
    }

    public void Add(SimpleListSector<T> arg)
    {
        arg.obj = null;
        arg.before = null;
        arg.next = forward;
        forward = arg;
    }

    public void Allocate(int arg)
    {
        for (int i = 0; i < arg; ++i)
        {
            Add(new SimpleListSector<T>());
        }
    }

    public bool IsEmpty() { return forward == null; }
}

public class SimpleList<T> where T : class
{
    SimpleStack<T> sectorStack = new SimpleStack<T> { forward = null };
    SimpleListSector<T> forward = null;
    SimpleListSector<T> back = null;
    public int Count { get; private set; }
    static SimpleListIterator<T> buff = new SimpleListIterator<T>();
    public SimpleListIterator<T> GetForward() { buff.Obj = forward; return buff; }
    public T GetForwardRef() { return forward.obj; }
    public void SetForward(ref SimpleListIterator<T> write) { write.Obj = forward; }

    public void Clear()
    {
        forward = back = null;
        Count = 0;
        sectorStack = new SimpleStack<T> { forward = null };
    }

    public void SetctorStackAllocate(int arg)
    {
        sectorStack.Allocate(arg);
    }

    // 最善の要素を削除する いちいちイテレーター取得して云々するより早いと思う
    public void DeleteForward()
    {
        if (Count <= 1) { if (Count == 1) { sectorStack.Add(forward); } forward = back = null; Count = 0; return; }
        forward = forward.next;
        if (Count != 0) { sectorStack.Add(forward.before); }
        forward.before = null;
        --Count;
    }

    // イテレーターには次の要素が格納される
    public void Delete(ref SimpleListIterator<T> iterator)
    {
        // 
        if (Count <= 1) { if (Count == 1) { sectorStack.Add(iterator.Obj); } iterator.Obj = forward = back = null; Count = 0; return; }
        if (iterator.Obj.next == null) { back = iterator.Obj.before; }
        if (iterator.Obj.before == null) { forward = iterator.Obj.next; }
        sectorStack.Add(iterator.Delete_ReplaceNext());
        --Count;
    }

    public void Add(T arg)
    {
        SimpleListSector<T> newData = sectorStack.Get();
        if (newData == null) { newData = new SimpleListSector<T>(); }
        newData.before = back;
        newData.next = null;
        newData.obj = arg;

        if (back != null) { back.next = newData; }

        back = newData;

        if (Count == 0) { forward = newData; }

        ++Count;
    }
}
