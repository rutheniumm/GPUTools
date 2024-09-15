// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.PlaneCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  public class PlaneCollider : MonoBehaviour
  {
    public Vector4 GetWorldData()
    {
      Vector3 up = this.transform.up;
      Vector3 position = this.transform.position;
      float num = (float) ((double) position.x * (double) up.x + (double) position.y * (double) up.y + (double) position.z * (double) up.z);
      return new Vector4(up.x, up.y, up.z, -num);
    }

    private void OnEnable()
    {
      if (!Application.isPlaying)
        return;
      GPUCollidersManager.RegisterPlaneCollider(this);
    }

    private void OnDisable()
    {
      if (!Application.isPlaying)
        return;
      GPUCollidersManager.DeregisterPlaneCollider(this);
    }
  }
}
