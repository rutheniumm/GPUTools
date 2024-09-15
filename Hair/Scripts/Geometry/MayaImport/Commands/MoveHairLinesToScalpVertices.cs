// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.Commands.MoveHairLinesToScalpVertices
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
  public class MoveHairLinesToScalpVertices : ICacheCommand
  {
    private readonly MayaHairGeometryImporter importer;
    private readonly MayaHairData data;

    public MoveHairLinesToScalpVertices(MayaHairGeometryImporter importer)
    {
      this.importer = importer;
      this.data = importer.Data;
    }

    public void Cache()
    {
      int[] indices = this.importer.ScalpProvider.Mesh.GetIndices(0);
      Vector3[] vertices = this.importer.ScalpProvider.Mesh.vertices;
      List<Vector3> vector3List = new List<Vector3>();
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < indices.Length; index1 += 3)
      {
        Vector3 tringlesCenter = this.data.TringlesCenters[index1 / 3];
        int standIndex1 = this.FindStandIndex(this.data.Lines, tringlesCenter, this.data.Segments);
        if (standIndex1 != -1)
        {
          for (int index2 = 0; index2 < 3; ++index2)
          {
            Vector3 vertex = vertices[indices[index1 + index2]];
            Vector3 offset = vertex - tringlesCenter;
            int standIndex2 = this.FindStandIndex(vector3List, vertex, this.data.Segments);
            if (standIndex2 == -1 || !this.CompareStands(vector3List, standIndex2, this.data.Lines, standIndex1, this.data.Segments))
            {
              intList.Add(vector3List.Count / this.data.Segments);
              vector3List.AddRange((IEnumerable<Vector3>) this.CreateStandWithOffsetForRegion(this.data.Lines, offset, standIndex1, this.data.Segments));
            }
            else
              intList.Add(standIndex2 / this.data.Segments);
          }
        }
      }
      this.data.Vertices = vector3List;
      this.data.Indices = intList.ToArray();
    }

    private bool CompareStands(
      List<Vector3> hairStands1,
      int stand1,
      List<Vector3> hairStands2,
      int stand2,
      int segments)
    {
      float num = this.importer.RegionThresholdDistance * this.importer.RegionThresholdDistance;
      for (int index = 0; index < segments; ++index)
      {
        if ((double) (hairStands1[stand1 + index] - hairStands2[stand2 + index]).sqrMagnitude > (double) num)
          return false;
      }
      return true;
    }

    private int FindStandIndex(List<Vector3> hairVertices, Vector3 vertex, int segments)
    {
      for (int index = 0; index < hairVertices.Count; index += segments)
      {
        if (hairVertices[index] == vertex)
          return index;
      }
      return -1;
    }

    private List<Vector3> CreateStandWithOffsetForRegion(
      List<Vector3> vertices,
      Vector3 offset,
      int start,
      int segments)
    {
      List<Vector3> withOffsetForRegion = new List<Vector3>();
      int num = start + segments;
      for (int index = start; index < num; ++index)
        withOffsetForRegion.Add(vertices[index] + offset);
      return withOffsetForRegion;
    }
  }
}
