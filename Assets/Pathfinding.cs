using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public float gridSize = 0.32f;
    public GameObject firstTile;
    public GameObject lastTile;
    public GameObject target;

    private Vector2 startPos;
    private Vector2 endPos;

    void Start()
    {
        startPos = firstTile.transform.position;
        endPos = lastTile.transform.position;
    }

    public List<Vector2> findPath(Vector2 pos, Vector2 end)
    {
        

        return null;
    }

    private Vector2 nodeToWorld(IntVector pos)
    {
        Vector2 result = new Vector2(pos.x, pos.y);
        result.x *= gridSize;
        result.y *= gridSize;
        result += startPos;
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

class Node : IEquatable<Node>
{
    public IntVector pos;
    public Node prev;
    public int gScore = 0;
    public float hScore = 0;
    public float total = 0;

    public Node(IntVector position, Node previous)
    {
        pos = position;
        recalculate(previous);
    }

    public void recalculate(Node p)
    {
        int cost = 10;
        if (Mathf.Abs(p.pos.x - pos.x) == 1 && Mathf.Abs(p.pos.y - pos.y) == 1)
        {
            cost = 14;
        }
        gScore = prev.gScore + cost;
        if (p != null)
        {
            hScore = Pathfinding.estimateDistance(pos, p.pos);
        }
        total = hScore + gScore;
        prev = p;
    }

    public bool Equals(Node other)
    {
        return this.pos.x == other.pos.x && this.pos.y == other.pos.y;
    }
}

class IntVector
{
    public int x, y;
    public void IntVector()
    {
        x = y = 0;
    }
    public void IntVector(int xx, int yy)
    {
        x = xx;
        y = yy;
    }
}