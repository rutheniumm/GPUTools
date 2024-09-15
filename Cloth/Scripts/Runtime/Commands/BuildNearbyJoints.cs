// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildNearbyJoints
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
  public class BuildNearbyJoints : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private GroupedData<GPDistanceJoint> nearbyJoints;
    private GpuBuffer<GPDistanceJoint> nearbyJointsBuffer;

    public BuildNearbyJoints(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.CreateNearbyJoints();
      this.settings.Runtime.NearbyJoints = this.nearbyJoints;
      if (this.nearbyJoints.Data.Length > 0)
      {
        this.nearbyJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.nearbyJoints.Data, GPDistanceJoint.Size());
        this.settings.Runtime.NearbyJointsBuffer = this.nearbyJointsBuffer;
      }
      else
        this.settings.Runtime.NearbyJointsBuffer = (GpuBuffer<GPDistanceJoint>) null;
    }

    protected override void OnUpdateSettings()
    {
      this.CreateNearbyJoints();
      this.settings.Runtime.NearbyJoints = this.nearbyJoints;
      this.nearbyJointsBuffer.Data = this.nearbyJoints.Data;
      this.nearbyJointsBuffer.PushData();
    }

    private void CreateNearbyJoints()
    {
      this.nearbyJoints = new GroupedData<GPDistanceJoint>();
      List<Int2ListContainer> nearbyJointGroups = this.settings.GeometryData.NearbyJointGroups;
      GPParticle[] data = this.settings.Runtime.Particles.Data;
      foreach (Int2ListContainer int2ListContainer in nearbyJointGroups)
      {
        List<GPDistanceJoint> list = new List<GPDistanceJoint>();
        foreach (Int2 int2 in int2ListContainer.List)
        {
          float distance = Vector3.Distance(data[int2.X].Position, data[int2.Y].Position) / this.settings.transform.lossyScale.x;
          GPDistanceJoint gpDistanceJoint = new GPDistanceJoint(int2.X, int2.Y, distance, this.settings.Stiffness);
          list.Add(gpDistanceJoint);
        }
        this.nearbyJoints.AddGroup(list);
      }
    }

    protected override void OnDispose()
    {
      if (this.nearbyJoints != null)
        this.nearbyJoints.Dispose();
      if (this.nearbyJointsBuffer == null)
        return;
      this.nearbyJointsBuffer.Dispose();
    }
  }
}
