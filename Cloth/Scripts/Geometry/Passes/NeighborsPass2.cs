// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.NeighborsPass2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Common.Scripts.Tools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class NeighborsPass2 : ICacheCommand
  {
    private readonly ClothGeometryData data;
    private readonly Mesh mesh;

    public NeighborsPass2(ClothSettings settings)
    {
      this.mesh = settings.MeshProvider.MeshForImport;
      this.data = settings.GeometryData;
    }

    public void Cache()
    {
      List<int>[] allTrianglesList = this.CreateAllTrianglesList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
      this.SortList(allTrianglesList);
      this.data.ParticleToNeiborCounts = this.CreateConts(allTrianglesList);
      this.data.ParticleToNeibor = this.ConvertTo1DArray(allTrianglesList);
    }

    private List<int>[] CreateAllTrianglesList(int[] triangles, int[] meshToPhysicsVerticesMap)
    {
      List<int>[] physList = new List<int>[this.data.MeshToPhysicsVerticesMap.Length];
      for (int index = 0; index < triangles.Length; index += 3)
      {
        int triangle1 = triangles[index];
        int triangle2 = triangles[index + 1];
        int triangle3 = triangles[index + 2];
        int toPhysicsVertices1 = meshToPhysicsVerticesMap[triangle1];
        int toPhysicsVertices2 = meshToPhysicsVerticesMap[triangle2];
        int toPhysicsVertices3 = meshToPhysicsVerticesMap[triangle3];
        this.Add(physList, triangle1, toPhysicsVertices2, toPhysicsVertices3);
        this.Add(physList, triangle2, toPhysicsVertices3, toPhysicsVertices1);
        this.Add(physList, triangle3, toPhysicsVertices1, toPhysicsVertices2);
      }
      return physList;
    }

    private void Add(List<int>[] physList, int p, int p1, int p2)
    {
      if (physList[p] == null)
        physList[p] = new List<int>();
      if (!physList[p].Contains(p1))
        physList[p].Add(p1);
      if (physList[p].Contains(p2))
        return;
      physList[p].Add(p2);
    }

    private int[] CreateConts(List<int>[] list)
    {
      int[] conts = new int[list.Length + 1];
      int num = 0;
      for (int index = 1; index < list.Length + 1; ++index)
      {
        num += list[index - 1].Count;
        conts[index] = num;
      }
      return conts;
    }

    private void SortList(List<int>[] list)
    {
      Vector3[] vertices = this.mesh.vertices;
      Vector3[] normals = this.mesh.normals;
      for (int index = 0; index < list.Length; ++index)
      {
        List<int> intList = list[index];
        Vector3 normal = normals[index];
        Vector3 vertex = vertices[index];
        intList.Sort((Comparison<int>) ((i1, i2) => (int) Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(this.data.Particles[i1] - vertex, this.data.Particles[i2] - vertex)))));
      }
    }

    private int[] ConvertTo1DArray(List<int>[] list) => ((IEnumerable<List<int>>) list).SelectMany<List<int>, int>((Func<List<int>, IEnumerable<int>>) (neibors => (IEnumerable<int>) neibors)).ToArray<int>();
  }
}
