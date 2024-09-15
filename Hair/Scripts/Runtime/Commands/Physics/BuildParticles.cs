// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Physics.Scripts.Types.Dynamic;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildParticles : BuildChainCommand
  {
    private readonly HairSettings settings;
    private readonly GeometryProviderBase provider;
    protected GPParticle[] gpuState;

    public BuildParticles(HairSettings settings)
    {
      this.settings = settings;
      this.provider = settings.StandsSettings.Provider;
    }

    protected override void OnBuild()
    {
      GPParticle[] gpParticleArray = new GPParticle[this.provider.GetVertices().Count];
      float[] particleRootToTipRatios = new float[this.provider.GetVertices().Count];
      this.ComputeParticles(gpParticleArray, particleRootToTipRatios);
      if (this.settings.RuntimeData.Particles != null)
        this.settings.RuntimeData.Particles.Dispose();
      this.settings.RuntimeData.Particles = gpParticleArray.Length <= 0 ? (GpuBuffer<GPParticle>) null : new GpuBuffer<GPParticle>(gpParticleArray, GPParticle.Size());
      this.settings.RuntimeData.ParticleRootToTipRatios = particleRootToTipRatios;
    }

    public void UpdateParticleRadius()
    {
      if (this.settings.RuntimeData.Particles == null)
        return;
      GPParticle[] gpParticleArray = (GPParticle[]) this.settings.RuntimeData.Particles.Data.Clone();
      this.settings.RuntimeData.Particles.PullData();
      float num1;
      float num2;
      if (this.settings.PhysicsSettings.StyleMode)
      {
        num1 = this.settings.PhysicsSettings.StyleModeStrandRadius * this.provider.transform.lossyScale.x;
        num2 = !this.settings.PhysicsSettings.UseSeparateRootRadius ? num1 : this.settings.PhysicsSettings.StyleModeStrandRootRadius * this.provider.transform.lossyScale.x;
      }
      else
      {
        num1 = this.settings.PhysicsSettings.StandRadius * this.provider.transform.lossyScale.x;
        num2 = !this.settings.PhysicsSettings.UseSeparateRootRadius ? num1 : this.settings.PhysicsSettings.StandRootRadius * this.provider.transform.lossyScale.x;
      }
      int segments = this.settings.StandsSettings.Segments;
      GPParticle[] data = this.settings.RuntimeData.Particles.Data;
      for (int index = 0; index < data.Length; ++index)
      {
        if (index % segments == 1)
        {
          data[index].Radius = num2;
          gpParticleArray[index].Radius = num2;
        }
        else
        {
          data[index].Radius = num1;
          gpParticleArray[index].Radius = num1;
        }
      }
      this.settings.RuntimeData.Particles.PushData();
      this.settings.RuntimeData.Particles.Data = gpParticleArray;
    }

    public void SaveGPUState()
    {
      if (this.settings.RuntimeData.Particles == null)
        return;
      GPParticle[] gpParticleArray = (GPParticle[]) this.settings.RuntimeData.Particles.Data.Clone();
      this.settings.RuntimeData.Particles.PullData();
      this.gpuState = this.settings.RuntimeData.Particles.Data;
      this.settings.RuntimeData.Particles.Data = gpParticleArray;
    }

    public void RestoreGPUState()
    {
      if (this.settings.RuntimeData.Particles == null || this.gpuState == null)
        return;
      this.settings.RuntimeData.Particles.Data = this.gpuState;
      this.settings.RuntimeData.Particles.PushData();
    }

    protected override void OnUpdateSettings()
    {
      float[] particleRootToTipRatios = new float[this.provider.GetVertices().Count];
      if (this.settings.RuntimeData.Particles == null)
        return;
      this.ComputeParticles(this.settings.RuntimeData.Particles.Data, particleRootToTipRatios);
      this.settings.RuntimeData.Particles.PushData();
      this.settings.RuntimeData.ParticleRootToTipRatios = particleRootToTipRatios;
    }

    private void ComputeParticles(GPParticle[] particles, float[] particleRootToTipRatios)
    {
      Matrix4x4 toWorldMatrix = this.provider.GetToWorldMatrix();
      float radius1 = this.settings.PhysicsSettings.StandRadius * this.provider.transform.lossyScale.x;
      float radius2 = !this.settings.PhysicsSettings.UseSeparateRootRadius ? radius1 : this.settings.PhysicsSettings.StandRootRadius * this.provider.transform.lossyScale.x;
      List<Vector3> vertices = this.provider.GetVertices();
      int segments = this.settings.StandsSettings.Segments;
      float num1 = 0.0f;
      float num2 = 0.0f;
      Vector3 zero = Vector3.zero;
      for (int index = 0; index < vertices.Count; ++index)
      {
        if (index % segments == 0)
        {
          if ((double) num1 > (double) num2)
            num2 = num1;
          num1 = 0.0f;
        }
        else
        {
          float magnitude = (vertices[index] - zero).magnitude;
          num1 += magnitude;
        }
        zero = vertices[index];
      }
      for (int index = 0; index < vertices.Count; ++index)
      {
        int num3 = index % segments;
        if (num3 == 0)
        {
          num1 = 0.0f;
        }
        else
        {
          float magnitude = (vertices[index] - zero).magnitude;
          num1 += magnitude;
        }
        zero = vertices[index];
        float num4 = num1 / num2;
        Vector3 position = toWorldMatrix.MultiplyPoint3x4(vertices[index]);
        switch (num3)
        {
          case 0:
            particles[index] = new GPParticle(position, radius1);
            particles[index].CollisionEnabled = 0;
            break;
          case 1:
            particles[index] = new GPParticle(position, radius2);
            break;
          default:
            particles[index] = new GPParticle(position, radius1);
            break;
        }
        particleRootToTipRatios[index] = num4;
      }
    }

    protected override void OnDispose()
    {
      if (this.settings.RuntimeData.Particles == null)
        return;
      this.settings.RuntimeData.Particles.Dispose();
    }
  }
}
