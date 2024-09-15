// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildVertexData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildVertexData : BuildChainCommand
  {
    private readonly ClothSettings settings;

    public BuildVertexData(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.settings.Runtime.MeshVertexToNeiborsMap = new GpuBuffer<int>(this.settings.GeometryData.ParticleToNeibor, 4);
      this.settings.Runtime.MeshVertexToNeiborsMapCounts = new GpuBuffer<int>(this.settings.GeometryData.ParticleToNeiborCounts, 4);
      this.settings.Runtime.MeshToPhysicsVerticesMap = new GpuBuffer<int>(this.settings.GeometryData.MeshToPhysicsVerticesMap, 4);
      this.settings.Runtime.ClothVertices = new GpuBuffer<ClothVertex>(new ClothVertex[this.settings.GeometryData.MeshToPhysicsVerticesMap.Length], ClothVertex.Size());
      this.settings.Runtime.ClothOnlyVertices = new GpuBuffer<Vector3>(new Vector3[this.settings.GeometryData.MeshToPhysicsVerticesMap.Length], 12);
    }

    protected override void OnDispose()
    {
      this.settings.Runtime.MeshVertexToNeiborsMap.Dispose();
      this.settings.Runtime.MeshVertexToNeiborsMapCounts.Dispose();
      this.settings.Runtime.MeshToPhysicsVerticesMap.Dispose();
      this.settings.Runtime.ClothVertices.Dispose();
      this.settings.Runtime.ClothOnlyVertices.Dispose();
    }
  }
}
