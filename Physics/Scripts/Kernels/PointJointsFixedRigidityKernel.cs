// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.PointJointsFixedRigidityKernel
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
  public class PointJointsFixedRigidityKernel : KernelBase
  {
    public PointJointsFixedRigidityKernel()
      : base("Compute/PointJointsFixedRigidity", "CSPointJointsFixedRigidity")
    {
    }

    [GpuData("weight")]
    public GpuValue<float> Weight { set; get; }

    [GpuData("isFixed")]
    public GpuValue<int> IsFixed { set; get; }

    [GpuData("fixedRigidity")]
    public GpuValue<float> FixedRigidity { set; get; }

    [GpuData("pointJoints")]
    public GpuBuffer<GPPointJoint> PointJoints { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("transforms")]
    public GpuBuffer<Matrix4x4> Transforms { set; get; }

    public override int GetGroupsNumX() => this.PointJoints != null ? Mathf.CeilToInt((float) this.PointJoints.ComputeBuffer.count / 256f) : 0;
  }
}
