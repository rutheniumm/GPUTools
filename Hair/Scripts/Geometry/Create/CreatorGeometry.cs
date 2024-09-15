// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.CreatorGeometry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
  [Serializable]
  public class CreatorGeometry
  {
    public System.Collections.Generic.List<GeometryGroupData> List = new System.Collections.Generic.List<GeometryGroupData>();
    public int SelectedIndex;

    public GeometryGroupData Selected => this.SelectedIndex >= 0 && this.SelectedIndex < this.List.Count ? this.List[this.SelectedIndex] : (GeometryGroupData) null;

    public bool Validate(bool log)
    {
      if (this.List.Count != 0)
        return true;
      if (log)
        Debug.LogError((object) "No geometry was created");
      return false;
    }
  }
}
