// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Wind.NoiseOctave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;

namespace GPUTools.Physics.Scripts.Wind
{
  [Serializable]
  public struct NoiseOctave
  {
    public float Scale;
    public float Amplitude;

    public NoiseOctave(float scale, float amplitude)
    {
      this.Scale = scale;
      this.Amplitude = amplitude;
    }
  }
}
