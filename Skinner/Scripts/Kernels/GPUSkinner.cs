// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Kernels.GPUSkinner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
  public class GPUSkinner : KernelBase
  {
    private readonly SkinnedMeshRenderer skin;
    private Transform[] bones;
    private Matrix4x4[] bindposes;

    public GPUSkinner(SkinnedMeshRenderer skin)
      : base("Compute/Skinner", "CSComputeMatrices")
    {
      this.skin = skin;
      Mesh sharedMesh = skin.sharedMesh;
      this.bones = skin.bones;
      this.bindposes = sharedMesh.bindposes;
      this.TransformMatricesBuffer = new GpuBuffer<Matrix4x4>(sharedMesh.vertexCount, 64);
      this.BonesBuffer = new GpuBuffer<Matrix4x4>(new Matrix4x4[skin.bones.Length], 64);
      this.WeightsBuffer = new GpuBuffer<Weight>(this.GetWeightsArray(sharedMesh), 32);
    }

    [GpuData("weights")]
    public GpuBuffer<Weight> WeightsBuffer { get; set; }

    [GpuData("bones")]
    public GpuBuffer<Matrix4x4> BonesBuffer { get; set; }

    [GpuData("transforms")]
    public GpuBuffer<Matrix4x4> TransformMatricesBuffer { get; set; }

    public override void Dispatch()
    {
      this.CalculateBones();
      base.Dispatch();
    }

    public override void Dispose()
    {
      this.TransformMatricesBuffer.Dispose();
      this.BonesBuffer.Dispose();
      this.WeightsBuffer.Dispose();
    }

    public override int GetGroupsNumX() => Mathf.CeilToInt((float) this.skin.sharedMesh.vertexCount / 256f);

    private void CalculateBones()
    {
      for (int index = 0; index < this.BonesBuffer.Data.Length; ++index)
        this.BonesBuffer.Data[index] = this.bones[index].localToWorldMatrix * this.bindposes[index];
      this.BonesBuffer.PushData();
    }

    private Weight[] GetWeightsArray(Mesh mesh)
    {
      Weight[] weightsArray = new Weight[mesh.boneWeights.Length];
      BoneWeight[] boneWeights = mesh.boneWeights;
      for (int index = 0; index < boneWeights.Length; ++index)
      {
        BoneWeight boneWeight = boneWeights[index];
        Weight weight = new Weight()
        {
          bi0 = boneWeight.boneIndex0,
          bi1 = boneWeight.boneIndex1,
          bi2 = boneWeight.boneIndex2,
          bi3 = boneWeight.boneIndex3,
          w0 = boneWeight.weight0,
          w1 = boneWeight.weight1,
          w2 = boneWeight.weight2,
          w3 = boneWeight.weight3
        };
        weightsArray[index] = weight;
      }
      return weightsArray;
    }
  }
}
