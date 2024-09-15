// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Utils.ConvertUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
  public static class ConvertUtils
  {
    public static Color ToColor(this Vector4 vector) => new Color(vector.x, vector.y, vector.z, vector.w);

    public static Vector4 ToVector(this Color color) => (Vector4) new Color(color.r, color.g, color.b, color.a);
  }
}
