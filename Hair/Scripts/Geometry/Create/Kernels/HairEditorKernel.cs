// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.Kernels.HairEditorKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Physics.Scripts.Behaviours;
using GPUTools.Physics.Scripts.Types.Shapes;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create.Kernels
{
  public class HairEditorKernel : KernelBase
  {
    private readonly HairGeometryCreator creator;
    private readonly CacheProvider<SphereCollider> sphereCollidersCache;
    private readonly CacheProvider<LineSphereCollider> lineSphereCollidersCache;

    public HairEditorKernel(
      Vector3[] vertices,
      Color[] colors,
      float[] distances,
      HairGeometryCreator creator,
      string kernelName)
      : base("Compute/HairEditor", kernelName)
    {
      this.creator = creator;
      this.sphereCollidersCache = new CacheProvider<SphereCollider>(creator.ColliderProviders);
      this.lineSphereCollidersCache = new CacheProvider<LineSphereCollider>(creator.ColliderProviders);
      this.Vertices = new GpuBuffer<Vector3>(vertices, 12);
      this.Distances = new GpuBuffer<float>(distances, 4);
      this.Colors = new GpuBuffer<Color>(colors, 16);
      this.Matrices = new GpuBuffer<Matrix4x4>(new Matrix4x4[3], 64);
      this.StaticSpheres = this.sphereCollidersCache.Items.Count <= 0 ? new GpuBuffer<GPSphere>(1, GPSphere.Size()) : new GpuBuffer<GPSphere>(this.sphereCollidersCache.Items.Count, GPSphere.Size());
      this.StaticLineSpheres = this.lineSphereCollidersCache.Items.Count <= 0 ? new GpuBuffer<GPLineSphere>(1, GPSphere.Size()) : new GpuBuffer<GPLineSphere>(this.lineSphereCollidersCache.Items.Count, GPLineSphere.Size());
      this.Segments = new GpuValue<int>();
      this.BrushPosition = new GpuValue<Vector3>();
      this.BrushRadius = new GpuValue<float>();
      this.BrushLenght1 = new GpuValue<float>();
      this.BrushLenght2 = new GpuValue<float>();
      this.BrushStrength = new GpuValue<float>();
      this.BrushCollisionDistance = new GpuValue<float>();
      this.BrushSpeed = new GpuValue<Vector3>();
      this.BrushLengthSpeed = new GpuValue<float>();
      this.BrushColor = new GpuValue<Vector3>();
    }

    [GpuData("vertices")]
    public GpuBuffer<Vector3> Vertices { get; set; }

    [GpuData("colors")]
    public GpuBuffer<Color> Colors { get; set; }

    [GpuData("distances")]
    public GpuBuffer<float> Distances { get; set; }

    [GpuData("matrices")]
    public GpuBuffer<Matrix4x4> Matrices { get; set; }

    [GpuData("staticSpheres")]
    public GpuBuffer<GPSphere> StaticSpheres { get; set; }

    [GpuData("staticLineSpheres")]
    public GpuBuffer<GPLineSphere> StaticLineSpheres { get; set; }

    [GpuData("segments")]
    public GpuValue<int> Segments { get; set; }

    [GpuData("brushPosition")]
    public GpuValue<Vector3> BrushPosition { get; set; }

    [GpuData("brushRadius")]
    public GpuValue<float> BrushRadius { get; set; }

    [GpuData("brushLenght1")]
    public GpuValue<float> BrushLenght1 { get; set; }

    [GpuData("brushLenght2")]
    public GpuValue<float> BrushLenght2 { get; set; }

    [GpuData("brushStrength")]
    public GpuValue<float> BrushStrength { get; set; }

    [GpuData("brushCollisionDistance")]
    public GpuValue<float> BrushCollisionDistance { get; set; }

    [GpuData("brushSpeed")]
    public GpuValue<Vector3> BrushSpeed { get; set; }

    [GpuData("brushLengthSpeed")]
    public GpuValue<float> BrushLengthSpeed { get; set; }

    [GpuData("brushColor")]
    public GpuValue<Vector3> BrushColor { get; set; }

    private void ComputeStaticSpheres(GPSphere[] spheres)
    {
      List<SphereCollider> items = this.sphereCollidersCache.Items;
      if (spheres == null)
        spheres = new GPSphere[items.Count];
      for (int index = 0; index < items.Count; ++index)
      {
        SphereCollider sphereCollider = items[index];
        Vector3 position = sphereCollider.transform.TransformPoint(sphereCollider.center);
        float radius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
        spheres[index] = new GPSphere(position, radius);
      }
    }

    private void ComputeStaticSpheres(GPLineSphere[] lineSpheres)
    {
      List<LineSphereCollider> items = this.lineSphereCollidersCache.Items;
      if (lineSpheres == null)
        lineSpheres = new GPLineSphere[items.Count];
      for (int index = 0; index < items.Count; ++index)
      {
        LineSphereCollider lineSphereCollider = items[index];
        float worldRadiusA = lineSphereCollider.WorldRadiusA;
        float worldRadiusB = lineSphereCollider.WorldRadiusB;
        Vector3 worldA = lineSphereCollider.WorldA;
        Vector3 worldB = lineSphereCollider.WorldB;
        lineSpheres[index] = new GPLineSphere(worldA, worldB, worldRadiusA, worldRadiusB);
      }
    }

    public override void Dispatch()
    {
      this.Matrices.Data[0] = Camera.current.transform.worldToLocalMatrix;
      this.Matrices.Data[1] = this.creator.ScalpProvider.ToWorldMatrix;
      this.Matrices.Data[2] = this.creator.ScalpProvider.ToWorldMatrix.inverse;
      this.Matrices.PushData();
      if (this.StaticSpheres != null)
      {
        this.ComputeStaticSpheres(this.StaticSpheres.Data);
        this.StaticSpheres.PushData();
      }
      if (this.StaticLineSpheres != null)
      {
        this.ComputeStaticSpheres(this.StaticLineSpheres.Data);
        this.StaticLineSpheres.PushData();
      }
      this.Segments.Value = this.creator.Segments;
      this.BrushPosition.Value = this.creator.Brush.Position;
      this.BrushRadius.Value = this.creator.Brush.Radius;
      this.BrushLenght1.Value = this.creator.Brush.Lenght1;
      this.BrushLenght2.Value = this.creator.Brush.Lenght2;
      this.BrushStrength.Value = this.creator.Brush.Strength;
      this.BrushCollisionDistance.Value = this.creator.Brush.CollisionDistance;
      this.BrushSpeed.Value = this.creator.Brush.Speed;
      this.BrushColor.Value = new Vector3(this.creator.Brush.Color.r, this.creator.Brush.Color.g, this.creator.Brush.Color.b);
      base.Dispatch();
    }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.Vertices.Count / 256f);

    public override void Dispose()
    {
      this.Vertices.Dispose();
      this.Distances.Dispose();
      this.Matrices.Dispose();
      this.Colors.Dispose();
      if (this.StaticSpheres != null)
        this.StaticSpheres.Dispose();
      if (this.StaticLineSpheres == null)
        return;
      this.StaticLineSpheres.Dispose();
    }
  }
}
