using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public float width = 0.32f;
	public float height = 0.32f;
	public bool visible = true;

	static private Rect srect;
	static private bool rectvisible = false;

	void OnDrawGizmos(){
		if (width < 0.1 || height < 0.1) {
				visible = false;
		}
		Vector3 pos = Camera.current.transform.position;
		Gizmos.color = Color.grey;
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

		if(rectvisible == true)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(new Vector3(srect.x, srect.y, 1f),
			                new Vector3(srect.x+srect.width, srect.y, 1f));
			Gizmos.DrawLine(new Vector3(srect.x, srect.y, 1f),
			                new Vector3(srect.x, srect.y+srect.height, 1f));
			Gizmos.DrawLine(new Vector3(srect.x, srect.y+srect.height, 1f),
			                new Vector3(srect.x+srect.width, srect.y+srect.height, 1f));
			Gizmos.DrawLine(new Vector3(srect.x+srect.width, srect.y, 1f),
			                new Vector3(srect.x+srect.width, srect.y+srect.height, 1f));
		}
	}

	static public void setRect(Rect rect)
	{
		srect = rect;
	}

	static public void toggleRect(bool visible)
	{
		rectvisible = visible;
	}
}
