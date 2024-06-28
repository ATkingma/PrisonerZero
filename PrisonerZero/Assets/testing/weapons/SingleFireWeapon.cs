using UnityEngine;

public class SingleFireWeapon : BaseWeapon
{
    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
}
