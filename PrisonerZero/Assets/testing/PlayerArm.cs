using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;

    private void Start()
    {
        Instantiate(WeaponManager.Instance.GetWeapon().GetWeaponPrefab(),objectToRotate);
    }

    void Update()
    {
        RotateObject();
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
}
