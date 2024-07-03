using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public override void Death()
    {
        GameObject xpPickup =  XpPool.Instance.GetPooledObject();
        xpPickup.transform.position = this.transform.position;
        xpPickup.transform.rotation = this.transform.rotation;
        xpPickup.SetActive(true);
        base.Death();
    }
}
