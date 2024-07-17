using UnityEngine;

public class UpgradeManagerHelper : MonoBehaviour
{
    public void IncreaseBulletSize(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Bullet_Size(increasePercentage);
    }

    public void IncreaseBulletSpread(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Bullet_Spread(increasePercentage);
    }

    public void IncreaseBulletProjectals(int increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Bullet_Projectals(increasePercentage);
    }

    public void IncreaseBulletPierce(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Bullet_Pierce(increasePercentage);
    }

    public void IncreaseBulletBounces(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Bullet_Bounces(increasePercentage);
    }

    public void DecreaseReloadTime(float decreasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Decrease_Reload_Time(decreasePercentage);
    }

    public void IncreaseMagSize(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Increase_Mag_Size(increasePercentage);
    }

    public void IncreaseMovementSpeed(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Movement_Speed(increasePercentage);
    }

    public void IncreaseHealth(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Health(increasePercentage);
    }

    public void IncreasePickupRange(float increasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increase_Pickup_Range(increasePercentage);
    }

    public void IncreaseFireRate(float decreasePercentage)
    {
        if (UpgradeManager.Instance == null)
            return;

        UpgradeManager.Instance.Increse_Fire_Rate(decreasePercentage);
    }
}
