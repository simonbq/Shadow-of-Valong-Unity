using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridEditor : EditorWindow {
	static private GameObject gridGameObject;
	static private Grid grid;

	private bool snapping = true;
	private Vector3 prev;
	private Sprite selectedSprite;
	private int selectedSpriteId = 0;
	private bool placeObjects = false;
	private int tileLayer = 0;
	private int prevSpriteId = -1;
	private int selectedTileId = 0;
	private Texture2D[] buttons;
	private Sprite[] tiles;

	private Vector2 scrollPos;
	private int tileGridWidth;

	private static string[] tileNames;

	[MenuItem("Tileset/Grid settings")]

	static void Init()
	{
		var window = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
		window.minSize = new Vector2 (240, 320);
		window.title = "Grid settings";
		gridGameObject = GameObject.Find ("Grid");
		if(gridGameObject == null)
		{
			gridGameObject = new GameObject ("Grid");
			gridGameObject.AddComponent("Grid");
		}
		loadTiles ();
	}

	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += SceneGUI;
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
		placeObjects = EditorGUILayout.Toggle ("Place tiles", placeObjects);
		tileLayer = EditorGUILayout.IntField ("Layer", tileLayer);
		selectedSpriteId = EditorGUILayout.IntPopup (selectedSpriteId, tileNames, index);

		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
		if(prevSpriteId != selectedSpriteId)
		{
			selectedTileId = 0;
			tiles = Resources.LoadAll<Sprite> ("Tilemaps/"+tileNames[selectedSpriteId]);
			buttons = new Texture2D[tiles.Length];
			for(int i = 0; i < tiles.Length; i++)
			{
				Texture2D tex = new Texture2D((int)tiles[i].rect.width, (int)tiles[i].rect.height);
				Color[] pix = tiles[i].texture.GetPixels((int)tiles[i].rect.x,
				                                     	(int)tiles[i].rect.y,
				                                     	(int)tiles[i].rect.width,
				                                     	(int)tiles[i].rect.height);
				tex.SetPixels(pix);
				tex.Apply();
				buttons[i] = tex;
			}

			tileGridWidth = tiles[0].texture.width/buttons[0].width;
		}

		selectedTileId = GUILayout.SelectionGrid (selectedTileId, buttons, tileGridWidth);
		selectedSprite = tiles [selectedTileId];
		prevSpriteId = selectedSpriteId;
		EditorGUILayout.EndScrollView ();
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

	void SceneGUI(SceneView sceneView)
	{
		if(Event.current.type == EventType.mouseDown &&
		   placeObjects)
		{
			Vector2 mpos = Event.current.mousePosition;
			mpos.y = sceneView.camera.pixelHeight - mpos.y;
			Vector3 pos = sceneView.camera.ScreenPointToRay(mpos).origin;
			pos.x = move (pos.x, grid.width);
			pos.y = move (pos.y, grid.height);
			pos.z = 0;

			placeTile (pos);
		}

		if(placeObjects)
		{
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		}

		else{
			HandleUtility.Repaint ();
		}
	}

	void OnDestroy()
	{
		placeObjects = false;
		GameObject.DestroyImmediate (gridGameObject);
	}

	private void placeTile(Vector3 pos)
	{
		GameObject created = new GameObject("Tile");
		created.transform.position = pos;
		created.AddComponent("SpriteRenderer");
		var renderer = created.GetComponent<SpriteRenderer>();
		renderer.sprite = selectedSprite;
		renderer.sortingOrder = tileLayer;
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
