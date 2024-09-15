// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Physics.ClothPhysicsWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Cloth.Scripts.Runtime.Kernels;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Kernels;
using GPUTools.Physics.Scripts.DebugDraw;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Physics
{
  public class ClothPhysicsWorld : PrimitiveBase
  {
    private readonly ClothDataFacade data;
    private GPUMatrixCopyPaster matrixCopyPaster;
    private GPUVector3CopyPaster vector3CopyPaster;
    private ResetToPointJointsKernel resetKernel;
    private ResetToPointJointsPreCalculatedKernel resetToPointJointsPreCalculatedKernel;
    private IntegrateVelocityKernel integrateVelocityKernel;
    private IntegrateVelocityInnerKernel integrateVelocityInnerKernel;
    private IntegrateIterKernel integrateIterKernel;
    private StiffnessJointsKernel stiffnessJointsKernel;
    private StiffnessJointsKernel nearbyJointsKernel;
    private ParticleCollisionResetKernel particleCollisionResetKernel;
    private ParticleNeiborsCollisionKernel particleNeiborsCollisionKernel;
    private ParticlePlaneCollisionKernel particlePlaneCollisionKernel;
    private ParticleSphereCollisionKernel sphereCollisionKernel;
    private ParticleLineSphereCollisionKernel lineSphereCollisionKernel;
    private PointJointsPreCalculatedKernel pointJointsPreCalculatedKernel;
    private PointJointsKernel pointJointsKernel;
    private GrabJointsKernel grabJointsKernel;
    private PointJointsPreCalculatedFinalKernel pointJointsPreCalculatedFinalKernel;
    private PointJointsFinalKernel pointJointsFinalKernel;
    private CopySpecificParticlesKernel copySpecificParticlesKernel;
    private CreateVertexDataKernel createVertexDataKernel;
    private CreateVertexOnlyDataKernel createVertexOnlyDataKernel;
    private int frame;
    private int outerIterations;
    private int iterations;
    private bool usesPreCalcVerts;
    protected int fixedDispatchCount;

    public ClothPhysicsWorld(ClothDataFacade data)
    {
      this.data = data;
      this.T = new GpuValue<float>();
      this.DT = new GpuValue<float>();
      this.DTRecip = new GpuValue<float>();
      this.Weight = new GpuValue<float>();
      this.Step = new GpuValue<float>();
      this.AccelDT2 = new GpuValue<Vector3>();
      this.InvDrag = new GpuValue<float>();
      this.Stiffness = new GpuValue<float>();
      this.CompressionResistance = new GpuValue<float>();
      this.DistanceScale = new GpuValue<float>();
      this.BreakThreshold = new GpuValue<float>();
      this.JointStrength = new GpuValue<float>();
      this.JointPrediction = new GpuValue<float>();
      this.Friction = new GpuValue<float>();
      this.StaticFriction = new GpuValue<float>();
      this.CollisionPower = new GpuValue<float>();
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

    [GpuData("stiffness")]
    public GpuValue<float> Stiffness { set; get; }

    [GpuData("distanceScale")]
    public GpuValue<float> DistanceScale { set; get; }

    [GpuData("compressionResistance")]
    public GpuValue<float> CompressionResistance { set; get; }

    [GpuData("breakThreshold")]
    public GpuValue<float> BreakThreshold { set; get; }

    [GpuData("jointStrength")]
    public GpuValue<float> JointStrength { set; get; }

    [GpuData("jointPrediction")]
    public GpuValue<float> JointPrediction { set; get; }

    [GpuData("friction")]
    public GpuValue<float> Friction { set; get; }

    [GpuData("staticFriction")]
    public GpuValue<float> StaticFriction { set; get; }

    [GpuData("collisionPower")]
    public GpuValue<float> CollisionPower { set; get; }

    [GpuData("particles")]
    public GpuBuffer<GPParticle> Particles { set; get; }

    [GpuData("transforms")]
    public GpuBuffer<Matrix4x4> Transforms { set; get; }

    [GpuData("oldTransforms")]
    public GpuBuffer<Matrix4x4> OldTransforms { set; get; }

    [GpuData("positions")]
    public GpuBuffer<Vector3> PreCalculatedPositions { set; get; }

    [GpuData("oldPositions")]
    public GpuBuffer<Vector3> OldPreCalculatedPositions { set; get; }

    [GpuData("pointJoints")]
    public GpuBuffer<GPPointJoint> PointJoints { set; get; }

    [GpuData("allPointJoints")]
    public GpuBuffer<GPPointJoint> AllPointJoints { set; get; }

    [GpuData("grabSpheres")]
    public GpuBuffer<GPGrabSphere> GrabSpheres { set; get; }

    [GpuData("processedSpheres")]
    public GpuBuffer<GPSphereWithDelta> ProcessedSpheres { set; get; }

    [GpuData("processedLineSpheres")]
    public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres { set; get; }

    [GpuData("planes")]
    public GpuBuffer<Vector4> Planes { set; get; }

    [GpuData("outParticles")]
    public GpuBuffer<GPParticle> OutParticles { set; get; }

    [GpuData("outParticlesMap")]
    public GpuBuffer<float> OutParticlesMap { set; get; }

    [GpuData("clothVertices")]
    public GpuBuffer<ClothVertex> ClothVertices { get; set; }

    [GpuData("clothOnlyVertices")]
    public GpuBuffer<Vector3> ClothOnlyVertices { get; set; }

    [GpuData("meshToPhysicsVerticesMap")]
    public GpuBuffer<int> MeshToPhysicsVerticesMap { get; set; }

    [GpuData("meshVertexToNeiborsMap")]
    public GpuBuffer<int> MeshVertexToNeiborsMap { get; set; }

    [GpuData("meshVertexToNeiborsMapCounts")]
    public GpuBuffer<int> MeshVertexToNeiborsMapCounts { get; set; }

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
      this.InvDrag.Value = this.data.InvDrag;
      this.Stiffness.Value = this.data.Stiffness;
      this.CompressionResistance.Value = this.data.CompressionResistance;
      this.DistanceScale.Value = this.data.DistanceScale * this.data.WorldScale;
      this.Friction.Value = this.data.Friction;
      this.StaticFriction.Value = this.data.Friction * this.data.StaticMultiplier;
      this.CollisionPower.Value = this.data.CollisionPower;
      this.JointPrediction.Value = this.fixedDispatchCount != 1 ? (this.fixedDispatchCount != 2 ? 1.1f : 1.05f) : 1f;
      this.JointStrength.Value = this.data.JointStrength;
      if (this.data.BreakEnabled && this.frame > 15)
        this.BreakThreshold.Value = this.data.BreakThreshold;
      else
        this.BreakThreshold.Value = 1000000f;
    }

    private void InitBuffers()
    {
      this.usesPreCalcVerts = this.data.MeshProvider.Type == ScalpMeshType.PreCalc;
      this.Particles = this.data.Particles;
      if (this.usesPreCalcVerts)
      {
        this.PreCalculatedPositions = this.data.PreCalculatedVerticesBuffer;
        this.OldPreCalculatedPositions = new GpuBuffer<Vector3>(this.PreCalculatedPositions.Data, 12);
        this.OldPreCalculatedPositions.PushData();
      }
      else
      {
        this.Transforms = this.data.MatricesBuffer;
        this.OldTransforms = new GpuBuffer<Matrix4x4>(this.Transforms.Data, 64);
        this.OldTransforms.PushData();
      }
      this.AllPointJoints = this.data.AllPointJoints;
      this.PointJoints = this.data.PointJoints;
      this.ProcessedSpheres = this.data.ProcessedSpheres;
      this.ProcessedLineSpheres = this.data.ProcessedLineSpheres;
      this.Planes = this.data.Planes;
      this.GrabSpheres = this.data.GrabSpheres;
      this.ClothVertices = this.data.ClothVertices;
      this.ClothOnlyVertices = this.data.ClothOnlyVertices;
      this.MeshToPhysicsVerticesMap = this.data.MeshToPhysicsVerticesMap;
      this.MeshVertexToNeiborsMap = this.data.MeshVertexToNeiborsMap;
      this.MeshVertexToNeiborsMapCounts = this.data.MeshVertexToNeiborsMapCounts;
      this.OutParticles = this.data.OutParticles;
      this.OutParticlesMap = this.data.OutParticlesMap;
    }

    private void UpdateBuffers()
    {
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
        this.particlePlaneCollisionKernel.Planes = this.Planes;
        this.particlePlaneCollisionKernel.ClearCacheAttributes();
      }
      if (this.GrabSpheres != this.data.GrabSpheres)
      {
        this.GrabSpheres = this.data.GrabSpheres;
        this.grabJointsKernel.GrabSpheres = this.GrabSpheres;
        this.grabJointsKernel.ClearCacheAttributes();
      }
      if (!this.usesPreCalcVerts || this.PreCalculatedPositions == this.data.PreCalculatedVerticesBuffer)
        return;
      this.PreCalculatedPositions = this.data.PreCalculatedVerticesBuffer;
      this.vector3CopyPaster.Vector3From = this.PreCalculatedPositions;
      this.vector3CopyPaster.ClearCacheAttributes();
      this.pointJointsPreCalculatedKernel.Positions = this.PreCalculatedPositions;
      this.pointJointsPreCalculatedKernel.ClearCacheAttributes();
      this.pointJointsPreCalculatedFinalKernel.Positions = this.PreCalculatedPositions;
      this.pointJointsPreCalculatedFinalKernel.ClearCacheAttributes();
      this.resetToPointJointsPreCalculatedKernel.Positions = this.PreCalculatedPositions;
      this.resetToPointJointsPreCalculatedKernel.ClearCacheAttributes();
      this.resetToPointJointsPreCalculatedKernel.Dispatch();
    }

    private void InitPasses()
    {
      if (this.usesPreCalcVerts)
        this.AddPass((IPass) (this.resetToPointJointsPreCalculatedKernel = new ResetToPointJointsPreCalculatedKernel()));
      else
        this.AddPass((IPass) (this.resetKernel = new ResetToPointJointsKernel()));
      this.AddPass((IPass) (this.integrateVelocityKernel = new IntegrateVelocityKernel()));
      this.AddPass((IPass) (this.integrateVelocityInnerKernel = new IntegrateVelocityInnerKernel()));
      this.AddPass((IPass) (this.integrateIterKernel = new IntegrateIterKernel()));
      this.AddPass((IPass) (this.stiffnessJointsKernel = new StiffnessJointsKernel(this.data.StiffnessJoints, this.data.StiffnessJointsBuffer)));
      this.AddPass((IPass) (this.nearbyJointsKernel = new StiffnessJointsKernel(this.data.NearbyJoints, this.data.NearbyJointsBuffer)));
      this.AddPass((IPass) (this.particleCollisionResetKernel = new ParticleCollisionResetKernel()));
      this.AddPass((IPass) (this.particlePlaneCollisionKernel = new ParticlePlaneCollisionKernel()));
      this.AddPass((IPass) (this.sphereCollisionKernel = new ParticleSphereCollisionKernel()));
      this.AddPass((IPass) (this.lineSphereCollisionKernel = new ParticleLineSphereCollisionKernel()));
      if (this.data.PointJoints != null)
      {
        if (this.usesPreCalcVerts)
        {
          this.AddPass((IPass) (this.pointJointsPreCalculatedKernel = new PointJointsPreCalculatedKernel()));
          this.AddPass((IPass) (this.pointJointsPreCalculatedFinalKernel = new PointJointsPreCalculatedFinalKernel()));
        }
        else
        {
          this.AddPass((IPass) (this.pointJointsKernel = new PointJointsKernel()));
          this.AddPass((IPass) (this.pointJointsFinalKernel = new PointJointsFinalKernel()));
        }
      }
      this.AddPass((IPass) (this.grabJointsKernel = new GrabJointsKernel()));
      if (this.data.OutParticles != null)
        this.AddPass((IPass) (this.copySpecificParticlesKernel = new CopySpecificParticlesKernel()));
      if (this.usesPreCalcVerts)
      {
        this.AddPass((IPass) (this.createVertexOnlyDataKernel = new CreateVertexOnlyDataKernel()));
        this.AddPass((IPass) (this.vector3CopyPaster = new GPUVector3CopyPaster(this.PreCalculatedPositions, this.OldPreCalculatedPositions)));
      }
      else
      {
        this.AddPass((IPass) (this.createVertexDataKernel = new CreateVertexDataKernel()));
        this.AddPass((IPass) (this.matrixCopyPaster = new GPUMatrixCopyPaster(this.Transforms, this.OldTransforms)));
      }
    }

    public void FixedDispatch()
    {
      ++this.fixedDispatchCount;
      this.DispatchPhysicsImpl();
    }

    public void DispatchCopyToOld()
    {
      if (this.usesPreCalcVerts)
        this.vector3CopyPaster.Dispatch();
      else
        this.matrixCopyPaster.Dispatch();
    }

    public override void Dispatch()
    {
      this.fixedDispatchCount = 0;
      this.DispatchImpl();
    }

    public void Reset() => this.frame = 0;

    public void PartialReset()
    {
      if (this.frame <= 10)
        return;
      this.frame = 10;
    }

    private void DispatchPhysicsImpl()
    {
      this.InitData();
      this.UpdateBuffers();
      if (this.frame < 10)
      {
        if (this.usesPreCalcVerts)
        {
          this.vector3CopyPaster.Dispatch();
          this.resetToPointJointsPreCalculatedKernel.Dispatch();
        }
        else
        {
          this.matrixCopyPaster.Dispatch();
          this.resetKernel.Dispatch();
        }
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
          for (int index2 = 1; index2 <= this.iterations; ++index2)
          {
            this.T.Value = (float) index2 / (float) this.iterations;
            if (this.data.IntegrateEnabled && this.frame > 20)
              this.integrateIterKernel.Dispatch();
            if (this.pointJointsPreCalculatedKernel != null)
              this.pointJointsPreCalculatedKernel.Dispatch();
            else if (this.pointJointsKernel != null)
              this.pointJointsKernel.Dispatch();
            if ((double) this.data.Stiffness > 0.0)
            {
              this.stiffnessJointsKernel.Dispatch();
              this.nearbyJointsKernel.Dispatch();
            }
            this.integrateVelocityInnerKernel.Dispatch();
          }
          this.Step.Value = 1f / (float) this.outerIterations;
          this.DT.Value = num1;
          this.DTRecip.Value = 1f / num1;
          if (this.data.CollisionEnabled)
          {
            this.particleCollisionResetKernel.Dispatch();
            if (this.particleNeiborsCollisionKernel != null)
              this.particleNeiborsCollisionKernel.Dispatch();
            if (this.particlePlaneCollisionKernel.Planes != null)
              this.particlePlaneCollisionKernel.Dispatch();
            if (this.sphereCollisionKernel.ProcessedSpheres != null)
              this.sphereCollisionKernel.Dispatch();
            if (this.lineSphereCollisionKernel.ProcessedLineSpheres != null)
              this.lineSphereCollisionKernel.Dispatch();
            if (this.grabJointsKernel.GrabSpheres != null)
              this.grabJointsKernel.Dispatch();
          }
          this.integrateVelocityKernel.Dispatch();
        }
      }
    }

    private void DispatchImpl()
    {
      this.InitData();
      this.UpdateBuffers();
      this.Step.Value = 1f;
      if (this.frame < 1)
      {
        if (this.usesPreCalcVerts)
        {
          this.vector3CopyPaster.Dispatch();
          this.resetToPointJointsPreCalculatedKernel.Dispatch();
        }
        else
        {
          this.matrixCopyPaster.Dispatch();
          this.resetKernel.Dispatch();
        }
      }
      if (this.pointJointsPreCalculatedFinalKernel != null)
        this.pointJointsPreCalculatedFinalKernel.Dispatch();
      else if (this.pointJointsFinalKernel != null)
        this.pointJointsFinalKernel.Dispatch();
      if (this.copySpecificParticlesKernel != null)
        this.copySpecificParticlesKernel.Dispatch();
      if (this.createVertexOnlyDataKernel != null)
        this.createVertexOnlyDataKernel.Dispatch();
      if (this.createVertexDataKernel != null)
        this.createVertexDataKernel.Dispatch();
      ++this.frame;
    }

    public override void Dispose()
    {
      base.Dispose();
      if (this.usesPreCalcVerts)
        this.OldPreCalculatedPositions.Dispose();
      else
        this.OldTransforms.Dispose();
    }

    public void DebugDraw()
    {
      if (!this.data.DebugDraw)
        return;
      GPDebugDraw.Draw(this.stiffnessJointsKernel.StiffnessJointsBuffer, this.nearbyJointsKernel.StiffnessJointsBuffer, this.Particles, false, true, true);
    }
  }
}
