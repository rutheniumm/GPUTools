// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildCompressionJoints
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
  public class BuildCompressionJoints : BuildChainCommand
  {
    private readonly HairSettings settings;
    private GroupedData<GPDistanceJoint> compressionJoints;
    private GpuBuffer<GPDistanceJoint> compressionJointsBuffer;

    public BuildCompressionJoints(HairSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.CreateCompressionJoints();
      if (this.compressionJointsBuffer != null)
        this.compressionJointsBuffer.Dispose();
      this.compressionJointsBuffer = this.compressionJoints.Data.Length <= 0 ? (GpuBuffer<GPDistanceJoint>) null : new GpuBuffer<GPDistanceJoint>(this.compressionJoints.Data, GPDistanceJoint.Size());
      this.settings.RuntimeData.CompressionJoints = this.compressionJoints;
      this.settings.RuntimeData.CompressionJointsBuffer = this.compressionJointsBuffer;
    }

    protected override void OnUpdateSettings()
    {
      this.CreateCompressionJoints();
      this.settings.RuntimeData.CompressionJoints = this.compressionJoints;
      if (this.compressionJointsBuffer == null)
        return;
      this.compressionJointsBuffer.Data = this.compressionJoints.Data;
      this.compressionJointsBuffer.PushData();
    }

    private void CreateCompressionJoints()
    {
      int segments = this.settings.StandsSettings.Segments;
      this.compressionJoints = new GroupedData<GPDistanceJoint>();
      List<GPDistanceJoint> list1 = new List<GPDistanceJoint>();
      List<GPDistanceJoint> list2 = new List<GPDistanceJoint>();
      if (this.settings.RuntimeData.Particles != null)
      {
        for (int body2Id = 0; body2Id < this.settings.RuntimeData.Particles.Count; ++body2Id)
        {
          if (body2Id % segments != 0 && body2Id % segments != 1)
          {
            float distance = Vector3.Distance(this.settings.RuntimeData.Particles.Data[body2Id - 2].Position, this.settings.RuntimeData.Particles.Data[body2Id].Position) * 0.95f;
            (body2Id % 4 == 2 || body2Id % 4 == 3 ? list1 : list2).Add(new GPDistanceJoint(body2Id - 2, body2Id, distance, 1f));
          }
        }
      }
      this.compressionJoints.AddGroup(list1);
      this.compressionJoints.AddGroup(list2);
    }

    protected override void OnDispose()
    {
      if (this.compressionJoints != null)
        this.compressionJoints.Dispose();
      if (this.compressionJointsBuffer == null)
        return;
      this.compressionJointsBuffer.Dispose();
    }
  }
}
