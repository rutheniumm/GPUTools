// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Data.HairDataFacade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Data
{
  public class HairDataFacade
  {
    private readonly HairSettings settings;

    public HairDataFacade(HairSettings settings) => this.settings = settings;

    public bool DebugDraw => this.settings.PhysicsSettings.DebugDraw;

    public bool DebugDrawNearbyJoints => this.settings.PhysicsSettings.DebugDrawNearbyJoints;

    public int Iterations => this.settings.PhysicsSettings.Iterations;

    public float Drag => this.settings.PhysicsSettings.Drag;

    public float InvDrag => 1f - this.settings.PhysicsSettings.Drag * (1f / (float) this.Iterations);

    public float WorldScale => this.settings.PhysicsSettings.WorldScale;

    public bool FastMovement => this.settings.PhysicsSettings.FastMovement;

    public Vector3 Gravity => this.StyleMode ? UnityEngine.Physics.gravity * this.settings.PhysicsSettings.StyleModeGravityMultiplier : UnityEngine.Physics.gravity * this.settings.PhysicsSettings.GravityMultiplier;

    public Vector3 Wind => this.settings.RuntimeData.Wind;

    public bool IsPhysicsEnabled => this.settings.PhysicsSettings.IsEnabled;

    public bool IsCollisionEnabled => this.settings.PhysicsSettings.IsCollisionEnabled;

    public float Weight => this.settings.PhysicsSettings.Weight;

    public float Friction => this.settings.PhysicsSettings.Friction;

    public float CollisionPower => this.settings.PhysicsSettings.CollisionPower;

    public float CompressionJointPower => this.settings.PhysicsSettings.CompressionJointPower;

    public float NearbyJointPower => this.settings.PhysicsSettings.NearbyJointPower;

    public float NearbyJointPowerRolloff => this.settings.PhysicsSettings.NearbyJointPowerRolloff;

    public float SplineJointPower => this.settings.PhysicsSettings.SplineJointPower;

    public float ReverseSplineJointPower => this.settings.PhysicsSettings.ReverseSplineJointPower;

    public bool RunPhysicsOnUpdate => this.settings.PhysicsSettings.RunPhysicsOnUpdate;

    public bool UsePaintedRigidity => this.settings.PhysicsSettings.UsePaintedRigidity;

    public bool StyleMode => this.settings.PhysicsSettings.StyleMode;

    public GpuBuffer<Matrix4x4> MatricesBuffer => this.settings.StandsSettings.Provider.GetTransformsBuffer();

    public GpuBuffer<Vector3> NormalsBuffer => this.settings.StandsSettings.Provider.GetNormalsBuffer();

    public GpuBuffer<GPParticle> Particles => this.settings.RuntimeData.Particles;

    public GroupedData<GPDistanceJoint> DistanceJoints => this.settings.RuntimeData.DistanceJoints;

    public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer => this.settings.RuntimeData.DistanceJointsBuffer;

    public GroupedData<GPDistanceJoint> CompressionJoints => this.settings.RuntimeData.CompressionJoints;

    public GpuBuffer<GPDistanceJoint> CompressionJointsBuffer => this.settings.RuntimeData.CompressionJointsBuffer;

    public GroupedData<GPDistanceJoint> NearbyDistanceJoints => this.settings.RuntimeData.NearbyDistanceJoints;

    public GpuBuffer<GPDistanceJoint> NearbyDistanceJointsBuffer => this.settings.RuntimeData.NearbyDistanceJointsBuffer;

    public GpuBuffer<GPPointJoint> PointJoints => this.settings.RuntimeData.PointJoints;

    public GpuBuffer<float> PointToPreviousPointDistances => this.settings.RuntimeData.PointToPreviousPointDistances;

    public List<HairJointArea> JointAreas => this.settings.PhysicsSettings.JointAreas;

    public Vector4 Size => new Vector4((float) this.settings.StandsSettings.Provider.GetStandsNum(), (float) this.settings.StandsSettings.Provider.GetSegmentsNum());

    public GpuBuffer<GPSphereWithDelta> ProcessedSpheres => this.settings.RuntimeData.ProcessedSpheres;

    public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres => this.settings.RuntimeData.ProcessedLineSpheres;

    public GpuBuffer<Vector4> Planes => this.settings.RuntimeData.Planes;

    public GpuBuffer<GPLineSphere> CutLineSpheres => this.settings.RuntimeData.CutLineSpheres;

    public GpuBuffer<GPLineSphere> GrowLineSpheres => this.settings.RuntimeData.GrowLineSpheres;

    public GpuBuffer<GPLineSphere> HoldLineSpheres => this.settings.RuntimeData.HoldLineSpheres;

    public GpuBuffer<GPLineSphereWithMatrixDelta> GrabLineSpheres => this.settings.RuntimeData.GrabLineSpheres;

    public GpuBuffer<GPLineSphere> PushLineSpheres => this.settings.RuntimeData.PushLineSpheres;

    public GpuBuffer<GPLineSphere> PullLineSpheres => this.settings.RuntimeData.PullLineSpheres;

    public GpuBuffer<GPLineSphereWithDelta> BrushLineSpheres => this.settings.RuntimeData.BrushLineSpheres;

    public GpuBuffer<GPLineSphere> RigidityIncreaseLineSpheres => this.settings.RuntimeData.RigidityIncreaseLineSpheres;

    public GpuBuffer<GPLineSphere> RigidityDecreaseLineSpheres => this.settings.RuntimeData.RigidityDecreaseLineSpheres;

    public GpuBuffer<GPLineSphere> RigiditySetLineSpheres => this.settings.RuntimeData.RigiditySetLineSpheres;

    public GpuBuffer<TessRenderParticle> TessRenderParticles => this.settings.RuntimeData.TessRenderParticles;

    public GpuBuffer<GPParticle> OutParticles => this.settings.RuntimeData.OutParticles;

    public GpuBuffer<float> OutParticlesMap => this.settings.RuntimeData.OutParticlesMap;

    public GpuBuffer<RenderParticle> RenderParticles => this.settings.RuntimeData.RenderParticles;

    public GpuBuffer<Vector3> RandomsPerStrand => this.settings.RuntimeData.RandomsPerStrand;

    public Vector3 WavinessAxis => this.settings.RenderSettings.WavinessAxis;

    public Vector3 WorldWavinessAxis => this.settings.transform.TransformVector(this.WavinessAxis);

    public float WavinessFrequencyRandomness => this.settings.RenderSettings.WavinessFrequencyRandomness;

    public float WavinessScaleRandomness => this.settings.RenderSettings.WavinessScaleRandomness;

    public bool WavinessAllowReverse => this.settings.RenderSettings.WavinessAllowReverse;

    public bool WavinessAllowFlipAxis => this.settings.RenderSettings.WavinessAllowFlipAxis;

    public float WavinessNormalAdjust => this.settings.RenderSettings.WavinessNormalAdjust;

    public bool StyleModeShowCurls => this.settings.RenderSettings.StyleModeShowCurls;

    public Vector3 LightCenter => this.settings.StandsSettings.HeadCenterWorld;

    public float StandWidth => this.settings.LODSettings.GetWidth(this.LightCenter);

    public Vector3 TessFactor
    {
      get
      {
        int detail = this.settings.LODSettings.GetDetail(this.LightCenter);
        int dencity = this.settings.LODSettings.GetDencity(this.LightCenter);
        return (Vector3) new Vector4((float) detail, (float) dencity, 0.99f / (float) detail, 0.99f / (float) dencity);
      }
    }

    public bool IsPhysicsEnabledLOD => this.settings.LODSettings.IsPhysicsEnabled(this.LightCenter);

    public bool CastShadows => this.settings.ShadowSettings.CastShadows;

    public bool ReseiveShadows => this.settings.ShadowSettings.ReseiveShadows;

    public float SpecularShift => this.settings.RenderSettings.SpecularShift;

    public float PrimarySpecular => this.settings.RenderSettings.PrimarySpecular;

    public float SecondarySpecular => this.settings.RenderSettings.SecondarySpecular;

    public Color SpecularColor => this.settings.RenderSettings.SpecularColor;

    public float Diffuse => this.settings.RenderSettings.Diffuse;

    public float FresnelPower => this.settings.RenderSettings.FresnelPower;

    public float FresnelAttenuation => this.settings.RenderSettings.FresnelAttenuation;

    public float NormalRandomize => this.settings.RenderSettings.NormalRandomize;

    public Vector3 Length => (Vector3) new Vector4(this.settings.RenderSettings.Length1, this.settings.RenderSettings.Length2, this.settings.RenderSettings.Length3);

    public Material material => this.settings.RenderSettings.material;

    public GpuBuffer<RenderParticle> ParticlesData => this.settings.RuntimeData.RenderParticles;

    public GpuBuffer<Vector3> Barycentrics => this.settings.RuntimeData.Barycentrics;

    public GpuBuffer<Vector3> BarycentricsFixed => this.settings.RuntimeData.BarycentricsFixed;

    public int[] Indices => this.settings.StandsSettings.Provider.GetIndices();

    public Bounds Bounds => this.settings.StandsSettings.Provider.GetBounds();

    public float Volume => this.settings.RenderSettings.Volume;

    public float RandomTexColorPower => this.settings.RenderSettings.RandomTexColorPower;

    public float RandomTexColorOffset => this.settings.RenderSettings.RandomTexColorOffset;

    public float IBLFactor => this.settings.RenderSettings.IBLFactor;

    public float MaxSpread => this.settings.RenderSettings.MaxSpread;

    public bool UpdateRigidityJointsBeforeRender => this.settings.PhysicsSettings.UpdateRigidityJointsBeforeRender;
  }
}
