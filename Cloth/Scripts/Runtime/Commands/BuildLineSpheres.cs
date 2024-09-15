// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildLineSpheres
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildLineSpheres : BuildChainCommand
  {
    private readonly ClothSettings settings;

    public BuildLineSpheres(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild() => this.settings.Runtime.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;

    protected override void OnFixedDispatch() => this.settings.Runtime.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
  }
}
