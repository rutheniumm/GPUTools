// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.BuildRuntimeCloth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Passes;
using GPUTools.Cloth.Scripts.Runtime.Commands;
using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Cloth.Scripts.Runtime.Physics;
using GPUTools.Cloth.Scripts.Runtime.Render;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime
{
  public class BuildRuntimeCloth : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private GameObject obj;
    private ClothRender render;

    public BuildRuntimeCloth(ClothSettings settings)
    {
      this.settings = settings;
      this.physicsBlend = new BuildPhysicsBlend(settings);
      this.Add((IBuildCommand) this.physicsBlend);
      this.Add((IBuildCommand) new BuildPhysicsStrength(settings));
      this.particles = new BuildParticles(settings);
      this.Add((IBuildCommand) this.particles);
      this.Add((IBuildCommand) new BuildPlanes(settings));
      this.spheres = new BuildSpheres(settings);
      this.Add((IBuildCommand) this.spheres);
      this.grabSpheres = new BuildGrabSpheres(settings);
      this.Add((IBuildCommand) this.grabSpheres);
      this.lineSpheres = new BuildLineSpheres(settings);
      this.Add((IBuildCommand) this.lineSpheres);
      this.distanceJoints = new BuildDistanceJoints(settings);
      this.Add((IBuildCommand) this.distanceJoints);
      this.stiffnessJoints = new BuildStiffnessJoints(settings);
      this.Add((IBuildCommand) this.stiffnessJoints);
      this.nearbyJoints = new BuildNearbyJoints(settings);
      this.Add((IBuildCommand) this.nearbyJoints);
      this.pointJoints = new BuildPointJoints(settings);
      this.Add((IBuildCommand) this.pointJoints);
      this.Add((IBuildCommand) new BuildVertexData(settings));
      this.Add((IBuildCommand) new BuildClothAccessories(settings));
    }

    public ClothPhysics physics { get; private set; }

    public BuildParticles particles { get; private set; }

    public BuildSpheres spheres { get; private set; }

    public BuildLineSpheres lineSpheres { get; private set; }

    public BuildPointJoints pointJoints { get; private set; }

    public BuildDistanceJoints distanceJoints { get; private set; }

    public BuildStiffnessJoints stiffnessJoints { get; private set; }

    public BuildNearbyJoints nearbyJoints { get; private set; }

    public BuildPhysicsBlend physicsBlend { get; private set; }

    public BuildGrabSpheres grabSpheres { get; private set; }

    protected override void OnBuild()
    {
      this.obj = this.settings.gameObject;
      this.obj.layer = this.settings.gameObject.layer;
      this.obj.transform.SetParent(this.settings.transform.parent, false);
      ClothDataFacade data = new ClothDataFacade(this.settings);
      this.physics = this.obj.AddComponent<ClothPhysics>();
      this.physics.Initialize(data);
      if (this.settings.MeshProvider.Type == ScalpMeshType.PreCalc)
        return;
      this.render = this.obj.AddComponent<ClothRender>();
      this.render.Initialize(data);
    }

    protected override void OnUpdateSettings() => this.physics.ResetPhysics();

    public void DispatchCopyToOld() => this.physics.DispatchCopyToOld();

    protected override void OnDispatch() => this.physics.Dispatch();

    protected override void OnFixedDispatch() => this.physics.FixedDispatch();
  }
}
