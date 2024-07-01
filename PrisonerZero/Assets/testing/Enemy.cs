using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private float damage;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float knockBack;

    [SerializeField]
    private float stunTime;

    [SerializeField]
    private LayerMask targetLayerMask;

    private bool addKnockback = false;

    private void Start()
    {
        if (agent == null)
            return;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned.");
            return;
        }
        if (agent!=null&&agent.enabled)
        {
            agent.SetDestination(target.position);
        }

        if (addKnockback)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        if (_hitInfo.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BaseHealth playerHealth = _hitInfo.GetComponent<BaseHealth>();
            if (playerHealth != null)
            {
                playerHealth.DoDamage(damage);
                StartCoroutine(ApplyKnockbackAndStun(_hitInfo));
            }
        }
    }

    private IEnumerator ApplyKnockbackAndStun(Collider2D _hitInfo)
    {
        agent.enabled = false;

        Vector2 knockbackDirection =   transform.position- _hitInfo.transform.position;
        knockbackDirection.Normalize();
        rb.AddForce(knockbackDirection * knockBack, ForceMode2D.Impulse);

        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        agent.enabled = true;
    }
}
