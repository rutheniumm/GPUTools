// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.ClothSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Geometry.DebugDraw;
using GPUTools.Cloth.Scripts.Geometry.Tools;
using GPUTools.Cloth.Scripts.Runtime;
using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Painter.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts
{
  public class ClothSettings : GPUCollidersConsumer
  {
    [SerializeField]
    public bool GeometryDebugDraw;
    [SerializeField]
    public bool GeometryDebugDrawDistanceJoints = true;
    [SerializeField]
    public bool GeometryDebugDrawStiffnessJoints = true;
    [SerializeField]
    public ClothEditorType EditorType;
    [SerializeField]
    public ClothEditorType EditorStrengthType;
    [SerializeField]
    public Texture2D EditorTexture;
    [SerializeField]
    public Texture2D EditorStrengthTexture;
    [SerializeField]
    public PainterSettings EditorPainter;
    [SerializeField]
    public PainterSettings EditorStrengthPainter;
    [SerializeField]
    public ColorChannel SimulateVsKinematicChannel;
    [SerializeField]
    public ColorChannel StrengthChannel;
    [SerializeField]
    public bool PhysicsDebugDraw;
    [SerializeField]
    public bool IntegrateEnabled = true;
    [SerializeField]
    public Vector3 Gravity = new Vector3(0.0f, -1f, 0.0f);
    [SerializeField]
    public float Drag = 0.06f;
    [SerializeField]
    public float Stretchability;
    [SerializeField]
    public float Stiffness = 0.5f;
    [SerializeField]
    public float DistanceScale = 1f;
    [SerializeField]
    public float WorldScale = 1f;
    [SerializeField]
    public float CompressionResistance = 0.5f;
    [SerializeField]
    public float Weight = 1f;
    [SerializeField]
    public float Friction = 0.5f;
    [SerializeField]
    public float StaticMultiplier = 2f;
    [SerializeField]
    public bool CreateNearbyJoints;
    [SerializeField]
    public float NearbyJointsMaxDistance = 1f / 1000f;
    [SerializeField]
    public bool CollisionEnabled = true;
    [SerializeField]
    public float CollisionPower = 0.5f;
    [SerializeField]
    public float GravityMultiplier = 1f;
    [SerializeField]
    public float ParticleRadius = 0.01f;
    [SerializeField]
    public bool BreakEnabled;
    [SerializeField]
    public float BreakThreshold = 0.005f;
    [SerializeField]
    public float JointStrength = 1f;
    [SerializeField]
    public int Iterations = 3;
    [SerializeField]
    public int InnerIterations = 2;
    [SerializeField]
    public float WindMultiplier;
    [SerializeField]
    public List<GameObject> ColliderProviders = new List<GameObject>();
    [SerializeField]
    public List<GameObject> AccessoriesProviders = new List<GameObject>();
    [SerializeField]
    public MeshProvider MeshProvider = new MeshProvider();
    [SerializeField]
    public ClothSphereBrash Brush = new ClothSphereBrash();
    public bool CustomBounds;
    public Bounds Bounds = new Bounds();
    [SerializeField]
    public ClothGeometryData GeometryData;
    private ClothGeometryImporter geometryImporter;
    private bool _wasInit;

    public Material Material => this.GetComponent<Renderer>().material;

    public Material SharedMaterial => this.GetComponent<Renderer>().sharedMaterial;

    public Material[] Materials => this.GetComponent<Renderer>().materials;

    public Material[] SharedMaterials => this.GetComponent<Renderer>().sharedMaterials;

    public RuntimeData Runtime { private set; get; }

    public BuildRuntimeCloth builder { private set; get; }

    public void ProcessGeometry()
    {
      this.ProcessGeometryMainThread();
      this.ProcessGeometryThreaded();
    }

    public void ProcessGeometryMainThread()
    {
      this.GeometryData = new ClothGeometryData();
      this.geometryImporter = new ClothGeometryImporter(this);
      this.geometryImporter.Cache();
    }

    public void CancelProcessGeometryThreaded()
    {
      if (this.geometryImporter == null)
        return;
      this.geometryImporter.CancelCacheThreaded();
    }

    public void ProcessGeometryThreaded()
    {
      if (this.geometryImporter != null)
        this.geometryImporter.CacheThreaded();
      this.GeometryData.LogStatistics();
    }

    private void Init()
    {
      if (this._wasInit || !this.MeshProvider.Validate(false) || this.GeometryData == null || !this.GeometryData.IsProcessed)
        return;
      this.MeshProvider.Dispatch();
      if (this.Runtime == null)
        this.Runtime = new RuntimeData();
      if (this.builder == null)
        this.builder = new BuildRuntimeCloth(this);
      this.builder.Build();
      this._wasInit = true;
    }

    public void Reset()
    {
      if (this.builder == null)
        return;
      if (this.builder.particles != null)
        this.builder.particles.UpdateSettings();
      if (this.builder.distanceJoints != null)
        this.builder.distanceJoints.UpdateSettings();
      if (this.builder.pointJoints == null)
        return;
      this.builder.pointJoints.UpdateSettings();
    }

    private void Start() => this.Init();

    private void FixedUpdate()
    {
      if (!this._wasInit)
        return;
      this.builder.FixedDispatch();
    }

    private void LateUpdate()
    {
      this.Init();
      if (!this._wasInit)
        return;
      this.builder.DispatchCopyToOld();
      this.MeshProvider.Dispatch();
      this.builder.Dispatch();
      if (this.MeshProvider.Type != ScalpMeshType.PreCalc || !((Object) this.MeshProvider.PreCalcProvider != (Object) null))
        return;
      this.MeshProvider.PreCalcProvider.PostProcessDispatch(this.Runtime.ClothOnlyVertices.ComputeBuffer);
    }

    private void OnDestroy()
    {
      this.MeshProvider.Dispose();
      if (this.builder == null)
        return;
      this.builder.Dispose();
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.MeshProvider.Stop();
    }

    public void UpdateSettings()
    {
      if (!Application.isPlaying || this.builder == null)
        return;
      this.builder.UpdateSettings();
    }

    public void OnDrawGizmos()
    {
      if (this.GeometryDebugDraw)
        ClothDebugDraw.Draw(this);
      ClothDebugDraw.DrawAlways(this);
    }
  }
}
