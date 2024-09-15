// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Ranges.FloatRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Ranges
{
  [Serializable]
  public struct FloatRange
  {
    public float Min;
    public float Max;

    public FloatRange(float min, float max)
    {
      this.Min = min;
      this.Max = max;
    }

    public float GetRandom() => UnityEngine.Random.Range(this.Min, this.Max);

    public float GetLerp(float t) => Mathf.Lerp(this.Min, this.Max, t);
  }
}
