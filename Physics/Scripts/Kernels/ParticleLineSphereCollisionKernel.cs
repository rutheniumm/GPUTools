// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.ParticleLineSphereCollisionKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class ParticleLineSphereCollisionKernel : KernelBase
  {
    public ParticleLineSphereCollisionKernel()
      : base("Compute/ParticleLineSphereCollision", "CSParticleLineSphereCollision")
    {
    }

    [GpuData("t")]
    public GpuValue<float> T { set; get; }

    [GpuData("friction")]
    public GpuValue<float> Friction { set; get; }

    [GpuData("staticFriction")]
    public GpuValue<float> StaticFriction { set; get; }

    [GpuData("collisionPower")]
    public GpuValue<float> CollisionPower { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("processedLineSpheres")]
    public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres { set; get; }

    public override int GetGroupsNumX() => this.Particles != null ? Mathf.CeilToInt((float) this.Particles.Count / 256f) : 0;
  }
}
