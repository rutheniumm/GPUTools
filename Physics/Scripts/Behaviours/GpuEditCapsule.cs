// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.GpuEditCapsule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  [ExecuteInEditMode]
  public class GpuEditCapsule : LineSphereCollider
  {
    public CapsuleCollider capsuleCollider;
    public float oversizeRadius;
    public float oversizeHeight;
    public float strength = 1f;

    public virtual void UpdateData()
    {
      if (!((Object) this.capsuleCollider != (Object) null))
        return;
      float num1 = this.capsuleCollider.radius + this.oversizeRadius;
      float num2 = (float) (((double) this.capsuleCollider.height + (double) this.oversizeHeight) * 0.5);
      if ((double) num2 < (double) num1)
        num2 = num1;
      this.RadiusA = num1;
      this.RadiusB = num1;
      Vector3 center = this.capsuleCollider.center;
      float num3 = num2 - num1;
      switch (this.capsuleCollider.direction)
      {
        case 0:
          this.A.x = center.x + num3;
          this.A.y = center.y;
          this.A.z = center.z;
          this.B.x = center.x - num3;
          this.B.y = center.y;
          this.B.z = center.z;
          break;
        case 1:
          this.A.x = center.x;
          this.A.y = center.y + num3;
          this.A.z = center.z;
          this.B.x = center.x;
          this.B.y = center.y - num3;
          this.B.z = center.z;
          break;
        case 2:
          this.A.x = center.x;
          this.A.y = center.y;
          this.A.z = center.z + num3;
          this.B.x = center.x;
          this.B.y = center.y;
          this.B.z = center.z - num3;
          break;
      }
    }
  }
}
