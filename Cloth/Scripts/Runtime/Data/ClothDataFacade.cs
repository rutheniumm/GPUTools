// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Data.ClothDataFacade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Data
{
  public class ClothDataFacade
  {
    private readonly ClothSettings settings;

    public ClothDataFacade(ClothSettings settings) => this.settings = settings;

    public MeshProvider MeshProvider => this.settings.MeshProvider;

    public bool DebugDraw => this.settings.PhysicsDebugDraw;

    public int Iterations => this.settings.Iterations;

    public int InnerIterations => this.settings.InnerIterations;

    public float Drag => this.settings.Drag;

    public float InvDrag => 1f - this.settings.Drag * (1f / (float) this.Iterations);

    public bool IntegrateEnabled => this.settings.IntegrateEnabled;

    public Vector3 Gravity => UnityEngine.Physics.gravity * this.settings.GravityMultiplier;

    public float GravityMultiplier => this.settings.GravityMultiplier;

    public Vector3 Wind => this.settings.Runtime.Wind;

    public float Stretchability => this.settings.Stretchability;

    public float Stiffness => this.settings.Stiffness;

    public float CompressionResistance => this.settings.CompressionResistance;

    public float WorldScale => this.settings.WorldScale;

    public float DistanceScale => this.settings.DistanceScale;

    public bool BreakEnabled => this.settings.BreakEnabled;

    public float BreakThreshold => this.settings.BreakThreshold;

    public float JointStrength => this.settings.JointStrength;

    public float Weight => this.settings.Weight;

    public float Friction => this.settings.Friction;

    public float StaticMultiplier => this.settings.StaticMultiplier;

    public bool CollisionEnabled => this.settings.CollisionEnabled;

    public float CollisionPower => this.settings.CollisionPower;

    public GpuBuffer<Matrix4x4> MatricesBuffer => this.settings.MeshProvider.ToWorldMatricesBuffer;

    public GpuBuffer<Vector3> PreCalculatedVerticesBuffer => this.settings.MeshProvider.PreCalculatedVerticesBuffer;

    public GpuBuffer<GPParticle> Particles => this.settings.Runtime.Particles;

    public GpuBuffer<GPSphereWithDelta> ProcessedSpheres => this.settings.Runtime.ProcessedSpheres;

    public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres => this.settings.Runtime.ProcessedLineSpheres;

    public GpuBuffer<Vector4> Planes => this.settings.Runtime.Planes;

    public GpuBuffer<GPGrabSphere> GrabSpheres => this.settings.Runtime.GrabSpheres;

    public GroupedData<GPDistanceJoint> DistanceJoints => this.settings.Runtime.DistanceJoints;

    public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer => this.settings.Runtime.DistanceJointsBuffer;

    public GroupedData<GPDistanceJoint> StiffnessJoints => this.settings.Runtime.StiffnessJoints;

    public GpuBuffer<GPDistanceJoint> StiffnessJointsBuffer => this.settings.Runtime.StiffnessJointsBuffer;

    public GroupedData<GPDistanceJoint> NearbyJoints => this.settings.Runtime.NearbyJoints;

    public GpuBuffer<GPDistanceJoint> NearbyJointsBuffer => this.settings.Runtime.NearbyJointsBuffer;

    public GpuBuffer<GPPointJoint> PointJoints => this.settings.Runtime.PointJoints;

    public GpuBuffer<GPPointJoint> AllPointJoints => this.settings.Runtime.AllPointJoints;

    public GpuBuffer<ClothVertex> ClothVertices => this.settings.Runtime.ClothVertices;

    public GpuBuffer<Vector3> ClothOnlyVertices => this.settings.Runtime.ClothOnlyVertices;

    public GpuBuffer<int> MeshVertexToNeiborsMap => this.settings.Runtime.MeshVertexToNeiborsMap;

    public GpuBuffer<int> MeshVertexToNeiborsMapCounts => this.settings.Runtime.MeshVertexToNeiborsMapCounts;

    public GpuBuffer<int> MeshToPhysicsVerticesMap => this.settings.Runtime.MeshToPhysicsVerticesMap;

    public GpuBuffer<GPParticle> OutParticles => this.settings.Runtime.OutParticles;

    public GpuBuffer<float> OutParticlesMap => this.settings.Runtime.OutParticlesMap;

    public Material Material => this.settings.Material;

    public Material[] Materials => this.settings.Materials;

    public Bounds Bounds => this.settings.Bounds;

    public bool CustomBounds => this.settings.CustomBounds;
  }
}
