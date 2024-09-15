// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Kernels.GPUMatrixCopyPaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
  public class GPUMatrixCopyPaster : KernelBase
  {
    public GPUMatrixCopyPaster(GpuBuffer<Matrix4x4> matricesFrom, GpuBuffer<Matrix4x4> matricesTo)
      : base("Compute/MatrixCopyPaster", "CSMatrixCopyPaster")
    {
      this.MatricesFrom = matricesFrom;
      this.MatricesTo = matricesTo;
    }

    [GpuData("matricesFrom")]
    public GpuBuffer<Matrix4x4> MatricesFrom { get; set; }

    [GpuData("matricesTo")]
    public GpuBuffer<Matrix4x4> MatricesTo { get; set; }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.MatricesFrom.Count / 256f);
  }
}
