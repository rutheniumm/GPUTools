// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildDistanceJoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildDistanceJoints : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private GroupedData<GPDistanceJoint> distanceJoints;
    private GpuBuffer<GPDistanceJoint> distanceJointsBuffer;

    public BuildDistanceJoints(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.CreateDistanceJoints();
      this.distanceJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.distanceJoints.Data, GPDistanceJoint.Size());
      this.settings.Runtime.DistanceJoints = this.distanceJoints;
      this.settings.Runtime.DistanceJointsBuffer = this.distanceJointsBuffer;
    }

    protected override void OnUpdateSettings()
    {
      this.CreateDistanceJoints();
      this.settings.Runtime.DistanceJoints = this.distanceJoints;
      this.distanceJointsBuffer.Data = this.distanceJoints.Data;
      this.distanceJointsBuffer.PushData();
    }

    private void CreateDistanceJoints()
    {
      this.distanceJoints = new GroupedData<GPDistanceJoint>();
      List<Int2ListContainer> jointGroups = this.settings.GeometryData.JointGroups;
      GPParticle[] data = this.settings.Runtime.Particles.Data;
      foreach (Int2ListContainer int2ListContainer in jointGroups)
      {
        List<GPDistanceJoint> list = new List<GPDistanceJoint>();
        foreach (Int2 int2 in int2ListContainer.List)
        {
          float distance = Vector3.Distance(data[int2.X].Position, data[int2.Y].Position) / this.settings.transform.lossyScale.x;
          GPDistanceJoint gpDistanceJoint = new GPDistanceJoint(int2.X, int2.Y, distance, 1f - this.settings.Stretchability);
          list.Add(gpDistanceJoint);
        }
        this.distanceJoints.AddGroup(list);
      }
    }

    protected override void OnDispose()
    {
      if (this.distanceJoints != null)
        this.distanceJoints.Dispose();
      if (this.distanceJointsBuffer == null)
        return;
      this.distanceJointsBuffer.Dispose();
    }
  }
}
