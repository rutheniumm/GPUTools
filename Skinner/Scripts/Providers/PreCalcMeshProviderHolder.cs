// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Providers.PreCalcMeshProviderHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
  public class PreCalcMeshProviderHolder : PreCalcMeshProvider
  {
    protected PreCalcMeshProvider _provider;

    public PreCalcMeshProvider provider
    {
      get => this._provider;
      set
      {
        if (!((Object) this._provider != (Object) value))
          return;
        this._provider = value;
      }
    }

    public override bool Validate(bool log) => (Object) this.provider != (Object) null && this.provider.Validate(log);

    public override Matrix4x4 ToWorldMatrix => (Object) this.provider != (Object) null ? this.provider.ToWorldMatrix : Matrix4x4.identity;

    public override GpuBuffer<Matrix4x4> ToWorldMatricesBuffer => (Object) this.provider != (Object) null ? this.provider.ToWorldMatricesBuffer : (GpuBuffer<Matrix4x4>) null;

    public override GpuBuffer<Vector3> PreCalculatedVerticesBuffer => (Object) this.provider != (Object) null ? this.provider.PreCalculatedVerticesBuffer : (GpuBuffer<Vector3>) null;

    public override GpuBuffer<Vector3> NormalsBuffer => (Object) this.provider != (Object) null ? this.provider.NormalsBuffer : (GpuBuffer<Vector3>) null;

    public override Mesh Mesh => (Object) this.provider != (Object) null ? this.provider.Mesh : (Mesh) null;

    public override Mesh BaseMesh => (Object) this.provider != (Object) null ? this.provider.BaseMesh : (Mesh) null;

    public override Mesh MeshForImport => (Object) this.provider != (Object) null ? this.provider.MeshForImport : (Mesh) null;

    public override Color[] VertexSimColors => (Object) this.provider != (Object) null ? this.provider.VertexSimColors : (Color[]) null;

    public override void Stop()
    {
      if (!((Object) this.provider != (Object) null))
        return;
      this.provider.Stop();
    }

    public override void Dispatch()
    {
      if (!((Object) this.provider != (Object) null))
        return;
      this.provider.provideToWorldMatrices = this.provideToWorldMatrices;
      this.provider.Dispatch();
    }

    public override void PostProcessDispatch(ComputeBuffer finalVerts)
    {
      if (!((Object) this.provider != (Object) null))
        return;
      this.provider.PostProcessDispatch(finalVerts);
    }

    public override void Dispose()
    {
      if (!((Object) this.provider != (Object) null))
        return;
      this.provider.Dispose();
    }
  }
}
