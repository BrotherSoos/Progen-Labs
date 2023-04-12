using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PineForest : Biome
{

    static readonly GameObject sandTexture;
    static readonly GameObject plainsTexture;

    static readonly string propFolder;
    private int Density;

    static PineForest() {
      sandTexture = Resources.Load("Assets/Prefab/plains_2") as GameObject;
      plainsTexture = Resources.Load("Assets/Prefab/sand") as GameObject;
      propFolder = "Assets/Prefabs/Biomes/Props/PineForest";
    }
    public PineForest(float biomeStart, float biomeEnd, float propStart, float propEnd, float heightConstraintStart, float heightConstraintEnd, float BiomeDominance, int Density) 
    : base("pine-forest", 1, sandTexture, plainsTexture, biomeStart, biomeEnd, propFolder, propStart, propEnd, heightConstraintStart, heightConstraintEnd, BiomeDominance)
    {
      this.Density = Density;
    }

    public override void GenerationStrategy(List<List<double>> noise, int x, int y, float height, GameObject parent)
    {
      if(height <= heightConstraintStart || height > heightConstraintEnd) {
        return;
      }
      if(IsPropToBePlaced(noise[x][y]) && IsLocalMaximum(x, y, noise)) {
        GameObject go = Instantiate(GetRandomProp());
        go.transform.parent = parent.transform;
        go.transform.localPosition = new Vector3((x-noise.Count/2)*10, (y-noise[x].Count/2)*10, 1);
        go.transform.localScale = new Vector3(noise.Count/5f, noise[x].Count/5f, 1);
        SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
        renderer.sortingOrder = (int)(BiomeDominance)+1;
      }
    }

    public bool IsLocalMaximum(int i, int j, List<List<double>> noise) {

      for(int k = Math.Max(i-(Density/2), 0); k <= Math.Min(i+(Density/2), noise.Count-1); k++) {
        for(int l = Math.Max(j-(Density/2), 0); l <= Math.Min(j+(Density/2), noise[i].Count-1); l++) {
          if(noise[i][j] < noise[k][l]) {
            return false;
          }
        }
      }
      return true;
    }
}