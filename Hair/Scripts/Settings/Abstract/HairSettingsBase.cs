// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Settings.Abstract.HairSettingsBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;

namespace GPUTools.Hair.Scripts.Settings.Abstract
{
  [Serializable]
  public class HairSettingsBase
  {
    public bool IsVisible;

    public virtual bool Validate() => true;

    public virtual void DrawGizmos()
    {
    }
  }
}
