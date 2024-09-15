// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Types.Joints.GPPointJoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Joints
{
  public struct GPPointJoint
  {
    public int BodyId;
    public int MatrixId;
    public Vector3 Point;
    public float Rigidity;

    public GPPointJoint(int bodyId, int matrixId, Vector3 point, float rigidity)
    {
      this.BodyId = bodyId;
      this.Point = point;
      this.MatrixId = matrixId;
      this.Rigidity = rigidity;
    }

    public static int Size() => 24;
  }
}
