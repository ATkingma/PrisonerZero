using System;
using System.Collections.Generic;
using System.ComponentModel;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnRoom;

    [SerializeField]
    private GameObject bossRoom;

    [SerializeField]
    private List<GameObject> roomList;

    [SerializeField]
    private List<GameObject> hallwayList;

    [SerializeField]
    private int maxRooms;
    private int currentRooms = 1000000;

    private List<SpawnInformation> placementQueue;
    private List<Collider2D> spawned;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            placementQueue = new();
            spawned = new();

            currentRooms = 0;
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            GameObject randomRoom = roomList[UnityEngine.Random.Range(0, roomList.Count)];
            Vector2 spawnlocation = Vector2.zero;

            if(!CheckRoomPlacement(spawnlocation, randomRoom.transform.localScale))
                placementQueue.Add(new SpawnInformation(spawnlocation, randomRoom));
        }

        if (currentRooms < maxRooms && placementQueue.Count > 0)
        {
            if (placementQueue[0].objectToSpawn.CompareTag("Room"))
            { 
                if (!CheckRoomPlacement(placementQueue[0].spawnlocation, placementQueue[0].objectToSpawn.transform.localScale))
                {
                    Transform spawnedObject = SpawnObject(placementQueue[0]);
                    placementQueue.RemoveAt(0);

                    for (int i = 0; i < 4; ++i)
                        MakeHallway(spawnedObject, i);
                }
                else
                {
                    placementQueue.RemoveAt(0);
                }
            }
            else if(placementQueue[0].objectToSpawn.CompareTag("Hallway"))
            {
                Vector2 localScale = placementQueue[0].objectToSpawn.transform.GetChild(0).localScale;
                print((localScale, placementQueue[0].spawnlocation));
                if (!CheckRoomPlacement(placementQueue[0].spawnlocation, localScale))
                {
                    Transform spawnedObject = SpawnObject(placementQueue[0]);
                    placementQueue.RemoveAt(0);
                    MakeRoom(spawnedObject);
                }
                else
                {
                    placementQueue.RemoveAt(0);
                }
            }
        }
    }

    private Transform SpawnObject(SpawnInformation spawnInformation)
    {
        return Instantiate(spawnInformation.objectToSpawn, spawnInformation.spawnlocation, Quaternion.identity, transform).transform;
    }

    private void MakeRoom(Transform spawnedHallway)
    {
        GameObject randomRoom = roomList[UnityEngine.Random.Range(0, roomList.Count)];
        Vector2 localScale = randomRoom.transform.localScale;
        float halfWidth = localScale.x / 2;
        float halfHeight = localScale.y / 2;

        Hallway component = spawnedHallway.GetComponent<Hallway>();

        Vector2 leftSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(-halfWidth, 0);
        Vector2 rightSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(halfWidth, 0);
        Vector2 upSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(0, halfHeight);
        Vector2 downSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(0, -halfHeight);

        if (component.Positions[0].direction == SpawnDirection.Left && !CheckRoomPlacement(leftSpawnlocation, localScale))
            placementQueue.Add(new SpawnInformation(leftSpawnlocation, randomRoom));
        else if (component.Positions[0].direction == SpawnDirection.Right && !CheckRoomPlacement(rightSpawnlocation, localScale))
            placementQueue.Add(new SpawnInformation(rightSpawnlocation, randomRoom));
        else if (component.Positions[0].direction == SpawnDirection.Up && !CheckRoomPlacement(upSpawnlocation, localScale))
            placementQueue.Add(new SpawnInformation(upSpawnlocation, randomRoom));
        else if (component.Positions[0].direction == SpawnDirection.Down && !CheckRoomPlacement(downSpawnlocation, localScale))
            placementQueue.Add(new SpawnInformation(downSpawnlocation, randomRoom));
        else
            currentRooms--;
        currentRooms++;
    }

    private void MakeHallway(Transform spawnedObject, int hallwayIndex)
    {
        Vector2 newSpawnlocation = spawnedObject.position;
        float halfWidthRoom = spawnedObject.localScale.x / 2;
        float halfHeightRoom = spawnedObject.localScale.y / 2;

        GameObject newHallway = hallwayList[UnityEngine.Random.Range(0, hallwayList.Count)];
        Hallway component = newHallway.GetComponent<Hallway>();
        Vector2 localScale = newHallway.transform.GetChild(0).localScale;
        float halfWidthHallway = localScale.x / 2;
        float halfHeightHallway = localScale.y / 2;

        Vector2 leftSpawnlocation = newSpawnlocation - new Vector2(halfWidthRoom, 0);
        Vector2 rightSpawnlocation = newSpawnlocation + new Vector2(halfWidthRoom, 0);
        Vector2 upSpawnlocation = newSpawnlocation + new Vector2(0, halfHeightRoom);
        Vector2 downSpawnlocation = newSpawnlocation - new Vector2(0, halfHeightRoom);

        SpawnDirection direction = component.Positions[0].direction;

        if (direction == SpawnDirection.Left && !CheckRoomPlacement(leftSpawnlocation + new Vector2(-halfWidthHallway,0), localScale))
            placementQueue.Add(new SpawnInformation(leftSpawnlocation, newHallway));
        else if (direction == SpawnDirection.Right && !CheckRoomPlacement(rightSpawnlocation + new Vector2(halfWidthHallway, 0), localScale))
            placementQueue.Add(new SpawnInformation(rightSpawnlocation, newHallway));
        else if (direction == SpawnDirection.Up && !CheckRoomPlacement(upSpawnlocation + new Vector2(0, halfHeightHallway), localScale))
            placementQueue.Add(new SpawnInformation(upSpawnlocation, newHallway));
        else if (direction == SpawnDirection.Down && !CheckRoomPlacement(downSpawnlocation + new Vector2(0, -halfHeightHallway), localScale))
            placementQueue.Add(new SpawnInformation(downSpawnlocation, newHallway));
    }

    private bool CheckRoomPlacement(Vector2 location, Vector2 size)
    {
        print(location - (size / 2 * .9f));
        print(location + (size / 2 * .9f));
        if (Physics2D.OverlapAreaAll(location - (size / 2 * .9f), location + (size / 2 * .9f)).Length > 0)
            return true;

        return false;
    }
}

[Serializable]
public class SpawnInformation
{
    public Vector2 spawnlocation;
    public GameObject objectToSpawn;

    public SpawnInformation(Vector2 spawnlocation, GameObject objectToSpawn)
    {
        this.spawnlocation = spawnlocation;
        this.objectToSpawn = objectToSpawn;
    }
}
