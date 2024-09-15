// Decompiled with JetBrains decompiler
// Type: GPUTools.ClothDemo.Scripts.TestKernels
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.ClothDemo.Scripts
{
  public class TestKernels : MonoBehaviour
  {
    private TestPrimitive primitive;
    private GpuBuffer<GPParticle> buffer;

    private void Start()
    {
      GPParticle[] data = new GPParticle[6849];
      for (int index = 0; index < data.Length; ++index)
        data[index] = new GPParticle(Vector3.left * 0.1f * (float) index, 0.1f);
      this.buffer = new GpuBuffer<GPParticle>(data, GPParticle.Size());
      this.primitive = new TestPrimitive();
      this.primitive.Particles = this.buffer;
      this.primitive.Dt = new GpuValue<float>(0.02f);
      this.primitive.InvDrag = new GpuValue<float>(1f);
      this.primitive.Gravity = new GpuValue<Vector3>(Vector3.down * (1f / 1000f));
      this.primitive.Wind = new GpuValue<Vector3>(Vector3.zero);
      this.primitive.Start();
    }

    private void Update()
    {
      this.primitive.Dt.Value = Time.deltaTime;
      this.primitive.Dispatch();
      this.buffer.PullData();
    }

    private void OnDrawGizmos()
    {
      if (this.buffer == null)
        return;
      for (int index = 0; index < this.buffer.Data.Length; ++index)
        Gizmos.DrawWireSphere(this.buffer.Data[index].Position, 0.1f);
    }
  }
}
