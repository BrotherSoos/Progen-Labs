using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Biome: MonoBehaviour
{
    public string Name;
    int uid;
    public GameObject textureSand;
    public GameObject texturePlains;
    public float BiomeStart;
    public float BiomeEnd;
    List<GameObject> props;
    float propStart;
    float propEnd;

    public float heightConstraintStart;
    public float heightConstraintEnd;

    public float BiomeDominance;

    public Biome(
      string name,
      int uid,
      GameObject textureSand,
      GameObject texturePlains,
      float BiomeStart,
      float BiomeEnd,
      string propFolder,
      float propStart,
      float propEnd,
      float heightConstraintStart,
      float heightConstraintEnd,
      float BiomeDominance)
    {
      this.Name = name;
      this.uid = uid;
      this.textureSand = textureSand;
      this.texturePlains = texturePlains;
      this.BiomeStart = BiomeStart;
      this.BiomeEnd = BiomeEnd;
      this.propStart = propStart;
      this.propEnd = propEnd;
      this.heightConstraintStart = heightConstraintStart;
      this.heightConstraintEnd = heightConstraintEnd;
      this.BiomeDominance = BiomeDominance;
      string[] assets = AssetDatabase.FindAssets("t:prefab", new string[] {propFolder});
      this.props = new List<GameObject>();
      foreach( var guid in assets )
      {
          var path = AssetDatabase.GUIDToAssetPath( guid );
          props.Add(AssetDatabase.LoadAssetAtPath<GameObject>( path ));
      }
      Debug.Log("tex " + texturePlains + " " + texturePlains.GetInstanceID());
    }

    public virtual GameObject GetRandomProp() {
      return props[Random.Range(0, props.Count)];
    }

    public virtual bool IsPropToBePlaced(double level) {
      if(level >= propStart && level <= propEnd && props.Count > 0) {
        return true;
      } 
      //Debug.Log(level + " not between " + propStart + " and " + propEnd);
      return false;
    }

    public abstract void GenerationStrategy(List<List<double>> noise, int x, int y, float height, GameObject parent);
}
