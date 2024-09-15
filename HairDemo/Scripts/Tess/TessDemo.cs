// Decompiled with JetBrains decompiler
// Type: GPUTools.HairDemo.Scripts.Tess.TessDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.Tess
{
  public class TessDemo : MonoBehaviour
  {
    [SerializeField]
    [Range(6f, 64f)]
    private int count = 64;
    private int oldCount;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;
    [SerializeField]
    private Vector3 c;
    private FixedList<Vector3> barycentric = new FixedList<Vector3>(64);

    private void Start()
    {
      this.Gen();
      this.oldCount = this.count;
    }

    private void Update()
    {
      if (this.count != this.oldCount)
        this.Gen();
      this.oldCount = this.count;
    }

    private void Gen()
    {
      this.barycentric.Reset();
      int steps = 1;
      float num1 = 0.2f;
      if (this.count >= 15)
      {
        num1 = 0.1f;
        steps = 2;
      }
      if (this.count >= 45)
      {
        num1 = 0.05f;
        steps = 3;
      }
      float num2 = 1f - num1;
      float num3 = (float) ((1.0 - (double) num2) * 0.5);
      this.Split(new Vector3(num2, num3, num3), new Vector3(num3, num2, num3), new Vector3(num3, num3, num2), steps);
      while (this.barycentric.Count < this.count)
      {
        if (!this.barycentric.Contains(this.GetRandomK()))
          this.barycentric.Add(this.GetRandomK());
      }
      Debug.Log((object) this.barycentric.Count);
    }

    private void Split(Vector3 b1, Vector3 b2, Vector3 b3, int steps)
    {
      --steps;
      this.TryAdd(b1);
      this.TryAdd(b2);
      this.TryAdd(b3);
      Vector3 vector3_1 = (b1 + b2) * 0.5f;
      Vector3 vector3_2 = (b2 + b3) * 0.5f;
      Vector3 b3_1 = (b3 + b1) * 0.5f;
      if (steps < 0)
        return;
      this.Split(b1, vector3_1, b3_1, steps);
      this.Split(b2, vector3_1, vector3_2, steps);
      this.Split(b3, vector3_2, b3_1, steps);
      this.Split(vector3_1, vector3_2, b3_1, steps);
    }

    private void TryAdd(Vector3 v)
    {
      if (this.barycentric.Contains(v))
        return;
      this.barycentric.Add(v);
    }

    private Vector3 GetRandomK()
    {
      float x = Random.Range(0.0f, 1f);
      float y = Random.Range(0.0f, 1f);
      if ((double) x + (double) y > 1.0)
      {
        x = 1f - x;
        y = 1f - y;
      }
      float z = (float) (1.0 - ((double) x + (double) y));
      return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
      Gizmos.DrawLine(this.a, this.b);
      Gizmos.DrawLine(this.b, this.c);
      Gizmos.DrawLine(this.c, this.a);
      Gizmos.color = new Color(1f, 0.0f, 0.0f, 1f);
      for (int i = 0; i < this.barycentric.Count; ++i)
      {
        Vector3 vector3 = this.barycentric[i];
        Gizmos.DrawSphere(this.a * vector3.x + this.b * vector3.y + this.c * vector3.z, 0.01f);
      }
    }
  }
}
