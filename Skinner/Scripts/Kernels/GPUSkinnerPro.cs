// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Kernels.GPUSkinnerPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Kernels;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
  public class GPUSkinnerPro : PrimitiveBase
  {
    private GPUSkinner skinner;
    private GPUBlendShapePlayer blendShapePlayer;
    private GPUMatrixMultiplier matrixMultiplier;

    public GPUSkinnerPro(SkinnedMeshRenderer skin)
    {
      this.skinner = new GPUSkinner(skin);
      this.AddPass((IPass) this.skinner);
      if (skin.sharedMesh.blendShapeCount == 0)
        return;
      this.blendShapePlayer = new GPUBlendShapePlayer(skin);
      this.matrixMultiplier = new GPUMatrixMultiplier(this.blendShapePlayer.TransformMatricesBuffer, this.skinner.TransformMatricesBuffer);
      this.AddPass((IPass) this.blendShapePlayer);
      this.AddPass((IPass) this.matrixMultiplier);
    }

    public GpuBuffer<Matrix4x4> TransformMatricesBuffer => this.matrixMultiplier != null ? this.matrixMultiplier.ResultMatrices : this.skinner.TransformMatricesBuffer;
  }
}
