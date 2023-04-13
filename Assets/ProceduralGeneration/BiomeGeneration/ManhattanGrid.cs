using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManhattanGrid
{
    public int x;
    public double globMax;
    public int y;
    public ManhattanGrid(List<List<double>> values) {
      this.values = values;
      this.visited = new List<List<bool>>();
      for(int i = 0; i < values.Count; i++) {
        this.visited.Add(new List<bool>());
        for(int j = 0; j < values[i].Count; j++) {
          this.visited[i].Add(false);
        }
      }
    }
    List<List<bool>> visited = new List<List<bool>>();
    List<List<double>> values;

    public double FindMaxInHamilton(int i, int j, int distance) {
      clearVisited(i, j, distance);
      globMax = -1;
      return f(i, j, distance);
    }

    public double f(int i, int j, int distance) {
      if(distance == 0) {
        visited[i][j] = true;
        if(values[i][j] > globMax) {
          globMax = values[i][j];
          x = i;
          y = j;
        }
        return values[i][j];
      }
      List<double> adjacent = new List<double>();
      if(!visited[Math.Max(0, i-1)][j]) {
        adjacent.Add(f(Math.Max(0, i-1), j, distance-1));
      }
      if(!visited[Math.Min(values.Count-1, i+1)][j]) {
        adjacent.Add(f(Math.Min(values.Count-1, i+1), j, distance-1));
      }
      if(!visited[i][Math.Max(0, j-1)]) {
        adjacent.Add(f(i,Math.Max(0, j-1), distance-1));
      }
      if(!visited[i][Math.Min(values[i].Count-1, j+1)]) {
        adjacent.Add(f(i, Math.Min(values[i].Count-1, j+1), distance-1));
      }
      double max = values[i][j];
      if(max > globMax) {
        globMax = max;
        x = i;
        y = j;
      }
      foreach(double value in adjacent) {
        max = Math.Max(value, max);
      }
      return max;
    }

    private void clearVisited(int x, int y, int distance) {
      for(int i = Math.Max(0, x-distance); i < Math.Min(visited.Count-1, x+distance); i++ ) {
        for(int j = Math.Max(0, y-distance); j < Math.Min(visited.Count-1, y+distance); j++) {
          visited[i][j] = false;
        }
      }
    }
}