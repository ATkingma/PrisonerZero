using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayGenerator : MonoBehaviour
{
    private float edgeBringBackChance;
    private GameObject tilePrefab;
    private GameObject trianglesContainer;
    private List<LineRenderer> edges = new List<LineRenderer>();
    private List<GameObject> importantRooms = new List<GameObject>();
    private int[] parents;

    private GameObject MSTEdgeParent;
    private List<LineRenderer> playerLine = new List<LineRenderer>();

    public void Initialize(float edgeBringBackChance, GameObject tilePrefab, GameObject trianglesContainer, List<LineRenderer> edges, List<GameObject> importantRooms, int[] parents)
    {
        this.edgeBringBackChance = edgeBringBackChance;
        this.tilePrefab = tilePrefab;
        this.trianglesContainer = trianglesContainer;
        this.edges = edges;
        this.importantRooms = importantRooms;
        this.parents = parents;

        VisualizeMST();
        MakeSquareLines();
        SpawnTilesAlongPlayerLine();
    }

    private void VisualizeMST()
    {
        MSTEdgeParent = new GameObject("MSTEdges");
        List<LineRenderer> notUssedEdges = new List<LineRenderer>(edges);

        for (int i = 1; i < parents.Length; i++)
        {
            GameObject mstEdgeObject = new GameObject("MSTEdge");
            mstEdgeObject.transform.parent = MSTEdgeParent.transform;
            LineRenderer lineRenderer = mstEdgeObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, importantRooms[i].transform.position + new Vector3(0, 0, -2));
            lineRenderer.SetPosition(1, importantRooms[parents[i]].transform.position + new Vector3(0, 0, -2));

            lineRenderer.material.color = Color.green;
            lineRenderer.material.SetColor("_EmissionColor", Color.green);
            playerLine.Add(lineRenderer);


            for (int x = 0; x < notUssedEdges.Count; x++)
            {
                if (lineRenderer.GetPosition(0) == notUssedEdges[x].GetPosition(0) && lineRenderer.GetPosition(1) == notUssedEdges[x].GetPosition(1))
                {
                    notUssedEdges.Remove(notUssedEdges[x]);
                }
            }
        }

        for (int i = 0; i < notUssedEdges.Count - 1; i++)
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
            for (int count = 0; count < lineRenderer.positionCount - 1; count++)
            {
                Vector3 startPos = lineRenderer.GetPosition(count); // Start position of the edge
                Vector3 endPos = lineRenderer.GetPosition(count + 1); // End position of the edge

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
                    if (!InBoundsOfRoom(tilePosition))
                    {
                        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                        tile.transform.parent = transform; // Parent to DungionGenerator
                    }
                }
            }
        }
        Destroy(MSTEdgeParent);
        Destroy(trianglesContainer);
    }

    private bool InBoundsOfRoom(Vector3 position)
    {
        foreach (GameObject room in importantRooms)
        {
            Vector3 roomPosition = room.transform.position;
            if (position.x >= roomPosition.x - 20 / 2 && position.x <= roomPosition.x + 20 / 2 &&
                position.y >= roomPosition.y - 20 / 2 && position.y <= roomPosition.y + 20 / 2)
            {
                return true;
            }
        }
        return false;
    }

    private void MakeSquareLines()
    {
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

                lineRenderer.SetPosition(0, startPos - new Vector3(diffX, 0 / 2, -5));
                lineRenderer.SetPosition(1, startPos - new Vector3(diffX, diffY, -5));
            }
            else if (diffY <= 20 / 2 && diffY >= 0 || diffY >= -20 / 2 && diffY <= 0)
            {
                // straight line on x axis

                lineRenderer.SetPosition(0, startPos - new Vector3(0, diffY, -5));
                lineRenderer.SetPosition(1, startPos - new Vector3(diffX, diffY, -5));
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
