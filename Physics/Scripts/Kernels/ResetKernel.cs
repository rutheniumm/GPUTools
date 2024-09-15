// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.ResetKernel
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
  public class ResetKernel : KernelBase
  {
    public ResetKernel()
      : base("Compute/Reset", "CSReset")
    {
    }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    public override int GetGroupsNumX() => this.Particles != null ? Mathf.CeilToInt((float) this.Particles.ComputeBuffer.count / 256f) : 0;
  }
}
