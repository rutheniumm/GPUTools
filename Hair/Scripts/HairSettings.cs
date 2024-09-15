// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.HairSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Hair.Scripts.Runtime.Commands;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Settings;
using UnityEngine;

namespace GPUTools.Hair.Scripts
{
  public class HairSettings : GPUCollidersConsumer
  {
    public HairStandsSettings StandsSettings = new HairStandsSettings();
    public HairPhysicsSettings PhysicsSettings = new HairPhysicsSettings();
    public HairRenderSettings RenderSettings = new HairRenderSettings();
    public HairLODSettings LODSettings = new HairLODSettings();
    public HairShadowSettings ShadowSettings = new HairShadowSettings();
    protected bool consumerRegistered;

    public RuntimeData RuntimeData { private set; get; }

    public BuildRuntimeHair HairBuidCommand { private set; get; }

    private void Awake()
    {
      if (!this.ValidateImpl())
        return;
      this.RuntimeData = new RuntimeData();
      this.HairBuidCommand = new BuildRuntimeHair(this);
      this.HairBuidCommand.Build();
    }

    public void ReStart()
    {
      if (!this.ValidateImpl())
        return;
      if (this.HairBuidCommand != null)
        this.HairBuidCommand.Dispose();
      this.Awake();
    }

    private void FixedUpdate()
    {
      if (this.HairBuidCommand == null)
        return;
      this.SyncConsumer();
      this.HairBuidCommand.FixedDispatch();
    }

    private void LateUpdate()
    {
      if (this.HairBuidCommand == null)
        return;
      this.StandsSettings.Provider.Dispatch();
      this.HairBuidCommand.Dispatch();
    }

    public void UpdateSettings()
    {
      if (this.HairBuidCommand == null || !Application.isPlaying)
        return;
      this.HairBuidCommand.UpdateSettings();
    }

    public void OnDestroy()
    {
      if (this.HairBuidCommand == null)
        return;
      this.HairBuidCommand.Dispose();
    }

    private bool ValidateImpl() => this.StandsSettings.Validate() && this.PhysicsSettings.Validate() && this.RenderSettings.Validate() && this.LODSettings.Validate() && this.ShadowSettings.Validate();

    private void OnDrawGizmos()
    {
      this.StandsSettings.DrawGizmos();
      this.PhysicsSettings.DrawGizmos();
      this.RenderSettings.DrawGizmos();
      this.LODSettings.DrawGizmos();
      this.ShadowSettings.DrawGizmos();
    }

    protected void SyncConsumer()
    {
      if (this.PhysicsSettings.IsEnabled && !this.consumerRegistered)
      {
        GPUCollidersManager.RegisterConsumer((GPUCollidersConsumer) this);
        this.consumerRegistered = true;
      }
      else
      {
        if (this.PhysicsSettings.IsEnabled || !this.consumerRegistered)
          return;
        GPUCollidersManager.DeregisterConsumer((GPUCollidersConsumer) this);
        this.consumerRegistered = false;
      }
    }

    protected override void OnEnable()
    {
      if (!Application.isPlaying)
        return;
      this.SyncConsumer();
    }

    protected override void OnDisable()
    {
      if (!Application.isPlaying || !this.consumerRegistered)
        return;
      GPUCollidersManager.DeregisterConsumer((GPUCollidersConsumer) this);
      this.consumerRegistered = false;
    }
  }
}
