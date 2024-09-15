// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.DebugVertices
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class DebugVertices : MonoBehaviour
  {
    public List<Vector3> Vertices;
    public float Radius;

    public static DebugVertices Draw(List<Vector3> vertices, float radius)
    {
      DebugVertices debugVertices = new GameObject(nameof (DebugVertices)).AddComponent<DebugVertices>();
      debugVertices.Vertices = vertices;
      debugVertices.Radius = radius;
      return debugVertices;
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.magenta;
      foreach (Vector3 vertex in this.Vertices)
        Gizmos.DrawWireSphere(vertex, this.Radius);
    }
  }
}
