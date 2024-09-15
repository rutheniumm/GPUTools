// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.DebugDraw.ClothDebugDraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.DebugDraw
{
  public class ClothDebugDraw
  {
    public static void Draw(ClothSettings settings)
    {
      if (settings.GeometryData.Particles == null)
        return;
      Gizmos.color = Color.green;
      Matrix4x4 toWorldMatrix = settings.MeshProvider.ToWorldMatrix;
      ClothGeometryData geometryData = settings.GeometryData;
      if (settings.GeometryDebugDrawDistanceJoints)
      {
        foreach (Int2ListContainer jointGroup in geometryData.JointGroups)
        {
          foreach (Int2 int2 in jointGroup.List)
            Gizmos.DrawLine(toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[int2.X]), toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[int2.Y]));
        }
      }
      if (!settings.GeometryDebugDrawStiffnessJoints)
        return;
      Gizmos.color = Color.yellow;
      foreach (Int2ListContainer stiffnessJointGroup in geometryData.StiffnessJointGroups)
      {
        foreach (Int2 int2 in stiffnessJointGroup.List)
          Gizmos.DrawLine(toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[int2.X]), toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[int2.Y]));
      }
    }

    public static void DrawAlways(ClothSettings settings)
    {
      if (!settings.CustomBounds)
        return;
      Bounds bounds1 = settings.Bounds;
      Bounds bounds2 = new Bounds(settings.transform.TransformPoint(bounds1.center), bounds1.size);
      Gizmos.DrawWireCube(bounds2.center, bounds2.size);
    }
  }
}
