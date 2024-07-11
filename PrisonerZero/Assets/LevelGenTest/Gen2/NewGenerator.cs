using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    private int wait;

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

    private IEnumerator Generate(Vector2 spawnLocation, SpawnInfo info = null, Hallway hallway = null)
    {
        while (wait > 0)
            yield return null;

        GameObject test = rooms[Random.Range(0, rooms.Count)];
        Vector2 newSpawnPosition = spawnLocation;

        if (info != null)
        {
            Transform spawnTransform = test.transform;
            switch (info.direction)
            {
                case SpawnDirection.Up:
                    newSpawnPosition += new Vector2(0, spawnTransform.localScale.y / 2);
                    break;
                case SpawnDirection.Down:
                    newSpawnPosition += new Vector2(0, -spawnTransform.localScale.y / 2);
                    break;
                case SpawnDirection.Left:
                    newSpawnPosition += new Vector2(-spawnTransform.localScale.x / 2, 0);
                    break;
                case SpawnDirection.Right:
                    newSpawnPosition += new Vector2(spawnTransform.localScale.x / 2, 0);
                    break;
            }
        }

        GameObject spawnRoom = Instantiate(test, newSpawnPosition, Quaternion.identity, transform);
        if (!CheckOverlapRoom(newSpawnPosition, Vector2.zero, spawnRoom, null))
        { 
            spawnedRooms.Add(spawnRoom);

            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(.1f);

                if (count < roomCount - 1)
                {
                    count++;
                    GenerateHallway(spawnRoom, newSpawnPosition);
                }
            }
        }
        else
        {
            Destroy(spawnRoom);

            if (hallway != null)
            {
                spawnedHallways.Remove(hallway);
                Destroy(hallway.gameObject);
            }
        }
    }

    private void GenerateHallway(GameObject origin, Vector2 spawnLocation)
    {
        wait++;
        int randomIndex = Random.Range(0, hallways.Count);

        foreach (SpawnInfo info in hallways[randomIndex].GetComponent<Hallway>().Positions)
        {
            Transform spawnTransform = origin.transform;
            switch (info.direction)
            {
                case SpawnDirection.Up:
                    spawnLocation += new Vector2(0, spawnTransform.localScale.y / 2);
                    break;
                case SpawnDirection.Down:
                    spawnLocation += new Vector2(0, -spawnTransform.localScale.y / 2);
                    break;
                case SpawnDirection.Left:
                    spawnLocation += new Vector2(-spawnTransform.localScale.x / 2, 0);
                    break;
                case SpawnDirection.Right:
                    spawnLocation += new Vector2(spawnTransform.localScale.x / 2, 0);
                    break;
            }

            if (!CheckOverlapRoom(spawnLocation, info.newSpawnLocation, origin, hallways[randomIndex]))
            {
                Hallway newHallway = Instantiate(hallways[randomIndex], spawnLocation, Quaternion.identity, transform).GetComponent<Hallway>();
                spawnedHallways.Add(newHallway);

                StartCoroutine(Generate(spawnLocation + info.newSpawnLocation, info, newHallway));
            }
            else
            {
                count--;
            }
        }
        wait--;
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
        if(hallwayDimensions != null)
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
