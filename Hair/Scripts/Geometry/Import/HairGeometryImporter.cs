// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Import.HairGeometryImporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Import
{
  [ExecuteInEditMode]
  public class HairGeometryImporter : GeometryProviderBase
  {
    [SerializeField]
    public bool DebugDraw = true;
    [SerializeField]
    public int Segments = 5;
    [SerializeField]
    public HairGroupsProvider HairGroupsProvider = new HairGroupsProvider();
    [SerializeField]
    public MeshProvider ScalpProvider = new MeshProvider();
    [SerializeField]
    public int[] Indices;
    [SerializeField]
    public Bounds Bounds;

    public override bool Validate(bool log)
    {
      if (this.Indices != null && this.Indices.Length != 0)
        return this.ValidateImpl(log);
      if (log)
        Debug.LogError((object) "Provider does not have any generated hair geometry");
      return false;
    }

    private bool ValidateImpl(bool log)
    {
      if (this.ScalpProvider.Validate(false))
        return this.HairGroupsProvider.Validate(log);
      if (log)
        Debug.LogError((object) "Scalp field is null");
      return false;
    }

    public void Process()
    {
      if (!this.ValidateImpl(true))
        return;
      this.HairGroupsProvider.Process(this.ScalpProvider.ToWorldMatrix.inverse);
      this.Indices = this.ProcessIndices();
    }

    public override void Dispatch() => this.ScalpProvider.Dispatch();

    private void OnDestroy() => this.ScalpProvider.Dispose();

    private int[] ProcessMap()
    {
      float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(this.ScalpProvider.Mesh) * 0.1f;
      return this.ScalpProvider.Type == ScalpMeshType.Skinned || this.ScalpProvider.Type == ScalpMeshType.PreCalc ? ScalpProcessingTools.HairRootToScalpIndices(((IEnumerable<Vector3>) this.ScalpProvider.Mesh.vertices).ToList<Vector3>(), this.HairGroupsProvider.Vertices, this.GetSegmentsNum(), accuracy).ToArray() : new int[this.HairGroupsProvider.Vertices.Count / this.GetSegmentsNum()];
    }

    private int[] ProcessIndices()
    {
      float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(this.ScalpProvider.Mesh) * 0.1f;
      return ScalpProcessingTools.ProcessIndices(((IEnumerable<int>) this.ScalpProvider.Mesh.GetIndices(0)).ToList<int>(), ((IEnumerable<Vector3>) this.ScalpProvider.Mesh.vertices).ToList<Vector3>(), this.HairGroupsProvider.VerticesGroups, this.GetSegmentsNum(), accuracy).ToArray();
    }

    public override GpuBuffer<Matrix4x4> GetTransformsBuffer() => this.ScalpProvider.ToWorldMatricesBuffer;

    public override GpuBuffer<Vector3> GetNormalsBuffer() => this.ScalpProvider.NormalsBuffer;

    public override Matrix4x4 GetToWorldMatrix() => this.ScalpProvider.ToWorldMatrix;

    public override int[] GetHairRootToScalpMap() => this.ProcessMap();

    public override Bounds GetBounds() => this.transform.TransformBounds(this.Bounds);

    public override int GetSegmentsNum() => this.Segments;

    public override int GetStandsNum() => this.HairGroupsProvider.Vertices.Count / this.Segments;

    public override int[] GetIndices() => this.Indices;

    public override List<Vector3> GetVertices() => this.HairGroupsProvider.Vertices;

    public override void SetVertices(List<Vector3> verts) => throw new NotImplementedException();

    public override List<float> GetRigidities() => (List<float>) null;

    public override void SetRigidities(List<float> rigidities) => throw new NotImplementedException();

    public override void CalculateNearbyVertexGroups() => throw new NotImplementedException();

    public override List<Vector4ListContainer> GetNearbyVertexGroups() => throw new NotImplementedException();

    public override List<Color> GetColors() => this.HairGroupsProvider.Colors;

    private void OnDrawGizmos()
    {
      if (!this.DebugDraw || this.GetVertices() == null || !this.ValidateImpl(false))
        return;
      Matrix4x4 toWorldMatrix = this.ScalpProvider.ToWorldMatrix;
      List<Vector3> vertices = this.GetVertices();
      for (int index = 1; index < vertices.Count; ++index)
      {
        if (index % this.Segments != 0)
          Gizmos.DrawLine(toWorldMatrix.MultiplyPoint3x4(vertices[index - 1]), toWorldMatrix.MultiplyPoint3x4(vertices[index]));
      }
      Bounds bounds = this.GetBounds();
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
  }
}
