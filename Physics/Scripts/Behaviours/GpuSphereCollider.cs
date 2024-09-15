// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.GpuSphereCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  [ExecuteInEditMode]
  public class GpuSphereCollider : MonoBehaviour
  {
    public SphereCollider sphereCollider;
    public float oversizeRadius;
    public float friction = 1f;

    public Vector3 center
    {
      get => (Object) this.sphereCollider != (Object) null ? this.sphereCollider.center : Vector3.zero;
      set
      {
        if (!((Object) this.sphereCollider != (Object) null) || !(this.sphereCollider.center != value))
          return;
        this.sphereCollider.center = value;
      }
    }

    public Vector3 worldCenter
    {
      set => this.center = this.transform.InverseTransformPoint(value);
      get => this.transform.TransformPoint(this.center);
    }

    public float radius
    {
      get => (Object) this.sphereCollider != (Object) null ? this.sphereCollider.radius + this.oversizeRadius : 0.0f;
      set
      {
        if (!((Object) this.sphereCollider != (Object) null) || (double) this.sphereCollider.radius == (double) value)
          return;
        this.sphereCollider.radius = value;
      }
    }

    public float worldRadius => this.radius * this.transform.lossyScale.x;

    private float Scale => Mathf.Max(Mathf.Max(this.transform.lossyScale.x, this.transform.lossyScale.y), this.transform.lossyScale.z);

    private void OnEnable()
    {
      if ((Object) this.sphereCollider == (Object) null)
        this.sphereCollider = this.GetComponent<SphereCollider>();
      if (!Application.isPlaying)
        return;
      GPUCollidersManager.RegisterSphereCollider(this);
    }

    private void OnDisable()
    {
      if (!Application.isPlaying)
        return;
      GPUCollidersManager.DeregisterSphereCollider(this);
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(this.worldCenter, this.worldRadius);
    }
  }
}
