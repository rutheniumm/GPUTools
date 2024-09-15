﻿// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Commands.BuildPlanes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
  public class BuildPlanes : BuildChainCommand
  {
    private readonly ClothSettings settings;

    public BuildPlanes(ClothSettings settings) => this.settings = settings;

    protected override void OnBuild() => this.settings.Runtime.Planes = GPUCollidersManager.planesBuffer;

    protected override void OnFixedDispatch() => this.settings.Runtime.Planes = GPUCollidersManager.planesBuffer;
  }
}
