using UnityEngine;
using UnityEngine.Pool;

public  class Weapon : MonoBehaviour, IWeaponUpgrade
{
    public Transform barrel; // The barrel from where bullets will be fired
    public float BulletVelocity => bulletVelocity;
    [SerializeField]
    private float bulletVelocity = 20;

    private void Start()
    {
        BulletObjectPool.SharedInstance.ChangeBulletSpeed(BulletVelocity);
    }

    public void Shoot()
    {
        GameObject bullet = BulletObjectPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = barrel.position;
            bullet.transform.rotation = barrel.rotation;
            bullet.SetActive(true);
        }
    }

    public void ReduceBulletVelProcentage(float precentage)
    {
        float tempPerc = 100 - precentage;
        bulletVelocity /= 100;
        bulletVelocity *= tempPerc;

        BulletObjectPool.SharedInstance.ChangeBulletSpeed(BulletVelocity);
    }
}
