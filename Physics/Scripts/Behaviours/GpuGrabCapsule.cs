// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.GpuGrabCapsule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  [ExecuteInEditMode]
  public class GpuGrabCapsule : GpuEditCapsule
  {
    protected Matrix4x4 _lastWorldToLocalMatrix;
    protected Matrix4x4 _changeMatrix;

    private void OnEnable()
    {
      if ((Object) this.capsuleCollider == (Object) null)
        this.capsuleCollider = this.GetComponent<CapsuleCollider>();
      if (Application.isPlaying)
        GPUCollidersManager.RegisterGrabCapsule((GpuEditCapsule) this);
      this.UpdateData();
    }

    private void OnDisable()
    {
      if (!Application.isPlaying)
        return;
      GPUCollidersManager.DeregisterGrabCapsule((GpuEditCapsule) this);
    }

    public Matrix4x4 changeMatrix => this._changeMatrix;

    public override void UpdateData()
    {
      base.UpdateData();
      this._changeMatrix = this.transform.localToWorldMatrix * this._lastWorldToLocalMatrix;
      this._lastWorldToLocalMatrix = this.transform.worldToLocalMatrix;
    }
  }
}
