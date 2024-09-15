// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Demo.DemoCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Demo
{
  public class DemoCamera : MonoBehaviour
  {
    [SerializeField]
    private Vector3 lookAt = new Vector3(0.0f, 0.05f, 0.0f);
    private float radius;
    private float angle = 1.57079637f;
    private float elevation;

    private void Awake()
    {
      this.radius = this.transform.position.z;
      this.elevation = this.transform.position.y;
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
      this.transform.position = new Vector3(Mathf.Cos(this.angle) * this.radius, this.elevation, Mathf.Sin(this.angle) * this.radius);
      this.transform.LookAt(this.lookAt);
      this.HandleWheel();
      this.HandleMove();
    }

    private void HandleWheel() => this.radius += Input.GetAxis("Mouse ScrollWheel");

    private void HandleMove()
    {
      if (!Input.GetMouseButton(0))
        return;
      this.angle -= Input.GetAxis("Mouse X") * Time.deltaTime;
      this.elevation -= Input.GetAxis("Mouse Y") * Time.deltaTime;
    }
  }
}
