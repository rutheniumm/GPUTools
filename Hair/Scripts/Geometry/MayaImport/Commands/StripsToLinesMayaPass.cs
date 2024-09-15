// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.Commands.StripsToLinesMayaPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
  public class StripsToLinesMayaPass : ICacheCommand
  {
    private readonly MayaHairGeometryImporter importer;

    public StripsToLinesMayaPass(MayaHairGeometryImporter importer) => this.importer = importer;

    public void Cache()
    {
      MeshFilter[] componentsInChildren = this.importer.HairContainer.GetComponentsInChildren<MeshFilter>();
      List<Vector3> vector3List = new List<Vector3>();
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        List<Vector3> collection = this.CacheStand(componentsInChildren[index]);
        vector3List.AddRange((IEnumerable<Vector3>) collection);
        this.importer.Data.Segments = collection.Count;
      }
      this.importer.Data.Lines = vector3List;
    }

    private List<Vector3> CacheStand(MeshFilter meshFilter)
    {
      Vector3[] vertices = meshFilter.sharedMesh.vertices;
      int[] triangles = meshFilter.sharedMesh.triangles;
      List<Vector3> vector3List = new List<Vector3>();
      for (int index1 = 0; index1 < triangles.Length; index1 += 6)
      {
        int index2 = triangles[index1 + 2];
        Vector3 scalpSpace = this.ToScalpSpace(meshFilter, vertices[index2]);
        vector3List.Add(scalpSpace);
      }
      return vector3List;
    }

    private Vector3 ToScalpSpace(MeshFilter filter, Vector3 point) => this.importer.ScalpProvider.ToWorldMatrix.inverse.MultiplyPoint3x4(filter.transform.TransformPoint(point));
  }
}
