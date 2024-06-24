using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate; // Reference to the object to rotate
    [SerializeField] private Weapon currentWeapon; // Reference to the weapon

    void Update()
    {
        RotateObject();
        HandleShooting();
    }

    void RotateObject()
    {
        if (objectToRotate == null)
        {
            Debug.LogWarning("Object to rotate is not assigned.");
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - objectToRotate.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        objectToRotate.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void HandleShooting()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("Current weapon is not assigned.");
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            currentWeapon.Shoot();
        }
    }
}
