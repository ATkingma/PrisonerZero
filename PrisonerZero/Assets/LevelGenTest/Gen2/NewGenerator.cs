using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewGenerator : MonoBehaviour
{
    [SerializeField]
    private int roomCount;

    [SerializeField]
    private float roomWidth = 25;

    [SerializeField]
    private GameObject room;

    [SerializeField]
    private List<GameObject> hallways;

    private List<GameObject> spawnedRooms;
    private List<Hallway> spawnedHallways;
    private int count;

    private void Start()
    {
        spawnedRooms = new List<GameObject>();
        spawnedHallways = new List<Hallway>();
        StartCoroutine(Generate(Vector2.zero));
    }

    private IEnumerator Generate(Vector2 spawnLocation)
    {
        GameObject spawnRoom = Instantiate(room, spawnLocation, Quaternion.identity);
        spawnedRooms.Add(spawnRoom);

        yield return new WaitForSeconds(.1f);
        StartCoroutine(GenerateHallway(spawnLocation));

        if (count == roomCount)
            StopAllCoroutines();
        count++;
    }

    private bool InBoundsOfRoomForRoom(Vector2 position)
    {
        foreach (GameObject room in spawnedRooms)
        {
            Vector2 roomPosition = room.transform.position;
            if (position.x >= roomPosition.x - roomWidth &&
                position.x <= roomPosition.x + roomWidth &&
                position.y >= roomPosition.y - roomWidth &&
                position.y <= roomPosition.y + roomWidth)
            {
                return true;
            }
        }
        return false;
    }

    private bool InBoundsHallwayForRoom(Vector2 position)
    {
        foreach (Hallway hallway in spawnedHallways)
        {
            foreach (Transform transform in hallway.Boundries)
            {
                Vector2 hallwayPosition = transform.position;
                Vector2 hallwayDimension = transform.localScale / 2;
                if (position.x >= hallwayPosition.x - hallwayDimension.x &&
                    position.x <= hallwayPosition.x + hallwayDimension.x &&
                    position.y >= hallwayPosition.y - hallwayDimension.y &&
                    position.y <= hallwayPosition.y + hallwayDimension.y)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator GenerateHallway(Vector2 spawnLocation)
    {
        for (int i = 0; i < hallways.Count; i++)
        {
            if (Random.Range(0, 101) > 40)
            {
                //spawn hallway
                if (!InBoundsOfRoomForHallway(hallways[i]) && !InBoundsHallwayForHallway(hallways[i]))
                {
                    Hallway newHallway = Instantiate(hallways[i], spawnLocation, Quaternion.identity).GetComponent<Hallway>();
                    spawnedHallways.Add(newHallway);
                    foreach (Vector2 position in newHallway.Positions)
                        if (!InBoundsOfRoomForRoom(spawnLocation + position) && !InBoundsHallwayForRoom(spawnLocation + position))
                            StartCoroutine(Generate(spawnLocation + position));
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private bool InBoundsOfRoomForHallway(GameObject hallwayObject)
    {
        Vector2 position = hallwayObject.transform.position + hallwayObject.transform.GetChild(0).localPosition;
        foreach (GameObject room in spawnedRooms)
        {
            Vector2 roomPosition = room.transform.position;
            if (position.x >= roomPosition.x - roomWidth &&
                position.x <= roomPosition.x + roomWidth &&
                position.y >= roomPosition.y - roomWidth &&
                position.y <= roomPosition.y + roomWidth)
            {
                return true;
            }
        }
        return false;
    }

    private bool InBoundsHallwayForHallway(GameObject hallwayObject)
    {
        Vector2 position = hallwayObject.transform.position + hallwayObject.transform.GetChild(0).localPosition;
        foreach (Hallway hallway in spawnedHallways)
        {
            foreach (Transform transform in hallway.Boundries)
            {
                Vector2 hallwayPosition = transform.position;
                Vector2 hallwayDimension = transform.localScale / 2;
                if (position.x >= hallwayPosition.x - hallwayDimension.x &&
                    position.x <= hallwayPosition.x + hallwayDimension.x &&
                    position.y >= hallwayPosition.y - hallwayDimension.y &&
                    position.y <= hallwayPosition.y + hallwayDimension.y)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
