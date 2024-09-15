// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Utils.TriangleUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
  public class TriangleUtils
  {
    public static Rect FindBoundRect(Vector2[] points)
    {
      Rect boundRect = new Rect()
      {
        min = points[0],
        max = points[0]
      };
      for (int index = 1; index < points.Length; ++index)
      {
        if ((double) points[index].x < (double) boundRect.min.x)
          boundRect.min = new Vector2(points[index].x, boundRect.min.y);
        if ((double) points[index].y < (double) boundRect.min.y)
          boundRect.min = new Vector2(boundRect.min.x, points[index].y);
        if ((double) points[index].x > (double) boundRect.max.x)
          boundRect.max = new Vector2(points[index].x, boundRect.max.y);
        if ((double) points[index].y > (double) boundRect.max.y)
          boundRect.max = new Vector2(boundRect.max.x, points[index].y);
      }
      return boundRect;
    }

    public static bool IsPointInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
      Vector2 barycentricInsideTriangle = TriangleUtils.GetBarycentricInsideTriangle(p, a, b, c);
      return (double) barycentricInsideTriangle.x >= 0.0 && (double) barycentricInsideTriangle.y >= 0.0 && (double) barycentricInsideTriangle.x + (double) barycentricInsideTriangle.y <= 1.0;
    }

    public static bool IsPointInsideTriangle(Vector2 barycentric) => (double) barycentric.x >= 0.0 && (double) barycentric.y >= 0.0 && (double) barycentric.x + (double) barycentric.y <= 1.0;

    public static Vector3 GetPointInsideTriangle(
      Vector3 a,
      Vector3 b,
      Vector3 c,
      Vector2 barycentric)
    {
      return a * (float) (1.0 - ((double) barycentric.x + (double) barycentric.y)) + b * barycentric.y + c * barycentric.x;
    }

    public static Vector2 GetBarycentricInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
      Vector2 vector2_1 = c - a;
      Vector2 vector2_2 = b - a;
      Vector2 rhs = p - a;
      float num1 = Vector2.Dot(vector2_1, vector2_1);
      float num2 = Vector2.Dot(vector2_1, vector2_2);
      float num3 = Vector2.Dot(vector2_1, rhs);
      float num4 = Vector2.Dot(vector2_2, vector2_2);
      float num5 = Vector2.Dot(vector2_2, rhs);
      float num6 = (float) (1.0 / ((double) num1 * (double) num4 - (double) num2 * (double) num2));
      return new Vector2((float) ((double) num4 * (double) num3 - (double) num2 * (double) num5) * num6, (float) ((double) num1 * (double) num5 - (double) num2 * (double) num3) * num6);
    }
  }
}
