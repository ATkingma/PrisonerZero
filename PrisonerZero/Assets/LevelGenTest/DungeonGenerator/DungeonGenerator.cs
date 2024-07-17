using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnRoom;

    [SerializeField]
    private GameObject bossRoom;

    [SerializeField]
    private GameObject wallTile;

    [SerializeField]
    private List<GameObject> roomList;

    [SerializeField]
    private List<GameObject> hallwayList;

    [SerializeField]
    private int maxRooms = 10;

    private int currentRooms;

    private List<SpawnInformation> placementQueue = new();
    private List<Transform> spawnedObjects;
    private List<Hallway> spawnedHallways = new();
    private List<Collider2D> wallColliders = new();

    private bool spawnedBossRoom = false;

    public UnityEvent OnDoneGenerating;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartGeneration();

        if (placementQueue.Count > 0)
        {
            if (placementQueue[0].objectToSpawn.CompareTag("Room"))
                Rooms();
            else if (placementQueue[0].objectToSpawn.CompareTag("Hallway"))
                Hallways();
            else if (placementQueue[0].objectToSpawn.CompareTag("BossRoom"))
                BossRoom();

        }
        else if(spawnedHallways.Count > 0)
        {
            EndGeneration();
        }
    }

    public void StartGeneration()
    {
        spawnedBossRoom = false;
        placementQueue = new();
        spawnedObjects = new();
        spawnedHallways = new();

        currentRooms = 0;
        foreach (Transform child in transform)
            Destroy(child.gameObject);


        if (!CheckRoomPlacement(Vector2.zero, spawnRoom.transform.localScale))
            placementQueue.Add(new SpawnInformation(Vector2.zero, spawnRoom));
    }

    private void EndGeneration()
    {
        if (!spawnedBossRoom)
            StartGeneration();

        CheckHallwayPlacement();
        SpawnWalls();
        ManageColliders();

        OnDoneGenerating?.Invoke();
    }

    private void Rooms()
    {
        if (!CheckRoomPlacement(placementQueue[0].spawnlocation, placementQueue[0].objectToSpawn.transform.localScale))
        {
            Transform spawnedObject = SpawnObject(placementQueue[0]);
            placementQueue.RemoveAt(0);

            if(currentRooms < maxRooms)
                for (int i = 0; i < 4; ++i)
                    MakeHallway(spawnedObject);
        }
        else
        {
            placementQueue.RemoveAt(0);
        }
    }

    private void BossRoom()
    {
        if (!CheckRoomPlacement(placementQueue[0].spawnlocation, placementQueue[0].objectToSpawn.transform.localScale))
        {
            Transform spawnedObject = SpawnObject(placementQueue[0]);
            placementQueue.RemoveAt(0);
        }
        else
        {
            StartGeneration();
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
        if(spawned.CompareTag("Room") || spawned.CompareTag("BossRoom"))
        {
            spawnedObjects.Add(spawned);
        }
        else if(spawned.CompareTag("Hallway"))
        {
            foreach (Transform child in spawned)
                spawnedObjects.Add(child);

            spawnedHallways.Add(spawned.GetComponent<Hallway>());
        }
        return spawned;
    }

    private void MakeRoom(Transform spawnedHallway)
    {
        if (currentRooms >= maxRooms && !spawnedBossRoom)
        {
            SpawnBossRoom(spawnedHallway);
            return;
        }

        GameObject randomRoom = roomList[UnityEngine.Random.Range(0, roomList.Count)];
        Vector2 localScale = randomRoom.transform.localScale;
        float halfWidth = localScale.x / 2;
        float halfHeight = localScale.y / 2;

        Hallway component = spawnedHallway.GetComponent<Hallway>();

        Vector2 leftSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(-halfWidth, 0);
        Vector2 rightSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(halfWidth, 0);
        Vector2 upSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(0, halfHeight);
        Vector2 downSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(0, -halfHeight);
        Vector2 location = Vector2.zero;

        if (component.Positions[0].direction == SpawnDirection.Left && !CheckRoomPlacement(leftSpawnlocation, localScale))
            location = leftSpawnlocation;
        else if (component.Positions[0].direction == SpawnDirection.Right && !CheckRoomPlacement(rightSpawnlocation, localScale))
            location = rightSpawnlocation;
        else if (component.Positions[0].direction == SpawnDirection.Up && !CheckRoomPlacement(upSpawnlocation, localScale))
            location = upSpawnlocation;
        else if (component.Positions[0].direction == SpawnDirection.Down && !CheckRoomPlacement(downSpawnlocation, localScale))
            location = downSpawnlocation;
        else
            currentRooms--;

        placementQueue.Add(new SpawnInformation(location, randomRoom));
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

    private void SpawnBossRoom(Transform spawnedHallway)
    {
        spawnedBossRoom = true;
        Vector2 localScale = bossRoom.transform.localScale;
        float halfWidth = localScale.x / 2;
        float halfHeight = localScale.y / 2;

        Hallway component = spawnedHallway.GetComponent<Hallway>();

        Vector2 leftSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(-halfWidth, 0);
        Vector2 rightSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(halfWidth, 0);
        Vector2 upSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(0, halfHeight);
        Vector2 downSpawnlocation = (Vector2)spawnedHallway.position + component.Positions[0].newSpawnLocation + new Vector2(0, -halfHeight);
        Vector2 location = Vector2.zero;

        if (component.Positions[0].direction == SpawnDirection.Left && !CheckRoomPlacement(leftSpawnlocation, localScale))
            location = leftSpawnlocation;
        else if (component.Positions[0].direction == SpawnDirection.Right && !CheckRoomPlacement(rightSpawnlocation, localScale))
            location = rightSpawnlocation;
        else if (component.Positions[0].direction == SpawnDirection.Up && !CheckRoomPlacement(upSpawnlocation, localScale))
            location = upSpawnlocation;
        else if (component.Positions[0].direction == SpawnDirection.Down && !CheckRoomPlacement(downSpawnlocation, localScale))
            location = downSpawnlocation;
        else
            currentRooms--;

        placementQueue.Add(new SpawnInformation(location, bossRoom));
    }

    private bool CheckRoomPlacement(Vector2 location, Vector2 size) => Physics2D.OverlapAreaAll(location - (size / 2 * .9f), location + (size / 2 * .9f)).Length > 0;

    private void CheckHallwayPlacement()
    {
        spawnedHallways.RemoveAll(x => x == null);
        foreach (Hallway item in spawnedHallways)
            if (!CheckRoomPlacement(item.startCheck.position, Vector2.one) || !CheckRoomPlacement(item.exitCheck.position, Vector2.one))
                Destroy(item.gameObject);

        spawnedHallways = new();
    }

    private void SpawnWalls()
    {
        foreach (Transform obj in spawnedObjects)
        {
            float x = obj.localScale.x / 2 + .5f;
            float y = obj.localScale.y / 2 + .5f;
            Vector2 topLeft = obj.position + new Vector3(-x, y);
            Vector2 topRight = obj.position + new Vector3(x, y);
            Vector2 bottomLeft = obj.position + new Vector3(-x, -y);
            Vector2 bottomRight = obj.position + new Vector3(x, -y);

            DrawWallLine(topLeft, topRight, obj);
            DrawWallLine(topRight, bottomRight, obj);
            DrawWallLine(bottomRight, bottomLeft, obj);
            DrawWallLine(bottomLeft, topLeft, obj);
        }
    }

    private void DrawWallLine(Vector2 start, Vector2 end, Transform parent)
    {
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        int numberOfSquares = Mathf.CeilToInt(distance);

        for (int i = 0; i <= numberOfSquares; i++)
        {
            Vector2 position = start + direction * i;
            if(!CheckRoomPlacement(position, new Vector2(.1f,.1f)))
            {
                Transform spawned = Instantiate(wallTile, position, Quaternion.identity, parent).transform;
                spawned.localScale = new Vector2(1 / parent.localScale.x, 1 / parent.localScale.y);
                wallColliders.Add(spawned.GetComponent<Collider2D>());
            }
        }
    }

    private void ManageColliders()
    {
        foreach (Transform item in spawnedObjects)
            Destroy(item.GetComponent<Collider2D>());

        // colliders on walls
        wallColliders.RemoveAll(x => x == null);
        foreach (Collider2D item in wallColliders)
            item.enabled = true;
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
