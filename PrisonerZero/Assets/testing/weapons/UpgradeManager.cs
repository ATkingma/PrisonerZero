using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance => instance;
    private static UpgradeManager instance;

    [SerializeField]
    private PlayerMovement pm;
    [Space(10)]
    [SerializeField]
    private float bulletSize;
    [SerializeField]
    private float maxBulletSize;

    [SerializeField]
    private float bulletSpread;
    [SerializeField]
    private float maxBulletSpread;

    [SerializeField]
    private float bulletProjectals;
    [SerializeField]
    private float maxBulletProjectals;

    [SerializeField]
    private float bulletPierce;
    [SerializeField]
    private float maxBulletPierce;

    [SerializeField]
    private float bulletBounces;
    [SerializeField]
    private float maxBulletBounces;

    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private float maxReloadTime;

    [SerializeField]
    private float magSize;
    [SerializeField]
    private float maxMagSize;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float pickupRange;
    [SerializeField]
    private float maxPickupRange;

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

    public void SetWeaponStats(float bulletSize, float bulletSpread, float bulletProjectals, float bulletPierce, float  bulletBounces, float reloadTime, float magSize)
    {
        this.bulletSize = bulletSize;   
        this.bulletSpread = bulletSpread;
        this.bulletProjectals = bulletProjectals;
        this.bulletBounces = bulletBounces;
        this.reloadTime = reloadTime;
        this.magSize=magSize;
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
        }
    }

    public void Increase_Bullet_Projectals(float increasePercentage)
    {
        float tempBulletProjectals = ((bulletProjectals / 100) * increasePercentage) + bulletProjectals;

        if (tempBulletProjectals >= maxBulletProjectals)
        {
            // Handle reaching max projectals
        }
        else
        {
            bulletProjectals = tempBulletProjectals;
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
        }
    }

    // Methods for new upgrades

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
        }
    }

    public void Increase_Reload_Time(float increasePercentage)
    {
        float tempReloadTime = ((reloadTime / 100) * increasePercentage) + reloadTime;

        if (tempReloadTime >= maxReloadTime)
        {
            // Handle reaching max reload speed
        }
        else
        {
            reloadTime = tempReloadTime;
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
}
