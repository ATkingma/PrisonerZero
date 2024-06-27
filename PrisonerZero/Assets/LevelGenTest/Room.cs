using System.Collections.Generic;
using UnityEngine;

public class Room: MonoBehaviour
{
    public Vector2 position;
    public Vector2 size;
    public GameObject roomObject;
    public List<GameObject> walls;

    public Room(GameObject roomObj, List<GameObject> wallObjects)
    {
        roomObject = roomObj;
        position = roomObj.transform.position;
        size = roomObj.GetComponent<BoxCollider2D>().bounds.size;
        walls = new List<GameObject>(wallObjects);
    }

    public void RemoveWall(Vector2 wallPosition)
    {
        foreach (GameObject wall in walls)
        {
            if ((Vector2)wall.transform.position == wallPosition)
            {
                wall.SetActive(false);
                break;
            }
        }
    }

    public bool Intersects(Room other)
    {
        return !(position.x + size.x / 2 < other.position.x - other.size.x / 2 ||
                 position.x - size.x / 2 > other.position.x + other.size.x / 2 ||
                 position.y + size.y / 2 < other.position.y - other.size.y / 2 ||
                 position.y - size.y / 2 > other.position.y + other.size.y / 2);
    }
}
