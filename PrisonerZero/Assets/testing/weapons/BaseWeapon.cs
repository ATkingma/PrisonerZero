using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BaseWeapon : MonoBehaviour
{
    public static BaseWeapon Instance => instance;
    private static BaseWeapon instance; 

    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private GameObject bulletPrefab;
    [Space(10)]


    [SerializeField]
    private float damage;
    [SerializeField]
    private float fireRate=5;

    public float FireRate => fireRate;

    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private float bulletSize;
    [SerializeField]
    private float bulletSpread;
    [SerializeField]
    private int bulletProjectals;
    [SerializeField]
    private int bulletPierce;

    public int BulletPierce => bulletPierce;
    [SerializeField]
    private int bulletBounces;
    public int BulletBounces => bulletBounces;  

    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private int magSize;


    private int currentMag;
    private bool reloading = false;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start()
    {
        SetWeaponStats();
        currentMag = magSize;
    }

    public virtual void SetWeaponStats()
    {
        BulletObjectPool.Instance.SetBullet(bulletPrefab);
        UpgradeManager.Instance.SetWeaponStats(bulletSize, bulletSpread, bulletProjectals, bulletPierce, bulletBounces, reloadTime, magSize,fireRate,this);
        BulletObjectPool.Instance.ChangeBulletSpeed(bulletVelocity);
        BulletObjectPool.Instance.ChangeBulletSize(bulletSize);
        BulletObjectPool.Instance.ChangeBulletDamage(damage);
        BulletObjectPool.Instance.ChangePoolSize(magSize, bulletVelocity);
    }



    public void Shoot()
    {
        if (reloading)
        {
            return;
        }

        if (currentMag < bulletProjectals)
        {
            StartCoroutine(Reload());
            return;
        }

        ShootBullet(barrel.rotation);

        for (int i = 1; i <= bulletProjectals / 2; i++)
        {
            float spreadAngle = i * bulletSpread;

            if (i * 2 - 1 < bulletProjectals)
            {
                ShootBullet(barrel.rotation * Quaternion.Euler(0, 0, spreadAngle));
            }

            if (i * 2 < bulletProjectals)
            {
                ShootBullet(barrel.rotation * Quaternion.Euler(0, 0, -spreadAngle));
            }
        }
    }

    private void ShootBullet(Quaternion rotation)
    {
        GameObject bullet = BulletObjectPool.Instance.GetPooledObject();
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.position = barrel.transform.position;
            bullet.transform.position = barrel.position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true);
            currentMag--;
        }

        if (currentMag <= 0)
        {
            StartCoroutine(Reload());
        }
    }



    public IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        currentMag = magSize;
    }

    public void ChangeBulletSize(float tempBulletSize)
    {
        bulletSize = tempBulletSize;
        BulletObjectPool.Instance.ChangeBulletSize(tempBulletSize);
    }

    public void ChangeBulletSpread(float tempBulletSpread)
    {
        bulletSize = tempBulletSpread;
    }

    public void ChangeBulletProjectals(int tempBulletProjectals)
    {
        bulletProjectals = tempBulletProjectals;
    }

    public void ChangeBulletPierce(float tempBulletPierce)
    {
        bulletPierce = (int)tempBulletPierce;
    }

    public void ChangeBulletBounces(float tempBulletBounces)
    {
        bulletBounces = (int)tempBulletBounces;
    }

    public void ChangeReloadTime(float tempReloadTime)
    {
        reloadTime = tempReloadTime;    
    }

    public void ChangeMagSize(float tempIncreaseMagSize)
    {
        magSize = (int)tempIncreaseMagSize;
    }

    public void ChangeFireRate(float tempFirerate)
    {
        fireRate = tempFirerate;
    }
}
