// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Commands.BuildChainCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Commands
{
  public class BuildChainCommand : IBuildCommand
  {
    private readonly List<IBuildCommand> commands = new List<IBuildCommand>();

    public void Add(IBuildCommand command) => this.commands.Add(command);

    public void Build()
    {
      for (int index = 0; index < this.commands.Count; ++index)
        this.commands[index].Build();
      this.OnBuild();
    }

    public void UpdateSettings()
    {
      for (int index = 0; index < this.commands.Count; ++index)
        this.commands[index].UpdateSettings();
      this.OnUpdateSettings();
    }

    public virtual void Dispatch()
    {
      for (int index = 0; index < this.commands.Count; ++index)
        this.commands[index].Dispatch();
      this.OnDispatch();
    }

    public virtual void FixedDispatch()
    {
      for (int index = 0; index < this.commands.Count; ++index)
        this.commands[index].FixedDispatch();
      this.OnFixedDispatch();
    }

    public void Dispose()
    {
      for (int index = 0; index < this.commands.Count; ++index)
        this.commands[index].Dispose();
      this.OnDispose();
    }

    protected virtual void OnBuild()
    {
    }

    protected virtual void OnUpdateSettings()
    {
    }

    protected virtual void OnDispatch()
    {
    }

    protected virtual void OnFixedDispatch()
    {
    }

    protected virtual void OnDispose()
    {
    }
  }
}
