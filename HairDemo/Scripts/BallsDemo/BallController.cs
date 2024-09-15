// Decompiled with JetBrains decompiler
// Type: GPUTools.HairDemo.Scripts.BallsDemo.BallController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.HairDemo.Scripts.BallsDemo
{
  public class BallController : MonoBehaviour
  {
    private Rigidbody body;

    private void Start() => this.body = this.GetComponent<Rigidbody>();

    private void Update()
    {
      if (Input.GetKey(KeyCode.A))
        this.body.velocity += Vector3.left;
      if (Input.GetKey(KeyCode.D))
        this.body.velocity += Vector3.right;
      if (Input.GetKey(KeyCode.W))
        this.body.velocity += Vector3.forward;
      if (Input.GetKey(KeyCode.S))
        this.body.velocity += Vector3.back;
      this.body.velocity = Vector3.ClampMagnitude(this.body.velocity, 2f);
    }
  }
}
