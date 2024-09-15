// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildEditLineSpheres
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildEditLineSpheres : BuildChainCommand
  {
    private readonly HairSettings settings;

    public BuildEditLineSpheres(HairSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      this.settings.RuntimeData.CutLineSpheres = GPUCollidersManager.cutLineSpheresBuffer;
      this.settings.RuntimeData.GrowLineSpheres = GPUCollidersManager.growLineSpheresBuffer;
      this.settings.RuntimeData.HoldLineSpheres = GPUCollidersManager.holdLineSpheresBuffer;
      this.settings.RuntimeData.GrabLineSpheres = GPUCollidersManager.grabLineSpheresBuffer;
      this.settings.RuntimeData.PushLineSpheres = GPUCollidersManager.pushLineSpheresBuffer;
      this.settings.RuntimeData.PullLineSpheres = GPUCollidersManager.pullLineSpheresBuffer;
      this.settings.RuntimeData.BrushLineSpheres = GPUCollidersManager.brushLineSpheresBuffer;
      this.settings.RuntimeData.RigidityIncreaseLineSpheres = GPUCollidersManager.rigidityIncreaseLineSpheresBuffer;
      this.settings.RuntimeData.RigidityDecreaseLineSpheres = GPUCollidersManager.rigidityDecreaseLineSpheresBuffer;
      this.settings.RuntimeData.RigiditySetLineSpheres = GPUCollidersManager.rigiditySetLineSpheresBuffer;
    }

    protected override void OnFixedDispatch()
    {
      this.settings.RuntimeData.CutLineSpheres = GPUCollidersManager.cutLineSpheresBuffer;
      this.settings.RuntimeData.GrowLineSpheres = GPUCollidersManager.growLineSpheresBuffer;
      this.settings.RuntimeData.HoldLineSpheres = GPUCollidersManager.holdLineSpheresBuffer;
      this.settings.RuntimeData.GrabLineSpheres = GPUCollidersManager.grabLineSpheresBuffer;
      this.settings.RuntimeData.PushLineSpheres = GPUCollidersManager.pushLineSpheresBuffer;
      this.settings.RuntimeData.PullLineSpheres = GPUCollidersManager.pullLineSpheresBuffer;
      this.settings.RuntimeData.BrushLineSpheres = GPUCollidersManager.brushLineSpheresBuffer;
      this.settings.RuntimeData.RigidityIncreaseLineSpheres = GPUCollidersManager.rigidityIncreaseLineSpheresBuffer;
      this.settings.RuntimeData.RigidityDecreaseLineSpheres = GPUCollidersManager.rigidityDecreaseLineSpheresBuffer;
      this.settings.RuntimeData.RigiditySetLineSpheres = GPUCollidersManager.rigiditySetLineSpheresBuffer;
    }
  }
}
