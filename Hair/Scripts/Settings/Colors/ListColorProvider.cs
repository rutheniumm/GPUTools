// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Settings.Colors.ListColorProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
  [Serializable]
  public class ListColorProvider : IColorProvider
  {
    public List<Color> Colors = new List<Color>();

    public Color GetColor(HairSettings settings, int x, int y, int sizeY) => this.GetStandColor((float) y / (float) sizeY);

    private Color GetStandColor(float t) => this.Colors.Count == 0 ? Color.black : this.Colors[(int) Mathf.Clamp((float) this.Colors.Count * t, 0.0f, (float) (this.Colors.Count - 1))];
  }
}
