﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfind_Object : MonoBehaviour {
    public GameObject target;

    private Vector2 start;
    private Vector2 goal;
    private List<Vector2> path;

	// Use this for initialization
	void Start () {
        Pathfinding.getInstance().setMapDimensions(new Vector2(-4.8f, 0.64f), new Vector2(12.48f, -14.4f));
        Pathfinding.getInstance().loadColliders();
	}
	
	// Update is called once per frame
	void Update () {
	    start = transform.position;
        goal = target.transform.position;
        path = Pathfinding.getInstance().findPath(start, goal);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.green);
        }
	}
}