using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private float speed;

    void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        gameObject.SetActive(false);
    }

    public void SetSpeed(float _speed)
    {
        this.speed = _speed;
    }

    public void ResetBullet()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void OnEnable()
    {
        rb.velocity = transform.right * speed;
    }
}
