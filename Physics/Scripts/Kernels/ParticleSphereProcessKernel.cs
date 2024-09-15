// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.ParticleSphereProcessKernel
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
  public class ParticleSphereProcessKernel : KernelBase
  {
    public ParticleSphereProcessKernel()
      : base("Compute/ParticleSphereProcess", "CSParticleSphereProcess")
    {
    }

    [GpuData("collisionPrediction")]
    public GpuValue<float> CollisionPrediction { set; get; }

    [GpuData("spheres")]
    public GpuBuffer<GPSphere> Spheres { set; get; }

    [GpuData("oldSpheres")]
    public GpuBuffer<GPSphere> OldSpheres { set; get; }

    [GpuData("processedSpheres")]
    public GpuBuffer<GPSphereWithDelta> ProcessedSpheres { set; get; }

    public override int GetGroupsNumX() => this.Spheres != null ? Mathf.CeilToInt((float) this.Spheres.Count / 256f) : 0;
  }
}
