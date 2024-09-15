// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.DebugDraw.GPDebugDraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.DebugDraw
{
  public class GPDebugDraw
  {
    public static void Draw(
      GpuBuffer<GPDistanceJoint> joints,
      GpuBuffer<GPDistanceJoint> joints2,
      GpuBuffer<GPParticle> particles,
      bool drawParticles,
      bool drawJoints,
      bool drawJoints2)
    {
      particles.PullData();
      Gizmos.color = Color.green;
      if (drawParticles)
      {
        foreach (GPParticle gpParticle in particles.Data)
          Gizmos.DrawWireSphere(gpParticle.Position, gpParticle.Radius);
      }
      if (drawJoints && joints != null)
      {
        joints.PullData();
        foreach (GPDistanceJoint gpDistanceJoint in joints.Data)
          Gizmos.DrawLine(particles.Data[gpDistanceJoint.Body1Id].Position, particles.Data[gpDistanceJoint.Body2Id].Position);
      }
      Gizmos.color = Color.yellow;
      if (!drawJoints2 || joints2 == null)
        return;
      joints2.PullData();
      foreach (GPDistanceJoint gpDistanceJoint in joints2.Data)
        Gizmos.DrawLine(particles.Data[gpDistanceJoint.Body1Id].Position, particles.Data[gpDistanceJoint.Body2Id].Position);
    }
  }
}
