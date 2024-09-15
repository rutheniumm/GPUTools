// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Commands.CacheChainCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Commands
{
  public class CacheChainCommand : ICacheCommand
  {
    private readonly List<ICacheCommand> commands = new List<ICacheCommand>();

    public void Cache()
    {
      foreach (ICacheCommand command in this.commands)
        command.Cache();
      this.OnCache();
    }

    protected void Add(ICacheCommand command) => this.commands.Add(command);

    protected virtual void OnCache()
    {
    }
  }
}
