// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.BuildRuntimeHair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Commands.Physics;
using GPUTools.Hair.Scripts.Runtime.Commands.Render;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Runtime.Physics;
using GPUTools.Hair.Scripts.Runtime.Render;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands
{
  public class BuildRuntimeHair : BuildChainCommand
  {
    private readonly HairSettings settings;
    private GameObject obj;

    public BuildRuntimeHair(HairSettings settings)
    {
      this.settings = settings;
      this.particles = new BuildParticles(settings);
      this.Add((IBuildCommand) this.particles);
      this.planes = new BuildPlanes(settings);
      this.Add((IBuildCommand) this.planes);
      this.spheres = new BuildSpheres(settings);
      this.Add((IBuildCommand) this.spheres);
      this.lineSpheres = new BuildLineSpheres(settings);
      this.Add((IBuildCommand) this.lineSpheres);
      this.editLineSpheres = new BuildEditLineSpheres(settings);
      this.Add((IBuildCommand) this.editLineSpheres);
      this.distanceJoints = new BuildDistanceJoints(settings);
      this.Add((IBuildCommand) this.distanceJoints);
      this.compressionJoints = new BuildCompressionJoints(settings);
      this.Add((IBuildCommand) this.compressionJoints);
      this.nearbyDistanceJoints = new BuildNearbyDistanceJoints(settings);
      this.Add((IBuildCommand) this.nearbyDistanceJoints);
      this.pointJoints = new BuildPointJoints(settings);
      this.Add((IBuildCommand) this.pointJoints);
      this.Add((IBuildCommand) new BuildAccessories(settings));
      this.barycentrics = new BuildBarycentrics(settings);
      this.Add((IBuildCommand) this.barycentrics);
      this.particlesData = new BuildParticlesData(settings);
      this.Add((IBuildCommand) this.particlesData);
      this.tesselation = new BuildTesselation(settings);
      this.Add((IBuildCommand) this.tesselation);
    }

    public GPHairPhysics physics { get; private set; }

    public HairRender render { get; private set; }

    public BuildParticles particles { get; private set; }

    public BuildPlanes planes { get; private set; }

    public BuildSpheres spheres { get; private set; }

    public BuildLineSpheres lineSpheres { get; private set; }

    public BuildEditLineSpheres editLineSpheres { get; private set; }

    public BuildPointJoints pointJoints { get; private set; }

    public BuildParticlesData particlesData { get; private set; }

    public BuildDistanceJoints distanceJoints { get; private set; }

    public BuildCompressionJoints compressionJoints { get; private set; }

    public BuildNearbyDistanceJoints nearbyDistanceJoints { get; private set; }

    public BuildTesselation tesselation { get; private set; }

    public BuildBarycentrics barycentrics { get; private set; }

    public void RebuildHair()
    {
      this.particles.Build();
      this.distanceJoints.Build();
      this.compressionJoints.Build();
      this.nearbyDistanceJoints.Build();
      this.pointJoints.Build();
      this.barycentrics.Build();
      this.particlesData.Build();
      this.tesselation.Build();
      this.render.InitializeMesh();
      this.physics.RebindData();
      this.physics.ResetPhysics();
    }

    protected override void OnBuild()
    {
      this.obj = new GameObject("Render");
      this.obj.layer = this.settings.gameObject.layer;
      this.obj.transform.SetParent(this.settings.transform, false);
      HairDataFacade data = new HairDataFacade(this.settings);
      this.physics = this.obj.AddComponent<GPHairPhysics>();
      this.physics.Initialize(data);
      this.render = this.obj.AddComponent<HairRender>();
      this.render.Initialize(data);
    }

    protected override void OnDispatch()
    {
      this.physics.Dispatch();
      this.render.Dispatch();
    }

    protected override void OnFixedDispatch() => this.physics.FixedDispatch();

    protected override void OnDispose() => Object.Destroy((Object) this.obj);

    public bool IsVisible => this.render.IsVisible;
  }
}
