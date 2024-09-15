// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildLineSpheres
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildLineSpheres : BuildChainCommand
  {
    private readonly HairSettings settings;

    public BuildLineSpheres(HairSettings settings) => this.settings = settings;

    protected override void OnBuild() => this.settings.RuntimeData.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;

    protected override void OnFixedDispatch() => this.settings.RuntimeData.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
  }
}
