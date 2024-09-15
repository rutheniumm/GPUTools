// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Types.Shapes.GPGrabSphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
  public struct GPGrabSphere
  {
    public int ID;
    public Vector3 Position;
    public float Radius;
    public int GrabbedThisFrame;

    public GPGrabSphere(int id, Vector3 position, float radius)
    {
      this.ID = id;
      this.Position = position;
      this.Radius = radius;
      this.GrabbedThisFrame = 0;
    }

    public static int Size() => 24;
  }
}
