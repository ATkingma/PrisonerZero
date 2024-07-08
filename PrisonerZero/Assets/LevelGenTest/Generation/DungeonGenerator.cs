using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab;

    [SerializeField]
    private List<GameObject> rooms;

    [SerializeField]
    [Range(1, 200)]
    private int minRooms;

    [SerializeField]
    [Range(1, 200)]
    private int maxRooms;

    [SerializeField]
    [Range(1, 50)]
    private int bigRoomSize;

    [SerializeField]
    [Range(1, 20)]
    private float circleRadius = 10f;

    [SerializeField]
    [Range(0, 100)]
    private float edgeBringBackChance = 10f;

    [SerializeField]
    private int seed;

    [SerializeField]
    private Sprite floorSprite;

    [SerializeField]
    private GameObject tilePrefab;

    private void Awake()
    {
        // Set seed.
        Random.InitState(seed);
    }

    void Start()
    {
        StartCoroutine(StartGeneration());
    }

    private IEnumerator StartGeneration()
    {
        GameObject roomGenObject = new("RoomGeneration");
        RoomGenerator roomGenerator = roomGenObject.AddComponent<RoomGenerator>();
        roomGenerator.Initialize(roomPrefab, circleRadius, rooms, bigRoomSize, minRooms, maxRooms);

        while (!roomGenerator.DoneGenerating)
            yield return null;

        Dictionary<int, object> returnedObjects = roomGenerator.GetInformation();
        GameObject hallwayGenObject = new("HallwayGeneration");
        HallwayGenerator hallwayGeneration = hallwayGenObject.AddComponent<HallwayGenerator>();
        hallwayGeneration.Initialize(edgeBringBackChance, tilePrefab, (GameObject)returnedObjects[0], (List<LineRenderer>)returnedObjects[1], (List<GameObject>)returnedObjects[2], (int[])returnedObjects[3]);
    }
}
