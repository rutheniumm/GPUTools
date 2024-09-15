// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Kernels.ParticleNeiborsCollisionKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Kernels
{
  public class ParticleNeiborsCollisionKernel : KernelBase
  {
    public ParticleNeiborsCollisionKernel()
      : base("Compute/ParticleNeiborsCollision", "CSParticleNeiborsCollision")
    {
    }

    [GpuData("step")]
    public GpuValue<float> Step { set; get; }

    [GpuData("t")]
    public GpuValue<float> T { set; get; }

    [GpuData("facesForNormalNum")]
    public GpuValue<int> FacesForNormalNum { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("meshVertexToNeiborsMap")]
    public GpuBuffer<int> MeshVertexToNeiborsMap { get; set; }

    [GpuData("meshVertexToNeiborsMapCounts")]
    public GpuBuffer<int> MeshVertexToNeiborsMapCounts { get; set; }

    public override int GetGroupsNumX() => this.Particles != null ? Mathf.CeilToInt((float) this.Particles.Count / 256f) : 0;
  }
}
