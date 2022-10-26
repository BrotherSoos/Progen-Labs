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

    int map_width = 300;
    int map_height = 300;

    private int renderProgress = 0;
    private int renderIncrement = 300;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    float magnification = 0.01f;

    int x_offset = 0;
    int y_offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        textureScript = gameObject.GetComponent<TilemapTextureSpread>();
        textureScript.SetupPrefabs(map_width, map_height);
        CreateTileSet();
        CreateTileGroup();
        GenerateMap();
    }

    private void GenerateMap()
    {
        for(int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());
            for(int y = 0; y < map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }

    void Update()
    {
        if (renderProgress >= map_height*map_width)
        {
            return;
        }
        
        for (int i = renderProgress; i < renderProgress + renderIncrement ; i++)
        {
            int x = i % map_width;
            int y = i / map_width;
            tile_grid[x][y] = textureScript.InterpolateTexture(tile_grid[x][y], map_width, map_height, x, y);
        }
        renderProgress += renderIncrement;
    }

    private void CreateTile(int tile_id, int x, int y)
    {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);
        //tile = textureScript.InterpolateTexture(tile, map_width, map_height, x, y);
        tile_grid[x].Add(tile);
        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);
    }

    private int GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise((x - x_offset) * magnification, (y - y_offset) * magnification);
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
    }


}
