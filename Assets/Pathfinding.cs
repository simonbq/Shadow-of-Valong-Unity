using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {
    public int directions = 8;
    public float increment = 0.32f;
    public GameObject target;

    private Vector2 targetPos;
    private List<Vector2> path = new List<Vector2>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        pathFind(target.transform.position);
       /* for (int i = 0; i < path.Count; i++)
        {
            Debug.DrawLine(new Vector3(path[i].x, path[i].y, 1), new Vector3(path[i].x + 0.32f, path[i].y, 1));
        }*/
	}

    private void pathFind(Vector3 targetPos)
    {
        path.Clear();
        bool pathFound = false;
        Vector2 pathPosition = transform.position;

        int debug = 0;
        while (!pathFound)
        {
            int angle = 360 / directions;
            Vector2 moveVector = new Vector2(0, 0);
            for (int i = 0; i < 360; i += angle)
            {
                Vector2 dirVector = new Vector2();
                dirVector.x = Mathf.Cos(i * Mathf.Deg2Rad);
                dirVector.y = Mathf.Sin(i * Mathf.Deg2Rad);

                RaycastHit2D hit = Physics2D.Raycast(pathPosition, dirVector, increment, 1 << LayerMask.NameToLayer("Solids"));
                Debug.DrawRay(pathPosition, dirVector, Color.blue);
                if (!hit)
                {
                    moveVector = new Vector2(dirVector.x * increment, dirVector.y * increment);
                    i = 360;
                }
            }

            pathPosition += moveVector;
            Debug.Log("Moving " + moveVector);
            path.Add(pathPosition);

            if(debug >= 25)
            {
                pathFound = true;
            }

            debug++;
        }
    }
}
