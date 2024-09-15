// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Kernels.CreateVertexDataKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Kernels
{
  public class CreateVertexDataKernel : KernelBase
  {
    public CreateVertexDataKernel()
      : base("Compute/CreateVertexData", "CSCreateVertexData")
    {
    }

    [GpuData("facesForNormalNum")]
    public GpuValue<int> FacesForNormalNum { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("clothVertices")]
    public GpuBuffer<ClothVertex> ClothVertices { get; set; }

    [GpuData("meshToPhysicsVerticesMap")]
    public GpuBuffer<int> MeshToPhysicsVerticesMap { get; set; }

    [GpuData("meshVertexToNeiborsMap")]
    public GpuBuffer<int> MeshVertexToNeiborsMap { get; set; }

    [GpuData("meshVertexToNeiborsMapCounts")]
    public GpuBuffer<int> MeshVertexToNeiborsMapCounts { get; set; }

    public override int GetGroupsNumX() => this.MeshToPhysicsVerticesMap != null ? Mathf.CeilToInt((float) this.MeshToPhysicsVerticesMap.ComputeBuffer.count / 256f) : 0;
  }
}
