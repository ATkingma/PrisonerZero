using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private float speed;

    private float damage;

    void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        if (_hitInfo.GetComponent<BaseHealth>())
        {
            _hitInfo.GetComponent<BaseHealth>().DoDamage(damage);
        }
        gameObject.SetActive(false);
        ResetBullet();
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
        transform.position = Vector2.zero;
        rb.velocity = transform.right * speed;
    }

    private void OnEnable()
    {
        rb.velocity = transform.right * speed;
    }
}