﻿// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Commands.IBuildCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

namespace GPUTools.Common.Scripts.Tools.Commands
{
  public interface IBuildCommand
  {
    void Build();

    void Dispatch();

    void FixedDispatch();

    void UpdateSettings();

    void Dispose();
  }
}
