// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.GeometryGroupData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
  [Serializable]
  public class GeometryGroupData
  {
    public float Length = 2f;
    public int Segments;
    public List<Vector3> GuideVertices;
    public List<float> Distances;
    public List<Vector3> Vertices;
    public List<Color> Colors;
    [SerializeField]
    private GeometryGroupHistory history = new GeometryGroupHistory();

    public void Generate(Mesh mesh, int segments)
    {
      this.Vertices = new List<Vector3>();
      this.GuideVertices = new List<Vector3>();
      this.Distances = new List<float>();
      List<Vector3> vector3List = new List<Vector3>();
      for (int index = 0; index < mesh.vertices.Length; ++index)
      {
        Vector3 vertex = mesh.vertices[index];
        Vector3 normal = mesh.normals[index];
        if (!vector3List.Contains(vertex))
        {
          List<Vector3> stand = this.CreateStand(vertex, normal, segments);
          this.Vertices.AddRange((IEnumerable<Vector3>) stand);
          this.Distances.Add(this.Length / (float) segments);
          this.GuideVertices.AddRange((IEnumerable<Vector3>) stand);
          vector3List.Add(vertex);
        }
      }
      this.Colors = new List<Color>();
      for (int index = 0; index < this.Vertices.Count; ++index)
        this.Colors.Add(new Color(1f, 1f, 1f));
      Debug.Log((object) ("Total nodes:" + (object) this.Vertices.Count));
    }

    public void Fix()
    {
    }

    public void Reset()
    {
      this.Vertices.Clear();
      this.Vertices = (List<Vector3>) null;
    }

    private List<Vector3> CreateStand(Vector3 start, Vector3 normal, int segments)
    {
      List<Vector3> stand = new List<Vector3>();
      float num = this.Length / (float) segments;
      for (int index = 0; index < segments; ++index)
        stand.Add(start + normal * (num * (float) index));
      return stand;
    }

    public void Record() => this.history.Record(this.Vertices);

    public void Undo()
    {
      if (!this.history.IsUndo)
        return;
      this.Vertices = this.history.Undo();
    }

    public void Redo()
    {
      if (!this.history.IsRedo)
        return;
      this.Vertices = this.history.Redo();
    }

    public bool IsUndo => this.history.IsUndo;

    public bool IsRedo => this.history.IsRedo;

    public void Clear() => this.history.Clear();

    public void OnDrawGizmos(int segments, bool isSelected, Matrix4x4 toWorld)
    {
      this.Segments = segments;
      if (this.Vertices == null)
        return;
      if (this.Colors == null || this.Colors.Count != this.Vertices.Count)
        this.FillColors();
      for (int index = 1; index < this.Vertices.Count; ++index)
      {
        if (index % segments != 0)
        {
          Color color = this.Colors[index];
          Gizmos.color = !isSelected ? Color.grey : color;
          Gizmos.DrawLine(toWorld.MultiplyPoint3x4(this.Vertices[index - 1]), toWorld.MultiplyPoint3x4(this.Vertices[index]));
        }
      }
    }

    private void FillColors()
    {
      this.Colors = new List<Color>();
      for (int index = 0; index < this.Vertices.Count; ++index)
        this.Colors.Add(Color.white);
    }
  }
}
