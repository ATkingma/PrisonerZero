using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private SpriteRenderer spriteRenderer; 

    private void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing from this GameObject.");
        }
     
        if (target == null)
        {
            Debug.LogError("Target Transform is not assigned.");
        }
    }

    private void Update()
    {
        if (target != null && spriteRenderer != null)
        {
            if (target.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}