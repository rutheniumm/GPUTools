// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Tools.GpuBuffer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
  public class GpuBuffer<T> : IBufferWrapper where T : struct
  {
    public GpuBuffer(ComputeBuffer computeBuffer) => this.ComputeBuffer = computeBuffer;

    public GpuBuffer(int count, int stride)
    {
      this.Data = new T[count];
      this.ComputeBuffer = new ComputeBuffer(count, stride);
      this.ComputeBuffer.SetData((Array) this.Data);
    }

    public GpuBuffer(T[] data, int stride)
    {
      this.Data = data;
      this.ComputeBuffer = new ComputeBuffer(data.Length, stride);
      this.ComputeBuffer.SetData((Array) data);
    }

    public ComputeBuffer ComputeBuffer { private set; get; }

    public T[] Data { set; get; }

    public void PushData() => this.ComputeBuffer.SetData((Array) this.Data);

    public void PullData() => this.ComputeBuffer.GetData((Array) this.Data);

    public void Dispose() => this.ComputeBuffer.Dispose();

    public int Count => this.ComputeBuffer.count;
  }
}
