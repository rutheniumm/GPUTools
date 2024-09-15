// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Utils.TransformUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
  public static class TransformUtils
  {
    public static Vector3[] TransformPoints(this Transform transform, Vector3[] points)
    {
      Vector3[] vector3Array = new Vector3[points.Length];
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = transform.TransformPoint(points[index]);
      return vector3Array;
    }

    public static Vector3[] InverseTransformPoints(this Transform transform, Vector3[] points)
    {
      Vector3[] vector3Array = new Vector3[points.Length];
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = transform.InverseTransformPoint(points[index]);
      return vector3Array;
    }

    public static void TransformPoints(this Transform transform, ref Vector3[] points)
    {
      for (int index = 0; index < points.Length; ++index)
        points[index] = transform.TransformPoint(points[index]);
    }

    public static Vector3[] TransformVectors(this Transform transform, Vector3[] vectors)
    {
      Vector3[] vector3Array = new Vector3[vectors.Length];
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = transform.TransformVector(vectors[index]);
      return vector3Array;
    }

    public static void TransformVectors(this Transform transform, ref Vector3[] vectors)
    {
      for (int index = 0; index < vectors.Length; ++index)
        vectors[index] = transform.TransformVector(vectors[index]);
    }

    public static Vector3[] TransformDirrections(this Transform transform, Vector3[] dirrections)
    {
      Vector3[] vector3Array = new Vector3[dirrections.Length];
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = transform.TransformDirection(dirrections[index]);
      return vector3Array;
    }

    public static void TransformDirrections(this Transform transform, ref Vector3[] dirrections)
    {
      for (int index = 0; index < dirrections.Length; ++index)
        dirrections[index] = transform.TransformDirection(dirrections[index]);
    }

    public static Vector3[] TransformPoints(this Matrix4x4 matrix, Vector3[] points)
    {
      Vector3[] vector3Array = new Vector3[points.Length];
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = matrix.MultiplyPoint3x4(points[index]);
      return vector3Array;
    }

    public static Vector3[] InverseTransformPoints(this Matrix4x4 matrix, Vector3[] points)
    {
      Vector3[] vector3Array = new Vector3[points.Length];
      Matrix4x4 inverse = matrix.inverse;
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = inverse.MultiplyPoint3x4(points[index]);
      return vector3Array;
    }
  }
}
