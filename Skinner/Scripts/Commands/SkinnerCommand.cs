// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Commands.SkinnerCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Common.Scripts.Tools.Kernels;
using GPUTools.Skinner.Scripts.Providers;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Commands
{
  public class SkinnerCommand : IBuildCommand
  {
    private readonly SkinnedMeshProvider provider;
    private readonly int[] indices;
    private List<KernelBase> kernels = new List<KernelBase>();

    public SkinnerCommand(SkinnedMeshProvider provider, int[] indices)
    {
      this.provider = provider;
      this.indices = indices;
    }

    public GpuBuffer<int> Indices { get; set; }

    public GpuBuffer<Matrix4x4> Matrices { get; set; }

    public GpuBuffer<Vector3> LocalPoints { get; set; }

    public GpuBuffer<Vector3> Points { get; set; }

    public GpuBuffer<Matrix4x4> SelectedMatrices { get; set; }

    public GpuBuffer<Vector3> SelectedPoints { get; set; }

    public void Build()
    {
      this.Matrices = this.provider.ToWorldMatricesBuffer;
      this.LocalPoints = new GpuBuffer<Vector3>(this.provider.Mesh.vertices, 12);
      this.Points = new GpuBuffer<Vector3>(this.provider.Mesh.vertexCount, 12);
      this.kernels.Add((KernelBase) new GPUMatrixPointMultiplier()
      {
        InPoints = this.LocalPoints,
        OutPoints = this.Points,
        Matrices = this.Matrices
      });
      if (this.indices == null || this.indices.Length <= 0)
        return;
      this.Indices = new GpuBuffer<int>(this.indices, 4);
      this.SelectedPoints = new GpuBuffer<Vector3>(this.indices.Length, 12);
      this.SelectedMatrices = new GpuBuffer<Matrix4x4>(this.indices.Length, 64);
      GPUMatrixSelector gpuMatrixSelector = new GPUMatrixSelector()
      {
        Indices = this.Indices,
        Matrices = this.Matrices,
        SelectedMatrices = this.SelectedMatrices
      };
      GPUPointsSelector gpuPointsSelector = new GPUPointsSelector()
      {
        Indices = this.Indices,
        Points = this.Points,
        SelectedPoints = this.SelectedPoints
      };
      this.kernels.Add((KernelBase) gpuMatrixSelector);
      this.kernels.Add((KernelBase) gpuPointsSelector);
    }

    public void Dispatch()
    {
      for (int index = 0; index < this.kernels.Count; ++index)
        this.kernels[index].Dispatch();
    }

    public void FixedDispatch()
    {
    }

    public void UpdateSettings()
    {
    }

    public void Dispose()
    {
      if (this.Indices != null)
        this.Indices.Dispose();
      if (this.SelectedPoints != null)
        this.SelectedPoints.Dispose();
      if (this.SelectedMatrices != null)
        this.SelectedMatrices.Dispose();
      this.LocalPoints.Dispose();
      this.Points.Dispose();
    }
  }
}
