// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.ParticleLineSphereCopyKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class ParticleLineSphereCopyKernel : KernelBase
  {
    public ParticleLineSphereCopyKernel()
      : base("Compute/ParticleLineSphereCopy", "CSParticleLineSphereCopy")
    {
    }

    [GpuData("lineSpheres")]
    public GpuBuffer<GPLineSphere> LineSpheres { set; get; }

    [GpuData("oldLineSpheres")]
    public GpuBuffer<GPLineSphere> OldLineSpheres { set; get; }

    public override int GetGroupsNumX() => this.LineSpheres != null ? Mathf.CeilToInt((float) this.LineSpheres.Count / 256f) : 0;
  }
}
