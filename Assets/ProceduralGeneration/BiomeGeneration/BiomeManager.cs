using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
  List<List<double>> BiomeMap;
  GameObject indirectParent;
    int waterLayers;
    List<int> OffsetX;
    List<int> OffsetY;
    int plainsLayers;
    int sandLayers;
    int mountainLayers;
    Dictionary<int, Biome> biomes;
    Dictionary<int, GameObject> defaultTextures;
    public BiomeManager(
      int waterLayers,
      int plainsLayers,
      int sandLayers,
      int mountainLayers,
      int width,
      int height,
      Dictionary<int, GameObject> defaultTextures,
      GameObject indirectParent) 
    {
      this.waterLayers = waterLayers;
      this.plainsLayers = plainsLayers;
      this.sandLayers = sandLayers;
      this.mountainLayers = mountainLayers;
      this.defaultTextures = defaultTextures;
      this.indirectParent = indirectParent;
      OffsetX = new List<int>();
      OffsetY = new List<int>();
      for(int i = 0; i < 3; i++) {
        OffsetX.Add(UnityEngine.Random.Range(-10000, +10000));
        OffsetY.Add(UnityEngine.Random.Range(-10000, +10000));
      }
      SetupBiomes();
      BiomeMap = new List<List<double>>();
      float range = BiomeRange();
      for(int i = 0; i < width; i++) {
        BiomeMap.Add(new List<double>());
        for(int j = 0; j < height; j++) {
          BiomeMap[i].Add(FindPerlin(i, j, range));
        }
      }
    }
    
    public void ApplyBiome(string identifier, List<List<float>> height) {
      foreach (KeyValuePair<int, Biome> entry in biomes)
      {
        if(entry.Value.Name.Equals(identifier)) {
          for(int i = 0; i < BiomeMap.Count; i++) {
            for(int j = 0; j < BiomeMap[i].Count; j++) {
              if(height[i][j] >= waterLayers+sandLayers && height[i][j] < waterLayers+sandLayers+plainsLayers) {
                entry.Value.GenerationStrategy(BiomeMap, i,j, height[i][j], indirectParent);
              }
            }
          }
        }
      }
    }

    private void SetupBiomes() {
      this.biomes = new Dictionary<int, Biome>();
      Biome pineForest = new PineForest(0f, 3f, 0.7f, 1.1f, waterLayers+sandLayers, waterLayers+sandLayers+plainsLayers-1f, 10,2);
      this.biomes.Add(1, pineForest);
    }

    private double FindPerlin(int x, int y, float range)
    {
      float magnification = 0.03f;
      double RawPerlin = (Mathf.PerlinNoise((x - OffsetX[0]) * magnification / 10, (y - OffsetY[0]) * magnification / 10)); 
      RawPerlin += (Mathf.PerlinNoise((x - OffsetX[1]) * magnification * 4, (y - OffsetY[1]) * magnification*4)-0.5f) * 0.0275f;
      RawPerlin += (Mathf.PerlinNoise((x - OffsetX[2]) * magnification, (y - OffsetY[2]) * magnification) -0.5f) / 5.5f;
      double ClampPerlin = Math.Max(Math.Min(RawPerlin, 1.0),0);
      
      double ScalePerlin = ClampPerlin * (range-1);
      if(ScalePerlin == range)
      {
          ScalePerlin = range-1;
      }
      return ScalePerlin;
    }

    public Biome GetDominantBiome(int x, int y) {
      Biome DominantBiome = null;
      int maxDominance = -1;
      foreach (KeyValuePair<int, Biome> entry in biomes)
      {
        if(entry.Value.BiomeStart >= BiomeMap[x][y] && entry.Value.BiomeEnd <= BiomeMap[x][y]) {
          if(entry.Value.BiomeDominance > maxDominance) {
            DominantBiome = entry.Value;
          };
        }
      }
      return DominantBiome;
    } 

    private float BiomeRange() {
      float max = 0;
      foreach(KeyValuePair<int, Biome> entry in biomes) {
        max = Math.Max(entry.Value.BiomeEnd, max); 
      }
      Debug.Log("Max was found to be " + max);
      return max;
    }

}
