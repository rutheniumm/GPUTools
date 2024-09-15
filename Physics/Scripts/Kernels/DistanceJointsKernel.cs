// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Kernels.DistanceJointsKernel
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
  public class DistanceJointsKernel : KernelBase
  {
    public DistanceJointsKernel(
      GroupedData<GPDistanceJoint> groupedData,
      GpuBuffer<GPDistanceJoint> distanceJointsBuffer)
      : base("Compute/DistanceJoints", "CSDistanceJoints")
    {
      this.DistanceJoints = groupedData;
      this.DistanceJointsBuffer = distanceJointsBuffer;
    }

    [GpuData("distanceScale")]
    public GpuValue<float> DistanceScale { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("distanceJoints")]
    public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer { set; get; }

    [GpuData("distanceJointPower")]
    public GpuValue<float> DistanceJointPower { set; get; }

    public GroupedData<GPDistanceJoint> DistanceJoints { set; get; }

    public override void Dispatch()
    {
      if (!this.IsEnabled)
        return;
      if (this.Props.Count == 0)
        this.CacheAttributes();
      this.BindAttributes();
      for (int index = 0; index < this.DistanceJoints.GroupsData.Count; ++index)
      {
        GroupData groupData = this.DistanceJoints.GroupsData[index];
        this.Shader.SetInt("startGroup", groupData.Start);
        this.Shader.SetInt("sizeGroup", groupData.Num);
        int threadGroupsX = Mathf.CeilToInt((float) groupData.Num / 256f);
        if (threadGroupsX > 0)
          this.Shader.Dispatch(this.KernelId, threadGroupsX, 1, 1);
      }
    }
  }
}
