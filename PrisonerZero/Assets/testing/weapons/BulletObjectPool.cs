using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool Instance => sharedInstance;    

    private static BulletObjectPool sharedInstance;

    private GameObject bulletPrefab;

    [SerializeField]
    private int poolSize = 20;

    private List<Bullet> pool;

    private int currentIndex = 0;

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void SpawnBullets()
    {
        pool = new List<Bullet>();
        for (int i = 0; i < poolSize; i++)
        {
            SpawnBullet();
        }
    }

    public Bullet SpawnBullet()
    {
        Bullet obj = Instantiate(bulletPrefab).GetComponent<Bullet>();
        obj.gameObject.SetActive(false);
        pool.Add(obj);

        return obj;
    }

    public GameObject GetPooledObject()
    {
        currentIndex++;
        if (currentIndex >= pool.Count)
        {
            currentIndex = 0;
        }

        if (pool[currentIndex].gameObject.activeInHierarchy)
        {
            Bullet tempBullet = SpawnBullet();
            tempBullet.ResetBullet();
            return tempBullet.gameObject;
        }
        else
        {
            pool[currentIndex].ResetBullet();
            return pool[currentIndex].gameObject;
        }
    }

    public void ChangeBulletSpeed(float newSpeed)
    {
        foreach (Bullet obj in pool)
        {
            obj.SetSpeed(newSpeed);
        }
    }

    public void ChangePoolSize(int _newPoolSize)
    {
        _newPoolSize *= 2;
        int newBullets = _newPoolSize-poolSize;
        poolSize = _newPoolSize;

        for (int i = 0; i < newBullets; i++)
        {
            Bullet obj = Instantiate(bulletPrefab).GetComponent<Bullet>();
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public void SetBullet(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
        SpawnBullets();
    }
}
