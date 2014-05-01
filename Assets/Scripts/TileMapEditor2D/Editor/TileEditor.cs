using UnityEngine;
using UnityEditor;
using System.Collections;

public class TileEditor : EditorWindow {
	static private GameObject gridGameObject;
	static private GameObject masterParent;
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

	[MenuItem("Edit/Tileset Editor %1")]

	static void Init()
	{
		var window = (TileEditor)EditorWindow.GetWindow(typeof(TileEditor));
		window.minSize = new Vector2 (240, 320);
		window.title = "Tileset Editor";

		gridGameObject = GameObject.Find ("Grid");
		if(gridGameObject == null)
		{
			gridGameObject = new GameObject ("Grid");
			gridGameObject.AddComponent<Grid>();
		}

		masterParent = GameObject.Find ("Tiles");
		if(masterParent == null)
		{
			masterParent = new GameObject("Tiles");
		}

		loadTiles ();
		lockAll(false);
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

		if(GUILayout.Button ("Replace selected tileset"))
		{
			replaceSelectedTileset();
		}

		if(GUILayout.Button ("Replace selected tile(s)"))
		{
			replaceSelectedTile();
		}
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

				if(tf.tag == "Tile")
				{
					tf.name = "Tile_" + pos.x + "x" + pos.y + "_" + tf.GetComponent<SpriteRenderer>().sortingOrder;
				}
			}

			prev = Selection.transforms[0].position;
		}
	}

	void SceneGUI(SceneView sceneView)
	{
		Event e = Event.current;
		if(e.type == EventType.mouseDown &&
		   e.button == 0 &&
		   e.isMouse &&
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

	void OnProjectChange()
	{
		loadTiles();
	}

	void OnDestroy()
	{
		lockAll(true);
		placeObjects = false;
		GameObject.DestroyImmediate (gridGameObject);
	}

	private void replaceSelectedTileset()
	{
		if(Selection.transforms.Length > 0)
		{
			foreach(var r in Selection.transforms)
			{
				if(r.tag == "Tile")
				{
					int t = r.GetComponent<Tile>().tileID;
					if(t >= tiles.Length)
					{
						t = 0;
					}
					var s = r.GetComponent<SpriteRenderer>();
					s.sprite = tiles[t];
				}
			}
		}
	}

	private void replaceSelectedTile()
	{
		if(Selection.transforms.Length > 0)
		{
			foreach(var r in Selection.transforms)
			{
				if(r.tag == "Tile")
				{
					r.GetComponent<Tile>().tileID = selectedTileId;
					var s = r.GetComponent<SpriteRenderer>();
					s.sprite = selectedSprite;
				}
			}
		}
	}

	private void placeTile(Vector3 pos)
	{
		string name = "Tile_" + pos.x + "x" + pos.y + "_" + tileLayer;
		GameObject created = GameObject.Find (name);
		if(created == null)
		{
			GameObject parentObject = GameObject.Find ("TileLayer_" +tileLayer);
			if(parentObject == null)
			{
				parentObject = new GameObject("TileLayer_" +tileLayer);
				parentObject.transform.parent = masterParent.transform;
			}
			created = new GameObject(name);
			created.AddComponent<SpriteRenderer> ();
			created.AddComponent<Tile> ();
			created.transform.parent = parentObject.transform;
		}
		created.tag = "Tile";
		created.transform.position = pos;
		var renderer = created.GetComponent<SpriteRenderer>();
		renderer.sprite = selectedSprite;
		renderer.sortingOrder = tileLayer;
		var tid = created.GetComponent<Tile> ();
		tid.tileID = selectedTileId;
	}

	private float move(float val, float snap)
	{
		return snap * Mathf.Round (val / snap);
	}

	private static void lockAll(bool locking)
	{
		foreach(Transform c in masterParent.transform)
		{
			foreach(Transform cc in c.transform)
			{
				if(cc.tag == "Tile")
				{
					cc.GetComponent<Tile>().locked = locking;
				}
			}
		}
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
