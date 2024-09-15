// Decompiled with JetBrains decompiler
// Type: GPUTools.Painter.Scripts.Kernels.PaintKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Painter.Scripts.Kernels
{
  public class PaintKernel : KernelBase
  {
    public PaintKernel()
      : base("Compute/Paint", "CSPaint")
    {
    }

    [GpuData("vertices")]
    public GpuBuffer<Vector3> Vertices { get; set; }

    [GpuData("normals")]
    public GpuBuffer<Vector3> Normals { get; set; }

    [GpuData("colors")]
    public GpuBuffer<Color> Colors { get; set; }

    [GpuData("rayOrigin")]
    public GpuValue<Vector3> RayOrigin { get; set; }

    [GpuData("rayDirection")]
    public GpuValue<Vector3> RayDirection { get; set; }

    [GpuData("brushColor")]
    public GpuValue<Color> BrushColor { get; set; }

    [GpuData("brushRadius")]
    public GpuValue<float> BrushRadius { get; set; }

    [GpuData("brushStrength")]
    public GpuValue<float> BrushStrength { get; set; }

    [GpuData("channel")]
    public GpuValue<int> Channel { get; set; }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.Vertices.Count / 256f);
  }
}
