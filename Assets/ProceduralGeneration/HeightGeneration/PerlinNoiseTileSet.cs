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

    private List<GameObject> instantiatedExtras = new List<GameObject>();

    private TilemapTextureSpread textureScript;
    private BiomeManager BiomeManager;
    public int MapWidth = 1000;
    public int MapHeight = 1000;

    int waterLayers = 8;
        int sandLayers = 1;
        int plainsLayers = 5;
        int hillLayers = 7;

    private int renderProgress = 0;
    private int renderIncrement = 300;
    public float noiseModifier = 0.5f;

    List<List<int>> heightLevelGrid = new List<List<int>>();
    List<List<float>> heightGrid = new List<List<float>>();

    Dictionary<int, GameObject> textureLayers = new Dictionary<int, GameObject>();

    public float magnification = 0.03f;
    private int mountainLayerId;
    private int mountainLayersSnowless = 2;
    private int mountainLayersSmall = 1;
    private int mountainLayersLarge = 4;

    private float mountainSize = 10f/3f;

    List<int> OffsetX = new List<int>();
    List<int> OffsetY = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        LoadMountainTextures();
        textureScript = gameObject.GetComponent<TilemapTextureSpread>();
        textureScript.SetupPrefabs(MapWidth, MapHeight);
        SeedGenerator.generateNew();
        SeedGenerator.fillOffsets1(OffsetX, OffsetY);
        CreateTileSet();
        CreateTileGroup();
        InitRawTextures("Assets/Prefabs/Textures");
        textureLayers = textureScript.cleanLayers(tileset, MapWidth, MapHeight);
        GenerateMap();
        textureScript.ApplyLayers();
        Debug.Log("Done ---");
    }

    private void GenerateMap()
    {
      BiomeManager = new BiomeManager(waterLayers, plainsLayers, sandLayers, hillLayers, MapWidth, MapHeight, 3,  tileset, gameObject);
      heightLevelGrid = new List<List<int>>();
      heightGrid = new List<List<float>>();
      for(int x = 0; x < MapWidth; x++)
        {
            heightLevelGrid.Add(new List<int>());
            heightGrid.Add(new List<float>());
            for(int y = 0; y < MapHeight; y++)
            {
                float noise = GetIdUsingPerlin(x, y);
                heightGrid[x].Add(noise);
                int tile_id = Mathf.FloorToInt(noise);
                heightLevelGrid[x].Add(tile_id);
               // Debug.Log("tileset " + tile_id);
                //Debug.Log(" is in both" + tileset[tile_id] + " asdf "+  textureLayers[tile_id]);
                GameObject texture = tileset[tile_id];
                if(BiomeManager.HasDominantBiome(x,y)) {
                  if(tile_id >= waterLayers && tile_id < waterLayers+sandLayers) {
                    texture = BiomeManager.GetDominantBiome(x,y).textureSand;
                  }
                  if(tile_id >= waterLayers+sandLayers && tile_id < waterLayers+sandLayers+plainsLayers) {
                    texture = BiomeManager.GetDominantBiome(x,y).texturePlains;
                  }
                }
                textureScript.InterpolateTexture(tile_id, texture, textureLayers[tile_id], MapWidth, MapHeight, x, y);
            }
        }
        BiomeManager.ApplyBiome("pine-forest", heightGrid);
        BiomeManager.ApplyBiome("green-forest", heightGrid);
        BiomeManager.ApplyBiome("cities", heightGrid);
        SetMountains();
    }


    private float GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = (Mathf.PerlinNoise((x - OffsetX[0]) * magnification / 10, (y - OffsetY[0]) * magnification / 10)); 
        raw_perlin += (Mathf.PerlinNoise((x - OffsetX[1]) * magnification * 4, (y - OffsetY[1]) * magnification*4)-0.5f) * 0.0275f;
        raw_perlin += (Mathf.PerlinNoise((x - OffsetX[2]) * magnification, (y - OffsetY[2]) * magnification) -0.5f) / 5.5f;
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
      for(int x = 0; x < MapWidth; x++)
        {
            for(int y = 0; y < MapHeight; y++)
            {
              float noise = heightGrid[x][y];
              PlaceMountain(noise, x, y);
            }
        }
    }
    bool checkAdjacent(int x, int y, float noise, int rasterSize) {
      int startmaxX = Math.Max(0, x-rasterSize);
      int startmaxY = Math.Max(0, y-rasterSize);
      int endminX = Math.Min(heightGrid.Count-1, x+rasterSize);
      int endminY = Math.Min(heightGrid[x].Count-1, y+rasterSize);
      for(int xi = startmaxX; xi <= endminX; xi++) {
        for(int yi = startmaxY; yi <= endminY; yi++) {
          if(heightGrid[xi][yi] > noise) {
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
        go.transform.localPosition = new Vector3((x-MapWidth/2)*10, (y-MapHeight/2)*10, 1);
        go.transform.localScale = new Vector3(MapWidth/mountainSize, MapHeight/mountainSize, 1);
        SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
        renderer.sortingOrder = 1000+y;
        instantiatedExtras.Add(go);
    }

    void SetGlobalVariables() {
      PlayerPrefs.SetInt("mapWidth", MapWidth);
      PlayerPrefs.SetInt("mapHeight", MapHeight);
      PlayerPrefs.SetFloat("mountainSize", mountainSize);
    }

    void InitRawTextures(string directory) {
      Dictionary<int, Color32[]> rawTextures = new Dictionary<int, Color32[]>();
      string[] textures = AssetDatabase.FindAssets("t:prefab", new string[]{directory});
        foreach(string guid in textures)
        {
          var path = AssetDatabase.GUIDToAssetPath(guid);
          GameObject go = (AssetDatabase.LoadAssetAtPath<GameObject>(path));
          Texture2D texture = go.GetComponent<SpriteRenderer>().sprite.texture;
          Debug.Log("Adding " + texture.GetInstanceID());
          rawTextures.Add(texture.GetInstanceID(), texture.GetRawTextureData<Color32>().ToArray());
        }
      textureScript.InitRawTextures(rawTextures);
    }

    public void RegenerateMap() {
        SeedGenerator.fillOffsets1(OffsetX, OffsetY);
        Debug.Log("Offsets now " + OffsetX[0] + " an "+ OffsetY[0]);
        textureLayers = textureScript.cleanLayers(tileset, MapWidth, MapHeight);
        foreach(GameObject mt in instantiatedExtras) {
          Destroy(mt);
        }
        BiomeManager.clearInstantiatedExtras();
        GenerateMap();
        textureScript.ApplyLayers();
    }
}

