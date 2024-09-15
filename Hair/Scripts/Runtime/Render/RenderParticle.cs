// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Render.RenderParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
  public struct RenderParticle
  {
    public Vector3 Color;
    public float Interpolation;
    public float WavinessScale;
    public float WavinessFrequency;
    public int RootIndex;

    public static int Size() => 28;
  }
}
