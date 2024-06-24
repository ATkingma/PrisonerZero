using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool SharedInstance;
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private List<Bullet> pool;

    void Awake()
    {
        if(SharedInstance== null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        pool = new List<Bullet>();
        for (int i = 0; i < poolSize; i++)
        {
            Bullet obj = Instantiate(bulletPrefab).GetComponent<Bullet>();
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (Bullet obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.ResetBullet();
                return obj.gameObject;
            }
        }
        return null;
    }

    public void ChangeBulletSpeed(float newSpeed)
    {
        foreach (Bullet obj in pool)
        {
            obj.SetSpeed(newSpeed);
        }
    }
}
