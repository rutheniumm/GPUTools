// Decompiled with JetBrains decompiler
// Type: GPUTools.HairDemo.Scripts.DemoView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GPUTools.HairDemo.Scripts
{
  public class DemoView : MonoBehaviour
  {
    [SerializeField]
    private Button play;
    [SerializeField]
    private Button stop;
    [SerializeField]
    private Button next;
    [SerializeField]
    private Button prev;
    [SerializeField]
    private Button rotate;
    [SerializeField]
    private GameObject[] styles;
    [SerializeField]
    private ConstantRotation rotation;
    private int currentStyleIndex;

    private void Start()
    {
      this.SetStartStyle();
      this.play.onClick.AddListener(new UnityAction(this.OnClickPlay));
      this.stop.onClick.AddListener(new UnityAction(this.OnClickStop));
      this.next.onClick.AddListener(new UnityAction(this.OnClickNext));
      this.prev.onClick.AddListener(new UnityAction(this.OnClickPrev));
      this.rotate.onClick.AddListener(new UnityAction(this.OnClickRotate));
    }

    private void OnClickRotate()
    {
      this.rotation.Speed += 200f;
      if ((double) this.rotation.Speed < 800.0)
        return;
      this.rotation.Speed = 0.0f;
    }

    private void OnClickPrev() => --this.CurrentStyleIndex;

    private void OnClickNext() => ++this.CurrentStyleIndex;

    private void OnClickStop() => this.CurrentStyle.GetComponent<Animator>().enabled = false;

    private void OnClickPlay() => this.CurrentStyle.GetComponent<Animator>().enabled = true;

    private GameObject CurrentStyle => this.styles[this.currentStyleIndex];

    private int CurrentStyleIndex
    {
      set
      {
        this.currentStyleIndex = value;
        if (this.currentStyleIndex < 0)
          this.currentStyleIndex = this.styles.Length - 1;
        if (this.currentStyleIndex > this.styles.Length - 1)
          this.currentStyleIndex = 0;
        this.ApplyStyle();
      }
      get => this.currentStyleIndex;
    }

    private void ApplyStyle()
    {
      for (int index = 0; index < this.styles.Length; ++index)
        this.styles[index].SetActive(index == this.currentStyleIndex);
    }

    private void SetStartStyle()
    {
      for (int index = 0; index < this.styles.Length; ++index)
      {
        if (this.styles[index].activeSelf)
          this.CurrentStyleIndex = index;
      }
    }
  }
}
