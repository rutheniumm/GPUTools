// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Kernels.GPUMatrixSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
  public class GPUMatrixSelector : KernelBase
  {
    public GPUMatrixSelector()
      : base("Compute/Selector", "CSMatrixSelector")
    {
    }

    [GpuData("indices")]
    public GpuBuffer<int> Indices { get; set; }

    [GpuData("matrices")]
    public GpuBuffer<Matrix4x4> Matrices { get; set; }

    [GpuData("selectedMatrices")]
    public GpuBuffer<Matrix4x4> SelectedMatrices { get; set; }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.Indices.Count / 256f);
  }
}
