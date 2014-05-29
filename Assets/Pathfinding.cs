using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public float gridSize = 0.32f;
    public GameObject target;

    public Vector2 startPos;
    public Vector2 endPos;

    private List<Vector2> path;
    private List<Vector2> nodes = new List<Vector2>();
	private List<IntVector> colliders = new List<IntVector>();

    void Start()
    {
		loadColliders ();
		path = findPath(new Vector2(0.96f, -2.24f), new Vector2(3.84f, -1.6f));
        //Debug.Log(path.Count);
        Debug.Log(path[path.Count - 1]);
    }

    void Update()
    {
        for (int i = 0; i < path.Count-1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.green);
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Gizmos.DrawSphere(nodes[i], 0.1f);
        }
    }

	public void loadColliders()
	{
		GameObject tiles = GameObject.Find ("Tiles");
		foreach(Transform layer in tiles.transform)
		{
			foreach(Transform t in layer.transform)
			{
				if(t.gameObject.layer == 11)
				{
					IntVector pos = worldToNode (t.transform.position);
					colliders.Add(pos);
				}
			}
		}
	}

    public List<Vector2> findPath(Vector2 pos, Vector2 end)
    {
        int debug = 0;
        
        OpenList openList = new OpenList();
        IntVector listSize = worldToNode(endPos);
        Debug.Log(listSize.x + "x" + listSize.y);
        bool[,] closedList = new bool[listSize.x + 1, listSize.y + 1];

		for(int i = 0; i < colliders.Count; i++)
		{
			closedList[colliders[i].x, colliders[i].y] = true;
		}


        Node start = new Node(worldToNode(pos), null);
        openList.addNode(start);

        IntVector goal = worldToNode(end);

        while (openList.size > 0)
        {
            debug++;
            //Debug.Log("Loop " + debug + " - Open size " + openList.size);
            Node node = openList.best.node;
            nodes.Add(nodeToWorld(node.pos));
            openList.removeBest();
            //Debug.Log("Score: " + node.total);
            //Debug.Log(node.pos.x + "x" + node.pos.y);
            closedList[node.pos.x, node.pos.y] = true;

            if (node.pos.x == goal.x &&
                node.pos.y == goal.y)
            {
                Debug.Log("SUCCESS!!!");
                return resolvePath(node, start);
            }

            IntVector[] a = new IntVector[8];
            a[0] = new IntVector(node.pos.x + 1, node.pos.y);
            a[1] = new IntVector(node.pos.x - 1, node.pos.y);
            a[2] = new IntVector(node.pos.x, node.pos.y + 1);
            a[3] = new IntVector(node.pos.x, node.pos.y - 1);
            a[4] = new IntVector(node.pos.x + 1, node.pos.y + 1);
            a[5] = new IntVector(node.pos.x - 1, node.pos.y + 1);
            a[6] = new IntVector(node.pos.x + 1, node.pos.y - 1);
            a[7] = new IntVector(node.pos.x - 1, node.pos.y - 1);

            for (int i = 0; i < 8; i++)
            {
                if (a[i].x < 0 ||
                    a[i].x > listSize.x ||
                    a[i].y > listSize.y ||
                    a[i].y < 0 ||
                    closedList[a[i].x, a[i].y] == true)
                {
                    continue;
                }

                Node adjacent = openList.find(a[i]);
                if (adjacent == null)
                {
                    adjacent = new Node(a[i], node);
                    openList.addNode(adjacent);
                }

                else if(node.gScore + adjacent.cost < adjacent.gScore)
                {
                    adjacent.recalculate(node);
                }
            }
        }
        Debug.Log("Found nothing");
        return null;
    }

    private List<Vector2> resolvePath(Node n, Node s)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 goal = nodeToWorld(s.pos);

        do
        {
            path.Add(nodeToWorld(n.pos));
            n = n.prev;
        } while (path[path.Count - 1] != goal);

        return path;
    }

    private Vector2 nodeToWorld(IntVector pos)
    {
        Vector2 result = new Vector2(pos.x, pos.y);
        result.x *= gridSize;
        result.y *= gridSize;
        result.x += startPos.x;
        result.y = startPos.y - result.y;
        result += new Vector2(gridSize / 2, -gridSize / 2);
        return result;
    }

    private IntVector worldToNode(Vector2 pos)
    {
        pos.x -= startPos.x;
        pos.y = startPos.y - pos.y;
        pos.x /= gridSize;
        pos.y /= gridSize;

        IntVector result = new IntVector();
        result.x = Mathf.RoundToInt(pos.x);
        result.y = Mathf.RoundToInt(pos.y);
        return result;
    }

    private IntVector worldToNode(Vector3 pos)
    {
        return worldToNode(new Vector2(pos.x, pos.y));
    }

    public static float estimateDistance(IntVector p1, IntVector p2)
    {
        return 10 * Mathf.Max(Mathf.Abs(p1.x - p2.x), Mathf.Abs(p1.y - p2.y));
    }
}

class OpenList
{
    public OpenNode best;
    public int size = 0;

    public void addNode(Node n)
    {
        OpenNode add = new OpenNode(n);
        if (best == null)
        {
            best = add;
            size = 0;
        }

        else
        {
            OpenNode search = best;
            for (int i = 0; i < size; i++)
            {
                if (search.node.total > add.node.total)
                {
                    if (search.prev != null)
                    {
                        add.prev = search.prev;
                    }

                    else
                    {
                        best = add;
                    }
                    add.next = search;
                    search.prev = add;
                    Debug.Log(add.node.total + " < " + search.node.total);
                    i = size;
                }

                else if (search.next != null)
                {
                    search = search.next;
                }

                else
                {
                    add.prev = search;
                    search.next = add;
                    i = size;
                }
            }
        }

        size++;
    }

    public void removeBest()
    {
        best = best.next;
        size--;
    }

    public Node find(IntVector n)
    {
        OpenNode search = best;
        if (search == null)
        {
            return null;
        }

        for (int i = 0; i < size; i++)
        {
            if (search.node.pos.x == n.x &&
                search.node.pos.y == n.y)
            {
                return search.node;
            }

            if (search.next != null)
            {
                search = search.next;
            }

            else
            {
                return null;
            }
        }

        return null;
    }
}

class OpenNode
{
    public Node node;
    public OpenNode next = null;
    public OpenNode prev = null;
    public OpenNode(Node n)
    {
        node = n;
    }
}

public class Node
{
    public IntVector pos;
    public Node prev;
    public int gScore = 0;
    public float hScore = 0;
    public float total = 0;
    public int cost = 0;

    public Node(IntVector position, Node previous)
    {
        pos = position;
        recalculate(previous);
    }

    public void recalculate(Node p)
    {
        if (p != null)
        {
            prev = p;
            cost = 10;
            //if (Mathf.Abs(p.pos.x - pos.x) == 1 && Mathf.Abs(p.pos.y - pos.y) == 1)
            //{
            //    cost = 14;
            //}
            gScore = prev.gScore + cost;
            if (p != null)
            {
                hScore = Pathfinding.estimateDistance(pos, p.pos);
            }
            total = hScore + gScore;
        }
    }

    public bool Equals(Node other)
    {
        return this.pos.x == other.pos.x && this.pos.y == other.pos.y;
    }
}

public class IntVector
{
    public int x, y;
    public IntVector()
    {
        x = y = 0;
    }
    public IntVector(int xx, int yy)
    {
        x = xx;
        y = yy;
    }
}