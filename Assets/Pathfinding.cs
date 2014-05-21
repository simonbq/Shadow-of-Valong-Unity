﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public float nodeSize = 0.32f;
    public GameObject target;
    public int testcount = 50;

    private List<Vector2> path = new List<Vector2>();
    private List<Vector2> debugPath = new List<Vector2>();
    private List<Vector2> debugPath2 = new List<Vector2>();

    // Use this for initialization
    void Start()
    {
        path = findPath();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < debugPath.Count - 1; i++)
        {
            Debug.DrawLine(debugPath[i], debugPath[i] - new Vector2(0.1f, 0.1f), Color.yellow);
        }
        for (int i = 0; i < debugPath2.Count - 1; i++)
        {
            Debug.DrawLine(debugPath2[i], debugPath2[i] - new Vector2(0.1f, 0.1f), Color.cyan);
        }
        for(int i = 0; i < path.Count-1; i++)
        {
            Debug.DrawLine(path[i], path[i+1], Color.green);
        }
    }

    private void findUnpassable(List<Node> closed)
    {
        GameObject[] g = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < g.Length; i++)
        {
            if (g[i].layer == 11)
            {
                Node temp = new Node();
                temp.position = grid(g[i].transform.position);
                closed.Add(temp);
            }
        }

        Debug.Log("Found " + closed.Count + " already closed!");
    }

    private List<Vector2> findPath()
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        Node start = new Node();
        Node goal = new Node();
        start.position = grid(transform.position);
        goal.position = grid(target.transform.position);
        open.Add(start);

        findUnpassable(closed);

        int debug = 0;

        while (open.Count != 0 && debug < testcount)
        {
            debug++;
            Node node = findNode(open);
            open.Remove(node);
            closed.Add(node);
            debugPath.Add(node.position);

            if (node.position == goal.position)
            {
                Debug.Log("SUCCESS!!! Only " + debug + " loops needed!!");
                return resolvePath(node, start);
            }

            List<Node> adjacent = findAdjacent(node, goal, closed);
            foreach (var a in adjacent)
            {
                if (!open.Exists(x => x.position == a.position))
                {
                    open.Add(a);
                    Debug.Log("Doesnt already exist!");
                }

                else if (a.G < node.G)
                {
                    a.G = node.G + a.dir;
                    a.F = a.G + a.H;
                    a.prevNode = node;
                    Debug.Log("Found better path!");
                }
            }
        }
        Debug.Log("Found nothing");
        return null;
    }

    private List<Vector2> resolvePath(Node n, Node s)
    {
        List<Vector2> path = new List<Vector2>();

        do
        {
            path.Add(n.prevNode.position);
            n = n.prevNode;
        } while (path[path.Count - 1] != s.position);

        return path;
    }

    private List<Node> findAdjacent(Node n, Node goal, List<Node> closed)
    {
        List<Vector2> pos = new List<Vector2>();
        pos.Add(new Vector2(nodeSize, 0));
        pos.Add(new Vector2(-nodeSize, 0));
        pos.Add(new Vector2(0, nodeSize));
        pos.Add(new Vector2(0, -nodeSize));
        pos.Add(new Vector2(nodeSize, nodeSize));
        pos.Add(new Vector2(-nodeSize, nodeSize));
        pos.Add(new Vector2(nodeSize, -nodeSize));
        pos.Add(new Vector2(-nodeSize, -nodeSize));

        List<Node> adjacent = new List<Node>();
        foreach (var p in pos)
        {
            Collider2D[] res = new Collider2D[1];
            int hit = Physics2D.OverlapCircleNonAlloc(p + new Vector2(nodeSize / 2, nodeSize / 2), nodeSize / 2, res);
            if (hit > 0)
            {
                
            }
            bool exists = false;
            foreach (var no in closed)
            {
                if (no.position == p)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                Node temp = new Node();
                temp.position = n.position + p;

                if (p.x == 0 || p.y == 0)
                {
                    temp.G = n.G + 10;
                    temp.dir = 10;
                }

                else
                {
                    temp.dir = 14;
                    temp.G = n.G + 14;
                }

                temp.H = estimateH(temp, goal);
                temp.F = temp.G + temp.H;
                temp.prevNode = n;

                adjacent.Add(temp);
                Debug.Log("Adding adjacent node");
            }

            else
            {
                debugPath2.Add(p);
                Debug.Log("Already closed or blocked!!!");
            }
        }

        return adjacent;
    }

    private float estimateH(Node n, Node goal)
    {
        return 10 * (Mathf.Abs(n.position.x + goal.position.x) + Mathf.Abs(n.position.y + goal.position.y));
    }

    private Node findNode(List<Node> open)
    {
        float lowest = Mathf.Infinity;
        Node best = new Node();
        foreach (var n in open)
        {
            if (n.F < lowest)
            {
                lowest = n.F;
                best = n;
            }
        }

        return best;
    }

    private Vector2 grid(Vector3 pos)
    {
        return grid(new Vector2(pos.x, pos.y));
    }

    private Vector2 grid(Vector2 pos)
    {
        pos.x = nodeSize * Mathf.Round(pos.x / nodeSize);
        pos.y = nodeSize * Mathf.Round(pos.y / nodeSize);
        return pos;
    }
}

public class Node
{
    public Vector2 position = new Vector2();
    public Node prevNode = null;
    public float dir = 0;
    public float F = 0;
    public float G = 0;
    public float H = 0;
}