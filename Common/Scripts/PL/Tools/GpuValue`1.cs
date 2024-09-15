// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Tools.GpuValue`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

namespace GPUTools.Common.Scripts.PL.Tools
{
  public class GpuValue<T>
  {
    public GpuValue(T value = null) => this.Value = value;

    public T Value { set; get; }
  }
}
