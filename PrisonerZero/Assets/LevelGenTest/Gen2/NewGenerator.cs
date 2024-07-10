using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewGenerator : MonoBehaviour
{
    [SerializeField]
    private int roomCount;

    [SerializeField]
    private List<GameObject> rooms;

    [SerializeField]
    private List<GameObject> hallways;

    private List<GameObject> spawnedRooms;
    private List<Hallway> spawnedHallways;
    private int count;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            count = 0;
            spawnedRooms = new List<GameObject>();
            spawnedHallways = new List<Hallway>();
            StartCoroutine(Generate(Vector2.zero));
        }
    }

    private IEnumerator Generate(Vector2 spawnLocation)
    {
        GameObject spawnRoom = Instantiate(rooms[Random.Range(0, rooms.Count)], spawnLocation, Quaternion.identity, transform);
        spawnedRooms.Add(spawnRoom);

        yield return new WaitForEndOfFrame();
        StartCoroutine(GenerateHallway(spawnRoom, spawnLocation));

        if (count >= roomCount)
            StopAllCoroutines();
        count++;
    }

    private IEnumerator GenerateHallway(GameObject origin, Vector2 spawnLocation)
    {
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, hallways.Count);
            foreach (Vector2 position in hallways[randomIndex].GetComponent<Hallway>().Positions)
            {
                if (Random.Range(0, 101) > 0 && !CheckOverlapRoom(spawnLocation, position, origin, hallways[randomIndex]))
                {
                    Hallway newHallway = Instantiate(hallways[randomIndex], spawnLocation, Quaternion.identity, transform).GetComponent<Hallway>();
                    spawnedHallways.Add(newHallway);
                    StartCoroutine(Generate(spawnLocation + position));
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    private bool CheckOverlapRoom(Vector2 spawnLocation, Vector2 addedPosition, GameObject origin, GameObject hallwayDimensions)
    {
        List<Collider2D> colliders = new();
        colliders.AddRange(spawnedRooms.Select(hallway => hallway.GetComponent<Collider2D>())
                           .ToList());
        colliders.AddRange(spawnedHallways.SelectMany(hallway => Enumerable.Range(0, hallway.transform.childCount)
                           .Select(i => hallway.transform.GetChild(i).GetComponent<Collider2D>()))
                           .ToList());

        List<Transform> boundies = new();
        boundies.AddRange(origin.GetComponent<Room>().Boundries);
        boundies.AddRange(hallwayDimensions.GetComponent<Hallway>().Boundries);

        foreach (Collider2D room in colliders)
        {
            if (room.gameObject == origin)
                continue;

            float width = origin.transform.localScale.x / 2;
            float height = origin.transform.localScale.y / 2;
            if (room.OverlapPoint(spawnLocation + addedPosition + new Vector2(width, height)) ||
                room.OverlapPoint(spawnLocation + addedPosition + new Vector2(-width, height)) ||
                room.OverlapPoint(spawnLocation + addedPosition + new Vector2(width, -height)) ||
                room.OverlapPoint(spawnLocation + addedPosition + new Vector2(-width, -height)))
                return true;

            foreach (Transform boundry in boundies)
            {
                float halfX = boundry.transform.localScale.x / 2;
                float halfY = boundry.transform.localScale.y / 2;

                Vector2 position = spawnLocation + (Vector2)boundry.transform.localPosition;

                if (room.OverlapPoint(position + new Vector2(halfX, halfY)) ||
                    room.OverlapPoint(position + new Vector2(-halfX, halfY)) ||
                    room.OverlapPoint(position + new Vector2(halfX, -halfY)) ||
                    room.OverlapPoint(position + new Vector2(-halfX, -halfY)))
                    return true;
            }
        }
        return false;
    }
}
