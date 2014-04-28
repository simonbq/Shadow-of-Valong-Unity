using UnityEngine;
using UnityEditor;

public class TileEditor : EditorWindow {
	private bool gridVisible = true;

	static private GameObject gridGameObject;
	static private Grid grid;

	[MenuItem("Edit/TileEditor")]
	static void Init()
	{
		var window = (TileEditor)EditorWindow.GetWindow(typeof(TileEditor));
		window.maxSize = new Vector2 (200, 100);
		gridGameObject = new GameObject ("GridObject");
		gridGameObject.AddComponent("Grid");
	}

	void OnGUI()
	{
		gridVisible = EditorGUILayout.Toggle("Toggle grid", gridVisible);
		if(gridVisible)
		{
			gridGameObject.GetComponent<Grid>().toggleVisible();
		}
	}

	void Update()
	{

	}

	void OnDestroy()
	{
		GameObject.DestroyImmediate (gridGameObject);
	}
}
