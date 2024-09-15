// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Tools.GroupedData`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace GPUTools.Common.Scripts.PL.Tools
{
  public class GroupedData<T> where T : IGroupItem
  {
    public List<GroupData> GroupsData = new List<GroupData>();
    public List<List<T>> Groups = new List<List<T>>();

    public void AddGroup(List<T> list) => this.Groups.Add(list);

    public void Add(T item)
    {
      for (int index1 = 0; index1 < this.Groups.Count; ++index1)
      {
        List<T> group = this.Groups[index1];
        bool flag = false;
        for (int index2 = 0; index2 < group.Count; ++index2)
        {
          T obj = group[index2];
          if (item.HasConflict((IGroupItem) obj))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          group.Add(item);
          return;
        }
      }
      this.Groups.Add(new List<T>() { item });
    }

    public T[] Data
    {
      get
      {
        List<T> objList = new List<T>();
        foreach (List<T> group in this.Groups)
        {
          this.GroupsData.Add(new GroupData(objList.Count, group.Count));
          objList.AddRange((IEnumerable<T>) group);
        }
        return objList.ToArray();
      }
    }

    public void Dispose()
    {
    }
  }
}
