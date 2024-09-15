// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.FloatSmoother
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
  public class FloatSmoother
  {
    private float[] buffer;

    public FloatSmoother(int bufferLength)
    {
      this.buffer = new float[bufferLength];
      for (int index = 0; index < bufferLength; ++index)
        this.buffer[index] = Time.fixedDeltaTime;
    }

    public void AddValue(float value)
    {
      for (int index = 0; index < this.buffer.Length - 1; ++index)
        this.buffer[index] = this.buffer[index + 1];
      this.buffer[this.buffer.Length - 1] = value;
    }

    public float GetSmoothedValue()
    {
      float num = 0.0f;
      for (int index = 0; index < this.buffer.Length; ++index)
        num += this.buffer[index];
      return num / (float) this.buffer.Length;
    }
  }
}
