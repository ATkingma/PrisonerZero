using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private float speed;

    private float damage;

    private float baseSpeed = 20;

    private int currentBounces;
    private int currentPierces; 


    void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        if (_hitInfo.GetComponent<BaseHealth>())
        {
            _hitInfo.GetComponent<BaseHealth>().DoDamage(damage);

            if (currentPierces > 0)
            {
                currentPierces--;
            }
            else
            {
                gameObject.SetActive(false);
                ResetBullet();
            }
        }
        else
        {
            if (currentBounces > 0)
            {
                ResetBullet();
                Debug.Log(_hitInfo.gameObject.name);
                currentBounces--;
                List<ContactPoint2D> contacts = new List<ContactPoint2D>();
                _hitInfo.GetContacts(contacts);
                if (contacts.Count > 0)
                {
                    Vector2 reflectDir = Vector2.Reflect(rb.velocity, contacts[0].normal);
                    rb.velocity = reflectDir.normalized * speed;
                    Debug.Log("bounce");
                }
            }
            else
            {
                gameObject.SetActive(false);
                ResetBullet();
            }
        }
    }

    public void SetSpeed(float _speed)
    {
        this.speed = _speed;
    }

    public void SetDamage(float _damage)
    {
        this.damage = _damage;
    }

    public void ResetBullet()
    {
        rb.velocity = Vector2.zero;
        rb.velocity = transform.right * speed;
    }

    private void OnEnable()
    {
        currentBounces = BaseWeapon.Instance.BulletBounces;
        currentPierces = BaseWeapon.Instance.BulletPierce;

        if (speed <= 0) {
            speed = baseSpeed;
        }
        rb.velocity = transform.right * speed;
    }
}