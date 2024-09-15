// Decompiled with JetBrains decompiler
// Type: GPUTools.Painter.Scripts.GPUPaint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Painter.Scripts.Kernels;
using UnityEngine;

namespace GPUTools.Painter.Scripts
{
  public class GPUPaint : PrimitiveBase
  {
    public GPUPaint(Vector3[] vertices, Vector3[] normals, Color[] colors)
    {
      this.Vertices = new GpuBuffer<Vector3>(vertices, 12);
      this.Normals = new GpuBuffer<Vector3>(normals, 12);
      this.Colors = new GpuBuffer<Color>(colors, 16);
      this.RayOrigin = new GpuValue<Vector3>();
      this.RayDirection = new GpuValue<Vector3>();
      this.BrushColor = new GpuValue<Color>();
      this.BrushRadius = new GpuValue<float>();
      this.BrushStrength = new GpuValue<float>();
      this.Channel = new GpuValue<int>();
      this.AddPass((IPass) new PaintKernel());
      this.Bind();
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

    public void Draw(ColorBrush brush, Ray ray)
    {
      this.RayOrigin.Value = ray.origin;
      this.RayDirection.Value = ray.direction;
      this.BrushColor.Value = brush.Color;
      this.BrushRadius.Value = brush.Radius;
      this.BrushStrength.Value = brush.Strength;
      this.Channel.Value = (int) brush.Channel;
      this.Dispatch();
    }

    public override void Dispose()
    {
      base.Dispose();
      this.Vertices.Dispose();
      this.Normals.Dispose();
      this.Colors.Dispose();
    }
  }
}
