// Decompiled with JetBrains decompiler
// Type: GPUTools.ClothDemo.Scripts.OpenSceneButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GPUTools.ClothDemo.Scripts
{
  public class OpenSceneButton : MonoBehaviour
  {
    [SerializeField]
    private string sceneName;

    public void Start() => this.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));

    private void OnClick() => SceneManager.LoadScene(this.sceneName);
  }
}
