// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Tools.BoneCollidersCopyPaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Tools
{
  public class BoneCollidersCopyPaster : MonoBehaviour
  {
    [SerializeField]
    private Transform from;
    [SerializeField]
    private Transform to;

    [ContextMenu("CopyPaste")]
    public void CopyPaste() => this.CopyPasteRecursive(this.from, this.to);

    private void CopyPasteRecursive(Transform from, Transform to)
    {
      this.CopyPasteForBone(from, to);
      for (int index = 0; index < from.childCount; ++index)
        this.CopyPasteRecursive(from.GetChild(index), to.GetChild(index));
    }

    private void CopyPasteForBone(Transform from, Transform to)
    {
      foreach (LineSphereCollider component in from.GetComponents<LineSphereCollider>())
      {
        LineSphereCollider lineSphereCollider = to.gameObject.AddComponent<LineSphereCollider>();
        lineSphereCollider.WorldA = component.WorldA;
        lineSphereCollider.WorldB = component.WorldB;
        lineSphereCollider.WorldRadiusA = component.WorldRadiusA;
        lineSphereCollider.WorldRadiusB = component.WorldRadiusB;
      }
      foreach (SphereCollider component in from.GetComponents<SphereCollider>())
      {
        SphereCollider sphereCollider = to.gameObject.AddComponent<SphereCollider>();
        sphereCollider.center = component.center;
        sphereCollider.radius = component.radius;
      }
    }
  }
}
