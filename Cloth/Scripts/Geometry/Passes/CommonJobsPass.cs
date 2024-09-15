// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.CommonJobsPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class CommonJobsPass : ICacheCommand
  {
    private readonly ClothSettings settings;

    public CommonJobsPass(ClothSettings settings) => this.settings = settings;

    public void Cache()
    {
      this.settings.GeometryData.ParticlesBlend = Enumerable.Repeat<float>(0.0f, this.settings.MeshProvider.MeshForImport.vertexCount).ToArray<float>();
      this.settings.GeometryData.ParticlesStrength = Enumerable.Repeat<float>(0.1f, this.settings.MeshProvider.MeshForImport.vertexCount).ToArray<float>();
      this.settings.GeometryData.AllTringles = this.GetAllTriangles();
    }

    private int[] GetAllTriangles()
    {
      Mesh meshForImport = this.settings.MeshProvider.MeshForImport;
      List<int> intList = new List<int>();
      for (int submesh = 0; submesh < meshForImport.subMeshCount; ++submesh)
        intList.AddRange((IEnumerable<int>) meshForImport.GetTriangles(submesh));
      return intList.ToArray();
    }
  }
}
