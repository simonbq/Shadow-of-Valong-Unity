using UnityEngine;
using System.Collections;

public class NavMeshAgent2D : MonoBehaviour {
    public float radius = 0.15f;
    public float speed = 3.5f;
    public float acceleration = 8;
    public float angularSpeed = 120;
    public float stoppingDistance = 0;

    public GameObject target; //remove later

    private NavMeshAgent agent;
    private GameObject agentObject;
    
	// Use this for initialization
	void Start () {
        agentObject = new GameObject();
        agent = agentObject.AddComponent<NavMeshAgent>();
        agentObject.transform.position = pointTo3D(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        agent.radius = radius;
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = stoppingDistance;
        agentObject.name = name + "(NavMeshAgent2D)";

        Vector3 pos = agentObject.transform.position;
        transform.position = pointTo2D(pos, transform.position.z);

        Debug.Log(setDestination(target.transform.position));
	}

    public Vector3 pointTo3D(Vector3 point)
    {
        return new Vector3(point.x, 1, point.y);
    }

    public Vector3 pointTo2D(Vector3 point, float zPos)
    {
        return new Vector3(point.x, point.z, zPos);
    }

    public bool setDestination(Vector3 target)
    {
        return agent.SetDestination(pointTo3D(target));
    }
}
