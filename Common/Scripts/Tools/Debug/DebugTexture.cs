// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.DebugTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class DebugTexture : MonoBehaviour
  {
    public static void SetTexture(Texture texture) => new GameObject(nameof (DebugTexture)).AddComponent<DebugTexture>().Texture = texture;

    public Texture Texture { get; set; }

    private void OnGUI() => GUI.DrawTexture(new Rect(0.0f, 0.0f, 400f, 400f), this.Texture, ScaleMode.ScaleToFit, false, 1f);
  }
}
