// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Abstract.KernelBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Abstract
{
  public class KernelBase : IPass
  {
    protected readonly string KernalName;
    protected readonly int KernelId;
    protected readonly List<KeyValuePair<GpuData, object>> Props = new List<KeyValuePair<GpuData, object>>();
    protected readonly Dictionary<IBufferWrapper, string> BufferToLengthAttributeName = new Dictionary<IBufferWrapper, string>();

    public KernelBase(string shaderPath, string kernelName)
    {
      this.Shader = Resources.Load<ComputeShader>(shaderPath);
      this.KernalName = kernelName;
      this.KernelId = this.Shader.FindKernel(kernelName);
      this.IsEnabled = true;
    }

    public bool IsEnabled { get; set; }

    protected ComputeShader Shader { get; private set; }

    public virtual void Dispatch()
    {
      if (!this.IsEnabled)
        return;
      if (this.Props.Count == 0)
        this.CacheAttributes();
      this.BindAttributes();
      int groupsNumX = this.GetGroupsNumX();
      int groupsNumY = this.GetGroupsNumY();
      int groupsNumZ = this.GetGroupsNumZ();
      if (groupsNumX == 0 || groupsNumY == 0 || groupsNumZ == 0)
        return;
      this.Shader.Dispatch(this.KernelId, groupsNumX, groupsNumY, groupsNumZ);
    }

    public virtual void Dispose()
    {
    }

    public virtual int GetGroupsNumX() => 1;

    public virtual int GetGroupsNumY() => 1;

    public virtual int GetGroupsNumZ() => 1;

    public void ClearCacheAttributes() => this.CacheAttributes();

    protected virtual void CacheAttributes()
    {
      this.Props.Clear();
      this.BufferToLengthAttributeName.Clear();
      foreach (PropertyInfo property in this.GetType().GetProperties())
      {
        if (Attribute.IsDefined((MemberInfo) property, typeof (GpuData)))
        {
          GpuData customAttribute = (GpuData) Attribute.GetCustomAttribute((MemberInfo) property, typeof (GpuData));
          object key = property.GetValue((object) this, (object[]) null);
          if (key is IBufferWrapper)
            this.BufferToLengthAttributeName.Add(key as IBufferWrapper, customAttribute.Name + "Length");
          this.Props.Add(new KeyValuePair<GpuData, object>(customAttribute, key));
        }
      }
    }

    protected void BindAttributes()
    {
      for (int index = 0; index < this.Props.Count; ++index)
      {
        GpuData key1 = this.Props[index].Key;
        object obj = this.Props[index].Value;
        switch (obj)
        {
          case IBufferWrapper _:
            IBufferWrapper key2 = (IBufferWrapper) obj;
            ComputeBuffer computeBuffer = key2.ComputeBuffer;
            if (computeBuffer != null)
            {
              if (!computeBuffer.IsValid())
              {
                Debug.LogError((object) ("Compute buffer " + (object) computeBuffer.GetHashCode() + " is not valid for " + this.KernalName + " " + key1.Name));
                break;
              }
              this.Shader.SetBuffer(this.KernelId, key1.Name, computeBuffer);
              string name;
              if (this.BufferToLengthAttributeName.TryGetValue(key2, out name))
              {
                this.Shader.SetInt(name, computeBuffer.count);
                break;
              }
              break;
            }
            Debug.LogError((object) ("Null compute buffer for " + this.KernalName));
            break;
          case Texture _:
            this.Shader.SetTexture(this.KernelId, key1.Name, (Texture) obj);
            break;
          case GpuValue<int> _:
            this.Shader.SetInt(key1.Name, ((GpuValue<int>) obj).Value);
            break;
          case GpuValue<float> _:
            this.Shader.SetFloat(key1.Name, ((GpuValue<float>) obj).Value);
            break;
          case GpuValue<Vector3> _:
            this.Shader.SetVector(key1.Name, (Vector4) ((GpuValue<Vector3>) obj).Value);
            break;
          case GpuValue<Color> _:
            this.Shader.SetVector(key1.Name, ((GpuValue<Color>) obj).Value.ToVector());
            break;
          case GpuValue<bool> _:
            this.Shader.SetBool(key1.Name, ((GpuValue<bool>) obj).Value);
            break;
          case GpuValue<GpuMatrix4x4> _:
            this.Shader.SetFloats(key1.Name, ((GpuValue<GpuMatrix4x4>) obj).Value.Values);
            break;
        }
      }
    }
  }
}
