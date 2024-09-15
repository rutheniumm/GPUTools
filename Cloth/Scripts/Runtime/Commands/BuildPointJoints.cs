// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildPointJoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Skinner.Scripts.Providers;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildPointJoints : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private List<GPPointJoint> pointJoints;
    private List<GPPointJoint> allPointJoints;
    private GpuBuffer<GPPointJoint> pointJointsBuffer;
    private GpuBuffer<GPPointJoint> allPointJointsBuffer;

    public BuildPointJoints(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.CreatePointJoints();
      if (this.pointJoints.Count > 0)
      {
        this.pointJointsBuffer = new GpuBuffer<GPPointJoint>(this.pointJoints.ToArray(), GPPointJoint.Size());
        this.settings.Runtime.PointJoints = this.pointJointsBuffer;
      }
      if (this.allPointJoints.Count <= 0)
        return;
      this.allPointJointsBuffer = new GpuBuffer<GPPointJoint>(this.allPointJoints.ToArray(), GPPointJoint.Size());
      this.settings.Runtime.AllPointJoints = this.allPointJointsBuffer;
    }

    protected override void OnUpdateSettings()
    {
      this.CreatePointJoints();
      if (this.pointJointsBuffer != null)
      {
        this.pointJointsBuffer.Data = this.pointJoints.ToArray();
        this.pointJointsBuffer.PushData();
      }
      if (this.allPointJointsBuffer == null)
        return;
      this.allPointJointsBuffer.Data = this.allPointJoints.ToArray();
      this.allPointJointsBuffer.PushData();
    }

    private void CreatePointJoints()
    {
      Vector3[] particles = this.settings.GeometryData.Particles;
      int[] toMeshVerticesMap = this.settings.GeometryData.PhysicsToMeshVerticesMap;
      float[] particlesBlend = this.settings.GeometryData.ParticlesBlend;
      this.pointJoints = new List<GPPointJoint>();
      this.allPointJoints = new List<GPPointJoint>();
      for (int bodyId = 0; bodyId < particles.Length; ++bodyId)
      {
        Vector3 point = particles[bodyId];
        int index = toMeshVerticesMap[bodyId];
        float f = particlesBlend[index];
        int matrixId = this.settings.MeshProvider.Type != ScalpMeshType.Static ? index : 0;
        float rigidity = Mathf.Pow(f, 4f);
        this.pointJoints.Add(new GPPointJoint(bodyId, matrixId, point, rigidity));
        this.allPointJoints.Add(new GPPointJoint(bodyId, matrixId, point, rigidity));
      }
    }

    protected override void OnDispose()
    {
      if (this.settings.Runtime.PointJoints != null)
        this.settings.Runtime.PointJoints.Dispose();
      if (this.settings.Runtime.AllPointJoints == null)
        return;
      this.settings.Runtime.AllPointJoints.Dispose();
    }
  }
}
