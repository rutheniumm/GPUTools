// Decompiled with JetBrains decompiler
// Type: GPUTools.Demo.Common.Scripts.PlayImageEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Demo.Common.Scripts
{
  public class PlayImageEffects : MonoBehaviour
  {
    [SerializeField]
    private Shader shader;
    private Material material;

    private void Start() => this.material = new Material(this.shader);

    private void OnRenderImage(RenderTexture src, RenderTexture dest) => Graphics.Blit((Texture) src, dest, this.material);
  }
}
