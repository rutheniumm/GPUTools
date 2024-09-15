// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Utils.CurveUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
  public class CurveUtils
  {
    public static Vector3 GetSplinePoint(List<Vector3> points, float t)
    {
      int b = points.Count - 1;
      int a = (int) ((double) t * (double) points.Count);
      float num = 1f / (float) points.Count;
      float t1 = t % num * (float) points.Count;
      int index1 = Mathf.Max(0, a - 1);
      int index2 = Mathf.Min(a, b);
      int index3 = Mathf.Min(a + 1, b);
      Vector3 point1 = points[index1];
      Vector3 point2 = points[index2];
      Vector3 point3 = points[index3];
      Vector3 p0 = (point1 + point2) * 0.5f;
      Vector3 p2 = (point2 + point3) * 0.5f;
      return CurveUtils.GetBezierPoint(p0, point2, p2, t1);
    }

    public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
      float num = 1f - t;
      return num * num * p0 + 2f * num * t * p1 + t * t * p2;
    }
  }
}
