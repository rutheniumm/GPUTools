// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Types.Joints.GPDistanceJoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;

namespace GPUTools.Physics.Scripts.Types.Joints
{
  public struct GPDistanceJoint : IGroupItem
  {
    public int Body1Id;
    public int Body2Id;
    public float Distance;
    public float Elasticity;

    public GPDistanceJoint(int body1Id, int body2Id, float distance, float elasticity)
    {
      this.Body1Id = body1Id;
      this.Body2Id = body2Id;
      this.Distance = distance;
      this.Elasticity = elasticity;
    }

    public static int Size() => 16;

    public bool HasConflict(IGroupItem item)
    {
      GPDistanceJoint gpDistanceJoint = (GPDistanceJoint) item;
      return gpDistanceJoint.Body1Id == this.Body1Id || gpDistanceJoint.Body2Id == this.Body1Id || gpDistanceJoint.Body1Id == this.Body2Id || gpDistanceJoint.Body2Id == this.Body2Id;
    }
  }
}
