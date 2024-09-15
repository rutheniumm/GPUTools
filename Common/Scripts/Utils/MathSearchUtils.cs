// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Utils.MathSearchUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
  public class MathSearchUtils
  {
    public static List<Vector3> FindCloseVertices(
      Vector3[] vertices,
      Vector3 testVertex,
      int count)
    {
      List<Vector3> ignoreList = new List<Vector3>();
      for (int index = 0; index < count; ++index)
      {
        Vector3 closeVertex = MathSearchUtils.FindCloseVertex(vertices, testVertex, ignoreList);
        ignoreList.Add(closeVertex);
      }
      return ignoreList;
    }

    public static Vector3 FindCloseVertex(
      Vector3[] vertices,
      Vector3 testVertex,
      List<Vector3> ignoreList = null)
    {
      float num = float.PositiveInfinity;
      Vector3 closeVertex = vertices[0];
      foreach (Vector3 vertex in vertices)
      {
        if (ignoreList == null || !ignoreList.Contains(vertex))
        {
          float sqrMagnitude = (vertex - testVertex).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            closeVertex = vertex;
          }
        }
      }
      return closeVertex;
    }

    public static Vector3 FindCloseVertex(
      List<Vector3> vertices,
      Vector3 testVertex,
      List<Vector3> ignoreList = null)
    {
      float num = float.PositiveInfinity;
      Vector3 closeVertex = vertices[0];
      foreach (Vector3 vertex in vertices)
      {
        if (ignoreList == null || !ignoreList.Contains(vertex))
        {
          float sqrMagnitude = (vertex - testVertex).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            closeVertex = vertex;
          }
        }
      }
      return closeVertex;
    }
  }
}
