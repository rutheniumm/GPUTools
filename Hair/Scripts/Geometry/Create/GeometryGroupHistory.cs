// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.GeometryGroupHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
  [Serializable]
  public class GeometryGroupHistory
  {
    [SerializeField]
    private readonly List<List<Vector3>> history = new List<List<Vector3>>();
    [SerializeField]
    private int pointer;

    public void Record(List<Vector3> list)
    {
      this.pointer = this.history.Count;
      this.history.Add(list.ToList<Vector3>());
      if (this.history.Count <= 10)
        return;
      this.history.RemoveAt(0);
    }

    public List<Vector3> Undo()
    {
      if (this.pointer > 0)
        --this.pointer;
      return this.history[this.pointer].ToList<Vector3>();
    }

    public List<Vector3> Redo()
    {
      if (this.pointer < this.history.Count - 1)
        ++this.pointer;
      return this.history[this.pointer].ToList<Vector3>();
    }

    public void Clear()
    {
      this.history.Clear();
      this.pointer = 0;
    }

    public bool IsUndo => this.history.Count > 0 && this.pointer > 0;

    public bool IsRedo => this.history.Count > 1 && this.pointer < this.history.Count - 1;
  }
}
