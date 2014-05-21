using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public float nodeSize = 0.32f;
    public GameObject target;

    private List<Node> reachable = new List<Node>();
    private List<Node> explored = new List<Node>();

    private List<Vector2> walkable = new List<Vector2>();

    public int testcount = 50;

    // Use this for initialization
    void Start()
    {
        findWalkable();
        findPath(roundToGrid(target.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < explored.Count-1; i++)
        {
            Debug.DrawLine(new Vector3(explored[i].position.x, explored[i].position.y, 0), new Vector3(explored[i+1].position.x, explored[i+1].position.y, 0));
        }
    }

    private List<Vector2> buildPath(Node node)
    {
        List<Vector2> path = new List<Vector2>();

        while (node.prevNode != null)
        {
            path.Add(node.position);
            node = node.prevNode;
        }

        return path;
    }

    private List<Vector2> findPath(Vector2 end)
    {
        Node startNode = new Node();
        startNode.position = roundToGrid(transform.position);
        reachable.Add(startNode);

        int test = 0;
        while (/*reachable.Count >= 0*/ test < testcount)
        {
            Node node = chooseNode(end);

            if (node.position == roundToGrid(end))
            {
                Debug.Log("SUCCESS!");
                return buildPath(node);
            }

            reachable.Remove(node);
            explored.Add(node);
            var adjacent = getAdjacentNodes(node);
            for(int i = 0; i < adjacent.Count; i++)
            {
                if (!reachable.Exists(x => x == adjacent[i]))
                {
                    reachable.Add(adjacent[i]);
                }

                if (node.cost + 1 < adjacent[i].cost)
                {
                    adjacent[i].prevNode = node;
                    adjacent[i].cost = node.cost + 1;
                }
            }

            test++;
        }

        return null;
    }

    private List<Node> getAdjacentNodes(Node node)
    {
        List<Vector2> temp = walkable.FindAll(
            delegate(Vector2 v)
            {
                return ((v.x >= node.position.x-nodeSize && v.x <= node.position.x+nodeSize) &&
                    (v.y >= node.position.y-nodeSize && v.y <= node.position.y+nodeSize)) &&
                    v.x != node.position.x && v.y != node.position.y;
            }
            );

        List<Node> nodes = new List<Node>();
        for(int i = 0; i < temp.Count; i++)
        {
            walkable.Remove(temp[i]);
            Node newNode = new Node();
            newNode.position = temp[i];
            newNode.prevNode = node;
            nodes.Add(newNode);
        }

        Debug.Log("Found " + nodes.Count + " adjacent nodes!");
        return nodes;
    }

    private Node chooseNode(Vector2 end)
    {
        float minCost = Mathf.Infinity;
        Node bestNode = new Node();

        for (int i = 0; i < reachable.Count; i++)
        {
            {
                float totalCost = reachable[i].cost + estimateDistance(reachable[i].position, end);
                if (minCost > totalCost)
                {
                    minCost = totalCost;
                    bestNode = reachable[i];
                }
            }
        }
        Debug.Log("Found best node! " + bestNode.position);
        return bestNode;
    }

    private float estimateDistance(Vector2 start, Vector2 stop)
    {
        float temp = Mathf.Atan2(start.y - stop.y, start.x - stop.x);
        temp = Mathf.Sqrt(temp * temp);
        return temp;
    }

    private void findWalkable()
    {
        GameObject tiles = GameObject.Find("Tiles");
        foreach (Transform t in tiles.transform)
        {
            foreach (Transform tt in t.transform)
            {
                if (tt.gameObject.layer != 11 &&
                    tt.gameObject.tag == "Tile")
                {
                    walkable.Add(roundToGrid(tt.position));
                }
            }
        }

        Debug.Log("Walkable tiles = " + walkable.Count);
    }

    private Vector2 roundToGrid(Vector3 pos)
    {
        return roundToGrid(new Vector2(pos.x, pos.y));
    }

    private Vector2 roundToGrid(Vector2 pos)
    {
        Vector2 temp = new Vector2();
        temp.x = nodeSize * Mathf.Round(pos.x / nodeSize);
        temp.y = nodeSize * Mathf.Round(pos.y / nodeSize);

        return temp;
    }
}

public class Node
{
    public Vector2 position = new Vector2();
    public Node prevNode = null;
    public float cost = 0;
}
