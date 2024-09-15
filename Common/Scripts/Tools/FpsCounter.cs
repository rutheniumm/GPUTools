// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.FpsCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace GPUTools.Common.Scripts.Tools
{
  public class FpsCounter : MonoBehaviour
  {
    [SerializeField]
    private Text textField;
    private float deltaTime;

    private void Update()
    {
      this.deltaTime += (float) (((double) Time.deltaTime - (double) this.deltaTime) * 0.10000000149011612);
      this.textField.text = string.Format("{0:0.0} ms ({1:0.} fps)", (object) (this.deltaTime * 1000f), (object) (1f / this.deltaTime));
    }
  }
}
