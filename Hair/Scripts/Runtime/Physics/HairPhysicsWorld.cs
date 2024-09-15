// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Physics.HairPhysicsWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Runtime.Kernels;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.DebugDraw;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Physics
{
  public class HairPhysicsWorld : PrimitiveBase
  {
    private readonly HairDataFacade data;
    private ResetToPointJointsKernel resetKernel;
    private IntegrateVelocityKernel integrateVelocityKernel;
    private IntegrateVelocityInnerKernel integrateVelocityInnerKernel;
    private IntegrateIterKernel integrateIterKernel;
    private IntegrateIterWithParticleHoldKernel integrateIterWithParticleHoldKernel;
    private DistanceJointsKernel distanceJointsKernel;
    private CompressionJointsKernel compressionJointsKernel;
    private NearbyDistanceJointsKernel nearbyDistanceJointsKernel;
    private ParticleCollisionResetKernel particleCollisionResetKernel;
    private ParticleLineSphereCollisionKernel lineSphereCollisionKernel;
    private ParticleSphereCollisionKernel sphereCollisionKernel;
    private ParticlePlaneCollisionKernel planeCollisionKernel;
    private ParticleLineSphereHoldResetKernel lineSphereHoldResetKernel;
    private ParticleLineSphereHoldKernel lineSphereHoldKernel;
    private ParticleLineSphereGrabKernel lineSphereGrabKernel;
    private ParticleLineSpherePushKernel lineSpherePushKernel;
    private ParticleLineSpherePullKernel lineSpherePullKernel;
    private ParticleLineSphereBrushKernel lineSphereBrushKernel;
    private ParticleLineSphereGrowKernel lineSphereGrowKernel;
    private ParticleLineSphereCutKernel lineSphereCutKernel;
    private ParticleLineSphereRigidityIncreaseKernel lineSphereRigidityIncreaseKernel;
    private ParticleLineSphereRigidityDecreaseKernel lineSphereRigidityDecreaseKernel;
    private ParticleLineSphereRigiditySetKernel lineSphereRigiditySetKernel;
    private DistanceJointsAdjustKernel distanceJointsAdjustKernel;
    private SplineJointsKernel splineJointsKernel;
    private SplineJointsReverseKernel splineJointsReverseKernel;
    private SplineJointsWithParticleHoldKernel splineJointsWithParticleHoldKernel;
    private SplineJointsReverseWithParticleHoldKernel splineJointsReverseWithParticleHoldKernel;
    private PointJointsKernel pointJointsKernel;
    private PointJointsFixedRigidityKernel pointJointsFixedRigidityKernel;
    private PointJointsFinalKernel pointJointsFinalKernel;
    private MovePointJointsToParticlesKernel movePointJointsToParticlesKernel;
    private CopySpecificParticlesKernel copySpecificParticlesKernel;
    private TesselateKernel tesselateKernel;
    private TesselateWithNormalsKernel tesselateWithNormalsKernel;
    private TesselateWithNormalsRenderRigidityKernel tesselateWithNormalsRenderRigidityKernel;
    private int frame;
    private int outerIterations;
    private int iterations;
    private List<KernelBase> staticQueue = new List<KernelBase>();
    private bool isPhysics;

    public HairPhysicsWorld(HairDataFacade data)
    {
      this.data = data;
      this.T = new GpuValue<float>();
      this.DT = new GpuValue<float>();
      this.DTRecip = new GpuValue<float>();
      this.Weight = new GpuValue<float>();
      this.Step = new GpuValue<float>();
      this.AccelDT2 = new GpuValue<Vector3>();
      this.InvDrag = new GpuValue<float>();
      this.DistanceScale = new GpuValue<float>();
      this.CompressionDistanceScale = new GpuValue<float>();
      this.NearbyDistanceScale = new GpuValue<float>();
      this.Friction = new GpuValue<float>();
      this.StaticFriction = new GpuValue<float>();
      this.CollisionPower = new GpuValue<float>();
      this.CompressionJointPower = new GpuValue<float>();
      this.NearbyJointPower = new GpuValue<float>();
      this.NearbyJointPowerRolloff = new GpuValue<float>();
      this.SplineJointPower = new GpuValue<float>();
      this.ReverseSplineJointPower = new GpuValue<float>();
      this.DistanceJointPower = new GpuValue<float>();
      this.Segments = new GpuValue<int>();
      this.TessSegments = new GpuValue<int>();
      this.TessRenderParticlesCount = new GpuValue<int>();
      this.WavinessAxis = new GpuValue<Vector3>();
      this.WavinessFrequencyRandomness = new GpuValue<float>();
      this.WavinessScaleRandomness = new GpuValue<float>();
      this.WavinessAllowReverse = new GpuValue<bool>();
      this.WavinessAllowFlipAxis = new GpuValue<bool>();
      this.WavinessNormalAdjust = new GpuValue<float>();
      this.IsFixed = new GpuValue<int>();
      this.FixedRigidity = new GpuValue<float>();
      this.LightCenter = new GpuValue<Vector3>();
      this.NormalRandomize = new GpuValue<float>();
      this.InitData();
      this.InitBuffers();
      this.InitPasses();
      this.Bind();
    }

    [GpuData("weight")]
    public GpuValue<float> Weight { set; get; }

    [GpuData("step")]
    public GpuValue<float> Step { set; get; }

    [GpuData("dt")]
    public GpuValue<float> DT { set; get; }

    [GpuData("dtrecip")]
    public GpuValue<float> DTRecip { set; get; }

    [GpuData("t")]
    public GpuValue<float> T { set; get; }

    [GpuData("accelDT2")]
    public GpuValue<Vector3> AccelDT2 { set; get; }

    [GpuData("invDrag")]
    public GpuValue<float> InvDrag { set; get; }

    [GpuData("distanceScale")]
    public GpuValue<float> DistanceScale { set; get; }

    [GpuData("compressionDistanceScale")]
    public GpuValue<float> CompressionDistanceScale { set; get; }

    [GpuData("nearbyDistanceScale")]
    public GpuValue<float> NearbyDistanceScale { set; get; }

    [GpuData("friction")]
    public GpuValue<float> Friction { set; get; }

    [GpuData("staticFriction")]
    public GpuValue<float> StaticFriction { set; get; }

    [GpuData("collisionPower")]
    public GpuValue<float> CollisionPower { set; get; }

    [GpuData("compressionJointPower")]
    public GpuValue<float> CompressionJointPower { set; get; }

    [GpuData("nearbyJointPower")]
    public GpuValue<float> NearbyJointPower { set; get; }

    [GpuData("nearbyJointPowerRolloff")]
    public GpuValue<float> NearbyJointPowerRolloff { set; get; }

    [GpuData("splineJointPower")]
    public GpuValue<float> SplineJointPower { set; get; }

    [GpuData("reverseSplineJointPower")]
    public GpuValue<float> ReverseSplineJointPower { set; get; }

    [GpuData("distanceJointPower")]
    public GpuValue<float> DistanceJointPower { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("normals")]
    public GpuBuffer<Vector3> Normals { set; get; }

    [GpuData("transforms")]
    public GpuBuffer<Matrix4x4> Transforms { set; get; }

    [GpuData("oldTransforms")]
    public GpuBuffer<Matrix4x4> OldTransforms { set; get; }

    [GpuData("pointJoints")]
    public GpuBuffer<GPPointJoint> PointJoints { set; get; }

    [GpuData("pointToPreviousPointDistances")]
    public GpuBuffer<float> PointToPreviousPointDistances { set; get; }

    [GpuData("isFixed")]
    public GpuValue<int> IsFixed { set; get; }

    [GpuData("fixedRigidity")]
    public GpuValue<float> FixedRigidity { set; get; }

    [GpuData("processedSpheres")]
    public GpuBuffer<GPSphereWithDelta> ProcessedSpheres { set; get; }

    [GpuData("processedLineSpheres")]
    public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres { set; get; }

    [GpuData("planes")]
    public GpuBuffer<Vector4> Planes { set; get; }

    [GpuData("cutLineSpheres")]
    public GpuBuffer<GPLineSphere> CutLineSpheres { set; get; }

    [GpuData("growLineSpheres")]
    public GpuBuffer<GPLineSphere> GrowLineSpheres { set; get; }

    [GpuData("holdLineSpheres")]
    public GpuBuffer<GPLineSphere> HoldLineSpheres { set; get; }

    [GpuData("grabLineSpheres")]
    public GpuBuffer<GPLineSphereWithMatrixDelta> GrabLineSpheres { set; get; }

    [GpuData("pushLineSpheres")]
    public GpuBuffer<GPLineSphere> PushLineSpheres { set; get; }

    [GpuData("pullLineSpheres")]
    public GpuBuffer<GPLineSphere> PullLineSpheres { set; get; }

    [GpuData("brushLineSpheres")]
    public GpuBuffer<GPLineSphereWithDelta> BrushLineSpheres { set; get; }

    [GpuData("rigidityIncreaseLineSpheres")]
    public GpuBuffer<GPLineSphere> RigidityIncreaseLineSpheres { set; get; }

    [GpuData("rigidityDecreaseLineSpheres")]
    public GpuBuffer<GPLineSphere> RigidityDecreaseLineSpheres { set; get; }

    [GpuData("rigiditySetLineSpheres")]
    public GpuBuffer<GPLineSphere> RigiditySetLineSpheres { set; get; }

    [GpuData("outParticles")]
    public GpuBuffer<GPParticle> OutParticles { set; get; }

    [GpuData("outParticlesMap")]
    public GpuBuffer<float> OutParticlesMap { set; get; }

    [GpuData("renderParticles")]
    public GpuBuffer<RenderParticle> RenderParticles { set; get; }

    [GpuData("tessRenderParticles")]
    public GpuBuffer<TessRenderParticle> TessRenderParticles { set; get; }

    [GpuData("tessRenderParticlesCount")]
    public GpuValue<int> TessRenderParticlesCount { set; get; }

    [GpuData("randomsPerStrand")]
    public GpuBuffer<Vector3> RandomsPerStrand { set; get; }

    [GpuData("segments")]
    public GpuValue<int> Segments { set; get; }

    [GpuData("tessSegments")]
    public GpuValue<int> TessSegments { set; get; }

    [GpuData("wavinessAxis")]
    public GpuValue<Vector3> WavinessAxis { set; get; }

    [GpuData("wavinessFrequencyRandomness")]
    public GpuValue<float> WavinessFrequencyRandomness { set; get; }

    [GpuData("wavinessScaleRandomness")]
    public GpuValue<float> WavinessScaleRandomness { set; get; }

    [GpuData("wavinessAllowReverse")]
    public GpuValue<bool> WavinessAllowReverse { set; get; }

    [GpuData("wavinessAllowFlipAxis")]
    public GpuValue<bool> WavinessAllowFlipAxis { set; get; }

    [GpuData("wavinessNormalAdjust")]
    public GpuValue<float> WavinessNormalAdjust { set; get; }

    [GpuData("lightCenter")]
    public GpuValue<Vector3> LightCenter { set; get; }

    [GpuData("normalRandomize")]
    public GpuValue<float> NormalRandomize { set; get; }

    private void InitData()
    {
      if ((double) Time.fixedDeltaTime > 0.019999999552965164)
      {
        this.outerIterations = 2;
        this.iterations = this.data.Iterations;
      }
      else
      {
        this.outerIterations = 1;
        this.iterations = this.data.Iterations;
      }
      this.Weight.Value = this.data.Weight;
      this.InvDrag.Value = !this.data.StyleMode ? this.data.InvDrag : 0.0f;
      this.DistanceScale.Value = 1f;
      this.CompressionDistanceScale.Value = 1f;
      this.NearbyDistanceScale.Value = this.data.WorldScale;
      this.Friction.Value = this.data.Friction;
      this.StaticFriction.Value = this.data.Friction * 2f;
      this.CollisionPower.Value = this.data.CollisionPower;
      this.CompressionJointPower.Value = this.data.CompressionJointPower / (float) this.iterations;
      this.NearbyJointPower.Value = this.data.NearbyJointPower * 0.5f / (float) this.iterations;
      this.NearbyJointPowerRolloff.Value = this.data.NearbyJointPowerRolloff;
      this.ReverseSplineJointPower.Value = this.data.ReverseSplineJointPower;
      this.FixedRigidity.Value = 0.1f;
      this.SplineJointPower.Value = !this.data.StyleMode ? Mathf.Min(this.data.SplineJointPower * 2f / (float) this.iterations, 1f) : 1f;
      this.DistanceJointPower.Value = 0.5f;
      this.Segments.Value = (int) this.data.Size.y;
      this.TessSegments.Value = (int) this.data.TessFactor.y;
      this.TessRenderParticlesCount.Value = this.data.Particles == null ? 0 : (int) this.data.TessFactor.y * this.data.Particles.Count;
      if (this.data.StyleMode && !this.data.StyleModeShowCurls)
      {
        this.WavinessAxis.Value = Vector3.zero;
        this.WavinessNormalAdjust.Value = 0.0f;
      }
      else
      {
        this.WavinessAxis.Value = this.data.WavinessAxis;
        this.WavinessNormalAdjust.Value = this.data.WavinessNormalAdjust * this.data.WorldScale;
      }
      this.WavinessFrequencyRandomness.Value = this.data.WavinessFrequencyRandomness;
      this.WavinessScaleRandomness.Value = this.data.WavinessScaleRandomness;
      this.WavinessAllowReverse.Value = this.data.WavinessAllowReverse;
      this.WavinessAllowFlipAxis.Value = this.data.WavinessAllowFlipAxis;
      this.IsFixed.Value = !this.isPhysics ? 1 : 0;
      this.LightCenter.Value = this.data.LightCenter;
      this.NormalRandomize.Value = this.data.NormalRandomize;
    }

    private void InitBuffers()
    {
      this.Particles = this.data.Particles;
      this.Normals = this.data.NormalsBuffer;
      this.Transforms = this.data.MatricesBuffer;
      if (this.Transforms != null)
        this.OldTransforms = new GpuBuffer<Matrix4x4>(this.Transforms.Count, 64);
      this.PointJoints = this.data.PointJoints;
      this.PointToPreviousPointDistances = this.data.PointToPreviousPointDistances;
      this.ProcessedSpheres = this.data.ProcessedSpheres;
      this.ProcessedLineSpheres = this.data.ProcessedLineSpheres;
      this.CutLineSpheres = this.data.CutLineSpheres;
      this.GrowLineSpheres = this.data.GrowLineSpheres;
      this.HoldLineSpheres = this.data.HoldLineSpheres;
      this.GrabLineSpheres = this.data.GrabLineSpheres;
      this.PushLineSpheres = this.data.PushLineSpheres;
      this.PullLineSpheres = this.data.PullLineSpheres;
      this.BrushLineSpheres = this.data.BrushLineSpheres;
      this.RigidityIncreaseLineSpheres = this.data.RigidityIncreaseLineSpheres;
      this.RigidityDecreaseLineSpheres = this.data.RigidityDecreaseLineSpheres;
      this.RigiditySetLineSpheres = this.data.RigiditySetLineSpheres;
      this.RenderParticles = this.data.RenderParticles;
      this.TessRenderParticles = this.data.TessRenderParticles;
      this.RandomsPerStrand = this.data.RandomsPerStrand;
      this.OutParticles = this.data.OutParticles;
      this.OutParticlesMap = this.data.OutParticlesMap;
    }

    private void UpdateBuffers()
    {
      if (this.Transforms != this.data.MatricesBuffer)
      {
        this.Transforms = this.data.MatricesBuffer;
        if (this.resetKernel != null)
        {
          this.resetKernel.Transforms = this.Transforms;
          this.resetKernel.ClearCacheAttributes();
        }
        if (this.pointJointsKernel != null)
        {
          this.pointJointsKernel.Transforms = this.Transforms;
          this.pointJointsKernel.ClearCacheAttributes();
        }
        if (this.pointJointsFixedRigidityKernel != null)
        {
          this.pointJointsFixedRigidityKernel.Transforms = this.Transforms;
          this.pointJointsFixedRigidityKernel.ClearCacheAttributes();
        }
        if (this.pointJointsFinalKernel != null)
        {
          this.pointJointsFinalKernel.Transforms = this.Transforms;
          this.pointJointsFinalKernel.ClearCacheAttributes();
        }
        if (this.movePointJointsToParticlesKernel != null)
        {
          this.movePointJointsToParticlesKernel.Transforms = this.Transforms;
          this.movePointJointsToParticlesKernel.ClearCacheAttributes();
        }
        if (this.tesselateKernel != null)
        {
          this.tesselateKernel.Transforms = this.Transforms;
          this.tesselateKernel.ClearCacheAttributes();
        }
        else if (this.tesselateWithNormalsKernel != null)
        {
          this.tesselateWithNormalsKernel.Transforms = this.Transforms;
          this.tesselateWithNormalsKernel.ClearCacheAttributes();
          if (this.tesselateWithNormalsRenderRigidityKernel != null)
          {
            this.tesselateWithNormalsRenderRigidityKernel.Transforms = this.Transforms;
            this.tesselateWithNormalsRenderRigidityKernel.ClearCacheAttributes();
          }
        }
      }
      if (this.Normals != this.data.NormalsBuffer)
      {
        this.Normals = this.data.NormalsBuffer;
        if (this.tesselateWithNormalsKernel != null)
        {
          this.tesselateWithNormalsKernel.Normals = this.Normals;
          this.tesselateWithNormalsKernel.ClearCacheAttributes();
          if (this.tesselateWithNormalsRenderRigidityKernel != null)
          {
            this.tesselateWithNormalsRenderRigidityKernel.Normals = this.Normals;
            this.tesselateWithNormalsRenderRigidityKernel.ClearCacheAttributes();
          }
        }
      }
      if (this.ProcessedLineSpheres != this.data.ProcessedLineSpheres)
      {
        this.ProcessedLineSpheres = this.data.ProcessedLineSpheres;
        this.lineSphereCollisionKernel.ProcessedLineSpheres = this.ProcessedLineSpheres;
        this.lineSphereCollisionKernel.ClearCacheAttributes();
      }
      if (this.ProcessedSpheres != this.data.ProcessedSpheres)
      {
        this.ProcessedSpheres = this.data.ProcessedSpheres;
        this.sphereCollisionKernel.ProcessedSpheres = this.ProcessedSpheres;
        this.sphereCollisionKernel.ClearCacheAttributes();
      }
      if (this.Planes != this.data.Planes)
      {
        this.Planes = this.data.Planes;
        this.planeCollisionKernel.Planes = this.Planes;
        this.planeCollisionKernel.ClearCacheAttributes();
      }
      if (this.GrowLineSpheres != this.data.GrowLineSpheres)
      {
        this.GrowLineSpheres = this.data.GrowLineSpheres;
        this.lineSphereGrowKernel.GrowLineSpheres = this.GrowLineSpheres;
        this.lineSphereGrowKernel.ClearCacheAttributes();
      }
      if (this.CutLineSpheres != this.data.CutLineSpheres)
      {
        this.CutLineSpheres = this.data.CutLineSpheres;
        this.lineSphereCutKernel.CutLineSpheres = this.CutLineSpheres;
        this.lineSphereCutKernel.ClearCacheAttributes();
      }
      if (this.PushLineSpheres != this.data.PushLineSpheres)
      {
        this.PushLineSpheres = this.data.PushLineSpheres;
        this.lineSpherePushKernel.PushLineSpheres = this.PushLineSpheres;
        this.lineSpherePushKernel.ClearCacheAttributes();
      }
      if (this.PullLineSpheres != this.data.PullLineSpheres)
      {
        this.PullLineSpheres = this.data.PullLineSpheres;
        this.lineSpherePullKernel.PullLineSpheres = this.PullLineSpheres;
        this.lineSpherePullKernel.ClearCacheAttributes();
      }
      if (this.BrushLineSpheres != this.data.BrushLineSpheres)
      {
        this.BrushLineSpheres = this.data.BrushLineSpheres;
        this.lineSphereBrushKernel.BrushLineSpheres = this.BrushLineSpheres;
        this.lineSphereBrushKernel.ClearCacheAttributes();
      }
      if (this.HoldLineSpheres != this.data.HoldLineSpheres)
      {
        this.HoldLineSpheres = this.data.HoldLineSpheres;
        this.lineSphereHoldKernel.HoldLineSpheres = this.HoldLineSpheres;
        this.lineSphereHoldKernel.ClearCacheAttributes();
      }
      if (this.GrabLineSpheres != this.data.GrabLineSpheres)
      {
        this.GrabLineSpheres = this.data.GrabLineSpheres;
        this.lineSphereGrabKernel.GrabLineSpheres = this.GrabLineSpheres;
        this.lineSphereGrabKernel.ClearCacheAttributes();
      }
      if (this.RigidityIncreaseLineSpheres != this.data.RigidityIncreaseLineSpheres)
      {
        this.RigidityIncreaseLineSpheres = this.data.RigidityIncreaseLineSpheres;
        this.lineSphereRigidityIncreaseKernel.RigidityIncreaseLineSpheres = this.RigidityIncreaseLineSpheres;
        this.lineSphereRigidityIncreaseKernel.ClearCacheAttributes();
      }
      if (this.RigidityDecreaseLineSpheres != this.data.RigidityDecreaseLineSpheres)
      {
        this.RigidityDecreaseLineSpheres = this.data.RigidityDecreaseLineSpheres;
        this.lineSphereRigidityDecreaseKernel.RigidityDecreaseLineSpheres = this.RigidityDecreaseLineSpheres;
        this.lineSphereRigidityDecreaseKernel.ClearCacheAttributes();
      }
      if (this.RigiditySetLineSpheres == this.data.RigiditySetLineSpheres)
        return;
      this.RigiditySetLineSpheres = this.data.RigiditySetLineSpheres;
      this.lineSphereRigiditySetKernel.RigiditySetLineSpheres = this.RigiditySetLineSpheres;
      this.lineSphereRigiditySetKernel.ClearCacheAttributes();
    }

    private void AddStaticPass(KernelBase kernel)
    {
      this.AddPass((IPass) kernel);
      this.staticQueue.Add(kernel);
    }

    private void InitPasses()
    {
      this.AddPass((IPass) (this.integrateVelocityKernel = new IntegrateVelocityKernel()));
      this.AddPass((IPass) (this.integrateVelocityInnerKernel = new IntegrateVelocityInnerKernel()));
      this.AddPass((IPass) (this.integrateIterKernel = new IntegrateIterKernel()));
      this.AddPass((IPass) (this.integrateIterWithParticleHoldKernel = new IntegrateIterWithParticleHoldKernel()));
      this.AddPass((IPass) (this.distanceJointsKernel = new DistanceJointsKernel(this.data.DistanceJoints, this.data.DistanceJointsBuffer)));
      this.AddPass((IPass) (this.compressionJointsKernel = new CompressionJointsKernel(this.data.CompressionJoints, this.data.CompressionJointsBuffer)));
      this.AddPass((IPass) (this.nearbyDistanceJointsKernel = new NearbyDistanceJointsKernel(this.data.NearbyDistanceJoints, this.data.NearbyDistanceJointsBuffer)));
      this.AddPass((IPass) (this.splineJointsKernel = new SplineJointsKernel()));
      this.AddPass((IPass) (this.splineJointsReverseKernel = new SplineJointsReverseKernel()));
      this.AddPass((IPass) (this.splineJointsWithParticleHoldKernel = new SplineJointsWithParticleHoldKernel()));
      this.AddPass((IPass) (this.splineJointsReverseWithParticleHoldKernel = new SplineJointsReverseWithParticleHoldKernel()));
      this.AddPass((IPass) (this.particleCollisionResetKernel = new ParticleCollisionResetKernel()));
      this.AddPass((IPass) (this.planeCollisionKernel = new ParticlePlaneCollisionKernel()));
      this.AddPass((IPass) (this.sphereCollisionKernel = new ParticleSphereCollisionKernel()));
      this.AddPass((IPass) (this.lineSphereCollisionKernel = new ParticleLineSphereCollisionKernel()));
      this.AddPass((IPass) (this.lineSphereHoldResetKernel = new ParticleLineSphereHoldResetKernel()));
      this.AddPass((IPass) (this.lineSphereHoldKernel = new ParticleLineSphereHoldKernel()));
      this.AddPass((IPass) (this.lineSphereGrabKernel = new ParticleLineSphereGrabKernel()));
      this.AddPass((IPass) (this.lineSpherePushKernel = new ParticleLineSpherePushKernel()));
      this.AddPass((IPass) (this.lineSpherePullKernel = new ParticleLineSpherePullKernel()));
      this.AddPass((IPass) (this.lineSphereBrushKernel = new ParticleLineSphereBrushKernel()));
      this.AddPass((IPass) (this.lineSphereGrowKernel = new ParticleLineSphereGrowKernel()));
      this.AddPass((IPass) (this.lineSphereCutKernel = new ParticleLineSphereCutKernel()));
      this.AddPass((IPass) (this.lineSphereRigidityIncreaseKernel = new ParticleLineSphereRigidityIncreaseKernel()));
      this.AddPass((IPass) (this.lineSphereRigidityDecreaseKernel = new ParticleLineSphereRigidityDecreaseKernel()));
      this.AddPass((IPass) (this.lineSphereRigiditySetKernel = new ParticleLineSphereRigiditySetKernel()));
      this.AddPass((IPass) (this.distanceJointsAdjustKernel = new DistanceJointsAdjustKernel(this.data.DistanceJoints, this.data.DistanceJointsBuffer)));
      this.AddPass((IPass) (this.pointJointsKernel = new PointJointsKernel()));
      this.AddPass((IPass) (this.pointJointsFixedRigidityKernel = new PointJointsFixedRigidityKernel()));
      this.AddPass((IPass) (this.movePointJointsToParticlesKernel = new MovePointJointsToParticlesKernel()));
      this.AddStaticPass((KernelBase) (this.pointJointsFinalKernel = new PointJointsFinalKernel()));
      if (this.data.OutParticles != null)
        this.AddStaticPass((KernelBase) (this.copySpecificParticlesKernel = new CopySpecificParticlesKernel()));
      if (this.data.NormalsBuffer != null)
      {
        this.AddStaticPass((KernelBase) (this.tesselateWithNormalsKernel = new TesselateWithNormalsKernel()));
        this.AddStaticPass((KernelBase) (this.tesselateWithNormalsRenderRigidityKernel = new TesselateWithNormalsRenderRigidityKernel()));
      }
      else
        this.AddStaticPass((KernelBase) (this.tesselateKernel = new TesselateKernel()));
      this.AddStaticPass((KernelBase) (this.resetKernel = new ResetToPointJointsKernel()));
    }

    public void RebindData()
    {
      this.Particles = this.data.Particles;
      this.Transforms = this.data.MatricesBuffer;
      this.Normals = this.data.NormalsBuffer;
      if (this.OldTransforms != null)
        this.OldTransforms.Dispose();
      this.OldTransforms = this.Transforms == null ? (GpuBuffer<Matrix4x4>) null : new GpuBuffer<Matrix4x4>(this.Transforms.Count, 64);
      this.PointJoints = this.data.PointJoints;
      this.PointToPreviousPointDistances = this.data.PointToPreviousPointDistances;
      this.RenderParticles = this.data.RenderParticles;
      this.TessRenderParticles = this.data.TessRenderParticles;
      this.RandomsPerStrand = this.data.RandomsPerStrand;
      this.BindAttributes();
      this.integrateIterKernel.ClearCacheAttributes();
      this.integrateIterWithParticleHoldKernel.ClearCacheAttributes();
      this.integrateVelocityKernel.ClearCacheAttributes();
      this.integrateVelocityInnerKernel.ClearCacheAttributes();
      this.distanceJointsKernel.DistanceJoints = this.data.DistanceJoints;
      this.distanceJointsKernel.DistanceJointsBuffer = this.data.DistanceJointsBuffer;
      this.distanceJointsKernel.ClearCacheAttributes();
      this.compressionJointsKernel.CompressionJoints = this.data.CompressionJoints;
      this.compressionJointsKernel.CompressionJointsBuffer = this.data.CompressionJointsBuffer;
      this.compressionJointsKernel.ClearCacheAttributes();
      this.nearbyDistanceJointsKernel.NearbyDistanceJoints = this.data.NearbyDistanceJoints;
      this.nearbyDistanceJointsKernel.NearbyDistanceJointsBuffer = this.data.NearbyDistanceJointsBuffer;
      this.nearbyDistanceJointsKernel.ClearCacheAttributes();
      this.splineJointsKernel.ClearCacheAttributes();
      this.splineJointsReverseKernel.ClearCacheAttributes();
      this.splineJointsWithParticleHoldKernel.ClearCacheAttributes();
      this.splineJointsReverseWithParticleHoldKernel.ClearCacheAttributes();
      this.particleCollisionResetKernel.ClearCacheAttributes();
      this.planeCollisionKernel.ClearCacheAttributes();
      this.sphereCollisionKernel.ClearCacheAttributes();
      this.lineSphereCollisionKernel.ClearCacheAttributes();
      this.lineSphereHoldResetKernel.ClearCacheAttributes();
      this.lineSphereHoldKernel.ClearCacheAttributes();
      this.lineSphereGrabKernel.ClearCacheAttributes();
      this.lineSpherePushKernel.ClearCacheAttributes();
      this.lineSpherePullKernel.ClearCacheAttributes();
      this.lineSphereBrushKernel.ClearCacheAttributes();
      this.lineSphereGrowKernel.ClearCacheAttributes();
      this.lineSphereCutKernel.ClearCacheAttributes();
      this.lineSphereRigidityIncreaseKernel.ClearCacheAttributes();
      this.lineSphereRigidityDecreaseKernel.ClearCacheAttributes();
      this.lineSphereRigiditySetKernel.ClearCacheAttributes();
      this.distanceJointsAdjustKernel.DistanceJoints = this.data.DistanceJoints;
      this.distanceJointsAdjustKernel.DistanceJointsBuffer = this.data.DistanceJointsBuffer;
      this.distanceJointsAdjustKernel.ClearCacheAttributes();
      this.pointJointsKernel.ClearCacheAttributes();
      this.pointJointsFixedRigidityKernel.ClearCacheAttributes();
      this.pointJointsFinalKernel.ClearCacheAttributes();
      this.movePointJointsToParticlesKernel.ClearCacheAttributes();
      if (this.copySpecificParticlesKernel != null)
        this.copySpecificParticlesKernel.ClearCacheAttributes();
      if (this.tesselateKernel != null)
        this.tesselateKernel.ClearCacheAttributes();
      else if (this.tesselateWithNormalsKernel != null)
      {
        this.tesselateWithNormalsKernel.ClearCacheAttributes();
        if (this.tesselateWithNormalsRenderRigidityKernel != null)
          this.tesselateWithNormalsRenderRigidityKernel.ClearCacheAttributes();
      }
      this.resetKernel.ClearCacheAttributes();
    }

    public void Reset() => this.frame = 0;

    public void PartialReset()
    {
      if (this.frame <= 10)
        return;
      this.frame = 10;
    }

    public void FixedDispatch()
    {
      if (!this.isPhysics || this.data.RunPhysicsOnUpdate)
        return;
      this.DispatchPhysicsImpl();
    }

    public override void Dispatch()
    {
      this.UpdateIsPhysics();
      if (!this.isPhysics)
        this.DispatchStaticImpl();
      else if (this.data.RunPhysicsOnUpdate)
        this.DispatchPhysicsImpl();
      this.DispatchImpl();
    }

    private void DispatchPhysicsImpl()
    {
      this.InitData();
      this.UpdateBuffers();
      if (this.frame < 10)
      {
        this.resetKernel.Dispatch();
        this.particleCollisionResetKernel.Dispatch();
      }
      else
      {
        for (int index1 = 1; index1 <= this.outerIterations; ++index1)
        {
          float num1 = Time.fixedDeltaTime * Mathf.Max(this.data.Weight, 0.01f) / (float) this.outerIterations;
          this.Step.Value = 1f / (float) (this.iterations * this.outerIterations);
          float num2 = num1 / (float) this.iterations;
          this.DT.Value = num2;
          this.DTRecip.Value = 1f / num2;
          this.AccelDT2.Value = (float) ((double) num2 * (double) num2 * 0.5) * (this.data.Gravity + this.data.Wind);
          if (this.data.StyleMode)
          {
            this.lineSphereHoldResetKernel.Dispatch();
            if (this.lineSphereHoldKernel.HoldLineSpheres != null)
              this.lineSphereHoldKernel.Dispatch();
            if (this.lineSphereBrushKernel.BrushLineSpheres != null)
              this.lineSphereBrushKernel.Dispatch();
            this.splineJointsReverseWithParticleHoldKernel.Dispatch();
            this.splineJointsWithParticleHoldKernel.Dispatch();
            if (this.lineSphereBrushKernel.BrushLineSpheres != null)
              this.lineSphereBrushKernel.Dispatch();
            if (this.lineSpherePullKernel.PullLineSpheres != null)
              this.lineSpherePullKernel.Dispatch();
            if (this.lineSpherePushKernel.PushLineSpheres != null)
              this.lineSpherePushKernel.Dispatch();
            if (this.lineSphereGrabKernel.GrabLineSpheres != null)
              this.lineSphereGrabKernel.Dispatch();
            this.splineJointsReverseWithParticleHoldKernel.Dispatch();
            this.splineJointsWithParticleHoldKernel.Dispatch();
            if (this.lineSphereGrowKernel.GrowLineSpheres != null)
            {
              this.lineSphereGrowKernel.Dispatch();
              this.splineJointsReverseKernel.Dispatch();
            }
            for (int index2 = 0; index2 < 4; ++index2)
            {
              if (this.lineSphereCutKernel.CutLineSpheres != null)
              {
                this.lineSphereCutKernel.Dispatch();
                this.splineJointsKernel.Dispatch();
              }
            }
            this.movePointJointsToParticlesKernel.Dispatch();
            for (int index3 = 1; index3 <= this.iterations; ++index3)
            {
              this.T.Value = (float) index3 / (float) this.iterations;
              if (this.frame > 20)
                this.integrateIterWithParticleHoldKernel.Dispatch();
              this.pointJointsFixedRigidityKernel.Dispatch();
              this.splineJointsWithParticleHoldKernel.Dispatch();
              this.splineJointsReverseWithParticleHoldKernel.Dispatch();
              this.integrateVelocityInnerKernel.Dispatch();
            }
            this.Step.Value = 1f / (float) this.outerIterations;
            this.DT.Value = num1;
            this.DTRecip.Value = 1f / num1;
            this.particleCollisionResetKernel.Dispatch();
            if (this.data.IsCollisionEnabled)
            {
              if (this.planeCollisionKernel.Planes != null)
                this.planeCollisionKernel.Dispatch();
              if (this.sphereCollisionKernel.ProcessedSpheres != null)
                this.sphereCollisionKernel.Dispatch();
              if (this.lineSphereCollisionKernel.ProcessedLineSpheres != null)
                this.lineSphereCollisionKernel.Dispatch();
            }
            this.pointJointsFixedRigidityKernel.Dispatch();
            this.splineJointsWithParticleHoldKernel.Dispatch();
            this.splineJointsReverseWithParticleHoldKernel.Dispatch();
            this.integrateVelocityKernel.Dispatch();
            if (this.data.UsePaintedRigidity)
            {
              if (this.lineSphereRigidityIncreaseKernel.RigidityIncreaseLineSpheres != null)
                this.lineSphereRigidityIncreaseKernel.Dispatch();
              if (this.lineSphereRigidityDecreaseKernel.RigidityDecreaseLineSpheres != null)
                this.lineSphereRigidityDecreaseKernel.Dispatch();
              if (this.lineSphereRigiditySetKernel.RigiditySetLineSpheres != null)
                this.lineSphereRigiditySetKernel.Dispatch();
            }
          }
          else
          {
            for (int index4 = 1; index4 <= this.iterations; ++index4)
            {
              this.T.Value = (float) index4 / (float) this.iterations;
              if (this.frame > 20)
                this.integrateIterKernel.Dispatch();
              this.pointJointsKernel.Dispatch();
              this.distanceJointsKernel.Dispatch();
              if ((double) this.CompressionJointPower.Value != 0.0)
                this.compressionJointsKernel.Dispatch();
              if ((double) this.NearbyJointPower.Value != 0.0)
                this.nearbyDistanceJointsKernel.Dispatch();
              this.distanceJointsKernel.Dispatch();
              this.splineJointsKernel.Dispatch();
              this.distanceJointsKernel.Dispatch();
              this.integrateVelocityInnerKernel.Dispatch();
            }
            this.Step.Value = 1f / (float) this.outerIterations;
            this.DT.Value = num1;
            this.DTRecip.Value = 1f / num1;
            this.particleCollisionResetKernel.Dispatch();
            if (this.data.IsCollisionEnabled)
            {
              if (this.planeCollisionKernel.Planes != null)
                this.planeCollisionKernel.Dispatch();
              if (this.sphereCollisionKernel.ProcessedSpheres != null)
                this.sphereCollisionKernel.Dispatch();
              if (this.lineSphereCollisionKernel.ProcessedLineSpheres != null)
                this.lineSphereCollisionKernel.Dispatch();
            }
            this.pointJointsKernel.Dispatch();
            this.integrateVelocityKernel.Dispatch();
          }
        }
      }
    }

    private void DispatchStaticImpl()
    {
      this.T.Value = 1f;
      for (int index = 0; index < this.staticQueue.Count; ++index)
        this.staticQueue[index].Dispatch();
    }

    private void DispatchImpl()
    {
      this.InitData();
      this.UpdateBuffers();
      this.Step.Value = 1f;
      if (this.frame < 1)
        this.resetKernel.Dispatch();
      if (!this.data.StyleMode && this.data.UpdateRigidityJointsBeforeRender)
        this.pointJointsFinalKernel.Dispatch();
      if (this.copySpecificParticlesKernel != null)
        this.copySpecificParticlesKernel.Dispatch();
      if (this.tesselateKernel != null)
        this.tesselateKernel.Dispatch();
      else if (this.tesselateWithNormalsKernel != null)
      {
        if (this.data.StyleMode && this.tesselateWithNormalsRenderRigidityKernel != null)
          this.tesselateWithNormalsRenderRigidityKernel.Dispatch();
        else
          this.tesselateWithNormalsKernel.Dispatch();
      }
      ++this.frame;
    }

    public override void Dispose()
    {
      base.Dispose();
      if (this.OldTransforms == null)
        return;
      this.OldTransforms.Dispose();
    }

    public void DebugDraw()
    {
      if (!this.data.DebugDraw || !this.data.IsPhysicsEnabled || !this.data.IsPhysicsEnabledLOD || this.nearbyDistanceJointsKernel == null)
        return;
      GPDebugDraw.Draw(this.distanceJointsKernel.DistanceJointsBuffer, this.nearbyDistanceJointsKernel.NearbyDistanceJointsBuffer, this.Particles, false, !this.data.DebugDrawNearbyJoints, this.data.DebugDrawNearbyJoints);
    }

    public void UpdateIsPhysics()
    {
      bool flag = this.data.IsPhysicsEnabled && this.data.IsPhysicsEnabledLOD || this.data.StyleMode;
      if (!this.isPhysics && flag)
        this.resetKernel.Dispatch();
      this.isPhysics = flag;
    }
  }
}
