// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildNearbyDistanceJoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Physics.Scripts.Types.Joints;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildNearbyDistanceJoints : BuildChainCommand
  {
    private readonly HairSettings settings;
    private GroupedData<GPDistanceJoint> nearbyDistanceJoints;
    private GpuBuffer<GPDistanceJoint> nearbyDistanceJointsBuffer;

    public BuildNearbyDistanceJoints(HairSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.CreateNearbyDistanceJoints();
      if (this.nearbyDistanceJointsBuffer != null)
        this.nearbyDistanceJointsBuffer.Dispose();
      this.nearbyDistanceJointsBuffer = this.nearbyDistanceJoints.Data.Length <= 0 ? (GpuBuffer<GPDistanceJoint>) null : new GpuBuffer<GPDistanceJoint>(this.nearbyDistanceJoints.Data, GPDistanceJoint.Size());
      this.settings.RuntimeData.NearbyDistanceJointsBuffer = this.nearbyDistanceJointsBuffer;
      this.settings.RuntimeData.NearbyDistanceJoints = this.nearbyDistanceJoints;
    }

    private void AddToHashSet(HashSet<Vector3> set, int i1, int i2, float distance)
    {
      if (i1 == -1 || i2 == -1)
        return;
      set.Add(i1 <= i2 ? new Vector3((float) i2, (float) i1, distance) : new Vector3((float) i1, (float) i2, distance));
    }

    private void CreateNearbyDistanceJoints()
    {
      this.nearbyDistanceJoints = new GroupedData<GPDistanceJoint>();
      int segments = this.settings.StandsSettings.Segments;
      float num1 = (float) (segments - 1);
      int num2 = 0;
      List<Vector4ListContainer> nearbyVertexGroups = this.settings.StandsSettings.Provider.GetNearbyVertexGroups();
      if (nearbyVertexGroups == null)
        return;
      foreach (Vector4ListContainer vector4ListContainer in nearbyVertexGroups)
      {
        List<GPDistanceJoint> list = new List<GPDistanceJoint>();
        this.nearbyDistanceJoints.AddGroup(list);
        foreach (Vector4 vector4 in vector4ListContainer.List)
        {
          int x = (int) vector4.x;
          float num3 = (float) (1.0 - (double) (x % segments) / (double) num1);
          int y = (int) vector4.y;
          float num4 = (float) (1.0 - (double) (y % segments) / (double) num1);
          float elasticity = (float) (((double) num3 + (double) num4) * 0.5);
          GPDistanceJoint gpDistanceJoint = new GPDistanceJoint(x, y, vector4.z, elasticity);
          list.Add(gpDistanceJoint);
          ++num2;
        }
      }
    }

    protected override void OnDispose()
    {
      if (this.nearbyDistanceJoints != null)
        this.nearbyDistanceJoints.Dispose();
      if (this.nearbyDistanceJointsBuffer == null)
        return;
      this.nearbyDistanceJointsBuffer.Dispose();
    }
  }
}
