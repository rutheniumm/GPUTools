// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildWind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Wind;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildWind : BuildChainCommand
  {
    private readonly HairSettings settings;
    private readonly WindReceiver wind;

    public BuildWind(HairSettings settings)
    {
      this.settings = settings;
      this.wind = new WindReceiver();
    }

    protected override void OnDispatch() => this.settings.RuntimeData.Wind = this.wind.GetWind(this.settings.StandsSettings.HeadCenterWorld) * this.settings.PhysicsSettings.WindMultiplier;
  }
}
