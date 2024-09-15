// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildDistanceJoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Joints;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildDistanceJoints : BuildChainCommand
  {
    private readonly HairSettings settings;
    private GroupedData<GPDistanceJoint> distanceJoints;
    private GpuBuffer<GPDistanceJoint> distanceJointsBuffer;
    private float[] pointToPreviousPointDistances;

    public BuildDistanceJoints(HairSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.pointToPreviousPointDistances = this.settings.RuntimeData.Particles == null ? new float[0] : new float[this.settings.RuntimeData.Particles.Count];
      this.CreateDistanceJoints();
      if (this.distanceJointsBuffer != null)
        this.distanceJointsBuffer.Dispose();
      this.distanceJointsBuffer = this.distanceJoints.Data.Length <= 0 ? (GpuBuffer<GPDistanceJoint>) null : new GpuBuffer<GPDistanceJoint>(this.distanceJoints.Data, GPDistanceJoint.Size());
      this.settings.RuntimeData.DistanceJoints = this.distanceJoints;
      this.settings.RuntimeData.DistanceJointsBuffer = this.distanceJointsBuffer;
      if (this.settings.RuntimeData.PointToPreviousPointDistances != null)
        this.settings.RuntimeData.PointToPreviousPointDistances.Dispose();
      if (this.pointToPreviousPointDistances.Length > 0)
        this.settings.RuntimeData.PointToPreviousPointDistances = new GpuBuffer<float>(this.pointToPreviousPointDistances, 4);
      else
        this.settings.RuntimeData.PointToPreviousPointDistances = (GpuBuffer<float>) null;
    }

    protected override void OnUpdateSettings()
    {
      this.CreateDistanceJoints();
      this.settings.RuntimeData.DistanceJoints = this.distanceJoints;
      if (this.distanceJointsBuffer != null)
      {
        this.distanceJointsBuffer.Data = this.distanceJoints.Data;
        this.distanceJointsBuffer.PushData();
      }
      if (this.settings.RuntimeData.PointToPreviousPointDistances == null)
        return;
      this.settings.RuntimeData.PointToPreviousPointDistances.PushData();
    }

    public void RebuildFromGPUData()
    {
      this.settings.RuntimeData.DistanceJointsBuffer.PullData();
      GPDistanceJoint[] data = this.settings.RuntimeData.DistanceJointsBuffer.Data;
    }

    private void CreateDistanceJoints()
    {
      int segments = this.settings.StandsSettings.Segments;
      if (this.distanceJoints != null)
        this.distanceJoints.Dispose();
      this.distanceJoints = new GroupedData<GPDistanceJoint>();
      List<GPDistanceJoint> list1 = new List<GPDistanceJoint>();
      List<GPDistanceJoint> list2 = new List<GPDistanceJoint>();
      if (this.settings.RuntimeData.Particles != null)
      {
        for (int body2Id = 0; body2Id < this.settings.RuntimeData.Particles.Count; ++body2Id)
        {
          if (body2Id % segments != 0)
          {
            float distance = Vector3.Distance(this.settings.RuntimeData.Particles.Data[body2Id - 1].Position, this.settings.RuntimeData.Particles.Data[body2Id].Position);
            this.pointToPreviousPointDistances[body2Id] = distance;
            (body2Id % 2 != 0 ? list1 : list2).Add(new GPDistanceJoint(body2Id - 1, body2Id, distance, 1f));
          }
        }
      }
      this.distanceJoints.AddGroup(list1);
      this.distanceJoints.AddGroup(list2);
    }

    protected override void OnDispose()
    {
      if (this.distanceJoints != null)
        this.distanceJoints.Dispose();
      if (this.distanceJointsBuffer != null)
        this.distanceJointsBuffer.Dispose();
      if (this.settings.RuntimeData.PointToPreviousPointDistances == null)
        return;
      this.settings.RuntimeData.PointToPreviousPointDistances.Dispose();
    }
  }
}
