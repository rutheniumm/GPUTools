// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Behaviours.SkinnedMeshCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
  public class SkinnedMeshCollider : MonoBehaviour
  {
    [SerializeField]
    private bool debugDraw;
    [SerializeField]
    private MeshFilter filter;

    private void OnDrawGizmos()
    {
      if (this.Vertices == null || !this.debugDraw)
        return;
      Gizmos.color = Color.red;
      foreach (Vector3 vertex in this.Vertices)
        Gizmos.DrawWireSphere(this.transform.TransformPoint(vertex), 0.01f);
    }

    public Vector3[] Vertices
    {
      get
      {
        Vector3[] vertices1 = new Vector3[this.filter.sharedMesh.vertexCount];
        Vector3[] vertices2 = this.filter.sharedMesh.vertices;
        for (int index = 0; index < vertices1.Length; ++index)
          vertices1[index] = this.transform.TransformPoint(vertices2[index]);
        return vertices1;
      }
    }
  }
}
