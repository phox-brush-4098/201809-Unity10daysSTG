using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCallBack : MonoBehaviour
{
    public Player parent;
    public SphereCollider col;

    int disableHitTime = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (disableHitTime > 0) { return; }
        // if (other.gameObject.tag.Equals("eneBul01"))
        {
            parent.HitCallBack(15);            
            disableHitTime = 150;
        }
    }

    public void Avoid(int frame)
    {
        if (frame < 0) { frame = 1; }
        if (disableHitTime < frame) { disableHitTime = frame; }
        col.radius = 8f;
    }

    public void Run()
    {
        if (disableHitTime > 0)
        {
            --disableHitTime;
            if (disableHitTime == 0) { col.radius = 0.4f; }
        }
    }
}
