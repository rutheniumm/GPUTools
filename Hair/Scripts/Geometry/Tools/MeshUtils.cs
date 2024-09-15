// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Tools.MeshUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
  public class MeshUtils
  {
    public static List<Vector3> GetWorldVertices(MeshFilter fiter)
    {
      List<Vector3> worldVertices = new List<Vector3>();
      foreach (Vector3 vertex in fiter.sharedMesh.vertices)
        worldVertices.Add(fiter.transform.TransformPoint(vertex));
      return worldVertices;
    }

    public static List<Vector3> GetVerticesInSpace(
      Mesh mesh,
      Matrix4x4 toWorld,
      Matrix4x4 toTransform)
    {
      List<Vector3> verticesInSpace = new List<Vector3>();
      for (int index = 0; index < mesh.vertexCount; ++index)
      {
        Vector3 vertex = mesh.vertices[index];
        Vector3 point = toWorld.MultiplyPoint3x4(vertex);
        Vector3 vector3 = toTransform.MultiplyPoint3x4(point);
        verticesInSpace.Add(vector3);
      }
      return verticesInSpace;
    }
  }
}
