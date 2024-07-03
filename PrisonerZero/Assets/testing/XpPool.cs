using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpPool : MonoBehaviour
{
    public static XpPool Instance => sharedInstance;

    private static XpPool sharedInstance;

    [SerializeField]
    private GameObject xpPrefab;

    [SerializeField]
    private int poolSize = 50;

    private List<XpPickup> pool;

    private int currentIndex = 0;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SpawnXpPickups();
    }

    private void SpawnXpPickups()
    {
        pool = new List<XpPickup>();
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(SpawnXpPickup());
        }
    }

    public XpPickup SpawnXpPickup()
    {
        XpPickup obj = Instantiate(xpPrefab).GetComponent<XpPickup>();
        obj.gameObject.SetActive(false);
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
            XpPickup xpPickup = SpawnXpPickup();
            return xpPickup.gameObject;
        }
        else
        {
            return pool[currentIndex].gameObject;
        }
    }
  

    public void ChangePoolSize(int _newPoolSize)
    {
        int xpAmount = _newPoolSize - poolSize;
        poolSize = _newPoolSize;

        for (int i = 0; i < xpAmount; i++)
        {
            XpPickup obj = Instantiate(xpPrefab).GetComponent<XpPickup>();
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }
}
