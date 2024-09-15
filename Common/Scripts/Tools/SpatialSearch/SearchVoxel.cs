// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.SpatialSearch.SearchVoxel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.SpatialSearch
{
  public class SearchVoxel
  {
    private readonly List<Vector3> vertices;
    private readonly List<int> mineIndices;
    private readonly Bounds bounds;

    public SearchVoxel(List<Vector3> vertices, Bounds bounds)
    {
      this.bounds = bounds;
      this.vertices = vertices;
      this.mineIndices = this.SearchMineIndices();
    }

    private List<int> SearchMineIndices()
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        if (this.bounds.Contains(this.vertices[index]))
          intList.Add(index);
      }
      return intList;
    }

    public List<int> SearchInSphere(Vector3 center, float radius)
    {
      List<int> intList = new List<int>();
      foreach (int mineIndex in this.mineIndices)
      {
        if ((double) (this.vertices[mineIndex] - center).sqrMagnitude < (double) radius * (double) radius)
          intList.Add(mineIndex);
      }
      return intList;
    }

    public List<int> SearchInSphere(Ray ray, float radius)
    {
      List<int> intList = new List<int>();
      foreach (int mineIndex in this.mineIndices)
      {
        Vector3 vertex = this.vertices[mineIndex];
        if ((double) Vector3.Cross(ray.direction, vertex - ray.origin).sqrMagnitude < (double) radius * (double) radius)
          intList.Add(mineIndex);
      }
      return intList;
    }

    public void DebugDraw(Transform transforms)
    {
    }

    public Bounds Bounds => this.bounds;

    public int TotalVertices => this.mineIndices.Count;
  }
}
