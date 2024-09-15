// Decompiled with JetBrains decompiler
// Type: GPUTools.HairDemo.Scripts.BlendShapesDemo.TestBlendShapes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Skinner.Scripts.Kernels;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.BlendShapesDemo
{
  public class TestBlendShapes : MonoBehaviour
  {
    [SerializeField]
    private SkinnedMeshRenderer skin;
    private GPUSkinnerPro skinner;
    private Vector3[] vertices;

    private void Start()
    {
      this.skinner = new GPUSkinnerPro(this.skin);
      this.skinner.Dispatch();
      this.vertices = this.skin.sharedMesh.vertices;
    }

    private void Update() => this.skinner.Dispatch();

    private void OnDestroy() => this.skinner.Dispose();

    private void OnDrawGizmos()
    {
      if (!Application.isPlaying)
        return;
      this.skinner.TransformMatricesBuffer.PullData();
      Matrix4x4[] data = this.skinner.TransformMatricesBuffer.Data;
      for (int index = 0; index < this.vertices.Length; ++index)
      {
        Vector3 vertex = this.vertices[index];
        Vector3 center = data[index].MultiplyPoint3x4(vertex);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, 1f / 500f);
      }
    }
  }
}
