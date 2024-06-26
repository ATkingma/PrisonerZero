using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BaseWeapon : MonoBehaviour
{
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
    [SerializeField]
    private int bulletBounces;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private int magSize;


    private int currentMag;
    private bool reloading = false;

    private void Start()
    {
        SetWeaponStats();
        currentMag = magSize;
    }

    public virtual void SetWeaponStats()
    {
        BulletObjectPool.Instance.SetBullet(bulletPrefab);
        UpgradeManager.Instance.SetWeaponStats(bulletSize, bulletSpread, bulletProjectals, bulletPierce, bulletBounces, reloadTime, magSize);
        BulletObjectPool.Instance.ChangeBulletSpeed(bulletVelocity);
        BulletObjectPool.Instance.ChangePoolSize(magSize, bulletVelocity);
    }



    public void Shoot()
    {
        if (reloading)
        {
            return;
        }

        if (currentMag <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

 
        GameObject bullet = BulletObjectPool.Instance.GetPooledObject();
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.position = barrel.transform.position;
            bullet.transform.position = barrel.position;
            bullet.transform.rotation = barrel.rotation;
            bullet.SetActive(true);
            currentMag--;
        }

        if (currentMag <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
    }


    public IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        currentMag = magSize;
    }

}
