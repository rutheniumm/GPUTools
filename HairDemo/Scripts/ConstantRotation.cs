// Decompiled with JetBrains decompiler
// Type: GPUTools.HairDemo.Scripts.ConstantRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.HairDemo.Scripts
{
  public class ConstantRotation : MonoBehaviour
  {
    [SerializeField]
    private Vector3 axis;
    [SerializeField]
    public float Speed;

    private void Update() => this.transform.Rotate(this.axis, this.Speed * Time.deltaTime);
  }
}
