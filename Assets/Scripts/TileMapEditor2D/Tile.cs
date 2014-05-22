using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	public int tileID;
	public bool locked = false;
    
    BoxCollider2D c;
    private bool hasGottenCollider = false;

	void OnDrawGizmos()
	{
        if(!hasGottenCollider){
            c = GetComponent<BoxCollider2D>();
            hasGottenCollider = true;
        }

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
