// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.BuildPhysicsBlend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Painter.Scripts;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class BuildPhysicsBlend : BuildChainCommand
  {
    private readonly ClothSettings settings;

    public BuildPhysicsBlend(ClothSettings settings) => this.settings = settings;

    protected override void OnUpdateSettings() => this.OnBuild();

    protected override void OnBuild()
    {
      if (this.settings.EditorType == ClothEditorType.Texture)
        this.BlendFromTexture();
      if (this.settings.EditorType == ClothEditorType.Painter)
        this.BlendFromPainter();
      if (this.settings.EditorType != ClothEditorType.Provider)
        return;
      this.BlendFromProvider();
    }

    private void BlendFromPainter()
    {
      if ((Object) this.settings.EditorPainter == (Object) null)
      {
        Debug.LogError((object) "Painter field is uninitialized");
      }
      else
      {
        for (int i = 0; i < this.settings.GeometryData.ParticlesBlend.Length; ++i)
        {
          Color color = this.settings.EditorPainter.Colors[i];
          this.SetBlend(i, color);
        }
      }
    }

    private void BlendFromTexture()
    {
      Texture2D editorTexture = this.settings.EditorTexture;
      Vector2[] uv = this.settings.MeshProvider.Mesh.uv;
      for (int i = 0; i < uv.Length; ++i)
      {
        Vector2 vector2 = uv[i];
        Color pixelBilinear = editorTexture.GetPixelBilinear(vector2.x, vector2.y);
        this.SetBlend(i, pixelBilinear);
      }
      if (uv.Length != 0)
        return;
      Debug.LogWarning((object) "Add uv to mesh to use vertices blend");
    }

    private void BlendFromProvider()
    {
      Color[] simColors = this.settings.MeshProvider.SimColors;
      if (simColors != null)
      {
        for (int i = 0; i < this.settings.GeometryData.ParticlesBlend.Length; ++i)
        {
          Color color = simColors[i];
          this.SetBlend(i, color);
        }
      }
      else
      {
        for (int i = 0; i < this.settings.GeometryData.ParticlesBlend.Length; ++i)
          this.SetZeroBlend(i);
      }
    }

    private void SetBlend(int i, Color color)
    {
      if (this.settings.SimulateVsKinematicChannel == ColorChannel.R)
        this.settings.GeometryData.ParticlesBlend[i] = color.r;
      if (this.settings.SimulateVsKinematicChannel == ColorChannel.G)
        this.settings.GeometryData.ParticlesBlend[i] = color.g;
      if (this.settings.SimulateVsKinematicChannel == ColorChannel.B)
        this.settings.GeometryData.ParticlesBlend[i] = color.b;
      if (this.settings.SimulateVsKinematicChannel != ColorChannel.A)
        return;
      this.settings.GeometryData.ParticlesBlend[i] = color.a;
    }

    private void SetZeroBlend(int i) => this.settings.GeometryData.ParticlesBlend[i] = 0.0f;
  }
}
