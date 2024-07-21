using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform projectileDirection;
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private int projectileAmount = 1;
    [SerializeField]
    private float projectileSpread = 10;
    [SerializeField]
    private float projectileSpeed = 1;

    [SerializeField]
    private float attackSpeed = 1;

    private float AttackTime => 1 / attackSpeed;
    private float waitTime;

    private void Update()
    {
        if (target != null)
        {
            LookAtTarget();

            if(waitTime <= Time.time)
            {
                waitTime = Time.time + AttackTime;
                Shoot();
            }
        }
    }

    private void LookAtTarget()
    {
        transform.right = target.position - transform.position;
    }

    public void Shoot()
    {
        ShootBullet(projectileDirection.rotation);

        for (int i = 1; i <= projectileAmount / 2; i++)
        {
            float spreadAngle = i * projectileSpread;

            if (i * 2 - 1 < projectileAmount)
            {
                ShootBullet(projectileDirection.rotation * Quaternion.Euler(0, 0, spreadAngle));
            }


            if (i * 2 < projectileAmount)
            {
                ShootBullet(projectileDirection.rotation * Quaternion.Euler(0, 0, -spreadAngle));
            }
        }
    }

    private void ShootBullet(Quaternion rotation)
    {
        GameObject bullet = Instantiate(projectilePrefab);
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.position = projectileDirection.transform.position;
            bullet.transform.position = projectileDirection.position;
            bullet.transform.rotation = rotation;
            rb.velocity = bullet.transform.right * projectileSpeed;
            bullet.SetActive(true);
        }
    }
}
