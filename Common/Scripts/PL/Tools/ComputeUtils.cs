// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.PL.Tools.ComputeUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
  public static class ComputeUtils
  {
    public static ComputeBuffer ToComputeBuffer<T>(
      this T[] array,
      int stride,
      ComputeBufferType type = ComputeBufferType.Default)
    {
      ComputeBuffer computeBuffer = new ComputeBuffer(array.Length, stride, type);
      computeBuffer.SetData((Array) array);
      return computeBuffer;
    }

    public static T[] ToArray<T>(this ComputeBuffer buffer)
    {
      T[] data = new T[buffer.count];
      buffer.GetData((Array) data);
      return data;
    }

    public static void LogBuffer<T>(ComputeBuffer buffer)
    {
      T[] data = new T[buffer.count];
      buffer.GetData((Array) data);
      for (int index = 0; index < data.Length; ++index)
        Debug.Log((object) string.Format("i:{0} val:{1}", (object) index, (object) data[index]));
    }

    public static void LogLargeBuffer<T>(ComputeBuffer buffer)
    {
      T[] data = new T[buffer.count];
      buffer.GetData((Array) data);
      string str = string.Empty;
      for (int index = 1; index <= data.Length; ++index)
      {
        str = str + "|" + (object) data[index - 1];
        if (index % 12 == 0)
        {
          Debug.Log((object) string.Format("from i:{0} values:{1}", (object) index, (object) str));
          str = string.Empty;
        }
      }
    }
  }
}
