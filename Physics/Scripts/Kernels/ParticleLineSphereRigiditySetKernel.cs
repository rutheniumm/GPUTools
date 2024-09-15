// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.ParticleLineSphereRigiditySetKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class ParticleLineSphereRigiditySetKernel : KernelBase
  {
    public ParticleLineSphereRigiditySetKernel()
      : base("Compute/ParticleLineSphereRigiditySet", "CSParticleLineSphereRigiditySet")
    {
    }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("pointJoints")]
    public GpuBuffer<GPPointJoint> PointJoints { set; get; }

    [GpuData("rigiditySetLineSpheres")]
    public GpuBuffer<GPLineSphere> RigiditySetLineSpheres { set; get; }

    public override int GetGroupsNumX() => this.Particles != null ? Mathf.CeilToInt((float) this.Particles.Count / 256f) : 0;
  }
}
