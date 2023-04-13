using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SeedGenerator
{
    public static int x1;
    public static int y1;
    public static int x2;
    public static int y2;

    public static void setSeed(string seed) {
      string[] offsets = DecodeBase64(seed).Split(' ');
      x1 = Int32.Parse(offsets[0]);
      y1 = Int32.Parse(offsets[1]);
      x2 = Int32.Parse(offsets[2]);
      y2 = Int32.Parse(offsets[3]);
    }

    public static void generateNew() {
      x1 = UnityEngine.Random.Range(-10000, +10000);
      y1 = UnityEngine.Random.Range(-10000, +10000);
      x2 = UnityEngine.Random.Range(-10000, +10000);
      y2 = UnityEngine.Random.Range(-10000, +10000);
    }

    public static void fillOffsets1(List<int> OffsetX, List<int> OffsetY) {
      OffsetX.Clear();
      OffsetY.Clear();
      OffsetX.Add(x1);
      OffsetY.Add(y1);
      OffsetY.Add((x1*y1) % 10000);
      OffsetY.Add((OffsetY[1]*y1) % 10000);
      OffsetX.Add((OffsetY[2]*x1) % 10000);
      OffsetX.Add((OffsetX[1]*x1) % 10000);
    }

    public static void fillOffsets2(List<int> OffsetX, List<int> OffsetY) {
      OffsetX.Clear();
      OffsetY.Clear();
      OffsetX.Add(x2);
      OffsetY.Add(y2);
      OffsetY.Add((x2*y2) % 10000);
      OffsetY.Add((OffsetY[1]*y2) % 10000);
      OffsetX.Add((OffsetY[2]*x2) % 10000);
      OffsetX.Add((OffsetX[1]*x2) % 10000);
    }

    public static string computeSeed() {
      return EncodeBase64(x1 + " " + y1 + " " + x2 + " " + y2);
    }

    public static string EncodeBase64(string value)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(valueBytes);
    }

    public static string DecodeBase64(string value)
    {
      Debug.Log("decoding " + value);
        var valueBytes = System.Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(valueBytes);
    }
}