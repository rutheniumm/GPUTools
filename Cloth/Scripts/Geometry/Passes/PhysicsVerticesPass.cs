// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.PhysicsVerticesPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Common.Scripts.Tools.Commands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class PhysicsVerticesPass : ICacheCommand
  {
    private readonly Mesh mesh;
    private readonly ClothGeometryData data;
    private int[] meshToPhysicsVerticesMap;
    private int[] physicsToMeshVerticesMap;

    public PhysicsVerticesPass(ClothSettings settings)
    {
      this.data = settings.GeometryData;
      this.mesh = settings.MeshProvider.MeshForImport;
    }

    public void Cache()
    {
      if (this.data == null || !((Object) this.mesh != (Object) null))
        return;
      Vector3[] vertices = this.mesh.vertices;
      this.data.Particles = this.CreatePhysicsVerticesArray(vertices);
      this.CreateMeshToPhysicsVerticesMap(vertices, this.data.Particles);
      this.data.MeshToPhysicsVerticesMap = this.meshToPhysicsVerticesMap;
      this.data.PhysicsToMeshVerticesMap = this.physicsToMeshVerticesMap;
    }

    private Vector3[] CreatePhysicsVerticesArray(Vector3[] vertices)
    {
      HashSet<Vector3> source = new HashSet<Vector3>();
      for (int index = 0; index < vertices.Length; ++index)
        source.Add(vertices[index]);
      return source.ToArray<Vector3>();
    }

    private void CreateMeshToPhysicsVerticesMap(Vector3[] vertices, Vector3[] physicsVertices)
    {
      Dictionary<Vector3, int> dictionary = new Dictionary<Vector3, int>();
      for (int index = 0; index < physicsVertices.Length; ++index)
      {
        Vector3 physicsVertex = physicsVertices[index];
        dictionary.Add(physicsVertex, index);
      }
      this.meshToPhysicsVerticesMap = new int[vertices.Length];
      this.physicsToMeshVerticesMap = new int[physicsVertices.Length];
      for (int index1 = 0; index1 < vertices.Length; ++index1)
      {
        Vector3 vertex = vertices[index1];
        if (dictionary.ContainsKey(vertex))
        {
          int index2 = dictionary[vertex];
          this.meshToPhysicsVerticesMap[index1] = index2;
          this.physicsToMeshVerticesMap[index2] = index1;
        }
        else
          this.meshToPhysicsVerticesMap[index1] = -1;
      }
    }
  }
}
