// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.SpatialSearch.SpatialSearcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.SpatialSearch
{
  public class SpatialSearcher
  {
    private readonly List<SearchVoxel> voxels;
    private readonly List<Vector3> vertices;
    private readonly Bounds bounds;
    private FixedList<int> fixedList;

    public SpatialSearcher(
      List<Vector3> vertices,
      Bounds bounds,
      int splitX,
      int splitY,
      int splitZ)
    {
      this.vertices = vertices;
      this.bounds = bounds;
      this.voxels = this.CreateVoxels(splitX, splitY, splitZ);
      this.fixedList = new FixedList<int>(vertices.Count);
    }

    private List<SearchVoxel> CreateVoxels(int splitX, int splitY, int splitZ)
    {
      Vector3 size = new Vector3(this.bounds.size.x / (float) splitX, this.bounds.size.y / (float) splitY, this.bounds.size.z / (float) splitZ);
      List<SearchVoxel> voxels = new List<SearchVoxel>();
      for (int index1 = 0; index1 <= splitX; ++index1)
      {
        for (int index2 = 0; index2 <= splitY; ++index2)
        {
          for (int index3 = 0; index3 <= splitZ; ++index3)
          {
            SearchVoxel searchVoxel = new SearchVoxel(this.vertices, new Bounds(this.bounds.center + new Vector3(size.x * (float) index1, size.y * (float) index2, size.z * (float) index3) - this.bounds.size * 0.5f, size));
            if (searchVoxel.TotalVertices > 0)
              voxels.Add(searchVoxel);
          }
        }
      }
      return voxels;
    }

    public FixedList<int> SearchInSphereSlow(Vector3 center, float radius)
    {
      this.fixedList.Reset();
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        Vector3 vertex = this.vertices[index];
        if ((double) (center - vertex).sqrMagnitude < (double) radius * (double) radius)
          this.fixedList.Add(index);
      }
      return this.fixedList;
    }

    public List<int> SearchInSphereSlow(Ray ray, float radius)
    {
      List<int> intList = new List<int>();
      float num = radius * radius;
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        Vector3 vertex = this.vertices[index];
        if ((double) Vector3.Cross(ray.direction, vertex - ray.origin).sqrMagnitude < (double) num)
          intList.Add(index);
      }
      return intList;
    }

    public List<int> SearchInSphere(Vector3 center, float radius)
    {
      List<SearchVoxel> searchVoxelList = this.SearchVoxelsInSphere(center, radius);
      List<int> intList = new List<int>();
      foreach (SearchVoxel searchVoxel in searchVoxelList)
        intList.AddRange((IEnumerable<int>) searchVoxel.SearchInSphere(center, radius));
      return intList;
    }

    public List<int> SearchInSphere(Ray ray, float radius)
    {
      List<SearchVoxel> searchVoxelList = this.SearchVoxelsInSphere(ray, radius);
      List<int> intList = new List<int>();
      foreach (SearchVoxel searchVoxel in searchVoxelList)
        intList.AddRange((IEnumerable<int>) searchVoxel.SearchInSphere(ray, radius));
      return intList;
    }

    private List<SearchVoxel> SearchVoxelsInSphere(Vector3 center, float radius)
    {
      List<SearchVoxel> searchVoxelList = new List<SearchVoxel>();
      foreach (SearchVoxel voxel in this.voxels)
      {
        if ((double) (voxel.Bounds.ClosestPoint(center) - center).sqrMagnitude < (double) radius * (double) radius)
          searchVoxelList.Add(voxel);
      }
      return searchVoxelList;
    }

    private List<SearchVoxel> SearchVoxelsInSphere(Ray ray, float radius)
    {
      List<SearchVoxel> searchVoxelList = new List<SearchVoxel>();
      foreach (SearchVoxel voxel in this.voxels)
      {
        if (voxel.Bounds.IntersectRay(ray))
          searchVoxelList.Add(voxel);
      }
      return searchVoxelList;
    }

    public void DebugDraw(Transform transform)
    {
      foreach (SearchVoxel voxel in this.voxels)
        voxel.DebugDraw(transform);
    }
  }
}
