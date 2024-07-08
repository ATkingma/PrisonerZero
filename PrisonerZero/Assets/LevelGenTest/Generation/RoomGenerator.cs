using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;

public class RoomGenerator : MonoBehaviour
{
    // given
    private GameObject roomPrefab;
    private float circleRadius;
    private List<GameObject> rooms;
    private int bigRoomSize;
    private int minRooms;
    private int maxRooms;

    // created
    private int randomRoomAmount;
    private Delaunator delaunator;
    private GameObject trianglesContainer;
    private List<GameObject> roomsList = new();
    private List<GameObject> importantRooms = new();
    private List<LineRenderer> edges = new();
    private int[] parents;

    public bool DoneGenerating { get; private set; }

    public Dictionary<int, object> GetInformation()
    {
        Dictionary<int, object> returnList = new()
        {
            { 0, trianglesContainer },
            { 1, edges },
            { 2, importantRooms },
            { 3, parents },
        };

        return returnList;
    }

    public void Initialize(GameObject roomPrefab, float circleRadius, List<GameObject> rooms, int bigRoomSize, int minRooms, int maxRooms)
    {
        this.roomPrefab = roomPrefab;
        this.circleRadius = circleRadius;
        this.rooms = rooms;
        this.bigRoomSize = bigRoomSize;
        this.minRooms = minRooms;
        this.maxRooms = maxRooms;

        SpawnRooms();
    }

    private void SpawnRooms()
    {
        randomRoomAmount = Random.Range(minRooms, maxRooms);

        importantRooms = new List<GameObject>();
        for (int i = 0; i < randomRoomAmount; i++)
        {
            bool test = false;
            Vector3 spawnLocation = Vector3.zero;
            while (!test)
            {
                spawnLocation = new Vector3(Random.Range(-randomRoomAmount * 20, randomRoomAmount * 20), Random.Range(-randomRoomAmount * 20, randomRoomAmount * 20), 0);
                test = !InBoundsOfRoom(spawnLocation);
            }

            GameObject spawnRoom = Instantiate(roomPrefab, spawnLocation, Quaternion.identity);
            importantRooms.Add(spawnRoom);
        }

        Delaunay();
    }

    private bool InBoundsOfRoom(Vector3 position)
    {
        foreach (GameObject room in importantRooms)
        {
            Vector3 roomPosition = room.transform.position;
            if (position.x >= roomPosition.x - 25 && position.x <= roomPosition.x + 25 &&
                position.y >= roomPosition.y - 25 && position.y <= roomPosition.y + 25)
            {
                return true;
            }
        }
        return false;
    }

    private void Delaunay()
    {
        List<IPoint> points = new List<IPoint>();

        foreach (GameObject obj in importantRooms)
            points.Add(new Point(obj.transform.position.x, obj.transform.position.y));

        delaunator = new Delaunator(points.ToArray());

        VisualizeDelaunay();
        CreateMST();
        RemoveBozoRoom();
        RemoveRoomAttachments();

        DoneGenerating = true;
    }

    private void VisualizeDelaunay()
    {
        if (delaunator == null) return;

        if (trianglesContainer == null)
        {
            trianglesContainer = new GameObject("DelaunayTriangles");
        }
        else
        {
            foreach (Transform child in trianglesContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        delaunator.ForEachTriangleEdge(edge =>
        {
            Vector3 p1 = edge.P.ToVector3();
            p1.z = -1;
            Vector3 p2 = edge.Q.ToVector3();
            p2.z = -1;

            GameObject triangleEdgeObject = new GameObject("TriangleEdge");
            triangleEdgeObject.transform.parent = trianglesContainer.transform;
            LineRenderer lineRenderer = triangleEdgeObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, p1);
            lineRenderer.SetPosition(1, p2);
            lineRenderer.material.color = Color.black;
            lineRenderer.material.SetColor("_EmissionColor", Color.black);

            edges.Add(lineRenderer);
        });
    }

    private void CreateMST()
    {
        int n = importantRooms.Count;
        bool[] inMST = new bool[n];
        float[] key = new float[n];
        int[] parent = new int[n];

        for (int i = 0; i < n; i++)
        {
            key[i] = float.MaxValue;
            parent[i] = -1;
        }

        key[0] = 0f;

        for (int count = 0; count < n - 1; count++)
        {
            int u = MinKey(key, inMST);
            inMST[u] = true;

            for (int v = 0; v < n; v++)
            {
                if (!inMST[v] && Distance(importantRooms[u], importantRooms[v]) < key[v])
                {
                    parent[v] = u;
                    key[v] = Distance(importantRooms[u], importantRooms[v]);
                }
            }
        }

        parents = parent;
    }

    private int MinKey(float[] key, bool[] inMST)
    {
        float min = float.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < key.Length; v++)
        {
            if (!inMST[v] && key[v] < min)
            {
                min = key[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    private void RemoveBozoRoom()
    {
        foreach (GameObject obj in roomsList)
        {
            if (!importantRooms.Contains(obj))
            {
                Destroy(obj);
            }
        }

        foreach (GameObject obj in roomsList)
        {
            if (obj == null)
            {
                roomsList.Remove(obj);
            }
        }
    }

    private float Distance(GameObject a, GameObject b)
    {
        return Vector2.Distance(a.transform.position, b.transform.position);
    }

    private void RemoveRoomAttachments()
    {
        foreach (GameObject room in roomsList)
        {
            Destroy(room.GetComponent<Rigidbody2D>());
            Destroy(room.GetComponent<BoxCollider2D>());
        }
    }
}
