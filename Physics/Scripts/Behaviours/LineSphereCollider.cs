// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.LineSphereCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  public class LineSphereCollider : MonoBehaviour
  {
    [SerializeField]
    public Vector3 A = Vector3.zero;
    [SerializeField]
    public Vector3 B = new Vector3(0.0f, -0.2f, 0.0f);
    [SerializeField]
    public float RadiusA = 0.1f;
    [SerializeField]
    public float RadiusB = 0.1f;

    public Vector3 WorldA
    {
      set => this.A = this.transform.InverseTransformPoint(value);
      get => this.transform.TransformPoint(this.A);
    }

    public Vector3 WorldB
    {
      set => this.B = this.transform.InverseTransformPoint(value);
      get => this.transform.TransformPoint(this.B);
    }

    public float WorldRadiusA
    {
      set => this.RadiusA = value / this.Scale;
      get => this.RadiusA * this.transform.lossyScale.x;
    }

    public float WorldRadiusB
    {
      set => this.RadiusB = value / this.Scale;
      get => this.RadiusB * this.transform.lossyScale.x;
    }

    private float Scale => Mathf.Max(Mathf.Max(this.transform.lossyScale.x, this.transform.lossyScale.y), this.transform.lossyScale.z);

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(this.WorldA, this.WorldRadiusA);
      Gizmos.DrawWireSphere(this.WorldB, this.WorldRadiusB);
      if (!(this.WorldA != this.WorldB))
        return;
      Vector3 lhs = Vector3.Normalize(this.WorldA - this.WorldB);
      Vector3 normalized = Vector3.Cross(lhs, new Vector3(lhs.z, lhs.y, -lhs.x)).normalized;
      float f = 0.314159274f;
      float num = Mathf.Cos(f);
      float w = Mathf.Sin(f);
      Quaternion quaternion = new Quaternion(num * lhs.x, num * lhs.y, num * lhs.z, w);
      if (quaternion == Quaternion.identity)
        return;
      Quaternion identity = Quaternion.identity;
      for (int index = 0; index < 5; ++index)
      {
        identity *= quaternion;
        Matrix4x4 matrix4x4_1 = Matrix4x4.TRS(this.WorldA, identity, Vector3.one * this.WorldRadiusA);
        Matrix4x4 matrix4x4_2 = Matrix4x4.TRS(this.WorldB, identity, Vector3.one * this.WorldRadiusB);
        Gizmos.DrawLine(matrix4x4_1.MultiplyPoint3x4(normalized), matrix4x4_2.MultiplyPoint3x4(normalized));
      }
    }
  }
}
