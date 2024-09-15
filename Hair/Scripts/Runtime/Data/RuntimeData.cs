// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Data.RuntimeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Data
{
  public class RuntimeData
  {
    public GpuBuffer<GPParticle> Particles { get; set; }

    public float[] ParticleRootToTipRatios { get; set; }

    public GpuBuffer<GPSphereWithDelta> ProcessedSpheres { get; set; }

    public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> CutLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> GrowLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> HoldLineSpheres { get; set; }

    public GpuBuffer<GPLineSphereWithMatrixDelta> GrabLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> PushLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> PullLineSpheres { get; set; }

    public GpuBuffer<GPLineSphereWithDelta> BrushLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> RigidityIncreaseLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> RigidityDecreaseLineSpheres { get; set; }

    public GpuBuffer<GPLineSphere> RigiditySetLineSpheres { get; set; }

    public GpuBuffer<Vector4> Planes { get; set; }

    public GroupedData<GPDistanceJoint> DistanceJoints { get; set; }

    public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer { get; set; }

    public GroupedData<GPDistanceJoint> CompressionJoints { get; set; }

    public GpuBuffer<GPDistanceJoint> CompressionJointsBuffer { get; set; }

    public GroupedData<GPDistanceJoint> NearbyDistanceJoints { get; set; }

    public GpuBuffer<GPDistanceJoint> NearbyDistanceJointsBuffer { get; set; }

    public GpuBuffer<GPPointJoint> PointJoints { get; set; }

    public GpuBuffer<float> PointToPreviousPointDistances { get; set; }

    public GpuBuffer<Vector3> Barycentrics { get; set; }

    public GpuBuffer<Vector3> BarycentricsFixed { get; set; }

    public GpuBuffer<RenderParticle> RenderParticles { get; set; }

    public GpuBuffer<TessRenderParticle> TessRenderParticles { get; set; }

    public GpuBuffer<Vector3> RandomsPerStrand { get; set; }

    public GpuBuffer<GPParticle> OutParticles { get; set; }

    public GpuBuffer<float> OutParticlesMap { get; set; }

    public Vector3 Wind { get; set; }
  }
}
