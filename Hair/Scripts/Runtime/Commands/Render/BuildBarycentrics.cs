// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Render.BuildBarycentrics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
  public class BuildBarycentrics : IBuildCommand
  {
    public static int MaxCount = 64;
    private readonly HairSettings settings;
    private List<Vector3> barycentric = new List<Vector3>();
    private List<Vector3> barycentricFixed = new List<Vector3>();

    public BuildBarycentrics(HairSettings settings) => this.settings = settings;

    public void Build()
    {
      this.Gen();
      if (this.settings.RuntimeData.Barycentrics != null)
        this.settings.RuntimeData.Barycentrics.Dispose();
      this.settings.RuntimeData.Barycentrics = this.barycentric.Count <= 0 ? (GpuBuffer<Vector3>) null : new GpuBuffer<Vector3>(this.barycentric.ToArray(), 12);
      if (this.settings.RuntimeData.BarycentricsFixed != null)
        this.settings.RuntimeData.BarycentricsFixed.Dispose();
      if (this.barycentricFixed.Count > 0)
        this.settings.RuntimeData.BarycentricsFixed = new GpuBuffer<Vector3>(this.barycentricFixed.ToArray(), 12);
      else
        this.settings.RuntimeData.BarycentricsFixed = (GpuBuffer<Vector3>) null;
    }

    public void Dispatch()
    {
    }

    public void FixedDispatch()
    {
    }

    public void UpdateSettings()
    {
      this.Gen();
      if (this.settings.RuntimeData.Barycentrics != null)
        this.settings.RuntimeData.Barycentrics.PushData();
      if (this.settings.RuntimeData.BarycentricsFixed == null)
        return;
      this.settings.RuntimeData.BarycentricsFixed.PushData();
    }

    public void Dispose()
    {
      if (this.settings.RuntimeData.Barycentrics != null)
        this.settings.RuntimeData.Barycentrics.Dispose();
      if (this.settings.RuntimeData.BarycentricsFixed == null)
        return;
      this.settings.RuntimeData.BarycentricsFixed.Dispose();
    }

    private void Gen()
    {
      Random.InitState(6);
      this.barycentric = new List<Vector3>();
      for (int index1 = 0; index1 < this.settings.StandsSettings.Provider.GetStandsNum(); ++index1)
      {
        for (int index2 = 0; index2 < BuildBarycentrics.MaxCount; ++index2)
          this.barycentric.Add(this.GetRandomK());
      }
      this.barycentricFixed = new List<Vector3>();
      for (int index3 = 0; index3 < this.settings.StandsSettings.Provider.GetStandsNum(); ++index3)
      {
        for (int index4 = 0; index4 < BuildBarycentrics.MaxCount; index4 += 3)
        {
          this.barycentricFixed.Add(new Vector3(0.99f, 0.005f, 0.005f));
          this.barycentricFixed.Add(new Vector3(0.005f, 0.99f, 0.005f));
          this.barycentricFixed.Add(new Vector3(0.005f, 0.005f, 0.99f));
        }
      }
      this.barycentricFixed = this.barycentricFixed.GetRange(0, BuildBarycentrics.MaxCount * this.settings.StandsSettings.Provider.GetStandsNum());
    }

    private void Split(Vector3 b1, Vector3 b2, Vector3 b3, int steps)
    {
      --steps;
      this.TryAdd(b1);
      this.TryAdd(b2);
      this.TryAdd(b3);
      Vector3 vector3_1 = (b1 + b2) * 0.5f;
      Vector3 vector3_2 = (b2 + b3) * 0.5f;
      Vector3 b3_1 = (b3 + b1) * 0.5f;
      if (steps < 0)
        return;
      this.Split(b1, vector3_1, b3_1, steps);
      this.Split(b2, vector3_1, vector3_2, steps);
      this.Split(b3, vector3_2, b3_1, steps);
      this.Split(vector3_1, vector3_2, b3_1, steps);
    }

    private void TryAdd(Vector3 v)
    {
      if (this.barycentric.Contains(v))
        return;
      this.barycentric.Add(v);
    }

    private Vector3 GetRandomK()
    {
      float x = Random.Range(0.0f, 1f);
      float y = Random.Range(0.0f, 1f);
      if ((double) x + (double) y > 1.0)
      {
        x = 1f - x;
        y = 1f - y;
      }
      float z = (float) (1.0 - ((double) x + (double) y));
      return new Vector3(x, y, z);
    }
  }
}
