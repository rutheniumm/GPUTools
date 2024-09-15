// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.Debug.MayaImporterDebugDraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Debug
{
  public class MayaImporterDebugDraw
  {
    public static void Draw(MayaHairGeometryImporter importer)
    {
      Gizmos.color = Color.green;
      List<Vector3> vertices = importer.Data.Vertices;
      Matrix4x4 toWorldMatrix = importer.ScalpProvider.ToWorldMatrix;
      for (int index = 1; index < vertices.Count; ++index)
      {
        if (index % importer.Data.Segments != 0)
          Gizmos.DrawLine(toWorldMatrix.MultiplyPoint3x4(vertices[index - 1]), toWorldMatrix.MultiplyPoint3x4(vertices[index]));
      }
      Bounds bounds = importer.GetBounds();
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
  }
}
