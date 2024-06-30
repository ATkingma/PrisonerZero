using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public float speed;

    void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        gameObject.SetActive(false);
        ResetBullet();
    }

    public void SetSpeed(float _speed)
    {
        this.speed = _speed;
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