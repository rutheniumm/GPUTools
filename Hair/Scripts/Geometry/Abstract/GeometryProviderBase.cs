// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Abstract.GeometryProviderBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Types;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Abstract
{
  public abstract class GeometryProviderBase : MonoBehaviour
  {
    public abstract Bounds GetBounds();

    public abstract int GetSegmentsNum();

    public abstract int GetStandsNum();

    public abstract int[] GetIndices();

    public abstract List<Vector3> GetVertices();

    public abstract void SetVertices(List<Vector3> verts);

    public abstract List<float> GetRigidities();

    public abstract void SetRigidities(List<float> rigidities);

    public abstract List<Color> GetColors();

    public abstract void CalculateNearbyVertexGroups();

    public abstract List<Vector4ListContainer> GetNearbyVertexGroups();

    public abstract GpuBuffer<Matrix4x4> GetTransformsBuffer();

    public abstract GpuBuffer<Vector3> GetNormalsBuffer();

    public abstract Matrix4x4 GetToWorldMatrix();

    public abstract int[] GetHairRootToScalpMap();

    public abstract void Dispatch();

    public abstract bool Validate(bool log);
  }
}
