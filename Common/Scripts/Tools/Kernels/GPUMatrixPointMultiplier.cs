// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Kernels.GPUMatrixPointMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
  public class GPUMatrixPointMultiplier : KernelBase
  {
    public GPUMatrixPointMultiplier()
      : base("Compute/MatrixPointMultiplier", "CSMatrixPointMultiplier")
    {
    }

    [GpuData("matrices")]
    public GpuBuffer<Matrix4x4> Matrices { get; set; }

    [GpuData("inPoints")]
    public GpuBuffer<Vector3> InPoints { get; set; }

    [GpuData("outPoints")]
    public GpuBuffer<Vector3> OutPoints { get; set; }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.Matrices.Count / 256f);
  }
}
