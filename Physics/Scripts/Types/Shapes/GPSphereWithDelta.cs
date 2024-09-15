// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Types.Shapes.GPSphereWithDelta
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
  public struct GPSphereWithDelta
  {
    public Vector3 Position;
    public float Radius;
    public float Friction;
    public Vector3 Delta;

    public GPSphereWithDelta(Vector3 position, float radius)
    {
      this.Position = position;
      this.Radius = radius;
      this.Friction = 1f;
      this.Delta = Vector3.zero;
    }

    public static int Size() => 32;
  }
}
