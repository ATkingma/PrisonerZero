using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance => instance;
    private static UpgradeManager instance;

    private BaseWeapon weapon;

    [SerializeField]
    private PlayerMovement pm;
    [Space(10)]

    private float bulletSize;

    [SerializeField]
    private float maxBulletSize;

    private float bulletSpread;
    [SerializeField]
    private float maxBulletSpread;

    private float bulletProjectals;
    [SerializeField]
    private float maxBulletProjectals;

    private float bulletPierce;
    [SerializeField]
    private float maxBulletPierce;

    private float bulletBounces;
    [SerializeField]
    private float maxBulletBounces;

    private float reloadTime;
    [SerializeField]
    private float minReloadTime;

    private float magSize;
    [SerializeField]
    private float maxMagSize;

    private float movementSpeed;
    [SerializeField]
    private float maxMovementSpeed;

    private float health;
    [SerializeField]
    private float maxHealth;

    private float pickupRange;
    [SerializeField]
    private float maxPickupRange;

    private float fireRate;

    [SerializeField]
    private float minFireRate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetWeaponStats(float bulletSize, float bulletSpread, float bulletProjectals, float bulletPierce, float  bulletBounces, float reloadTime, float magSize,float fireRate, BaseWeapon weapon)
    {
        this.bulletSize = bulletSize;   
        this.bulletSpread = bulletSpread;
        this.bulletProjectals = bulletProjectals;
        this.bulletBounces = bulletBounces;
        this.reloadTime = reloadTime;
        this.magSize=magSize;
        this.weapon = weapon;
        this.fireRate = fireRate;
    }

    public void SetPlayerStats(float movementSpeed, float health, float pickupRange)
    {
        this.movementSpeed= movementSpeed;
        this.health= health;
        this.pickupRange= pickupRange;
    }

    public void Increase_Bullet_Size(float increasePercentage)
    {
        float tempBulletSize = ((bulletSize / 100) * increasePercentage) + bulletSize;

        if (tempBulletSize >= maxBulletSize)
        {
            // Handle reaching max size
        }
        else
        {
            bulletSize = tempBulletSize;
            weapon.ChangeBulletSize(tempBulletSize);
        }
    }

    public void Increase_Bullet_Spread(float increasePercentage)
    {
        float tempBulletSpread = ((bulletSpread / 100) * increasePercentage) + bulletSpread;

        if (tempBulletSpread >= maxBulletSpread)
        {
            // Handle reaching max spread
        }
        else
        {
            bulletSpread = tempBulletSpread;
            weapon.ChangeBulletSpread(tempBulletSpread);
        }
    }

    public void Increase_Bullet_Projectals(int increasePercentage)
    {
        float tempBulletProjectals = ((bulletProjectals / 100) * increasePercentage) + bulletProjectals;

        if (tempBulletProjectals >= maxBulletProjectals)
        {
            // Handle reaching max projectals
        }
        else
        {
            bulletProjectals = tempBulletProjectals;
            weapon.ChangeBulletProjectals((int)tempBulletProjectals);
        }
    }

    public void Increase_Bullet_Pierce(float increasePercentage)
    {
        float tempBulletPierce = ((bulletPierce / 100) * increasePercentage) + bulletPierce;

        if (tempBulletPierce >= maxBulletPierce)
        {
            // Handle reaching max pierce
        }
        else
        {
            bulletPierce = tempBulletPierce;
            weapon.ChangeBulletPierce(tempBulletPierce);
        }
    }

    public void Increase_Bullet_Bounces(float increasePercentage)
    {
        float tempBulletBounces = ((bulletBounces / 100) * increasePercentage) + bulletBounces;

        if (tempBulletBounces >= maxBulletBounces)
        {
            // Handle reaching max bounces
        }
        else
        {
            bulletBounces = tempBulletBounces;
            weapon.ChangeBulletBounces(tempBulletBounces);
        }
    }

    public void Decrease_Reload_Time(float decreasePercentage)
    {
        float tempReloadTime = reloadTime - ((reloadTime / 100) * decreasePercentage);

        if (tempReloadTime < minReloadTime)
        {

        }
        else
        {
            tempReloadTime = minReloadTime;

            reloadTime = tempReloadTime;
            weapon.ChangeReloadTime(tempReloadTime);
        }
    }

    public void Increase_Increase_Mag_Size(float increasePercentage)
    {
        float tempIncreaseMagSize = ((magSize / 100) * increasePercentage) + magSize;

        if (tempIncreaseMagSize >= maxMagSize)
        {
            // Handle reaching max mag size increase
        }
        else
        {
            magSize = tempIncreaseMagSize;
            weapon.ChangeMagSize(tempIncreaseMagSize);
            
        }
    }

    public void Increase_Movement_Speed(float increasePercentage)
    {
        float tempMovementSpeed = ((movementSpeed / 100) * increasePercentage) + movementSpeed;

        if (tempMovementSpeed >= maxMovementSpeed)
        {
            // Handle reaching max movement speed
        }
        else
        {
            movementSpeed = tempMovementSpeed;
            pm.SetMovementSpeed(movementSpeed);

        }
    }

    public void Increase_Health(float increasePercentage)
    {
        float tempHealth = ((health / 100) * increasePercentage) + health;

        if (tempHealth >= maxHealth)
        {
            // Handle reaching max health
        }
        else
        {
            health = tempHealth;
        }
    }

    public void Increase_Pickup_Range(float increasePercentage)
    {
        float tempPickupRange = ((pickupRange / 100) * increasePercentage) + pickupRange;

        if (tempPickupRange >= maxPickupRange)
        {
            // Handle reaching max pickup range
        }
        else
        {
            pickupRange = tempPickupRange;
        }
    }

    public void Increse_Fire_Rate(float decreasePercentage)
    {
        float tempFirerate = fireRate - ((fireRate / 100) * decreasePercentage);

        if (tempFirerate < minFireRate)
        {
        Debug.Log("t");
            // Handle reaching max pickup range
        }
        else
        {
            Debug.Log("t");
            pickupRange = tempFirerate;
            weapon.ChangeFireRate(tempFirerate);
        }
    }
}
