using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int tileID;
	public bool locked = false;
    public bool dynamic = false;

	void OnDrawGizmos()
	{	
		BoxCollider2D c = GetComponent<BoxCollider2D> ();
		if(c != null)
		{
			Gizmos.color = Color.red;
			Vector3 pos = transform.position;
			Gizmos.DrawLine(new Vector3(pos.x, pos.y, 1f),
			                new Vector3(pos.x+c.size.x, pos.y, 1f));
			Gizmos.DrawLine(new Vector3(pos.x, pos.y, 1f),
			                new Vector3(pos.x, pos.y-c.size.y, 1f));
			Gizmos.DrawLine(new Vector3(pos.x, pos.y-c.size.y, 1f),
			                new Vector3(pos.x+c.size.x, pos.y-c.size.y, 1f));
			Gizmos.DrawLine(new Vector3(pos.x+c.size.x, pos.y, 1f),
			                new Vector3(pos.x+c.size.x, pos.y-c.size.y, 1f));
		}
	}
}
