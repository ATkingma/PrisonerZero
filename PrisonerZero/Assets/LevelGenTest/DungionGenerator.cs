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

    private int randomRoomAmount;
    private List<GameObject> roomsList = new List<GameObject>();
    private List<GameObject> importanMister = new List<GameObject>();

    // Delaunay triangulation variables
    private Delaunator delaunator;
    private GameObject trianglesContainer;

    void Start()
    {
        StartGeneration();
    }

    private void StartGeneration()
    {
        RandomizeRoomAmount();
        SpawnRooms();
        StartCoroutine(Delaunay(2));
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

            BoxCollider2D tempBox = room.GetComponent<BoxCollider2D>();
            Vector2 size = tempBox.bounds.size;
            float averageExtent = (size.x + size.y) / 2.0f;

            if (averageExtent >= bigRoomSize)
            {
                room.GetComponent<SpriteRenderer>().color = Color.red;
                importanMister.Add(room);
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

        // Extract positions of important rooms for Delaunay triangulation
        List<IPoint> points = new List<IPoint>();


        foreach (GameObject obj in importanMister)
        {
            points.Add(new Point(obj.transform.position.x, obj.transform.position.y));
        }

        delaunator = new Delaunator(points.ToArray());

        // Visualize the Delaunay triangulation (optional)
        VisualizeDelaunay();
        RemoveRoomAttachments();
    }

    private void VisualizeDelaunay()
    {
        if (delaunator == null) return;

        // Create container for triangles if not already created
        if (trianglesContainer == null)
        {
            trianglesContainer = new GameObject("DelaunayTriangles");
        }
        else
        {
            // Clear existing triangles
            foreach (Transform child in trianglesContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Iterate through each triangle and create visual representation
        delaunator.ForEachTriangleEdge(edge =>
        {
            Vector3 p1 = edge.P.ToVector3();
            p1.z = -1;
            Vector3 p2 = edge.Q.ToVector3();
            p2.z = -1;

            // Create line renderer for triangle edge
            GameObject triangleEdgeObject = new GameObject("TriangleEdge");
            triangleEdgeObject.transform.parent = trianglesContainer.transform;
            LineRenderer lineRenderer = triangleEdgeObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, p1);
            lineRenderer.SetPosition(1, p2);
            lineRenderer.material.color = Color.green;
        });
    }
}