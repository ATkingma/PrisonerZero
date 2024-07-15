using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

    private List<SpawnInformation> placementQueue = new();
    private List<Collider2D> spawnedObjects;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            placementQueue = new();
            spawnedObjects = new();

            currentRooms = 0;
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            GameObject randomRoom = roomList[UnityEngine.Random.Range(0, roomList.Count)];
            Vector2 spawnlocation = Vector2.zero;

            if(!CheckRoomPlacement(spawnlocation, randomRoom.transform.localScale))
                placementQueue.Add(new SpawnInformation(spawnlocation, randomRoom));
        }

        if (placementQueue.Count > 0)
        {
            if (placementQueue[0].objectToSpawn.CompareTag("Room"))
                Rooms();
            else if (placementQueue[0].objectToSpawn.CompareTag("Hallway"))
                Hallways();
        }
    }

    private void Rooms()
    {
        if (!CheckRoomPlacement(placementQueue[0].spawnlocation, placementQueue[0].objectToSpawn.transform.localScale))
        {
            Transform spawnedObject = SpawnObject(placementQueue[0]);
            placementQueue.RemoveAt(0);

            if(currentRooms < maxRooms)
                for (int i = 0; i < 2; ++i)
                    MakeHallway(spawnedObject);
        }
        else
        {
            placementQueue.RemoveAt(0);
        }
    }

    private void Hallways()
    {
        Vector2 localScale = placementQueue[0].objectToSpawn.transform.GetChild(0).localScale;
        float halfWidthHallway = localScale.x / 2;
        float halfHeightHallway = localScale.y / 2;

        SpawnDirection direction = placementQueue[0].objectToSpawn.GetComponent<Hallway>().Positions[0].direction;

        Hallway component = placementQueue[0].objectToSpawn.GetComponent<Hallway>();

        SpawnInformation nextSpawnRoom = placementQueue.FirstOrDefault(x => x.objectToSpawn.CompareTag("Room"));
        Vector2 roomScale = nextSpawnRoom == null ? roomList[0].transform.localScale : nextSpawnRoom.objectToSpawn.transform.localScale;
        float halfWidth = roomScale.x / 2;
        float halfHeight = roomScale.y / 2;

        // remove hallways die nergens naatoe gaan.
        Vector2 checkLocation = Vector2.zero;
        Vector2 checkNextRoomLocation = Vector2.zero;
        if (direction == SpawnDirection.Left)
        {
            checkLocation = placementQueue[0].spawnlocation + new Vector2(-halfWidthHallway, 0);
            checkNextRoomLocation = placementQueue[0].spawnlocation + component.Positions[0].newSpawnLocation + new Vector2(-halfWidth, 0);
        }
        else if (direction == SpawnDirection.Right)
        {
            checkLocation = placementQueue[0].spawnlocation + new Vector2(halfWidthHallway, 0);
            checkNextRoomLocation = placementQueue[0].spawnlocation + component.Positions[0].newSpawnLocation + new Vector2(halfWidth, 0);
        }
        else if (direction == SpawnDirection.Up)
        {
            checkLocation = placementQueue[0].spawnlocation + new Vector2(0, halfHeightHallway);
            checkNextRoomLocation = placementQueue[0].spawnlocation + component.Positions[0].newSpawnLocation + new Vector2(0, halfHeight);
        }
        else if (direction == SpawnDirection.Down)
        {
            checkLocation = placementQueue[0].spawnlocation + new Vector2(0, -halfHeightHallway);
            checkNextRoomLocation = placementQueue[0].spawnlocation + component.Positions[0].newSpawnLocation + new Vector2(0, -halfHeight);
        }

        if (!CheckRoomPlacement(checkLocation, localScale) && !CheckRoomPlacement(checkNextRoomLocation, roomScale))
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

    private Transform SpawnObject(SpawnInformation spawnInformation)
    {
        Transform spawned = Instantiate(spawnInformation.objectToSpawn, spawnInformation.spawnlocation, Quaternion.identity, transform).transform;
        if(spawned.CompareTag("Room"))
        {
            spawnedObjects.Add(spawned.GetComponent<Collider2D>());
        }
        else if(spawned.CompareTag("Room"))
        {

        }

        return spawned;
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

    private void MakeHallway(Transform spawnedObject)
    {
        if (currentRooms >= maxRooms)
            return;

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

    private bool CheckRoomPlacement(Vector2 location, Vector2 size) => Physics2D.OverlapAreaAll(location - (size / 2 * .9f), location + (size / 2 * .9f)).Length > 0;
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
