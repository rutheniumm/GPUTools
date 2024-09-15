// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Types.Dynamic.GPParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Dynamic
{
  public struct GPParticle
  {
    public Vector3 Position;
    public Vector3 LastPosition;
    public Vector3 LastPositionInner;
    public Vector3 DrawPosition;
    public Vector3 Velocity;
    public float Radius;
    public Vector3 ColliderDelta;
    public float CollisionDrag;
    public float CollisionHold;
    public float CollisionPower;
    public float Strength;
    public int CollisionEnabled;
    public int SphereCollisionID;
    public int LineSphereCollisionID;
    public int GrabID;
    public float GrabDistance;
    public float AuxData;

    public GPParticle(Vector3 position, float radius)
    {
      this.Position = position;
      this.LastPosition = position;
      this.LastPositionInner = position;
      this.DrawPosition = position;
      this.Velocity = Vector3.zero;
      this.Radius = radius;
      this.ColliderDelta = Vector3.zero;
      this.CollisionDrag = 0.0f;
      this.CollisionHold = 0.0f;
      this.CollisionPower = 0.0f;
      this.Strength = 0.1f;
      this.CollisionEnabled = 1;
      this.SphereCollisionID = -1;
      this.LineSphereCollisionID = -1;
      this.GrabID = -1;
      this.GrabDistance = 0.0f;
      this.AuxData = 0.0f;
    }

    public static int Size() => 116;
  }
}
