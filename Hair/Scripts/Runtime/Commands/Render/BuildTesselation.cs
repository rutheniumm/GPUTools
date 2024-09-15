// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Render.BuildTesselation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Render;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
  public class BuildTesselation : IBuildCommand
  {
    private readonly HairSettings settings;

    public BuildTesselation(HairSettings settings) => this.settings = settings;

    public void Build()
    {
      int count = this.settings.StandsSettings.Provider.GetStandsNum() * 64;
      if (this.settings.RuntimeData.TessRenderParticles != null)
        this.settings.RuntimeData.TessRenderParticles.Dispose();
      if (count > 0)
        this.settings.RuntimeData.TessRenderParticles = new GpuBuffer<TessRenderParticle>(count, TessRenderParticle.Size());
      else
        this.settings.RuntimeData.TessRenderParticles = (GpuBuffer<TessRenderParticle>) null;
    }

    public void Dispatch()
    {
    }

    public void FixedDispatch()
    {
    }

    public void UpdateSettings()
    {
    }

    public void Dispose()
    {
      if (this.settings.RuntimeData.TessRenderParticles == null)
        return;
      this.settings.RuntimeData.TessRenderParticles.Dispose();
    }
  }
}
