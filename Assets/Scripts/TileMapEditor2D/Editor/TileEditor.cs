using UnityEngine;
using UnityEditor;

public class TileEditor : EditorWindow {
	private bool showGrid = true;

	static private Grid grid;
	static private GameObject gridGameObject;

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
		showGrid = EditorGUILayout.Toggle ("Show grid", showGrid);
	}

	void OnDestroy()
	{
		GameObject.DestroyImmediate (gridGameObject);
	}
}
