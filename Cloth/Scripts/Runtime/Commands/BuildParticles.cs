// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildParticles : BuildChainCommand
  {
    private readonly ClothSettings settings;

    public BuildParticles(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      GPParticle[] gpParticleArray = new GPParticle[this.settings.GeometryData.Particles.Length];
      this.ComputeParticles(gpParticleArray);
      this.settings.Runtime.Particles = new GpuBuffer<GPParticle>(gpParticleArray, GPParticle.Size());
    }

    protected override void OnUpdateSettings()
    {
      this.ComputeParticles(this.settings.Runtime.Particles.Data);
      this.settings.Runtime.Particles.PushData();
      this.settings.builder.physics.ResetPhysics();
    }

    private void ComputeParticles(GPParticle[] particles)
    {
      Vector3[] particles1 = this.settings.GeometryData.Particles;
      Matrix4x4 toWorldMatrix = this.settings.MeshProvider.ToWorldMatrix;
      int[] toMeshVerticesMap = this.settings.GeometryData.PhysicsToMeshVerticesMap;
      float[] particlesStrength = this.settings.GeometryData.ParticlesStrength;
      float radius = this.settings.ParticleRadius * this.settings.transform.lossyScale.x;
      if (particles == null)
        particles = new GPParticle[particles1.Length];
      for (int index1 = 0; index1 < particles1.Length; ++index1)
      {
        Vector3 position = toWorldMatrix.MultiplyPoint3x4(particles1[index1]);
        int index2 = toMeshVerticesMap[index1];
        particles[index1] = new GPParticle(position, radius);
        float num = Mathf.Max(0.1f, particlesStrength[index2]);
        particles[index1].Strength = num;
      }
    }

    protected override void OnDispose() => this.settings.Runtime.Particles.Dispose();
  }
}
