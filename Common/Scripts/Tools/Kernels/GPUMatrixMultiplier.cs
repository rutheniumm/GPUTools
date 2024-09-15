// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Kernels.GPUMatrixMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
  public class GPUMatrixMultiplier : KernelBase
  {
    public GPUMatrixMultiplier(GpuBuffer<Matrix4x4> matrices1, GpuBuffer<Matrix4x4> matrices2)
      : base("Compute/MatrixMultiplier", "CSMatrixMultiplier")
    {
      this.Matrices1 = matrices1;
      this.Matrices2 = matrices2;
      this.ResultMatrices = new GpuBuffer<Matrix4x4>(matrices1.Count, 64);
    }

    [GpuData("matrices1")]
    public GpuBuffer<Matrix4x4> Matrices1 { get; set; }

    [GpuData("matrices2")]
    public GpuBuffer<Matrix4x4> Matrices2 { get; set; }

    [GpuData("resultMatrices")]
    public GpuBuffer<Matrix4x4> ResultMatrices { get; set; }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.Matrices1.Count / 256f);

    public override void Dispose() => this.ResultMatrices.Dispose();
  }
}
