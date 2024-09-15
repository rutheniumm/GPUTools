// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.IntegrateIterWithParticleHoldKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class IntegrateIterWithParticleHoldKernel : KernelBase
  {
    public IntegrateIterWithParticleHoldKernel()
      : base("Compute/IntegrateIterWithParticleHold", "CSIntegrateIterWithParticleHold")
    {
    }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("dt")]
    public GpuValue<float> DT { set; get; }

    [GpuData("invDrag")]
    public GpuValue<float> InvDrag { set; get; }

    [GpuData("accelDT2")]
    public GpuValue<Vector3> AccelDT2 { set; get; }

    public override int GetGroupsNumX() => this.Particles != null ? Mathf.CeilToInt((float) this.Particles.ComputeBuffer.count / 256f) : 0;
  }
}
