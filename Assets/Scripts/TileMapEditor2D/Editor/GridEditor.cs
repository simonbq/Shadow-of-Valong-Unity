using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Grid))]
public class GridEditor : Editor {
	Grid grid;

	Texture2D texture;

	public void OnEnable()
	{
		grid = (Grid)target;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Tile Width ");
		grid.width = EditorGUILayout.FloatField(grid.width, GUILayout.Width(50));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Tile Height ");
		grid.height = EditorGUILayout.FloatField(grid.height, GUILayout.Width(50));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal ();
		GUILayout.Label(" Visible ");
		grid.visible = EditorGUILayout.Toggle (grid.visible);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		texture = (Texture2D) EditorGUILayout.ObjectField(" Texture ", texture, typeof (Texture2D), false);
		GUILayout.EndHorizontal ();

		Rect posRect = new Rect (40, 300, 100, 100);
		float textureSize = texture.width / grid.width * 100;
		Rect randomRect = new Rect(0,0,textureSize,1);

		GUILayout.BeginHorizontal ();
		for(int i=0; i<textureSize;i++){
			GUI.DrawTextureWithTexCoords (posRect, texture, randomRect);
		}
		
		GUILayout.EndHorizontal ();

		SceneView.RepaintAll();
	}
}
