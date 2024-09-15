// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.SplineJointsReverseWithParticleHoldKernel
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
  public class SplineJointsReverseWithParticleHoldKernel : KernelBase
  {
    public SplineJointsReverseWithParticleHoldKernel()
      : base("Compute/SplineJointsReverseWithParticleHold", "CSSplineJointsReverseWithParticleHold")
    {
    }

    [GpuData("segments")]
    public GpuValue<int> Segments { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("pointToPreviousPointDistances")]
    public GpuBuffer<float> PointToPreviousPointDistances { set; get; }

    [GpuData("splineJointPower")]
    public GpuValue<float> SplineJointPower { set; get; }

    [GpuData("reverseSplineJointPower")]
    public GpuValue<float> ReverseSplineJointPower { set; get; }

    public override int GetGroupsNumX() => this.Particles != null ? Mathf.CeilToInt((float) (this.Particles.Count / this.Segments.Value) / 256f) : 0;
  }
}
