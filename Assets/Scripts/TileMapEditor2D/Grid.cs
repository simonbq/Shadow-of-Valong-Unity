using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public float width = 0.32f;
	public float height = 0.32f;
	public bool visible = true;

	void OnDrawGizmos(){
		if (width < 0.1 || height < 0.1) {
				visible = false;
		}
		Vector3 pos = Camera.current.transform.position;
		if (visible) {
						for (float y = pos.y - 400.0f; y < pos.y + 400.0f; y+= height) {
								Gizmos.DrawLine (new Vector3 (-1000000.0f, Mathf.Floor (y / height) * height, 0.0f),
			                new Vector3 (1000000.0f, Mathf.Floor (y / height) * height, 0.0f));
						}
		
						for (float x = pos.x - 600.0f; x < pos.x + 600.0f; x+= width) {
								Gizmos.DrawLine (new Vector3 (Mathf.Floor (x / width) * width, -1000000.0f, 0.0f),
			                new Vector3 (Mathf.Floor (x / width) * width, 1000000.0f, 0.0f));
						}
				}
	}
}
