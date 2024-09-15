// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Render.BuildParticlesData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Hair.Scripts.Settings;
using GPUTools.Hair.Scripts.Settings.Colors;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
  public class BuildParticlesData : IBuildCommand
  {
    private readonly HairSettings settings;

    public BuildParticlesData(HairSettings settings) => this.settings = settings;

    public void Build()
    {
      RenderParticle[] renderParticleArray = this.settings.RuntimeData.Particles == null ? new RenderParticle[0] : new RenderParticle[this.settings.RuntimeData.Particles.Count];
      this.UpdateBodies(renderParticleArray);
      List<Vector3> vector3List = this.GenRandoms();
      if (this.settings.RuntimeData.RenderParticles != null)
        this.settings.RuntimeData.RenderParticles.Dispose();
      if (this.settings.RuntimeData.RandomsPerStrand != null)
        this.settings.RuntimeData.RandomsPerStrand.Dispose();
      if (renderParticleArray.Length > 0)
      {
        this.settings.RuntimeData.RenderParticles = new GpuBuffer<RenderParticle>(renderParticleArray, RenderParticle.Size());
        this.settings.RuntimeData.RandomsPerStrand = new GpuBuffer<Vector3>(vector3List.ToArray(), 12);
      }
      else
      {
        this.settings.RuntimeData.RenderParticles = (GpuBuffer<RenderParticle>) null;
        this.settings.RuntimeData.RandomsPerStrand = (GpuBuffer<Vector3>) null;
      }
    }

    public void UpdateSettings()
    {
      if (this.settings.RuntimeData.RenderParticles == null)
        return;
      this.UpdateBodies(this.settings.RuntimeData.RenderParticles.Data);
      this.settings.RuntimeData.RenderParticles.PushData();
    }

    private List<Vector3> GenRandoms()
    {
      List<Vector3> vector3List = new List<Vector3>();
      Random.InitState(5);
      for (int index = 0; index < this.settings.StandsSettings.Provider.GetStandsNum(); ++index)
      {
        Vector3 vector3;
        vector3.x = Random.Range(0.0f, 1f);
        vector3.y = Random.Range(0.0f, 1f);
        vector3.z = Random.Range(0.0f, 1f);
        vector3List.Add(vector3);
      }
      return vector3List;
    }

    private void UpdateBodies(RenderParticle[] renderParticles)
    {
      HairRenderSettings renderSettings = this.settings.RenderSettings;
      int segmentsNum = this.settings.StandsSettings.Provider.GetSegmentsNum();
      RootTipColorProvider tipColorProvider = (RootTipColorProvider) null;
      if (renderSettings.ColorProvider is RootTipColorProvider)
        tipColorProvider = renderSettings.ColorProvider as RootTipColorProvider;
      float[] particleRootToTipRatios = this.settings.RuntimeData.ParticleRootToTipRatios;
      float num1 = Mathf.Clamp(renderSettings.InterpolationMidpoint, 1f / 1000f, 1f);
      float num2 = Mathf.Clamp(renderSettings.WavinessMidpoint, 1f / 1000f, 1f);
      for (int index = 0; index < renderParticles.Length; ++index)
      {
        int x = index / segmentsNum;
        int y1 = index % segmentsNum;
        float time = (float) y1 / (float) (segmentsNum - 1);
        Vector3 vector;
        if (tipColorProvider != null)
        {
          float y2 = particleRootToTipRatios[index];
          vector = this.ColorToVector(tipColorProvider.GetColor(this.settings, y2));
        }
        else
          vector = this.ColorToVector(renderSettings.ColorProvider.GetColor(this.settings, x, y1, segmentsNum));
        float num3;
        if (this.settings.PhysicsSettings.StyleMode)
          num3 = 0.0f;
        else if (renderSettings.UseInterpolationCurves)
          num3 = Mathf.Clamp01(renderSettings.InterpolationCurve.Evaluate(time));
        else if ((double) time <= (double) renderSettings.InterpolationMidpoint)
        {
          float t = Mathf.Pow(1f - time / num1, renderSettings.InterpolationCurvePower);
          num3 = 1f - Mathf.Lerp(renderSettings.InterpolationMid, renderSettings.InterpolationRoot, t);
        }
        else
        {
          float t = Mathf.Pow((float) (((double) time - (double) num1) / (1.0 - (double) num1)), renderSettings.InterpolationCurvePower);
          num3 = 1f - Mathf.Lerp(renderSettings.InterpolationMid, renderSettings.InterpolationTip, t);
        }
        float num4;
        float num5;
        if (this.settings.PhysicsSettings.StyleMode)
        {
          num4 = 0.0f;
          num5 = 1f;
        }
        else if (renderSettings.UseWavinessCurves)
        {
          num4 = Mathf.Clamp01(renderSettings.WavinessScaleCurve.Evaluate(time));
          num5 = Mathf.Clamp01(renderSettings.WavinessFrequencyCurve.Evaluate(time));
        }
        else
        {
          if ((double) time <= (double) renderSettings.WavinessMidpoint)
          {
            float t = Mathf.Pow(1f - time / num2, renderSettings.WavinessCurvePower);
            num4 = Mathf.Lerp(renderSettings.WavinessMid, renderSettings.WavinessRoot, t);
          }
          else
          {
            float t = Mathf.Pow((float) (((double) time - (double) num2) / (1.0 - (double) num2)), renderSettings.WavinessCurvePower);
            num4 = Mathf.Lerp(renderSettings.WavinessMid, renderSettings.WavinessTip, t);
          }
          num5 = 1f;
        }
        RenderParticle renderParticle = new RenderParticle()
        {
          RootIndex = x,
          Color = vector,
          Interpolation = num3,
          WavinessScale = num4 * renderSettings.WavinessScale,
          WavinessFrequency = num5 * renderSettings.WavinessFrequency
        };
        renderParticles[index] = renderParticle;
      }
    }

    public Vector3 ColorToVector(Color color) => new Vector3(color.r, color.g, color.b);

    public void Dispatch()
    {
    }

    public void FixedDispatch()
    {
    }

    public void Dispose()
    {
      if (this.settings.RuntimeData.RenderParticles != null)
        this.settings.RuntimeData.RenderParticles.Dispose();
      if (this.settings.RuntimeData.RandomsPerStrand == null)
        return;
      this.settings.RuntimeData.RandomsPerStrand.Dispose();
    }
  }
}
