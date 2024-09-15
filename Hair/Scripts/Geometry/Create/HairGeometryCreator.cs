// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.HairGeometryCreator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
  [Serializable]
  public class HairGeometryCreator : GeometryProviderBase
  {
    [SerializeField]
    public bool DebugDraw;
    [SerializeField]
    public bool DebugDrawUnselectedGroups = true;
    [SerializeField]
    public int Segments = 5;
    [SerializeField]
    public GeometryBrush Brush = new GeometryBrush();
    [SerializeField]
    public MeshProvider ScalpProvider = new MeshProvider();
    [SerializeField]
    public List<GameObject> ColliderProviders = new List<GameObject>();
    [SerializeField]
    public CreatorGeometry Geomery = new CreatorGeometry();
    [SerializeField]
    public Bounds Bounds;
    [SerializeField]
    public float NearbyVertexSearchMinDistance;
    [SerializeField]
    public float NearbyVertexSearchDistance = 0.01f;
    [SerializeField]
    public int MaxNearbyVertsPerVert = 2;
    [SerializeField]
    private int[] indices;
    [SerializeField]
    private List<Vector3> vertices;
    [SerializeField]
    private List<Color> colors;
    [SerializeField]
    private int[] hairRootToScalpIndices;
    [SerializeField]
    public List<Vector4ListContainer> nearbyVertexGroups;
    [SerializeField]
    private bool isProcessed;

    private void Awake()
    {
      if (this.isProcessed || !Application.isPlaying)
        return;
      this.Process();
    }

    public void Optimize() => this.Process();

    public void SetDirty() => this.isProcessed = false;

    public void ClearNearbyVertices()
    {
      this.nearbyVertexGroups = (List<Vector4ListContainer>) null;
      this.isProcessed = false;
    }

    public bool IsDirty() => !this.isProcessed;

    public void Process()
    {
      Debug.Log((object) "Hair Geometry Creator Process() called");
      if (!this.ScalpProvider.Validate(true))
        return;
      List<List<Vector3>> hairVerticesGroups = new List<List<Vector3>>();
      List<Vector3> vector3List = new List<Vector3>();
      List<Color> colorList = new List<Color>();
      foreach (GeometryGroupData geometryGroupData in this.Geomery.List)
      {
        hairVerticesGroups.Add(geometryGroupData.Vertices);
        vector3List.AddRange((IEnumerable<Vector3>) geometryGroupData.Vertices);
        colorList.AddRange((IEnumerable<Color>) geometryGroupData.Colors);
      }
      this.vertices = vector3List;
      this.colors = colorList;
      Mesh mesh = this.ScalpProvider.Mesh;
      float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(mesh) * 0.1f;
      this.indices = ScalpProcessingTools.ProcessIndices(((IEnumerable<int>) mesh.GetIndices(0)).ToList<int>(), ((IEnumerable<Vector3>) mesh.vertices).ToList<Vector3>(), hairVerticesGroups, this.Segments, accuracy).ToArray();
      this.hairRootToScalpIndices = this.ScalpProvider.Type != ScalpMeshType.Skinned ? (this.ScalpProvider.Type != ScalpMeshType.PreCalc ? new int[this.vertices.Count / this.GetSegmentsNum()] : ScalpProcessingTools.HairRootToScalpIndices(((IEnumerable<Vector3>) mesh.vertices).ToList<Vector3>(), this.vertices, this.Segments, accuracy).ToArray()) : ScalpProcessingTools.HairRootToScalpIndices(((IEnumerable<Vector3>) mesh.vertices).ToList<Vector3>(), this.vertices, this.Segments, accuracy).ToArray();
      this.CalculateNearbyVertexGroups();
      this.isProcessed = true;
    }

    public override void Dispatch()
    {
      if (this.ScalpProvider.Type == ScalpMeshType.PreCalc && (UnityEngine.Object) this.ScalpProvider.PreCalcProvider != (UnityEngine.Object) null)
        this.ScalpProvider.PreCalcProvider.provideToWorldMatrices = true;
      this.ScalpProvider.Dispatch();
    }

    public override bool Validate(bool log) => this.ScalpProvider.Validate(log) && this.Geomery.Validate(log);

    private void OnDestroy() => this.ScalpProvider.Dispose();

    public override Bounds GetBounds() => this.transform.TransformBounds(this.Bounds);

    public override int GetSegmentsNum() => this.Segments;

    public override int GetStandsNum() => this.vertices.Count / this.Segments;

    public override int[] GetIndices() => this.indices;

    public override List<Vector3> GetVertices() => this.vertices;

    public override void SetVertices(List<Vector3> verts) => this.vertices = verts;

    public override List<float> GetRigidities() => (List<float>) null;

    public override void SetRigidities(List<float> rigidities) => throw new NotImplementedException();

    private bool AddToHashSet(
      HashSet<Vector4> set,
      int i1,
      int i2,
      float distance,
      float distanceRatio)
    {
      return i1 != -1 && i2 != -1 && set.Add(i1 <= i2 ? new Vector4((float) i2, (float) i1, distance, distanceRatio) : new Vector4((float) i1, (float) i2, distance, distanceRatio));
    }

    public override void CalculateNearbyVertexGroups()
    {
      this.nearbyVertexGroups = new List<Vector4ListContainer>();
      Matrix4x4 toWorldMatrix = this.GetToWorldMatrix();
      List<Vector3> vector3List = new List<Vector3>();
      foreach (Vector3 vertex in this.vertices)
      {
        Vector3 vector3 = toWorldMatrix.MultiplyPoint3x4(vertex);
        vector3List.Add(vector3);
      }
      HashSet<Vector4> vector4Set = new HashSet<Vector4>();
      for (int index1 = 0; index1 < vector3List.Count; ++index1)
      {
        if (index1 % this.Segments != 0)
        {
          int num1 = index1 / this.Segments;
          List<HairGeometryCreator.VertDistance> vertDistanceList = new List<HairGeometryCreator.VertDistance>();
          for (int index2 = 0; index2 < vector3List.Count; ++index2)
          {
            if (index2 % this.Segments != 0)
            {
              int num2 = index2 / this.Segments;
              if (num1 != num2)
              {
                float num3 = Vector3.Distance(vector3List[index1], vector3List[index2]);
                if ((double) num3 < (double) this.NearbyVertexSearchDistance && (double) num3 > (double) this.NearbyVertexSearchMinDistance)
                {
                  HairGeometryCreator.VertDistance vertDistance;
                  vertDistance.vert = index2;
                  vertDistance.distance = num3;
                  vertDistanceList.Add(vertDistance);
                }
              }
            }
          }
          vertDistanceList.Sort((Comparison<HairGeometryCreator.VertDistance>) ((vd1, vd2) => vd1.distance.CompareTo(vd2.distance)));
          int num4 = 0;
          foreach (HairGeometryCreator.VertDistance vertDistance in vertDistanceList)
          {
            if (num4 <= this.MaxNearbyVertsPerVert)
            {
              this.AddToHashSet(vector4Set, index1, vertDistance.vert, vertDistance.distance, (this.NearbyVertexSearchDistance - vertDistance.distance) / this.NearbyVertexSearchDistance);
              ++num4;
            }
            else
              break;
          }
        }
      }
      Debug.Log((object) ("Found " + (object) vector4Set.Count + " nearby vertex pairs"));
      List<Vector4> list = vector4Set.ToList<Vector4>();
      List<HashSet<int>> intSetList = new List<HashSet<int>>();
      foreach (Vector4 vector4 in list)
      {
        bool flag = false;
        int x = (int) vector4.x;
        int y = (int) vector4.y;
        for (int index = 0; index < intSetList.Count; ++index)
        {
          HashSet<int> intSet = intSetList[index];
          if (!intSet.Contains(x) && !intSet.Contains(y))
          {
            flag = true;
            intSet.Add(x);
            intSet.Add(y);
            this.nearbyVertexGroups[index].List.Add(vector4);
            break;
          }
        }
        if (!flag)
        {
          HashSet<int> intSet = new HashSet<int>();
          intSetList.Add(intSet);
          intSet.Add(x);
          intSet.Add(y);
          this.nearbyVertexGroups.Add(new Vector4ListContainer()
          {
            List = {
              vector4
            }
          });
        }
      }
      Debug.Log((object) ("Created " + (object) this.nearbyVertexGroups.Count + " nearby vertex pair groups"));
    }

    public override List<Vector4ListContainer> GetNearbyVertexGroups()
    {
      if (this.nearbyVertexGroups == null || this.nearbyVertexGroups.Count == 0)
      {
        Debug.Log((object) "Vertex groups not precalculated. Must build at runtime which is slow");
        this.CalculateNearbyVertexGroups();
      }
      return this.nearbyVertexGroups;
    }

    public override List<Color> GetColors() => this.colors;

    public override Matrix4x4 GetToWorldMatrix() => this.ScalpProvider.ToWorldMatrix;

    public override GpuBuffer<Matrix4x4> GetTransformsBuffer() => this.ScalpProvider.ToWorldMatricesBuffer;

    public override GpuBuffer<Vector3> GetNormalsBuffer() => this.ScalpProvider.NormalsBuffer;

    public override int[] GetHairRootToScalpMap() => this.hairRootToScalpIndices;

    private void OnDrawGizmos()
    {
      if (!this.DebugDraw || !this.ScalpProvider.Validate(false))
        return;
      foreach (GeometryGroupData geometryGroupData in this.Geomery.List)
      {
        bool isSelected = this.Geomery.Selected == geometryGroupData;
        if (isSelected || this.DebugDrawUnselectedGroups)
          geometryGroupData.OnDrawGizmos(this.Segments, isSelected, this.ScalpProvider.ToWorldMatrix);
      }
      Bounds bounds = this.GetBounds();
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    protected struct VertDistance
    {
      public int vert;
      public float distance;
    }
  }
}
