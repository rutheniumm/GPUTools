// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Types.ClothVertex
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Cloth.Scripts.Types
{
  public struct ClothVertex
  {
    public Vector3 Position;
    public Vector3 LastPosition;
    public Vector3 Normal;

    public ClothVertex(Vector3 position, Vector3 normal)
    {
      this.Position = position;
      this.LastPosition = position;
      this.Normal = normal;
    }

    public static int Size() => 36;
  }
}
