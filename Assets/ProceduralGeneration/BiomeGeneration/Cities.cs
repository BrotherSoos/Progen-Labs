using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cities : Biome
{
    private ManhattanGrid manhattanGrid = null;

    static readonly GameObject sandTexture;
    static readonly GameObject plainsTexture;

    static readonly string propFolder;
    private int Density;

    static Cities() {
      plainsTexture = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Textures/plains.prefab", typeof(GameObject)) as GameObject;
      sandTexture = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Textures/sand.prefab", typeof(GameObject)) as GameObject;
      propFolder = "Assets/Prefabs/Biomes/Props/Cities";
    }
    public Cities(float biomeStart, float biomeEnd, float propStart, float propEnd, float heightConstraintStart, float heightConstraintEnd, float BiomeDominance, int Density) 
    : base("cities", 1, sandTexture, plainsTexture, biomeStart, biomeEnd, propFolder, propStart, propEnd, heightConstraintStart, heightConstraintEnd, BiomeDominance)
    {
      this.Density = Density;
    }

    private List<List<float>> heights;
    private List<List<double>> noise;
    private Dictionary<int, List<int>> visitedCenters = new Dictionary<int, List<int>>();
    private GameObject parent;
    private int width;
    private int height;

    public override void GenerationStrategy(List<List<double>> noiseMap, List<List<float>> heightMap, GameObject parent)
    {
      this.noise = noiseMap;
      width = noise.Count;
      height = noise[0].Count;
      this.heights = heightMap;
      this.parent = parent;
        if(manhattanGrid == null) {
          manhattanGrid = new ManhattanGrid(this.noise);
        }
        hamiltonIteration(0,0, Density);
    }

    public void hamiltonIteration(int x, int y, int distance) {
      if(visitedCenters.ContainsKey(x) && visitedCenters[x].Contains(y)) {
        return;
      }
      double max = manhattanGrid.FindMaxInHamilton(x, y, Density);
      if(!visitedCenters.ContainsKey(x)) {
        visitedCenters.Add(x, new List<int>());
      }
      visitedCenters[x].Add(y);
      if(IsPropToBePlaced(noise[manhattanGrid.x][manhattanGrid.y])) {
        if(heights[manhattanGrid.x][manhattanGrid.y] > heightConstraintStart && 
           heights[manhattanGrid.x][manhattanGrid.y] <= heightConstraintEnd) {
          placeProp(manhattanGrid.x, manhattanGrid.y);
        }
      }
      if(x + (distance+1)*2 < width) {
        hamiltonIteration(x + (distance+1)*2, y, distance);
      }
      if(x + distance + 1 < width && y + distance + 1 < height) {
        hamiltonIteration(x+distance+1, y+distance+1, distance);
      }
      if(y+(distance+1)*2 < height) {
        hamiltonIteration(x, y+(distance+1)*2, distance);
      }
    }

    private void placeProp(int x, int y) {
      GameObject go = Instantiate(GetRandomProp());
      go.transform.parent = parent.transform;
      go.transform.localPosition = new Vector3((x-width/2)*10, (y-height/2)*10, 1);
      go.transform.localScale = new Vector3(width/5f, height/5f, 1);
      SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
      renderer.sortingOrder = (int)(BiomeDominance)+7;
      instantiatedExtras.Add(go);
    }
}