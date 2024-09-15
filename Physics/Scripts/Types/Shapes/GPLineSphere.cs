// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Types.Shapes.GPLineSphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
  public struct GPLineSphere
  {
    public Vector3 PositionA;
    public Vector3 PositionB;
    public float RadiusA;
    public float RadiusB;
    public float Friction;

    public GPLineSphere(Vector3 positionA, Vector3 positionB, float radiusA, float radiusB)
    {
      this.PositionA = positionA;
      this.PositionB = positionB;
      this.RadiusA = radiusA;
      this.RadiusB = radiusB;
      this.Friction = 1f;
    }

    public static int Size() => 36;
  }
}
