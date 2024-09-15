// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.CopySpecificParticlesKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class CopySpecificParticlesKernel : KernelBase
  {
    public CopySpecificParticlesKernel()
      : base("Compute/CopySpecificParticles", "CSCopySpecificParticles")
    {
    }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("outParticles")]
    public GpuBuffer<GPParticle> OutParticles { set; get; }

    [GpuData("outParticlesMap")]
    public GpuBuffer<float> OutParticlesMap { set; get; }

    public override int GetGroupsNumX() => 1;
  }
}
