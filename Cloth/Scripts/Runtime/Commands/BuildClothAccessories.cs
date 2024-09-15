// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildClothAccessories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildClothAccessories : BuildChainCommand
  {
    private readonly ClothSettings settings;
    private CacheProvider<SphereCollider> sphereCollidersCache;

    public BuildClothAccessories(ClothSettings settings)
    {
      this.settings = settings;
      this.sphereCollidersCache = new CacheProvider<SphereCollider>(settings.AccessoriesProviders);
    }

    protected override void OnBuild()
    {
      if (this.sphereCollidersCache.Items.Count == 0)
        return;
      this.settings.Runtime.OutParticles = new GpuBuffer<GPParticle>(new GPParticle[this.sphereCollidersCache.Items.Count], GPParticle.Size());
      float[] numArray = new float[this.sphereCollidersCache.Items.Count];
      this.CalculateOutParticlesMap(numArray);
      this.settings.Runtime.OutParticlesMap = new GpuBuffer<float>(numArray, 4);
    }

    protected override void OnDispatch()
    {
      if (this.settings.Runtime.OutParticles == null)
        return;
      this.settings.Runtime.OutParticles.PullData();
      for (int index = 0; index < this.settings.Runtime.OutParticles.Data.Length; ++index)
      {
        GPParticle gpParticle = this.settings.Runtime.OutParticles.Data[index];
        this.sphereCollidersCache.Items[index].transform.position = gpParticle.Position;
      }
    }

    private void CalculateOutParticlesMap(float[] outParticlesMap)
    {
      float[] numArray = new float[this.sphereCollidersCache.Items.Count];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = float.PositiveInfinity;
      GPParticle[] data = this.settings.Runtime.Particles.Data;
      for (int index1 = 0; index1 < data.Length; ++index1)
      {
        GPParticle gpParticle = data[index1];
        for (int index2 = 0; index2 < this.sphereCollidersCache.Items.Count; ++index2)
        {
          SphereCollider sphereCollider = this.sphereCollidersCache.Items[index2];
          float num = Vector3.Distance(sphereCollider.transform.position, gpParticle.Position);
          if ((double) num < (double) sphereCollider.radius && (double) num < (double) numArray[index2])
          {
            numArray[index2] = num;
            outParticlesMap[index2] = (float) index1;
          }
        }
      }
    }

    protected override void OnDispose()
    {
      if (this.settings.Runtime.OutParticles != null)
        this.settings.Runtime.OutParticles.Dispose();
      if (this.settings.Runtime.OutParticlesMap == null)
        return;
      this.settings.Runtime.OutParticlesMap.Dispose();
    }
  }
}
