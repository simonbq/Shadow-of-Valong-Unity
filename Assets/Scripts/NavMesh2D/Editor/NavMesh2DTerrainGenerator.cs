using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NavMesh2DTerrainGenerator : EditorWindow {

    private int terrainResolution = 64;
    private float tileSize = 0.32f;
    private GameObject terrain;
	// Use this for initialization
    [MenuItem("Edit/2D Navigation")]

    static void Init()
    {
        var window = (NavMesh2DTerrainGenerator)EditorWindow.GetWindow(typeof(NavMesh2DTerrainGenerator));
        window.minSize = new Vector2(240, 480);
        window.title = "2D Navigation";
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        terrainResolution = EditorGUILayout.IntField("Terrain resolution", terrainResolution);  
        GUILayout.Label("Tile size: " + tileSize);
        GUILayout.Label("Terrain size: " + terrainResolution / tileSize);
        
        if (GUILayout.Button("Generate navigation terrain"))
        {
            terrain = generateNavTerrain();
        }
    }

    private GameObject generateNavTerrain()
    {
        GameObject tiles = GameObject.Find("Tiles");
        List<Rect> positions = new List<Rect>();
        if (tiles != null)
        {
            foreach (Transform layer in tiles.transform)
            {
                foreach(Transform tile in layer.transform)
                {
                    if (tile.gameObject.layer == 11)
                    {
                        SpriteRenderer spr = tile.transform.gameObject.GetComponent<SpriteRenderer>();

                        Rect t = new Rect();
                        t.x = -tile.transform.position.y;
                        t.y = tile.transform.position.x;
                        t.width = spr.bounds.size.x;
                        t.height = spr.bounds.size.y;
                        positions.Add(t);

                        if (spr.bounds.size.x < tileSize)
                        {
                            tileSize = Mathf.Round(spr.bounds.size.x * 100) / 100;
                        }
                    }
                }
            }
            TerrainData tData = new TerrainData();
            tData.heightmapResolution = terrainResolution;
            tData.size = new Vector3(terrainResolution * tileSize, 1, terrainResolution * tileSize);

            float[,] heights = new float[terrainResolution, terrainResolution];
            foreach (var rect in positions)
            {
                int x = Mathf.RoundToInt(rect.x / tileSize) + terrainResolution / 2;
                int y = Mathf.RoundToInt(rect.y / tileSize) + terrainResolution / 2;
                int w = Mathf.RoundToInt(rect.width / tileSize);
                int h = Mathf.RoundToInt(rect.height / tileSize);
                //Debug.Log(x + ", " + y + ", " + w + ", " + h);

                for (int xx = 0; xx < w; xx++)
                {
                    for (int yy = 0; yy < h; yy++)
                    {
                        heights[x+xx, y+yy] = 1;
                    }
                }

                tData.SetHeights(0, 0, heights);
            }

            GameObject res = Terrain.CreateTerrainGameObject(tData);
            res.transform.position = new Vector3(-(terrainResolution * tileSize) / 2, 1, -(terrainResolution * tileSize) / 2);
            res.name = "NavMesh2D Terrain";
            return res;
        }

        return null;
    }
}
