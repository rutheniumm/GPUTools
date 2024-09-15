// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Abstract.PrimitiveBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Abstract
{
  public class PrimitiveBase : IPass
  {
    private readonly List<IPass> passes = new List<IPass>();
    private readonly List<List<KeyValuePair<GpuData, PropertyInfo>>> passesAttributes = new List<List<KeyValuePair<GpuData, PropertyInfo>>>();
    private readonly List<KeyValuePair<GpuData, PropertyInfo>> ownAttributes = new List<KeyValuePair<GpuData, PropertyInfo>>();

    protected void Bind()
    {
      this.CachePassAttributes();
      this.CacheOwnAttributes();
      this.BindAttributes();
    }

    public virtual void Dispatch()
    {
      for (int index = 0; index < this.passes.Count; ++index)
        this.passes[index].Dispatch();
    }

    public virtual void Dispose()
    {
      for (int index = 0; index < this.passes.Count; ++index)
        this.passes[index].Dispose();
    }

    public void AddPass(IPass pass) => this.passes.Add(pass);

    public void RemovePass(IPass pass)
    {
      if (!this.passes.Contains(pass))
        Debug.LogError((object) "Can't find pass");
      else
        this.passes.Remove(pass);
    }

    private void CachePassAttributes()
    {
      this.passesAttributes.Clear();
      for (int index1 = 0; index1 < this.passes.Count; ++index1)
      {
        PropertyInfo[] properties = this.passes[index1].GetType().GetProperties();
        List<KeyValuePair<GpuData, PropertyInfo>> keyValuePairList = new List<KeyValuePair<GpuData, PropertyInfo>>();
        this.passesAttributes.Add(keyValuePairList);
        for (int index2 = 0; index2 < properties.Length; ++index2)
        {
          PropertyInfo element = properties[index2];
          if (Attribute.IsDefined((MemberInfo) element, typeof (GpuData)))
          {
            GpuData customAttribute = (GpuData) Attribute.GetCustomAttribute((MemberInfo) element, typeof (GpuData));
            keyValuePairList.Add(new KeyValuePair<GpuData, PropertyInfo>(customAttribute, element));
          }
        }
      }
    }

    private void CacheOwnAttributes()
    {
      this.ownAttributes.Clear();
      foreach (PropertyInfo property in this.GetType().GetProperties())
      {
        if (Attribute.IsDefined((MemberInfo) property, typeof (GpuData)))
          this.ownAttributes.Add(new KeyValuePair<GpuData, PropertyInfo>((GpuData) Attribute.GetCustomAttribute((MemberInfo) property, typeof (GpuData)), property));
      }
    }

    protected void BindAttributes()
    {
      for (int index1 = 0; index1 < this.ownAttributes.Count; ++index1)
      {
        KeyValuePair<GpuData, PropertyInfo> ownAttribute = this.ownAttributes[index1];
        for (int index2 = 0; index2 < this.passesAttributes.Count; ++index2)
        {
          List<KeyValuePair<GpuData, PropertyInfo>> passesAttribute = this.passesAttributes[index2];
          for (int index3 = 0; index3 < passesAttribute.Count; ++index3)
          {
            KeyValuePair<GpuData, PropertyInfo> keyValuePair = passesAttribute[index3];
            if (keyValuePair.Key.Name.Equals(ownAttribute.Key.Name))
              keyValuePair.Value.SetValue((object) this.passes[index2], ownAttribute.Value.GetValue((object) this, (object[]) null), (object[]) null);
          }
        }
      }
    }
  }
}
