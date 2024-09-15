// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Utils.CollectionUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Utils
{
  public static class CollectionUtils
  {
    public static bool NullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;

    public static bool NullOrEmpty<T>(this T[] array) => array == null || array.Length == 0;

    public static void SetValueForAll<T>(this T[] array, T value)
    {
      for (int index = 0; index < array.Length; ++index)
        array[index] = value;
    }
  }
}
