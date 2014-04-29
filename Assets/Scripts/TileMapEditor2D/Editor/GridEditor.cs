using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridEditor : EditorWindow {
	static private GameObject gridGameObject;
	static private Grid grid;

	private bool snapping = true;
	private Vector3 prev;
	private Sprite selectedSprite;
	private int selectedSpriteId;
	private int selectedTileId;

	private static string[] tileNames;


	[MenuItem("Tileset/Grid settings")]
	static void Init()
	{
		var window = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
		window.maxSize = new Vector2 (200, 100);
		window.title = "Grid settings";
		gridGameObject = GameObject.Find ("Grid");
		if(gridGameObject == null)
		{
			gridGameObject = new GameObject ("Grid");
			gridGameObject.AddComponent("Grid");
		}
		loadTiles ();
	}

	void OnGUI()
	{
		grid = gridGameObject.GetComponent<Grid>();
		grid.visible = EditorGUILayout.Toggle("Show grid", grid.visible);
		snapping = EditorGUILayout.Toggle ("Snap to grid", snapping);
		grid.width = EditorGUILayout.FloatField("Snap X", grid.width);
		grid.height = EditorGUILayout.FloatField("Snap Y", grid.height);

		EditorGUILayout.Space ();

		int[] index = new int[tileNames.Length];
		for(int i = 0; i < tileNames.Length; i++)
		{
			index[i] = i;
		}

		selectedSpriteId = EditorGUILayout.IntPopup (selectedSpriteId, tileNames, index);

		Sprite[] t = Resources.LoadAll<Sprite> ("Tilemaps/"+tileNames[selectedSpriteId]);
		Texture2D[] buttons = new Texture2D[t.Length];
		for(int i = 0; i < t.Length; i++)
		{
			Texture2D tex = new Texture2D((int)t[i].rect.width, (int)t[i].rect.height);
			Color[] pix = t[i].texture.GetPixels((int)t[i].rect.x,
			                                 (int)t[i].rect.y,
			                                 (int)t[i].rect.width,
			                                 (int)t[i].rect.height);
			tex.SetPixels(pix);
			tex.Apply();
			buttons[i] = tex;
		}

		selectedTileId = GUILayout.SelectionGrid (selectedTileId, buttons, 3);
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

	private static void loadTiles()
	{
		ArrayList t = new ArrayList ();
		string[] files = System.IO.Directory.GetFiles (Application.dataPath + "\\Resources\\Tilemaps");
		foreach(string fname in files)
		{
			int i = fname.LastIndexOf("\\");
			i+=1;
			string path = "";
			if(i > 0)
			{
				path = fname.Substring(i);
			}

			Object o = Resources.LoadAssetAtPath("Assets\\Resources\\Tilemaps\\"+path, typeof(Sprite));

			if(o != null)
			{
				t.Add (path);
			}
		}

		string[] res = new string[t.Count];
		for (int i = 0; i < t.Count; i++) 
		{
			res[i] = (string)t[i];
			int ii = res[i].LastIndexOf(".");
			res[i] = res[i].Remove(ii);
		}
		
		tileNames = res;
	}
}
