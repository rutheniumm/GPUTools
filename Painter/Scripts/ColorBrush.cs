// Decompiled with JetBrains decompiler
// Type: GPUTools.Painter.Scripts.ColorBrush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace GPUTools.Painter.Scripts
{
  [Serializable]
  public class ColorBrush
  {
    [SerializeField]
    public Color Color = new Color(0.95f, 0.0f, 0.0f);
    [SerializeField]
    public float Radius = 0.02f;
    [SerializeField]
    public float Strength = 1f;
    [SerializeField]
    public ColorChannel Channel;

    public Color CurrentDrawColor
    {
      get
      {
        if (this.Channel == ColorChannel.R)
          return Color.red;
        if (this.Channel == ColorChannel.G)
          return Color.green;
        return this.Channel == ColorChannel.B ? Color.blue : Color.white;
      }
    }

    public float CurrentChannelValue
    {
      get
      {
        if (this.Channel == ColorChannel.R)
          return this.Color.r;
        if (this.Channel == ColorChannel.G)
          return this.Color.g;
        return this.Channel == ColorChannel.B ? this.Color.b : this.Color.a;
      }
      set
      {
        if (this.Channel == ColorChannel.R)
          this.Color.r = value;
        if (this.Channel == ColorChannel.G)
          this.Color.g = value;
        if (this.Channel == ColorChannel.B)
          this.Color.b = value;
        if (this.Channel != ColorChannel.A)
          return;
        this.Color.a = value;
      }
    }
  }
}
