// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildWind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Wind;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildWind : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private readonly WindReceiver receiver;

    public BuildWind(ClothSettings settings)
    {
      this.settings = settings;
      this.receiver = new WindReceiver();
    }

    protected override void OnDispatch() => this.settings.Runtime.Wind = this.receiver.GetWind(this.settings.transform.position) * this.settings.WindMultiplier;
  }
}
