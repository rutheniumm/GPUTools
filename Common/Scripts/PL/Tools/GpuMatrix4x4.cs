// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Tools.GpuMatrix4x4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
  public class GpuMatrix4x4
  {
    private float[] values = new float[16];

    public GpuMatrix4x4(Matrix4x4 data) => this.Data = data;

    public Matrix4x4 Data { get; set; }

    public float[] Values
    {
      get
      {
        this.values[0] = this.Data[0];
        this.values[1] = this.Data[1];
        this.values[2] = this.Data[2];
        this.values[3] = this.Data[3];
        this.values[4] = this.Data[4];
        this.values[5] = this.Data[5];
        this.values[6] = this.Data[6];
        this.values[7] = this.Data[7];
        this.values[8] = this.Data[8];
        this.values[9] = this.Data[9];
        this.values[10] = this.Data[10];
        this.values[11] = this.Data[11];
        this.values[12] = this.Data[12];
        this.values[13] = this.Data[13];
        this.values[14] = this.Data[14];
        this.values[15] = this.Data[15];
        return this.values;
      }
    }
  }
}
