// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.CompressionJointsKernel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
  public class CompressionJointsKernel : KernelBase
  {
    public CompressionJointsKernel(
      GroupedData<GPDistanceJoint> groupedData,
      GpuBuffer<GPDistanceJoint> compressionJointsBuffer)
      : base("Compute/CompressionJoints", "CSCompressionJoints")
    {
      this.CompressionJoints = groupedData;
      this.CompressionJointsBuffer = compressionJointsBuffer;
    }

    [GpuData("compressionDistanceScale")]
    public GpuValue<float> CompressionDistanceScale { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("compressionJoints")]
    public GpuBuffer<GPDistanceJoint> CompressionJointsBuffer { set; get; }

    [GpuData("compressionJointPower")]
    public GpuValue<float> CompressionJointPower { set; get; }

    public GroupedData<GPDistanceJoint> CompressionJoints { set; get; }

    public override void Dispatch()
    {
      if (!this.IsEnabled)
        return;
      if (this.Props.Count == 0)
        this.CacheAttributes();
      this.BindAttributes();
      for (int index = 0; index < this.CompressionJoints.GroupsData.Count; ++index)
      {
        GroupData groupData = this.CompressionJoints.GroupsData[index];
        this.Shader.SetInt("startGroup", groupData.Start);
        this.Shader.SetInt("sizeGroup", groupData.Num);
        if (groupData.Num > 0)
          this.Shader.Dispatch(this.KernelId, Mathf.CeilToInt((float) groupData.Num / 256f), 1, 1);
      }
    }
  }
}
