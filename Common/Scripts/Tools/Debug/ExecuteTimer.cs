// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.ExecuteTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class ExecuteTimer
  {
    public static DateTime StartTime;

    public static void Start() => ExecuteTimer.StartTime = DateTime.Now;

    public static double TotalMiliseconds() => (DateTime.Now - ExecuteTimer.StartTime).TotalMilliseconds;

    public static void Log() => UnityEngine.Debug.Log((object) ("Total Miliseconds: " + (object) ExecuteTimer.TotalMiliseconds()));
  }
}
