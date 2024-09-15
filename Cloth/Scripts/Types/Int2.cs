// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Types.Int2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;

namespace GPUTools.Cloth.Scripts.Types
{
  [Serializable]
  public struct Int2
  {
    public int X;
    public int Y;

    public Int2(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public static int SizeOf() => 8;
  }
}
