using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : BaseWeapon
{

    private bool isSchooting = false;
    private float shotCounter;

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isSchooting = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isSchooting = false;
        }

        if(isSchooting)
        {
            shotCounter -= Time.deltaTime;

            if(shotCounter < 0)
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
