﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding
{
    private float gridSize = 0.32f;
    private Vector2 startPos;
    private Vector2 endPos;

    private List<Vector2> path;
    private List<Vector2> nodes = new List<Vector2>();
    private bool[,] colliders;

    private static Pathfinding instance = null;
    private Pathfinding() 
    {
    }
    public static Pathfinding getInstance()
    {
        if(instance == null)
        {
            instance = new Pathfinding();
        }

        return instance;
    }

	public List<Vector2> getNodes()
	{
		return nodes;
	}

    public void setMapDimensions(Vector2 start, Vector2 end)
    {
        startPos = start;
        endPos = end;
    }

    public void setGridSize(float size)
    {
        gridSize = size;
    }

	public void loadColliders()
	{
        IntVector listSize = worldToNode(endPos);
        colliders = new bool[listSize.x + 1, listSize.y + 1];
        
        GameObject tiles = GameObject.Find ("Tiles");
		foreach(Transform layer in tiles.transform)
		{
			foreach(Transform t in layer.transform)
			{
				if(t.gameObject.layer == 11)
				{
					IntVector pos = worldToNode (t.transform.position);
                    colliders[pos.x, pos.y] = true;
				}
			}
		}
	}

    public List<Vector2> findPath(Vector2 pos, Vector2 end)
    {
		nodes.Clear();
		int debug = 0;
        
        OpenList openList = new OpenList();
        IntVector listSize = worldToNode(endPos);
        Debug.Log(listSize.x + "x" + listSize.y);
        bool[,] closedList = new bool[listSize.x + 1, listSize.y + 1];

		IntVector goal = worldToNode(end);
		Node.setGoal(goal);

        Node start = new Node(worldToNode(pos), null);
        openList.addNode(start);

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
                /*
				if (a[i].x > 0 &&
                    a[i].y > 0 &&
                    colliders[a[i].x + 1, a[i].y + 1] == true) { continue; }
                if (a[i].x < listSize.x &&
                    a[i].y > 0 &&
                    colliders[a[i].x - 1, a[i].y + 1] == true) { continue; }
                if (a[i].x > 0 &&
                    a[i].y < listSize.y &&
                    colliders[a[i].x + 1, a[i].y - 1] == true) { continue; }
                if (a[i].x < listSize.x &&
                    a[i].y < listSize.y &&
                    colliders[a[i].x - 1, a[i].y - 1] == true) { continue; }
                    */

                if (a[i].x < 0 ||
                    a[i].x > listSize.x ||
                    a[i].y > listSize.y ||
                    a[i].y < 0 ||
                    closedList[a[i].x, a[i].y] == true ||
                    colliders[a[i].x, a[i].y] == true)
                {
                    continue;
                }

                Node adjacent = openList.find(a[i]);
                if (adjacent == null)
                {
                    adjacent = new Node(a[i], node);
                    openList.addNode(adjacent);
                }

                else if(node.gScore + adjacent.getCost(node) < adjacent.gScore)
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
        return 10 * (Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y));
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
                        add.next = search.next;
                    }

                    else
                    {
                        best = add;
                    }
                    add.prev = search;
                    search.next = add;
                    //Debug.Log(add.node.total + " > " + search.node.total);
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
{	public IntVector pos;
    public Node prev;
    public int gScore = 0;
    public float hScore = 0;
    public float total = 0;

	private static IntVector g;

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
            //if (Mathf.Abs(p.pos.x - pos.x) == 1 && Mathf.Abs(p.pos.y - pos.y) == 1)
            //{
            //    cost = 14;
            //}
			gScore = prev.gScore + getCost (p);
            hScore = Pathfinding.estimateDistance(pos, g);
            total = hScore + gScore;
        }
    }

	public int getCost(Node p)
	{
		if(Mathf.Abs(p.pos.x - pos.x) + Mathf.Abs(p.pos.y - pos.y) == 2)
		{
			return 14;
		}

		else{
			return 10;
		}
	}

    public bool Equals(Node other)
    {
        return this.pos.x == other.pos.x && this.pos.y == other.pos.y;
    }

	public static void setGoal(IntVector goal)
	{
		g = goal;
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
    public bool Equals(IntVector other)
    {
        return x == other.x && y == other.y;
    }
}