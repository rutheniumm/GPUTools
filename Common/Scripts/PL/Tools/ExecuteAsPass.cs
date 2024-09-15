// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Tools.ExecuteAsPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Abstract;
using System;

namespace GPUTools.Common.Scripts.PL.Tools
{
  public class ExecuteAsPass : IPass
  {
    private readonly Action action;

    public ExecuteAsPass(Action action) => this.action = action;

    public void Dispatch() => this.action();

    public void Dispose()
    {
    }
  }
}
