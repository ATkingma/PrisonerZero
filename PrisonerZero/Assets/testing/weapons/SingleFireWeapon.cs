using UnityEngine;

public class SingleFireWeapon : BaseWeapon
{
    private float shotCounter;
    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter < 0)
            {
                shotCounter = FireRate;
                base.Shoot();
            }
        }
        else
        {
            shotCounter -= Time.deltaTime;
        }
    }
}
