// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Tools.ScalpProcessingTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
  public class ScalpProcessingTools
  {
    public const float Accuracy = 1E-05f;

    public static List<int> HairRootToScalpIndices(
      List<Vector3> scalpVertices,
      List<Vector3> hairVertices,
      int segments,
      float accuracy = 1E-05f)
    {
      List<int> scalpIndices = new List<int>();
      for (int index1 = 0; index1 < hairVertices.Count; index1 += segments)
      {
        for (int index2 = 0; index2 < scalpVertices.Count; ++index2)
        {
          if ((double) (hairVertices[index1] - scalpVertices[index2]).sqrMagnitude < (double) accuracy * (double) accuracy)
          {
            scalpIndices.Add(index2);
            break;
          }
        }
      }
      return scalpIndices;
    }

    public static List<int> ProcessIndices(
      List<int> scalpIndices,
      List<Vector3> scalpVertices,
      List<List<Vector3>> hairVerticesGroups,
      int segments,
      float accuracy = 1E-05f)
    {
      List<int> intList = new List<int>();
      int startIndex = 0;
      foreach (List<Vector3> hairVerticesGroup in hairVerticesGroups)
      {
        List<int> collection = ScalpProcessingTools.ProcessIndicesForMesh(startIndex, scalpVertices, scalpIndices, hairVerticesGroup, segments, accuracy);
        intList.AddRange((IEnumerable<int>) collection);
        startIndex += hairVerticesGroup.Count;
      }
      for (int index = 0; index < intList.Count; ++index)
        intList[index] /= segments;
      return intList;
    }

    private static List<int> ProcessIndicesForMesh(
      int startIndex,
      List<Vector3> scalpVertices,
      List<int> scalpIndices,
      List<Vector3> hairVertices,
      int segments,
      float accuracy = 1E-05f)
    {
      List<int> hairIndices = new List<int>();
      for (int index1 = 0; index1 < scalpIndices.Count; ++index1)
      {
        int scalpIndex = scalpIndices[index1];
        Vector3 scalpVertex = scalpVertices[scalpIndex];
        if (index1 % 3 == 0)
          ScalpProcessingTools.FixNotCompletedPolygon(hairIndices);
        for (int index2 = 0; index2 < hairVertices.Count; index2 += segments)
        {
          if ((double) (hairVertices[index2] - scalpVertex).sqrMagnitude < (double) accuracy * (double) accuracy)
          {
            hairIndices.Add(startIndex + index2);
            break;
          }
        }
      }
      ScalpProcessingTools.FixNotCompletedPolygon(hairIndices);
      return hairIndices;
    }

    private static void FixNotCompletedPolygon(List<int> hairIndices)
    {
      int count = hairIndices.Count % 3;
      if (count <= 0)
        return;
      hairIndices.RemoveRange(hairIndices.Count - count, count);
    }

    public static float MiddleDistanceBetweenPoints(Mesh mesh)
    {
      Vector3[] vertices = mesh.vertices;
      int[] indices = mesh.GetIndices(0);
      float num1 = 0.0f;
      int num2 = 0;
      for (int index = 0; index < Mathf.Min(500, indices.Length); index += 3)
      {
        Vector3 a = vertices[indices[index]];
        Vector3 b = vertices[indices[index + 1]];
        num1 += Vector3.Distance(a, b);
        ++num2;
      }
      return num1 / (float) num2;
    }

    public static List<Vector3> ShiftToScalpRoot(
      List<Vector3> scalpVertices,
      List<Vector3> hairVertices,
      int segments)
    {
      for (int index1 = 0; index1 < hairVertices.Count; index1 += segments)
      {
        int index2 = 0;
        float num = float.MaxValue;
        for (int index3 = 0; index3 < scalpVertices.Count; ++index3)
        {
          float sqrMagnitude = (hairVertices[index1] - scalpVertices[index3]).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            index2 = index3;
            num = sqrMagnitude;
          }
        }
        hairVertices[index1] = scalpVertices[index2];
      }
      return hairVertices;
    }
  }
}
