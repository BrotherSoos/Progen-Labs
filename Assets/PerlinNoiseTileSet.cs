using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTileSet : MonoBehaviour
{

    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_water;
    public GameObject prefab_sand;
    public GameObject prefab_hill;
    public GameObject prefab_plains;

    private TilemapTextureSpread textureScript;

    int map_width = 1920;
    int map_height = 1920;

    private int renderProgress = 0;
    private int renderIncrement = 300;
    public float noiseModifier = 0.5f;

    List<List<int>> noise_grid = new List<List<int>>();

    Dictionary<int, GameObject> textureLayers = new Dictionary<int, GameObject>();

    float magnification = 0.01f;

    int x_offset = 0;
    int x2= 0;
    int x3= 0;
    int y_offset = 0;
    int y2= 0;
    int y3= 0;

    // Start is called before the first frame update
    void Start()
    {
        textureScript = gameObject.GetComponent<TilemapTextureSpread>();
        textureScript.SetupPrefabs(map_width, map_height);
        x_offset = UnityEngine.Random.Range(-10000, +10000);
        y_offset = UnityEngine.Random.Range(-10000, +10000);
        y2= UnityEngine.Random.Range(-10000, +10000);
        y3= UnityEngine.Random.Range(-10000, +10000);
        x2= UnityEngine.Random.Range(-10000, +10000);
        x3= UnityEngine.Random.Range(-10000, +10000);
        CreateTileSet();
        CreateTileGroup();
        textureScript.InitRawTextures(tileset);
        textureLayers = textureScript.cleanLayers(tileset, map_width, map_height);
        GenerateMap();
        textureScript.ApplyLayers();
        Debug.Log("Done ---");
    }

    private void GenerateMap()
    {
        for(int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            for(int y = 0; y < map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                textureScript.InterpolateTexture(tile_id, tileset[tile_id], textureLayers[tile_id], map_width, map_height, x, y);
            }
        }
    }


    private int GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = (Mathf.PerlinNoise((x - x_offset) * magnification / 10, (y - y_offset) * magnification / 10)); 
        raw_perlin += (Mathf.PerlinNoise((x - x2) * magnification * 4, (y - y2) * magnification*4)-0.5f) * 0.0675f;
        raw_perlin += (Mathf.PerlinNoise((x - x3) * magnification, (y - y3) * magnification) -0.5f) / 3.5f;
        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
        float scale_perlin = clamp_perlin * tileset.Count;

        if(scale_perlin == 8)
        {
            scale_perlin = 7;
        }

        return Mathf.FloorToInt(scale_perlin);
    }

    private void CreateTileGroup()
    {
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            Debug.Log(prefab_pair.Value.name);
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    void CreateTileSet()
    {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_water);
        tileset.Add(1, prefab_water);
        tileset.Add(2, prefab_water);
        tileset.Add(3, prefab_sand);
        tileset.Add(4, prefab_plains);
        tileset.Add(5, prefab_plains);
        tileset.Add(6, prefab_hill);
        tileset.Add(7, prefab_hill);
        noiseModifier /=  tileset.Count;
    }


}
