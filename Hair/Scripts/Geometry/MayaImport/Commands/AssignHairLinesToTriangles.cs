// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.Commands.AssignHairLinesToTriangles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Common.Scripts.Utils;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
  public class AssignHairLinesToTriangles : ICacheCommand
  {
    private readonly MayaHairGeometryImporter importer;
    private readonly MayaHairData data;

    public AssignHairLinesToTriangles(MayaHairGeometryImporter importer)
    {
      this.importer = importer;
      this.data = importer.Data;
    }

    public void Cache()
    {
      this.data.TringlesCenters = this.ComputeTringlesCenters();
      this.data.Lines = this.Assign(this.data.TringlesCenters);
    }

    private List<Vector3> Assign(List<Vector3> scalpTringlesCenters)
    {
      List<Vector3> lines = this.data.Lines;
      List<Vector3> vector3List1 = new List<Vector3>();
      List<Vector3> vector3List2 = new List<Vector3>();
      for (int index = 0; index < lines.Count; index += this.data.Segments)
      {
        Vector3 testVertex = lines[index];
        Vector3 closeVertex = MathSearchUtils.FindCloseVertex(scalpTringlesCenters, testVertex);
        Vector3 offset = closeVertex - testVertex;
        if (!vector3List2.Contains(closeVertex))
        {
          List<Vector3> withOffsetForRegion = this.CreateStandWithOffsetForRegion(lines, offset, index, index + this.data.Segments);
          vector3List1.AddRange((IEnumerable<Vector3>) withOffsetForRegion);
          vector3List2.Add(closeVertex);
        }
      }
      return vector3List1;
    }

    private List<Vector3> ComputeTringlesCenters()
    {
      int[] indices = this.importer.ScalpProvider.Mesh.GetIndices(0);
      Vector3[] vertices = this.importer.ScalpProvider.Mesh.vertices;
      List<Vector3> tringlesCenters = new List<Vector3>();
      for (int index = 0; index < indices.Length; index += 3)
      {
        Vector3 vector3_1 = vertices[indices[index]];
        Vector3 vector3_2 = vertices[indices[index + 1]];
        Vector3 vector3_3 = vertices[indices[index + 2]];
        tringlesCenters.Add((vector3_1 + vector3_2 + vector3_3) / 3f);
      }
      return tringlesCenters;
    }

    private List<Vector3> CreateStandWithOffsetForRegion(
      List<Vector3> vertices,
      Vector3 offset,
      int start,
      int end)
    {
      List<Vector3> withOffsetForRegion = new List<Vector3>();
      for (int index = start; index < end; ++index)
        withOffsetForRegion.Add(vertices[index] + offset);
      return withOffsetForRegion;
    }
  }
}
