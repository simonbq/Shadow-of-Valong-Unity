using UnityEngine;
using System.Collections;

public class TestTerrain : MonoBehaviour {
    private GameObject terrain;
    private TerrainData terrainData;

	// Use this for initialization
	void Start () {
        terrainData = new TerrainData();
        float[,] heights = new float[10, 10];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                heights[x, y] = 10;
            }
        }
        terrainData.heightmapResolution = 66;
        terrainData.SetHeights(0, 0, heights);
        terrain = Terrain.CreateTerrainGameObject(terrainData);
        terrain.transform.position = new Vector3(10, -10, 100);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
