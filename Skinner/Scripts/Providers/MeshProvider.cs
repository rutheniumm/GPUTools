// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Providers.MeshProvider
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
  public class MeshProvider : IMeshProvider
  {
    public ScalpMeshType Type;
    [SerializeField]
    public SkinnedMeshProvider SkinnedProvider = new SkinnedMeshProvider();
    [SerializeField]
    public StaticMeshProvider StaticProvider = new StaticMeshProvider();
    [SerializeField]
    public PreCalcMeshProvider PreCalcProvider;

    public bool Validate(bool log)
    {
      IMeshProvider currentProvider = this.GetCurrentProvider();
      return currentProvider != null && currentProvider.Validate(log);
    }

    public void Stop()
    {
      if (!((UnityEngine.Object) this.PreCalcProvider != (UnityEngine.Object) null))
        return;
      this.PreCalcProvider.Stop();
    }

    public void Dispatch() => this.GetCurrentProvider()?.Dispatch();

    public void Dispose() => this.GetCurrentProvider()?.Dispose();

    private IMeshProvider GetCurrentProvider()
    {
      if (this.Type == ScalpMeshType.Static)
        return (IMeshProvider) this.StaticProvider;
      return this.Type == ScalpMeshType.PreCalc ? (IMeshProvider) this.PreCalcProvider : (IMeshProvider) this.SkinnedProvider;
    }

    public Matrix4x4 ToWorldMatrix
    {
      get
      {
        IMeshProvider currentProvider = this.GetCurrentProvider();
        return currentProvider != null ? currentProvider.ToWorldMatrix : Matrix4x4.identity;
      }
    }

    public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer => this.GetCurrentProvider()?.ToWorldMatricesBuffer;

    public GpuBuffer<Vector3> PreCalculatedVerticesBuffer => this.GetCurrentProvider()?.PreCalculatedVerticesBuffer;

    public GpuBuffer<Vector3> NormalsBuffer => this.GetCurrentProvider()?.NormalsBuffer;

    public Mesh Mesh => this.GetCurrentProvider()?.Mesh;

    public Color[] SimColors => this.Type == ScalpMeshType.PreCalc && (UnityEngine.Object) this.PreCalcProvider != (UnityEngine.Object) null ? this.PreCalcProvider.VertexSimColors : (Color[]) null;

    public Mesh MeshForImport => this.GetCurrentProvider()?.MeshForImport;
  }
}
