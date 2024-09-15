// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Import.HairGroupsProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Debug;
using GPUTools.Hair.Scripts.Geometry.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Import
{
  [Serializable]
  public class HairGroupsProvider
  {
    [SerializeField]
    public List<MeshFilter> HairFilters = new List<MeshFilter>();
    [SerializeField]
    public List<List<Vector3>> VerticesGroups;
    [SerializeField]
    public List<Vector3> Vertices;
    [SerializeField]
    public List<List<Color>> ColorsGroups;
    [SerializeField]
    public List<Color> Colors;

    public void Process(Matrix4x4 worldToObject)
    {
      this.VerticesGroups = this.InitVerticesGroups(worldToObject);
      this.Vertices = this.InitVertices();
      this.ColorsGroups = this.InitColorGroups();
      this.Colors = this.InitColors();
    }

    private List<List<Vector3>> InitVerticesGroups(Matrix4x4 worldToObject) => this.HairFilters.Select<MeshFilter, List<Vector3>>((Func<MeshFilter, List<Vector3>>) (filter => MeshUtils.GetVerticesInSpace(filter.sharedMesh, filter.transform.localToWorldMatrix, worldToObject))).ToList<List<Vector3>>();

    private List<Vector3> InitVertices() => this.VerticesGroups.SelectMany<List<Vector3>, Vector3>((Func<List<Vector3>, IEnumerable<Vector3>>) (verticesGroup => (IEnumerable<Vector3>) verticesGroup)).ToList<Vector3>();

    private List<List<Color>> InitColorGroups() => this.HairFilters.Select<MeshFilter, List<Color>>((Func<MeshFilter, List<Color>>) (filter => ((IEnumerable<Color>) filter.sharedMesh.colors).ToList<Color>())).ToList<List<Color>>();

    private List<Color> InitColors() => this.ColorsGroups.SelectMany<List<Color>, Color>((Func<List<Color>, IEnumerable<Color>>) (colorsGroup => (IEnumerable<Color>) colorsGroup)).ToList<Color>();

    public bool Validate(bool log)
    {
      if (Validator.TestList<MeshFilter>(this.HairFilters))
        return true;
      if (log)
        UnityEngine.Debug.LogError((object) "Hair list is empty or contains empty elements ");
      return false;
    }
  }
}
