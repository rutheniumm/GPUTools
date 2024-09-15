// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Constrains.HairJointArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Constrains
{
  public class HairJointArea : MonoBehaviour
  {
    [SerializeField]
    private float radius;

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawWireSphere(this.transform.position, this.radius);
    }

    public float Radius => this.radius;
  }
}
