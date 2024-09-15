// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.ClothGeometryImporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Passes;

namespace GPUTools.Cloth.Scripts.Geometry
{
  public class ClothGeometryImporter
  {
    private readonly ClothSettings settings;
    protected CommonJobsPass commonJobsPass;
    protected PhysicsVerticesPass physicsVerticesPass;
    protected NeighborsPass neighborsPass;
    protected JointsPass jointsPass;
    protected StiffnessJointsPass stiffnessJointsPass;
    protected NearbyJointsPass nearbyJointsPass;
    protected bool cancelCacheThreaded;

    public ClothGeometryImporter(ClothSettings settings)
    {
      this.settings = settings;
      this.commonJobsPass = new CommonJobsPass(settings);
      this.physicsVerticesPass = new PhysicsVerticesPass(settings);
      this.neighborsPass = new NeighborsPass(settings);
      this.jointsPass = new JointsPass(settings);
      this.stiffnessJointsPass = new StiffnessJointsPass(settings);
      this.nearbyJointsPass = new NearbyJointsPass(settings);
    }

    public void Cache()
    {
      this.commonJobsPass.Cache();
      this.physicsVerticesPass.Cache();
      this.neighborsPass.Cache();
    }

    public void CancelCacheThreaded()
    {
      this.cancelCacheThreaded = true;
      this.jointsPass.CancelCache();
      this.stiffnessJointsPass.CancelCache();
      this.nearbyJointsPass.CancelCache();
    }

    public void PrepCacheThreaded()
    {
      this.cancelCacheThreaded = false;
      this.jointsPass.PrepCache();
      this.stiffnessJointsPass.PrepCache();
      this.nearbyJointsPass.PrepCache();
    }

    public void CacheThreaded()
    {
      if (!this.cancelCacheThreaded)
        this.jointsPass.Cache();
      if (!this.cancelCacheThreaded)
        this.stiffnessJointsPass.Cache();
      if (!this.cancelCacheThreaded)
        this.nearbyJointsPass.Cache();
      if (this.cancelCacheThreaded || this.settings.GeometryData.Particles == null)
        return;
      this.settings.GeometryData.IsProcessed = true;
    }
  }
}
