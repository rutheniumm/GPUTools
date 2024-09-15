// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.DistanceJointsAdjustKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class DistanceJointsAdjustKernel : KernelBase
  {
    public DistanceJointsAdjustKernel(
      GroupedData<GPDistanceJoint> groupedData,
      GpuBuffer<GPDistanceJoint> distanceJointsBuffer)
      : base("Compute/DistanceJointsAdjust", "CSDistanceJointsAdjust")
    {
      this.DistanceJoints = groupedData;
      this.DistanceJointsBuffer = distanceJointsBuffer;
    }

    [GpuData("segments")]
    public GpuValue<int> Segments { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("distanceJoints")]
    public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer { set; get; }

    public GroupedData<GPDistanceJoint> DistanceJoints { set; get; }

    public override int GetGroupsNumX() => this.DistanceJointsBuffer != null ? Mathf.CeilToInt((float) this.DistanceJointsBuffer.ComputeBuffer.count / 256f) : 0;
  }
}
