// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Render.TessRenderParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
  public struct TessRenderParticle
  {
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 LightCenter;
    public Vector3 Color;
    public float Interpolation;
    public int RootIndex;

    public static int Size() => 56;
  }
}
