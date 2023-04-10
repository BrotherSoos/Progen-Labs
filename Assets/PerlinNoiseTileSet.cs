using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PerlinNoiseTileSet : MonoBehaviour
{

    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_water;
    public GameObject prefab_sand;
    public GameObject prefab_hill;
    public GameObject prefab_plains;

    private List<GameObject> small_mts = new List<GameObject>();
    private List<GameObject> large_mts = new List<GameObject>();
    private List<GameObject> snowless_small_mts = new List<GameObject>();

    private TilemapTextureSpread textureScript;
    public int map_width = 1000;
    public int map_height = 1000;

    private int renderProgress = 0;
    private int renderIncrement = 300;
    public float noiseModifier = 0.5f;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<float>> precise_grid = new List<List<float>>();

    Dictionary<int, GameObject> textureLayers = new Dictionary<int, GameObject>();

    public float magnification = 0.03f;
    private int mountainLayerId;
    private int mountainLayersSnowless = 2;
    private int mountainLayersSmall = 1;
    private int mountainLayersLarge = 4;

    private float mountainSize = 10f/3f;

    int x_offset = 0;
    int x2= 0;
    int x3= 0;
    int y_offset = 0;
    int y2= 0;
    int y3= 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadMountainTextures();
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
      //List<Coordinate> mountainCoordinates = new List();
        for(int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            precise_grid.Add(new List<float>());
            for(int y = 0; y < map_height; y++)
            {
                float noise = GetIdUsingPerlin(x, y);
                precise_grid[x].Add(noise);
                int tile_id = Mathf.FloorToInt(noise);
                noise_grid[x].Add(tile_id);
               // Debug.Log("tileset " + tile_id);
                //Debug.Log(" is in both" + tileset[tile_id] + " asdf "+  textureLayers[tile_id]);
                textureScript.InterpolateTexture(tile_id, tileset[tile_id], textureLayers[tile_id], map_width, map_height, x, y);
            }
        }
        SetMountains();
    }


    private float GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = (Mathf.PerlinNoise((x - x_offset) * magnification / 10, (y - y_offset) * magnification / 10)); 
        raw_perlin += (Mathf.PerlinNoise((x - x2) * magnification * 4, (y - y2) * magnification*4)-0.5f) * 0.0275f;
        raw_perlin += (Mathf.PerlinNoise((x - x3) * magnification, (y - y3) * magnification) -0.5f) / 5.5f;
        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
        float scale_perlin = clamp_perlin * (tileset.Count-1);
        if(scale_perlin == tileset.Count)
        {
            scale_perlin = tileset.Count-1;
        }

        return scale_perlin;
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
        int waterLayers = 8;
        int sandLayers = 1;
        int plainsLayers = 5;
        int hillLayers = 7;
        int sum = waterLayers + sandLayers + plainsLayers + hillLayers;
        for(int i = 0; i < sum; i++) {
          if(i < waterLayers) {
            tileset.Add(i, prefab_water);
          }
          else if(i < waterLayers+sandLayers) {
            tileset.Add(i, prefab_sand);
          }
          else if(i < waterLayers+sandLayers+plainsLayers) {
            tileset.Add(i, prefab_plains);
          }
          else {
            tileset.Add(i, prefab_hill);
          }
        }
        noiseModifier /=  (tileset.Count);
    }


    void SetMountains() {
      for(int x = 0; x < map_width; x++)
        {
            for(int y = 0; y < map_height; y++)
            {
              float noise = precise_grid[x][y];
              PlaceMountain(noise, x, y);
            }
        }
    }
    bool checkAdjacent(int x, int y, float noise, int rasterSize) {
      int startmaxX = Math.Max(0, x-rasterSize);
      int startmaxY = Math.Max(0, y-rasterSize);
      int endminX = Math.Min(precise_grid.Count-1, x+rasterSize);
      int endminY = Math.Min(precise_grid[x].Count-1, y+rasterSize);
      for(int xi = startmaxX; xi <= endminX; xi++) {
        for(int yi = startmaxY; yi <= endminY; yi++) {
          if(precise_grid[xi][yi] > noise) {
            return false;
          }
        }
      }
      return true;
    }

    void LoadMountainTextures() {
      string[] guidsLarge = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/mountains/large"});
      string[] guidsSmall = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/mountains/small"});
      string[] guidsSmallSnowless = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/mountains/snowless_small"});
      foreach( var guid in guidsSmall )
      {
          var path = AssetDatabase.GUIDToAssetPath( guid );
          small_mts.Add(AssetDatabase.LoadAssetAtPath<GameObject>( path ));
      }
      foreach( var guid in guidsLarge )
      {
          var path = AssetDatabase.GUIDToAssetPath( guid );
          large_mts.Add(AssetDatabase.LoadAssetAtPath<GameObject>( path ));
      }

      foreach( var guid in guidsSmallSnowless )
      {
          var path = AssetDatabase.GUIDToAssetPath( guid );
          snowless_small_mts.Add(AssetDatabase.LoadAssetAtPath<GameObject>( path ));
      }
    }

    void PlaceMountain(float noise, int x, int y) {
      GameObject go;
      if(noise >= tileset.Count-mountainLayersLarge && checkAdjacent(x,y, noise, 3)) {
        go = Instantiate(large_mts[UnityEngine.Random.Range(0, 7)]);
      }
      else if(noise >= tileset.Count-mountainLayersSmall-mountainLayersLarge && checkAdjacent(x,y, noise, 4)) {
        go = Instantiate(small_mts[UnityEngine.Random.Range(0, 7)]);
      }
      else if(noise >= tileset.Count-mountainLayersSmall-mountainLayersLarge-mountainLayersSnowless
              && checkAdjacent(x,y, noise, 4)) {
        go = Instantiate(snowless_small_mts[UnityEngine.Random.Range(0, 7)]);
      }
      else {
        return;
      }
        go.transform.parent = gameObject.transform;
        go.transform.localPosition = new Vector3((x-map_width/2)*10, (y-map_height/2)*10, 1);
        go.transform.localScale = new Vector3(map_width/mountainSize, map_height/mountainSize, 1);
    }

    void SetGlobalVariables() {
      PlayerPrefs.SetInt("mapWidth", map_width);
      PlayerPrefs.SetInt("mapHeight", map_height);
      PlayerPrefs.SetFloat("mountainSize", mountainSize);
    }
}

