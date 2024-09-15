// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Settings.Colors.RootTipColorProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
  [Serializable]
  public class RootTipColorProvider : IColorProvider
  {
    public Color RootColor = new Color(0.35f, 0.15f, 0.15f);
    public Color TipColor = new Color(0.15f, 0.05f, 0.05f);
    public AnimationCurve Blend = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
    public float ColorRolloff = 1f;

    public Color GetColor(HairSettings settings, int x, int y, int sizeY) => this.GetStandColor((float) y / (float) sizeY);

    public Color GetColor(HairSettings settings, float y) => this.GetStandColor(y);

    private Color GetStandColor(float t)
    {
      float p = Mathf.Pow(2f, this.ColorRolloff) - 1f;
      return Color.Lerp(this.TipColor, this.RootColor, Mathf.Pow(1f - t, p));
    }
  }
}
