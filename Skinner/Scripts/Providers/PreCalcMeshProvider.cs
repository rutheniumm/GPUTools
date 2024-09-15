// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Providers.PreCalcMeshProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using System;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
  [Serializable]
  public class PreCalcMeshProvider : MonoBehaviour, IMeshProvider
  {
    public bool provideToWorldMatrices;
    public bool useBaseMesh;
    public int[] materialsToUse;
    public bool drawInPostProcess = true;

    public virtual bool Validate(bool log)
    {
      if (!log)
        ;
      return (UnityEngine.Object) this.Mesh != (UnityEngine.Object) null;
    }

    public virtual Matrix4x4 ToWorldMatrix => this.transform.localToWorldMatrix;

    public virtual GpuBuffer<Matrix4x4> ToWorldMatricesBuffer => (GpuBuffer<Matrix4x4>) null;

    public virtual GpuBuffer<Vector3> PreCalculatedVerticesBuffer { get; protected set; }

    public virtual GpuBuffer<Vector3> NormalsBuffer { get; protected set; }

    public virtual Mesh Mesh => (Mesh) null;

    public virtual Mesh BaseMesh => (Mesh) null;

    public virtual Mesh MeshForImport => (Mesh) null;

    public virtual Color[] VertexSimColors => (Color[]) null;

    public virtual void Stop()
    {
    }

    public virtual void Dispatch()
    {
    }

    public virtual void PostProcessDispatch(ComputeBuffer finalVerts)
    {
    }

    public virtual void Dispose()
    {
    }
  }
}
