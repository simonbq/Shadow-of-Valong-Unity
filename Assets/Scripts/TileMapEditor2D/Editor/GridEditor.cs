using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridEditor : EditorWindow {
	static private GameObject gridGameObject;
	static private Grid grid;

	private bool snapping = true;
	private Vector3 prev;

	[MenuItem("Tileset/Grid settings")]
	static void Init()
	{
		var window = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
		window.maxSize = new Vector2 (200, 100);
		window.title = "Grid settings";
		gridGameObject = new GameObject ("GridObject");
		gridGameObject.AddComponent("Grid");
		grid = gridGameObject.GetComponent<Grid>();
	}

	void OnGUI()
	{
		grid.visible = EditorGUILayout.Toggle("Show grid", grid.visible);
		snapping = EditorGUILayout.Toggle ("Snap to grid", snapping);
		grid.width = EditorGUILayout.FloatField("Snap X", grid.width);
		grid.height = EditorGUILayout.FloatField("Snap Y", grid.height);
	}

	void Update()
	{
		if(snapping &&
		   !EditorApplication.isPlaying &&
		   Selection.transforms.Length > 0 &&
		   Selection.transforms[0].position != prev)
		{
			foreach (var tf in Selection.transforms)
			{
				var pos = tf.transform.position;
				pos.x = move (pos.x, grid.width);
				pos.y = move (pos.y, grid.height);
				tf.transform.position = pos;
			}

			prev = Selection.transforms[0].position;
		}
	}

	void OnDestroy()
	{
		GameObject.DestroyImmediate (gridGameObject);
	}

	private float move(float val, float snap)
	{
		return snap * Mathf.Round (val / snap);
	}
}
