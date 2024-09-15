// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Kernels.GPUBlendShapePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
  public class GPUBlendShapePlayer : KernelBase
  {
    private readonly SkinnedMeshRenderer skin;
    private readonly Mesh mesh;

    public GPUBlendShapePlayer(SkinnedMeshRenderer skin)
      : base("Compute/BlendShaper", "CSBlendShaper")
    {
      this.skin = skin;
      this.mesh = skin.sharedMesh;
      this.VertexCount = new GpuValue<int>(this.mesh.vertexCount);
      this.ShapesCount = new GpuValue<int>(this.mesh.blendShapeCount);
      this.LocalToWorld = new GpuValue<GpuMatrix4x4>(new GpuMatrix4x4(skin.localToWorldMatrix));
      this.ShapesBuffer = new GpuBuffer<Vector3>(this.GetAllShapes(), 12);
      this.WeightsBuffer = new GpuBuffer<float>(this.mesh.blendShapeCount, 4);
      this.TransformMatricesBuffer = new GpuBuffer<Matrix4x4>(this.mesh.vertexCount, 64);
    }

    [GpuData("vertexCount")]
    public GpuValue<int> VertexCount { get; set; }

    [GpuData("shapesCount")]
    public GpuValue<int> ShapesCount { get; set; }

    [GpuData("localToWorld")]
    public GpuValue<GpuMatrix4x4> LocalToWorld { get; set; }

    [GpuData("shapes")]
    public GpuBuffer<Vector3> ShapesBuffer { get; set; }

    [GpuData("weights")]
    public GpuBuffer<float> WeightsBuffer { get; set; }

    [GpuData("transforms")]
    public GpuBuffer<Matrix4x4> TransformMatricesBuffer { get; set; }

    private Vector3[] GetAllShapes()
    {
      List<Vector3> vector3List = new List<Vector3>();
      Vector3[] vector3Array = new Vector3[this.VertexCount.Value];
      for (int shapeIndex = 0; shapeIndex < this.mesh.blendShapeCount; ++shapeIndex)
      {
        this.mesh.GetBlendShapeFrameVertices(shapeIndex, 0, vector3Array, (Vector3[]) null, (Vector3[]) null);
        vector3List.AddRange((IEnumerable<Vector3>) vector3Array);
      }
      return vector3List.ToArray();
    }

    public override void Dispatch()
    {
      this.LocalToWorld.Value.Data = this.skin.localToWorldMatrix;
      this.PushWeights();
      base.Dispatch();
    }

    private void PushWeights()
    {
      for (int index = 0; index < this.mesh.blendShapeCount; ++index)
      {
        float num = Mathf.Clamp01(this.skin.GetBlendShapeWeight(index) * 0.01f);
        this.WeightsBuffer.Data[index] = num;
      }
      this.WeightsBuffer.PushData();
    }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.VertexCount.Value / 256f);

    public override void Dispose()
    {
      this.ShapesBuffer.Dispose();
      this.WeightsBuffer.Dispose();
      this.TransformMatricesBuffer.Dispose();
    }
  }
}
