// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.FixedList`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

namespace GPUTools.Common.Scripts.Tools
{
  public class FixedList<T>
  {
    public FixedList(int size)
    {
      this.Data = new T[size];
      this.Size = size;
      this.Count = 0;
    }

    public int Size { private set; get; }

    public int Count { private set; get; }

    public T[] Data { private set; get; }

    public void Add(T item)
    {
      this.Data[this.Count] = item;
      ++this.Count;
    }

    public T this[int i]
    {
      set => this.Data[i] = value;
      get => this.Data[i];
    }

    public void Reset() => this.Count = 0;

    public bool Contains(T item)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (this.Data[index].Equals((object) item))
          return true;
      }
      return false;
    }
  }
}
