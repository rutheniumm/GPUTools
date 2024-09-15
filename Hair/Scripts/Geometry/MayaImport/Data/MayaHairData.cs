// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.Data.MayaHairData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Data
{
  [Serializable]
  public class MayaHairData
  {
    [SerializeField]
    public int Segments;
    [SerializeField]
    public List<Vector3> Lines;
    [SerializeField]
    public List<Vector3> TringlesCenters;
    [SerializeField]
    public int[] HairRootToScalpMap;
    [SerializeField]
    public int[] Indices;
    [SerializeField]
    public List<Vector3> Vertices;

    public bool Validate(bool log)
    {
      if (this.Indices != null && this.Indices.Length != 0)
        return true;
      if (log)
        Debug.LogError((object) "Maya data was not generated succesfuly");
      return false;
    }
  }
}
