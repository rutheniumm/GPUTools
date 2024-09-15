// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Settings.Colors.IColorProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
  public interface IColorProvider
  {
    Color GetColor(HairSettings settings, int x, int y, int sizeY);
  }
}
