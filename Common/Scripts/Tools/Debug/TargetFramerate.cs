// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.TargetFramerate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class TargetFramerate : MonoBehaviour
  {
    [SerializeField]
    private int targetFrameRate;

    private void Start() => Application.targetFrameRate = this.targetFrameRate;
  }
}
