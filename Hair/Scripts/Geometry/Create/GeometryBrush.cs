// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.GeometryBrush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
  [Serializable]
  public class GeometryBrush
  {
    public Vector3 Dirrection;
    public float Radius = 0.01f;
    public float Lenght1 = 1f;
    public float Lenght2 = 1f;
    public float Strength = 0.25f;
    public Color Color = Color.white;
    public float CollisionDistance = 0.01f;
    public GeometryBrushBehaviour Behaviour;
    [NonSerialized]
    public bool IsBrushEnabled;
    private Vector3 position;
    public Vector3 OldPosition;

    public Vector3 ToWorld(Matrix4x4 m, Vector3 local) => this.Position + (Vector3) (m * (Vector4) local);

    public bool Contains(Vector3 point)
    {
      Camera current = Camera.current;
      Vector3 vector3_1 = current.transform.InverseTransformPoint(point);
      Vector3 vector3_2 = current.transform.InverseTransformPoint(this.Position);
      bool flag1 = (double) ((Vector2) vector3_1 - (Vector2) vector3_2).magnitude < (double) this.Radius;
      bool flag2 = (double) vector3_2.z - (double) vector3_1.z > -(double) this.Lenght1;
      bool flag3 = (double) vector3_2.z - (double) vector3_1.z < (double) this.Lenght2;
      return flag1 && flag2 && flag3;
    }

    public Vector3 Position
    {
      set
      {
        this.OldPosition = this.position;
        this.position = value;
      }
      get => this.position;
    }

    public Vector3 Speed => this.position - this.OldPosition;
  }
}
