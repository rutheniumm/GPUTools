// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Attributes.GpuData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;

namespace GPUTools.Common.Scripts.PL.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class GpuData : Attribute
  {
    public string Name;

    public GpuData(string name) => this.Name = name;
  }
}
