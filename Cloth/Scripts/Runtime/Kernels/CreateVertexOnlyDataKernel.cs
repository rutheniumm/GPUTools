// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Kernels.CreateVertexOnlyDataKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Kernels
{
  public class CreateVertexOnlyDataKernel : KernelBase
  {
    public CreateVertexOnlyDataKernel()
      : base("Compute/CreateVertexOnlyData", "CSCreateVertexOnlyData")
    {
    }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("clothOnlyVertices")]
    public GpuBuffer<Vector3> ClothOnlyVertices { get; set; }

    [GpuData("meshToPhysicsVerticesMap")]
    public GpuBuffer<int> MeshToPhysicsVerticesMap { get; set; }

    public override int GetGroupsNumX() => this.MeshToPhysicsVerticesMap != null ? Mathf.CeilToInt((float) this.MeshToPhysicsVerticesMap.ComputeBuffer.count / 256f) : 0;
  }
}
