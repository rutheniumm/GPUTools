// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.CacheProvider`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
  public class CacheProvider<T> where T : Component
  {
    private readonly List<GameObject> providers;
    private List<T> items;

    public CacheProvider(List<GameObject> providers)
    {
      this.providers = providers;
      this.items = this.GetItems();
    }

    public List<T> GetItems()
    {
      List<T> items = new List<T>();
      foreach (GameObject provider in this.providers)
      {
        if ((Object) provider != (Object) null)
          items.AddRange((IEnumerable<T>) ((IEnumerable<T>) provider.GetComponentsInChildren<T>()).ToList<T>());
      }
      return items;
    }

    public List<T> Items
    {
      get
      {
        if (this.items == null)
          this.items = this.GetItems();
        return this.items;
      }
    }

    public static bool Verify(List<GameObject> list)
    {
      if (list.Count == 0)
        return false;
      foreach (Object @object in list)
      {
        if (@object == (Object) null)
          return false;
      }
      return true;
    }
  }
}
