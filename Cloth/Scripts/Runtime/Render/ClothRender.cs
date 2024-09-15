// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Render.ClothRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Render
{
  public class ClothRender : MonoBehaviour
  {
    private ClothDataFacade data;

    public void Initialize(ClothDataFacade data)
    {
      this.data = data;
      this.Update();
    }

    private void UpdateBounds() => this.data.MeshProvider.Mesh.bounds = this.transform.InverseTransformBounds(this.data.Bounds);

    private void Update()
    {
      for (int index = 0; index < this.data.Materials.Length; ++index)
      {
        Material material = this.data.Materials[index];
        material.EnableKeyword("VERTEX_FROM_BUFFER");
        material.SetBuffer("_ClothVertices", this.data.ClothVertices.ComputeBuffer);
      }
      if (!this.data.CustomBounds)
        return;
      this.UpdateBounds();
    }
  }
}
