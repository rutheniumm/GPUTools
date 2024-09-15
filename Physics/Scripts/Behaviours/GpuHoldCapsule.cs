﻿// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.GpuHoldCapsule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  [ExecuteInEditMode]
  public class GpuHoldCapsule : GpuEditCapsule
  {
    private void OnEnable()
    {
      if ((Object) this.capsuleCollider == (Object) null)
        this.capsuleCollider = this.GetComponent<CapsuleCollider>();
      if (Application.isPlaying)
        GPUCollidersManager.RegisterHoldCapsule((GpuEditCapsule) this);
      this.UpdateData();
    }

    private void OnDisable()
    {
      if (!Application.isPlaying)
        return;
      GPUCollidersManager.DeregisterHoldCapsule((GpuEditCapsule) this);
    }
  }
}
