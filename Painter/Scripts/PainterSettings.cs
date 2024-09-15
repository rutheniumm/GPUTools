// Decompiled with JetBrains decompiler
// Type: GPUTools.Painter.Scripts.PainterSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Painter.Scripts
{
  public class PainterSettings : MonoBehaviour
  {
    [SerializeField]
    public MeshProvider MeshProvider = new MeshProvider();
    [SerializeField]
    public ColorBrush Brush;
    [SerializeField]
    public Color[] Colors;

    public Material SharedMaterial => this.GetComponent<Renderer>().sharedMaterial;

    private void Start()
    {
    }

    private void Update()
    {
    }
  }
}
