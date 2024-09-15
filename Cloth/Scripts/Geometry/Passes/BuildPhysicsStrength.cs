// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.BuildPhysicsStrength
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Painter.Scripts;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class BuildPhysicsStrength : BuildChainCommand
  {
    private readonly ClothSettings settings;

    public BuildPhysicsStrength(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      if (this.settings.EditorStrengthType == ClothEditorType.Texture)
        this.BlendFromTexture();
      if (this.settings.EditorStrengthType != ClothEditorType.Painter)
        return;
      this.BlendFromPainter();
    }

    private void BlendFromPainter()
    {
      if ((Object) this.settings.EditorStrengthPainter == (Object) null)
      {
        Debug.LogError((object) "Painter field is uninitialized");
      }
      else
      {
        for (int i = 0; i < this.settings.GeometryData.ParticlesStrength.Length; ++i)
        {
          Color color = this.settings.EditorStrengthPainter.Colors[i];
          this.SetBlend(i, color);
        }
      }
    }

    private void BlendFromTexture()
    {
      Texture2D editorStrengthTexture = this.settings.EditorStrengthTexture;
      if (!((Object) editorStrengthTexture != (Object) null))
        return;
      Vector2[] uv = this.settings.MeshProvider.Mesh.uv;
      for (int i = 0; i < uv.Length; ++i)
      {
        Vector2 vector2 = uv[i];
        Color pixelBilinear = editorStrengthTexture.GetPixelBilinear(vector2.x, vector2.y);
        this.SetBlend(i, pixelBilinear);
      }
      if (uv.Length != 0)
        return;
      Debug.LogWarning((object) "Add uv to mesh to use vertices blend");
    }

    private void SetBlend(int i, Color color)
    {
      if (this.settings.StrengthChannel == ColorChannel.R)
        this.settings.GeometryData.ParticlesStrength[i] = color.r;
      if (this.settings.StrengthChannel == ColorChannel.G)
        this.settings.GeometryData.ParticlesStrength[i] = color.g;
      if (this.settings.StrengthChannel == ColorChannel.B)
        this.settings.GeometryData.ParticlesStrength[i] = color.b;
      if (this.settings.StrengthChannel != ColorChannel.A)
        return;
      this.settings.GeometryData.ParticlesStrength[i] = color.a;
    }
  }
}
