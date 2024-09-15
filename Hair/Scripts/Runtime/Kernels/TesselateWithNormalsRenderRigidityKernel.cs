// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Kernels.TesselateWithNormalsRenderRigidityKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Kernels
{
  public class TesselateWithNormalsRenderRigidityKernel : KernelBase
  {
    public TesselateWithNormalsRenderRigidityKernel()
      : base("Compute/TesselateWithNormalsRenderRigidity", "CSTesselateWithNormalsRenderRigidity")
    {
    }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("renderParticles")]
    public GpuBuffer<RenderParticle> RenderParticles { set; get; }

    [GpuData("tessRenderParticles")]
    public GpuBuffer<TessRenderParticle> TessRenderParticles { set; get; }

    [GpuData("tessRenderParticlesCount")]
    public GpuValue<int> TessRenderParticlesCount { set; get; }

    [GpuData("randomsPerStrand")]
    public GpuBuffer<Vector3> RandomsPerStrand { set; get; }

    [GpuData("transforms")]
    public GpuBuffer<Matrix4x4> Transforms { set; get; }

    [GpuData("normals")]
    public GpuBuffer<Vector3> Normals { set; get; }

    [GpuData("lightCenter")]
    public GpuValue<Vector3> LightCenter { set; get; }

    [GpuData("pointJoints")]
    public GpuBuffer<GPPointJoint> PointJoints { set; get; }

    [GpuData("normalRandomize")]
    public GpuValue<float> NormalRandomize { set; get; }

    [GpuData("segments")]
    public GpuValue<int> Segments { set; get; }

    [GpuData("tessSegments")]
    public GpuValue<int> TessSegments { set; get; }

    [GpuData("wavinessAxis")]
    public GpuValue<Vector3> WavinessAxis { set; get; }

    [GpuData("wavinessFrequencyRandomness")]
    public GpuValue<float> WavinessFrequencyRandomness { set; get; }

    [GpuData("wavinessScaleRandomness")]
    public GpuValue<float> WavinessScaleRandomness { set; get; }

    [GpuData("wavinessAllowReverse")]
    public GpuValue<bool> WavinessAllowReverse { set; get; }

    [GpuData("wavinessAllowFlipAxis")]
    public GpuValue<bool> WavinessAllowFlipAxis { set; get; }

    [GpuData("wavinessNormalAdjust")]
    public GpuValue<float> WavinessNormalAdjust { set; get; }

    public override int GetGroupsNumX() => this.TessRenderParticles != null ? Mathf.CeilToInt((float) this.TessRenderParticlesCount.Value / 256f) : 0;
  }
}
