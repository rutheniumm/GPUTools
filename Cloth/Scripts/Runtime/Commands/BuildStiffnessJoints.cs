// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildStiffnessJoints
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
  public class BuildStiffnessJoints : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private GroupedData<GPDistanceJoint> stiffnessJoints;
    private GpuBuffer<GPDistanceJoint> stiffnessJointsBuffer;

    public BuildStiffnessJoints(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.CreateStiffnessJoints();
      this.stiffnessJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.stiffnessJoints.Data, GPDistanceJoint.Size());
      this.settings.Runtime.StiffnessJoints = this.stiffnessJoints;
      this.settings.Runtime.StiffnessJointsBuffer = this.stiffnessJointsBuffer;
    }

    protected override void OnUpdateSettings()
    {
      this.CreateStiffnessJoints();
      this.settings.Runtime.StiffnessJoints = this.stiffnessJoints;
      this.stiffnessJointsBuffer.Data = this.stiffnessJoints.Data;
      this.stiffnessJointsBuffer.PushData();
    }

    private void CreateStiffnessJoints()
    {
      this.stiffnessJoints = new GroupedData<GPDistanceJoint>();
      List<Int2ListContainer> stiffnessJointGroups = this.settings.GeometryData.StiffnessJointGroups;
      GPParticle[] data = this.settings.Runtime.Particles.Data;
      foreach (Int2ListContainer int2ListContainer in stiffnessJointGroups)
      {
        List<GPDistanceJoint> list = new List<GPDistanceJoint>();
        foreach (Int2 int2 in int2ListContainer.List)
        {
          float distance = Vector3.Distance(data[int2.X].Position, data[int2.Y].Position) / this.settings.transform.lossyScale.x;
          GPDistanceJoint gpDistanceJoint = new GPDistanceJoint(int2.X, int2.Y, distance, this.settings.Stiffness);
          list.Add(gpDistanceJoint);
        }
        this.stiffnessJoints.AddGroup(list);
      }
    }

    protected override void OnDispose()
    {
      if (this.stiffnessJoints != null)
        this.stiffnessJoints.Dispose();
      if (this.stiffnessJointsBuffer == null)
        return;
      this.stiffnessJointsBuffer.Dispose();
    }
  }
}
