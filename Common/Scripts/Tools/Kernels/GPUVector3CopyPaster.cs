// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Kernels.GPUVector3CopyPaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
  public class GPUVector3CopyPaster : KernelBase
  {
    public GPUVector3CopyPaster(GpuBuffer<Vector3> vector3From, GpuBuffer<Vector3> vector3To)
      : base("Compute/Vector3CopyPaster", "CSVector3CopyPaster")
    {
      this.Vector3From = vector3From;
      this.Vector3To = vector3To;
    }

    [GpuData("vector3From")]
    public GpuBuffer<Vector3> Vector3From { get; set; }

    [GpuData("vector3To")]
    public GpuBuffer<Vector3> Vector3To { get; set; }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.Vector3From.Count / 256f);
  }
}
