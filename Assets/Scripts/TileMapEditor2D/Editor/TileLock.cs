using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileLock : Editor {
	private Tile tile;

	void Awake () {
		tile = target as Tile;
	}
	
	// Update is called once per frame
	void OnSceneGUI () {
		if(tile.locked == true &&
		   !EditorApplication.isPlaying &&
		   Selection.transforms.Length > 0)
		{
			Selection.activeGameObject = null;
		}
	}
}
