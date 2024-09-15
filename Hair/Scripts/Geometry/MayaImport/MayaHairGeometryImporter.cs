// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.MayaHairGeometryImporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Debug;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport
{
  public class MayaHairGeometryImporter : GeometryProviderBase
  {
    [SerializeField]
    public bool DebugDraw = true;
    [SerializeField]
    public Texture2D RegionsTexture;
    [SerializeField]
    public MeshProvider ScalpProvider;
    [SerializeField]
    public GameObject HairContainer;
    [SerializeField]
    public float RegionThresholdDistance = 0.5f;
    [SerializeField]
    public MayaHairData Data = new MayaHairData();
    [SerializeField]
    public Bounds Bounds;

    public void Process()
    {
      if (!this.ValidateImpl(true))
        return;
      new CacheMayaHairData(this).Cache();
      this.Data.Validate(true);
    }

    public override void Dispatch() => this.ScalpProvider.Dispatch();

    public bool ValidateHairContainer(bool log)
    {
      if (!((UnityEngine.Object) this.HairContainer == (UnityEngine.Object) null))
        return true;
      if (!log)
        ;
      return false;
    }

    public override bool Validate(bool log) => this.ValidateImpl(log) && this.Data.Validate(log);

    private bool ValidateImpl(bool log) => this.ScalpProvider.Validate(log) && this.ValidateHairContainer(log);

    public override Bounds GetBounds() => this.transform.TransformBounds(this.Bounds);

    public override int GetSegmentsNum() => this.Data.Segments;

    public override int GetStandsNum() => this.Data.Vertices.Count / this.Data.Segments;

    public override int[] GetIndices() => this.Data.Indices;

    public override List<Vector3> GetVertices() => this.Data.Vertices;

    public override void SetVertices(List<Vector3> verts) => throw new NotImplementedException();

    public override List<float> GetRigidities() => (List<float>) null;

    public override void SetRigidities(List<float> rigidities) => throw new NotImplementedException();

    public override void CalculateNearbyVertexGroups() => throw new NotImplementedException();

    public override List<Vector4ListContainer> GetNearbyVertexGroups() => throw new NotImplementedException();

    public override List<Color> GetColors() => ((IEnumerable<Color>) new Color[this.Data.Vertices.Count]).ToList<Color>();

    public override GpuBuffer<Matrix4x4> GetTransformsBuffer() => this.ScalpProvider.ToWorldMatricesBuffer;

    public override GpuBuffer<Vector3> GetNormalsBuffer() => this.ScalpProvider.NormalsBuffer;

    public override Matrix4x4 GetToWorldMatrix() => this.ScalpProvider.ToWorldMatrix;

    public override int[] GetHairRootToScalpMap() => this.Data.HairRootToScalpMap;

    private void OnDrawGizmos()
    {
      if (!this.DebugDraw || !this.Validate(false))
        return;
      MayaImporterDebugDraw.Draw(this);
    }
  }
}
