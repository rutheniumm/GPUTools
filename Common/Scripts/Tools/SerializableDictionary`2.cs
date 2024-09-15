// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.SerializableDictionary`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
  [Serializable]
  public class SerializableDictionary<TK, TV>
  {
    [SerializeField]
    private List<TK> keys = new List<TK>();
    [SerializeField]
    private List<TV> values = new List<TV>();

    public void Add(TK key, TV value)
    {
      if (this.keys.Contains(key))
        throw new Exception("Key already added");
      this.keys.Add(key);
      this.values.Add(value);
    }

    public TV GetValue(TK key)
    {
      if (!this.keys.Contains(key))
        throw new Exception("Can't found key");
      return this.values[this.keys.IndexOf(key)];
    }
  }
}
