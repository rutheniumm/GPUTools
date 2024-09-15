// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Procedural.CurveGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Utils;
using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Procedural
{
  [Serializable]
  public class CurveGrid
  {
    [SerializeField]
    public Vector3[] ControlPoints;
    [SerializeField]
    public int ControlSizeX;
    [SerializeField]
    public int ControlSizeY;
    [SerializeField]
    public int ViewSizeX;
    [SerializeField]
    public int ViewSizeY;

    public void GenerateControl()
    {
      this.ControlPoints = new Vector3[this.ControlSizeX * this.ControlSizeY];
      for (int x1 = 0; x1 < this.ControlSizeX; ++x1)
      {
        for (int y = 0; y < this.ControlSizeY; ++y)
        {
          float x2 = (float) x1 / (float) this.ControlSizeX;
          float z = (float) y / (float) this.ControlSizeY;
          this.SetControl(x1, y, new Vector3(x2, 0.0f, z));
        }
      }
    }

    public void GenerateView()
    {
      for (int index1 = 0; index1 < this.ViewSizeX; ++index1)
      {
        for (int index2 = 0; index2 < this.ViewSizeY; ++index2)
          this.GetSplinePoint((float) index1 / (float) this.ViewSizeX, (float) index2 / (float) this.ViewSizeY);
      }
    }

    public Vector3 GetSplinePoint(float tX, float tY)
    {
      int a1 = (int) ((double) tX * (double) this.ControlSizeX);
      int x1 = Mathf.Max(0, a1 - 1);
      int x2 = Mathf.Min(a1, this.ControlSizeX - 1);
      int x3 = Mathf.Min(a1 + 1, this.ControlSizeX - 1);
      int a2 = (int) ((double) tY * (double) this.ControlSizeY);
      int y1 = Mathf.Max(0, a2 - 1);
      int y2 = Mathf.Min(a2, this.ControlSizeY - 1);
      int y3 = Mathf.Min(a2 + 1, this.ControlSizeY - 1);
      Vector3 control1 = this.GetControl(x1, y1);
      Vector3 control2 = this.GetControl(x2, y1);
      Vector3 control3 = this.GetControl(x3, y1);
      Vector3 control4 = this.GetControl(x1, y2);
      Vector3 control5 = this.GetControl(x2, y2);
      Vector3 control6 = this.GetControl(x3, y2);
      Vector3 control7 = this.GetControl(x1, y3);
      Vector3 control8 = this.GetControl(x2, y3);
      Vector3 control9 = this.GetControl(x3, y3);
      Vector3 p0_1 = (control1 + control2) * 0.5f;
      Vector3 p2_1 = (control2 + control3) * 0.5f;
      Vector3 p0_2 = (control4 + control5) * 0.5f;
      Vector3 p2_2 = (control5 + control6) * 0.5f;
      Vector3 p0_3 = (control7 + control8) * 0.5f;
      Vector3 p2_3 = (control8 + control9) * 0.5f;
      float num1 = 1f / (float) this.ControlSizeX;
      float t1 = tX % num1 * (float) this.ControlSizeX;
      Vector3 bezierPoint1 = CurveUtils.GetBezierPoint(p0_1, control2, p2_1, t1);
      Vector3 bezierPoint2 = CurveUtils.GetBezierPoint(p0_2, control5, p2_2, t1);
      Vector3 bezierPoint3 = CurveUtils.GetBezierPoint(p0_3, control8, p2_3, t1);
      Vector3 p0_4 = (bezierPoint1 + bezierPoint2) * 0.5f;
      Vector3 p2_4 = (bezierPoint3 + bezierPoint2) * 0.5f;
      float num2 = 1f / (float) this.ControlSizeY;
      float t2 = tY % num2 * (float) this.ControlSizeY;
      return CurveUtils.GetBezierPoint(p0_4, bezierPoint2, p2_4, t2);
    }

    public void SetControl(int x, int y, Vector3 value) => this.ControlPoints[x * this.ControlSizeY + y] = value;

    public Vector3 GetControl(int x, int y) => this.ControlPoints[x * this.ControlSizeY + y];

    public void SetView(int x, int y, Vector3 value) => this.ControlPoints[x * this.ViewSizeY + y] = value;

    public Vector3 GetView(int x, int y) => this.ControlPoints[x * this.ViewSizeX + y];
  }
}
