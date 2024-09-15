// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.LoggerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class LoggerUtil
  {
    public static void LogArray<T>(T[] list, int max)
    {
      for (int index = 0; index < Math.Min(list.Length, max); ++index)
        UnityEngine.Debug.Log((object) list[index]);
    }

    public static void LogList<T>(List<T> list, int max)
    {
      for (int index = 0; index < Math.Min(list.Count, max); ++index)
        UnityEngine.Debug.Log((object) list[index]);
    }
  }
}
