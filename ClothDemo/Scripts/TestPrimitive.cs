// Decompiled with JetBrains decompiler
// Type: GPUTools.ClothDemo.Scripts.TestPrimitive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.ClothDemo.Scripts
{
  public class TestPrimitive : PrimitiveBase
  {
    public TestPrimitive() => this.AddPass((IPass) new IntegrateKernel());

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("gravity")]
    public GpuValue<Vector3> Gravity { set; get; }

    [GpuData("invDrag")]
    public GpuValue<float> InvDrag { set; get; }

    [GpuData("dt")]
    public GpuValue<float> Dt { set; get; }

    [GpuData("wind")]
    public GpuValue<Vector3> Wind { set; get; }

    public void Start() => this.Bind();
  }
}
