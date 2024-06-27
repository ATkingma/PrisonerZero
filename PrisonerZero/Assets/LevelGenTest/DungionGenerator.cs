using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using System.Linq;

public class DungionGenerator : MonoBehaviour
{
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

    private int randomRoomAmount;
    private List<GameObject> roomsList = new List<GameObject>();
    private List<GameObject> importantRooms = new List<GameObject>();

    private Delaunator delaunator;
    private GameObject trianglesContainer;

    private List<LineRenderer> edges = new List<LineRenderer>();

    private List<LineRenderer> playerLine = new List<LineRenderer>();   

    private void Awake()
    {
        Random.InitState(seed);
    }

    void Start()
    {
        StartGeneration();
    }

    private void StartGeneration()
    {
        RandomizeRoomAmount();
        SpawnRooms();
        StartCoroutine(Delaunay(5));
    }

    private void RandomizeRoomAmount()
    {
        randomRoomAmount = Random.Range(minRooms, maxRooms);
    }

    private void SpawnRooms()
    {
        for (int i = 0; i < randomRoomAmount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * circleRadius;
            int randomRoomIndex = Random.Range(0, rooms.Count);
            GameObject room = Instantiate(rooms[randomRoomIndex], randomPos, Quaternion.identity, transform);
            roomsList.Add(room);

            room.AddComponent<BoxCollider2D>();
            room.AddComponent<Rigidbody2D>();

            Rigidbody2D rb = room.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.velocity *= 20;

            BoxCollider2D tempBox = room.GetComponent<BoxCollider2D>();
            Vector2 size = tempBox.bounds.size;
            float averageExtent = (size.x + size.y) / 2.0f;

            if (averageExtent >= bigRoomSize)
            {
                room.GetComponent<SpriteRenderer>().color = Color.red;
                importantRooms.Add(room);
            }
        }
    }

    private void RemoveRoomAttachments()
    {
        foreach (GameObject room in roomsList)
        {
            Destroy(room.GetComponent<Rigidbody2D>());
            Destroy(room.GetComponent<BoxCollider2D>());
        }
    }

    IEnumerator Delaunay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        List<IPoint> points = new List<IPoint>();


        foreach (GameObject obj in importantRooms)
        {
            points.Add(new Point(obj.transform.position.x, obj.transform.position.y));
        }

        delaunator = new Delaunator(points.ToArray());

        VisualizeDelaunay();
        CreateMST();
        SpawnTilesAlongPlayerLine();
    }

    private void RemoveBozoRoom()
    {
        foreach(GameObject obj in roomsList)
        {
            if(!importantRooms.Contains(obj))
            {
                Destroy(obj);
            }  
        }

        foreach (GameObject obj in roomsList)
        {
            if(obj==null)
            {
                roomsList.Remove(obj);
            }
        }
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

        VisualizeMST(parent);
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

    private float Distance(GameObject a, GameObject b)
    {
        return Vector2.Distance(a.transform.position, b.transform.position);
    }

    private void VisualizeMST(int[] parent)
    {
        GameObject MSTEdgeParent = new GameObject("MSTEdges");
        List<LineRenderer> notUssedEdges = new List<LineRenderer>(edges);

        for (int i = 1; i < parent.Length; i++)
        {
            GameObject mstEdgeObject = new GameObject("MSTEdge");
            mstEdgeObject.transform.parent = MSTEdgeParent.transform;
            LineRenderer lineRenderer = mstEdgeObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, importantRooms[i].transform.position+new Vector3(0,0,-2));
            lineRenderer.SetPosition(1, importantRooms[parent[i]].transform.position + new Vector3(0, 0, -2));

            lineRenderer.material.color = Color.green;
            lineRenderer.material.SetColor("_EmissionColor", Color.green);
            playerLine.Add(lineRenderer);


            for (int x = 0; x < notUssedEdges.Count; x++)
            {
                if(lineRenderer.GetPosition(0)== notUssedEdges[x].GetPosition(0)&& lineRenderer.GetPosition(1) == notUssedEdges[x].GetPosition(1))
                {
                    notUssedEdges.Remove(notUssedEdges[x]);
                }
            }
        }

        for (int i = 0; i < notUssedEdges.Count; i++)
        {
            if (ChanceUtility.Chance(edgeBringBackChance))
            {
                GameObject mstEdgeObject = new GameObject("MSTEdge");
                mstEdgeObject.transform.parent = MSTEdgeParent.transform;
                LineRenderer lineRenderer = mstEdgeObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.2f;
                lineRenderer.endWidth = 0.2f;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, notUssedEdges[i].GetPosition(0) + new Vector3(0, 0, -1));
                lineRenderer.SetPosition(1, notUssedEdges[i].GetPosition(1) + new Vector3(0, 0, -1));

                lineRenderer.material.color = Color.green;
                lineRenderer.material.SetColor("_EmissionColor", Color.green);

                playerLine.Add(lineRenderer);
            }
        }
    }

    private void SpawnTilesAlongPlayerLine()
    {
        foreach (LineRenderer lineRenderer in playerLine)
        {
            Vector3 startPos = lineRenderer.GetPosition(0); // Start position of the edge
            Vector3 endPos = lineRenderer.GetPosition(1); // End position of the edge

            // Calculate distance between start and end of the edge
            float edgeLength = Vector3.Distance(startPos, endPos);

            // Number of tiles to spawn along this edge
            int numTiles = Mathf.FloorToInt(edgeLength); // Adjust as needed for spacing

            // Calculate increment vector for tile positions
            Vector3 increment = (endPos - startPos) / numTiles;

            // Spawn tiles along the edge
            for (int i = 0; i < numTiles; i++)
            {
                // Calculate position for the current tile
                Vector3 tilePosition = startPos + increment * i;

                // Instantiate tilePrefab at tilePosition
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform; // Parent to DungionGenerator

                // Optionally, if you want to add a sprite to the t

                // Adjust spriteRenderer properties (sorting layer, etc.) as needed
            }
        }
        MakeSquareLines();
    }

    private void MakeSquareLines()
    {
        RemoveBozoRoom();
        foreach (LineRenderer lineRenderer in playerLine)
        {
            Vector3 startPos = lineRenderer.GetPosition(0); // Start position of the edge
            Vector3 endPos = lineRenderer.GetPosition(1); // End position of the edge

            float diffX = startPos.x - endPos.x;
            float diffY = startPos.y - endPos.y;

            // 20 == width and height;
            if (diffX <= 20 / 2 && diffX >= 0 || diffX > -20 / 2 && diffX <= 0)
            {
                // straight line on y axis
                
                lineRenderer.SetPosition(0, startPos - new Vector3(diffX / 2, 0, -5));
                lineRenderer.SetPosition(1, startPos - new Vector3(diffX / 2, diffY, -5));
            }
            else if(diffY <= 20 / 2 && diffY >= 0 || diffY >= -20 / 2 && diffY <= 0)
            {
                // straight line on x axis

                lineRenderer.SetPosition(0, startPos - new Vector3(0, diffY / 2, -5));
                lineRenderer.SetPosition(1, startPos - new Vector3(diffX, diffY / 2, -5));
            }
            else
            {
                // square line

                lineRenderer.positionCount = 3;
                lineRenderer.SetPosition(0, startPos + new Vector3(0, 0, -5));
                lineRenderer.SetPosition(1, startPos + new Vector3(-diffX, 0, -5));
                lineRenderer.SetPosition(2, endPos + new Vector3(0, 0, -5));
            }
        }
    }
}