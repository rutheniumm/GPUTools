// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Create.RuntimeHairGeometryCreator
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
using System.IO;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
  public class RuntimeHairGeometryCreator : GeometryProviderBase
  {
    protected bool _usingAuxData;
    [SerializeField]
    protected int _Segments = 5;
    [SerializeField]
    protected float _SegmentLength = 0.02f;
    protected string _ScalpProviderName;
    protected bool[] _enabledIndices;
    public Transform[] ScalpProviders;
    [SerializeField]
    protected PreCalcMeshProvider _ScalpProvider;
    public Bounds Bounds;
    public float NearbyVertexSearchMinDistance;
    public float NearbyVertexSearchDistance = 0.01f;
    public int MaxNearbyVertsPerVert = 4;
    [NonSerialized]
    public RuntimeHairGeometryCreator.ScalpMask strandsMask;
    [NonSerialized]
    public RuntimeHairGeometryCreator.ScalpMask strandsMaskWorking;
    [NonSerialized]
    public RuntimeHairGeometryCreator.Strand[] strands;
    private int[] indices;
    private List<Vector3> vertices;
    private List<Vector3> verticesLoaded;
    private List<Color> colors;
    private int[] hairRootToScalpIndices;
    private List<Vector4ListContainer> nearbyVertexGroups;
    private List<Vector4ListContainer> nearbyVertexGroupsLoaded;
    private List<float> rigidities;
    private List<float> rigiditiesLoaded;
    public bool DebugDraw;
    public bool DebugDrawUnselectedGroups = true;
    private bool isProcessed;
    public string status = string.Empty;
    protected bool cancelCalculateNearbyVertexGroups;
    protected Matrix4x4 toWorldMatrix;

    public void StoreToBinaryWriter(BinaryWriter binWriter)
    {
      binWriter.Write(nameof (RuntimeHairGeometryCreator));
      if (this.rigidities == null)
        binWriter.Write("1.0");
      else
        binWriter.Write("1.1");
      binWriter.Write(this._ScalpProviderName);
      binWriter.Write(this._Segments);
      binWriter.Write(this.SegmentLength);
      this.strandsMask.StoreToBinaryWriter(binWriter);
      binWriter.Write(this.strands.Length);
      for (int index = 0; index < this.strands.Length; ++index)
        this.strands[index].StoreToBinaryWriter(binWriter);
      binWriter.Write(this.indices.Length);
      for (int index = 0; index < this.indices.Length; ++index)
        binWriter.Write(this.indices[index]);
      binWriter.Write(this.vertices.Count);
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        binWriter.Write(this.vertices[index].x);
        binWriter.Write(this.vertices[index].y);
        binWriter.Write(this.vertices[index].z);
      }
      if (this.rigidities != null)
      {
        binWriter.Write(this.rigidities.Count);
        for (int index = 0; index < this.rigidities.Count; ++index)
          binWriter.Write(this.rigidities[index]);
      }
      binWriter.Write(this.hairRootToScalpIndices.Length);
      for (int index = 0; index < this.hairRootToScalpIndices.Length; ++index)
        binWriter.Write(this.hairRootToScalpIndices[index]);
      if (this.nearbyVertexGroups != null)
      {
        binWriter.Write(this.nearbyVertexGroups.Count);
        for (int index1 = 0; index1 < this.nearbyVertexGroups.Count; ++index1)
        {
          List<Vector4> list = this.nearbyVertexGroups[index1].List;
          binWriter.Write(list.Count);
          for (int index2 = 0; index2 < list.Count; ++index2)
          {
            binWriter.Write(list[index2].x);
            binWriter.Write(list[index2].y);
            binWriter.Write(list[index2].z);
            binWriter.Write(list[index2].w);
          }
        }
      }
      else
        binWriter.Write(0);
    }

    public void StoreAuxToBinaryWriter(BinaryWriter binWriter)
    {
      this._usingAuxData = true;
      binWriter.Write("RuntimeHairGeometryCreatorAux");
      if (this.rigidities == null)
        binWriter.Write("1.0");
      else
        binWriter.Write("1.1");
      binWriter.Write(this.vertices.Count);
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        binWriter.Write(this.vertices[index].x);
        binWriter.Write(this.vertices[index].y);
        binWriter.Write(this.vertices[index].z);
      }
      if (this.rigidities != null)
      {
        binWriter.Write(this.rigidities.Count);
        for (int index = 0; index < this.rigidities.Count; ++index)
          binWriter.Write(this.rigidities[index]);
      }
      if (this.nearbyVertexGroups != null)
      {
        binWriter.Write(this.nearbyVertexGroups.Count);
        for (int index1 = 0; index1 < this.nearbyVertexGroups.Count; ++index1)
        {
          List<Vector4> list = this.nearbyVertexGroups[index1].List;
          binWriter.Write(list.Count);
          for (int index2 = 0; index2 < list.Count; ++index2)
          {
            binWriter.Write(list[index2].x);
            binWriter.Write(list[index2].y);
            binWriter.Write(list[index2].z);
            binWriter.Write(list[index2].w);
          }
        }
      }
      else
        binWriter.Write(0);
    }

    public bool usingAuxData => this._usingAuxData;

    public void LoadFromBinaryReader(BinaryReader binReader)
    {
      this._usingAuxData = false;
      string str = !(binReader.ReadString() != nameof (RuntimeHairGeometryCreator)) ? binReader.ReadString() : throw new Exception("Invalid binary format for binary data passed to RuntimeHairGeometryCreator");
      bool flag1 = false;
      if (str != "1.0" && str != "1.1")
        throw new Exception("Invalid schema version " + str + " for binary data passed to RuntimeHairGeometryCreator");
      if (str == "1.1")
        flag1 = true;
      this._ScalpProviderName = binReader.ReadString();
      bool flag2 = false;
      foreach (Transform scalpProvider in this.ScalpProviders)
      {
        if (scalpProvider.name == this._ScalpProviderName)
        {
          scalpProvider.gameObject.SetActive(true);
          PreCalcMeshProvider component = scalpProvider.GetComponent<PreCalcMeshProvider>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            flag2 = true;
            this.ScalpProvider = component;
          }
        }
        else
          scalpProvider.gameObject.SetActive(false);
      }
      if (!flag2)
      {
        this.ScalpProvider = (PreCalcMeshProvider) null;
        throw new Exception("Could not find scalp provider " + this._ScalpProviderName);
      }
      this._Segments = binReader.ReadInt32();
      this.SegmentLength = binReader.ReadSingle();
      this.strandsMask.LoadFromBinaryReader(binReader);
      int num1 = binReader.ReadInt32();
      int num2 = !this._ScalpProvider.useBaseMesh ? this._ScalpProvider.Mesh.vertexCount : this._ScalpProvider.BaseMesh.vertexCount;
      if (num1 != num2)
        throw new Exception("Binary hair data not compatible with chosen scalp mesh");
      for (int index = 0; index < num1; ++index)
        this.strands[index].LoadFromBinaryReader(binReader);
      int length1 = binReader.ReadInt32();
      this.indices = new int[length1];
      for (int index = 0; index < length1; ++index)
        this.indices[index] = binReader.ReadInt32();
      int num3 = binReader.ReadInt32();
      this.vertices = new List<Vector3>();
      for (int index = 0; index < num3; ++index)
      {
        Vector3 vector3;
        vector3.x = binReader.ReadSingle();
        vector3.y = binReader.ReadSingle();
        vector3.z = binReader.ReadSingle();
        this.vertices.Add(vector3);
      }
      this.verticesLoaded = this.vertices;
      if (flag1)
      {
        int num4 = binReader.ReadInt32();
        if ((double) num4 == 0.0)
        {
          this.rigidities = (List<float>) null;
        }
        else
        {
          this.rigidities = new List<float>();
          for (int index = 0; index < num4; ++index)
            this.rigidities.Add(binReader.ReadSingle());
        }
      }
      else
        this.rigidities = (List<float>) null;
      this.rigiditiesLoaded = this.rigidities;
      int length2 = binReader.ReadInt32();
      this.hairRootToScalpIndices = new int[length2];
      for (int index = 0; index < length2; ++index)
        this.hairRootToScalpIndices[index] = binReader.ReadInt32();
      int num5 = binReader.ReadInt32();
      this.nearbyVertexGroups = new List<Vector4ListContainer>();
      for (int index1 = 0; index1 < num5; ++index1)
      {
        Vector4ListContainer vector4ListContainer = new Vector4ListContainer();
        List<Vector4> vector4List = new List<Vector4>();
        vector4ListContainer.List = vector4List;
        this.nearbyVertexGroups.Add(vector4ListContainer);
        int num6 = binReader.ReadInt32();
        for (int index2 = 0; index2 < num6; ++index2)
        {
          Vector4 vector4;
          vector4.x = binReader.ReadSingle();
          vector4.y = binReader.ReadSingle();
          vector4.z = binReader.ReadSingle();
          vector4.w = binReader.ReadSingle();
          vector4List.Add(vector4);
        }
      }
      this.nearbyVertexGroupsLoaded = this.nearbyVertexGroups;
    }

    public void RevertToLoadedData()
    {
      if (!this._usingAuxData || this.verticesLoaded == null)
        return;
      this._usingAuxData = false;
      this.vertices = this.verticesLoaded;
      this.rigidities = this.rigiditiesLoaded;
      this.nearbyVertexGroups = this.nearbyVertexGroupsLoaded;
    }

    public void LoadAuxFromBinaryReader(BinaryReader binReader)
    {
      this._usingAuxData = true;
      string str = !(binReader.ReadString() != "RuntimeHairGeometryCreatorAux") ? binReader.ReadString() : throw new Exception("Invalid aux binary format for binary data passed to RuntimeHairGeometryCreator");
      bool flag = false;
      if (str != "1.0" && str != "1.1")
        throw new Exception("Invalid schema version " + str + " for binary data passed to RuntimeHairGeometryCreator");
      if (str == "1.1")
        flag = true;
      int num1 = binReader.ReadInt32();
      this.vertices = new List<Vector3>();
      for (int index = 0; index < num1; ++index)
      {
        Vector3 vector3;
        vector3.x = binReader.ReadSingle();
        vector3.y = binReader.ReadSingle();
        vector3.z = binReader.ReadSingle();
        this.vertices.Add(vector3);
      }
      if (flag)
      {
        int num2 = binReader.ReadInt32();
        if ((double) num2 == 0.0)
        {
          this.rigidities = (List<float>) null;
        }
        else
        {
          this.rigidities = new List<float>();
          for (int index = 0; index < num2; ++index)
            this.rigidities.Add(binReader.ReadSingle());
        }
      }
      else
        this.rigidities = (List<float>) null;
      int num3 = binReader.ReadInt32();
      this.nearbyVertexGroups = new List<Vector4ListContainer>();
      for (int index1 = 0; index1 < num3; ++index1)
      {
        Vector4ListContainer vector4ListContainer = new Vector4ListContainer();
        List<Vector4> vector4List = new List<Vector4>();
        vector4ListContainer.List = vector4List;
        this.nearbyVertexGroups.Add(vector4ListContainer);
        int num4 = binReader.ReadInt32();
        for (int index2 = 0; index2 < num4; ++index2)
        {
          Vector4 vector4;
          vector4.x = binReader.ReadSingle();
          vector4.y = binReader.ReadSingle();
          vector4.z = binReader.ReadSingle();
          vector4.w = binReader.ReadSingle();
          vector4List.Add(vector4);
        }
      }
    }

    public int Segments
    {
      get => this._Segments;
      set
      {
        if (this._Segments == value)
          return;
        this._Segments = value;
        this.Clear();
      }
    }

    public float SegmentLength
    {
      get => this._SegmentLength;
      set
      {
        if ((double) this._SegmentLength == (double) value)
          return;
        this._SegmentLength = value;
      }
    }

    public string ScalpProviderName => this._ScalpProviderName;

    public bool[] enabledIndices => this._enabledIndices;

    protected void SyncToScalpProvider()
    {
      if (!Application.isPlaying || !((UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null))
        return;
      this._ScalpProviderName = this._ScalpProvider.name;
      int vertexCount;
      if (this._ScalpProvider.useBaseMesh)
      {
        if ((UnityEngine.Object) this._ScalpProvider.BaseMesh == (UnityEngine.Object) null)
          return;
        vertexCount = this._ScalpProvider.BaseMesh.vertexCount;
      }
      else
      {
        if ((UnityEngine.Object) this._ScalpProvider.Mesh == (UnityEngine.Object) null)
          return;
        vertexCount = this._ScalpProvider.Mesh.vertexCount;
      }
      HashSet<int> intSet = new HashSet<int>();
      if (this._ScalpProvider.materialsToUse != null && this._ScalpProvider.materialsToUse.Length > 0)
      {
        for (int index = 0; index < this._ScalpProvider.materialsToUse.Length; ++index)
        {
          foreach (int num in !this._ScalpProvider.useBaseMesh ? this._ScalpProvider.Mesh.GetIndices(this._ScalpProvider.materialsToUse[index]) : this._ScalpProvider.BaseMesh.GetIndices(this._ScalpProvider.materialsToUse[index]))
            intSet.Add(num);
        }
      }
      else
      {
        int subMeshCount = this._ScalpProvider.Mesh.subMeshCount;
        for (int submesh = 0; submesh < subMeshCount; ++submesh)
        {
          foreach (int num in !this._ScalpProvider.useBaseMesh ? this._ScalpProvider.Mesh.GetIndices(submesh) : this._ScalpProvider.BaseMesh.GetIndices(submesh))
            intSet.Add(num);
        }
      }
      this._enabledIndices = new bool[vertexCount];
      for (int index = 0; index < vertexCount; ++index)
        this._enabledIndices[index] = intSet.Contains(index);
      this.strandsMask = new RuntimeHairGeometryCreator.ScalpMask(vertexCount);
      this.strandsMaskWorking = new RuntimeHairGeometryCreator.ScalpMask(vertexCount);
      this.strands = new RuntimeHairGeometryCreator.Strand[vertexCount];
      for (int index = 0; index < vertexCount; ++index)
        this.strands[index] = new RuntimeHairGeometryCreator.Strand(index);
    }

    public PreCalcMeshProvider ScalpProvider
    {
      get => this._ScalpProvider;
      set
      {
        if (!((UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) value))
          return;
        this._ScalpProvider = value;
        this.SyncToScalpProvider();
        this.ClearNearbyVertexGroups();
        this.GenerateOutput();
      }
    }

    private void Awake()
    {
      this.SyncToScalpProvider();
      if (!Application.isPlaying || this.isProcessed)
        return;
      this.Process();
    }

    public void Optimize() => this.Process();

    public void SetDirty() => this.isProcessed = false;

    public void ClearNearbyVertices() => this.ClearNearbyVertexGroups();

    public bool IsDirty() => !this.isProcessed;

    public void MaskClearAll()
    {
      if (this.strandsMaskWorking != null)
        this.strandsMaskWorking.ClearAll();
      this.ApplyMaskChanges();
    }

    public void MaskSetAll()
    {
      if (this.strandsMaskWorking != null)
        this.strandsMaskWorking.SetAll();
      this.ApplyMaskChanges();
    }

    public void SetWorkingMaskToCurrentMask()
    {
      if (this.strandsMask == null)
        return;
      this.strandsMaskWorking.CopyVerticesFrom(this.strandsMask);
    }

    public void ApplyMaskChanges()
    {
      if (this.strandsMask == null)
        return;
      this.strandsMask.CopyVerticesFrom(this.strandsMaskWorking);
      this.GenerateAll();
    }

    public void Clear()
    {
      this.ClearAllStrands();
      this.ClearNearbyVertexGroups();
      this.GenerateOutput();
    }

    public void ClearAllStrands()
    {
      if (this.strands == null)
        return;
      for (int index = 0; index < this.strands.Length; ++index)
        this.strands[index].vertices = (List<Vector3>) null;
    }

    public void GenerateAll()
    {
      this.GenerateStrands();
      this.ClearNearbyVertexGroups();
      this.GenerateOutput();
    }

    protected void GenerateStrands()
    {
      if (!((UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null))
        return;
      Mesh mesh = !this._ScalpProvider.useBaseMesh ? this._ScalpProvider.Mesh : this._ScalpProvider.BaseMesh;
      if (!((UnityEngine.Object) mesh != (UnityEngine.Object) null))
        return;
      Vector3[] vertices = mesh.vertices;
      Vector3[] normals = mesh.normals;
      for (int index1 = 0; index1 < this.strands.Length; ++index1)
      {
        if (this.strandsMask.vertices[index1] || !this._enabledIndices[index1])
          this.strands[index1].vertices = (List<Vector3>) null;
        else if (this.strands[index1].vertices == null || this.strands[index1].vertices.Count != this.Segments)
        {
          this.strands[index1].vertices = new List<Vector3>();
          this.strands[index1].vertices.Add(vertices[index1]);
          for (int index2 = 1; index2 < this.Segments; ++index2)
            this.strands[index1].vertices.Add(vertices[index1] + normals[index1] * (float) index2 * this.SegmentLength);
        }
      }
    }

    public void GenerateOutput()
    {
      if (!((UnityEngine.Object) this.ScalpProvider != (UnityEngine.Object) null) || this.strands == null)
        return;
      Mesh mesh = !this._ScalpProvider.useBaseMesh ? this._ScalpProvider.Mesh : this._ScalpProvider.BaseMesh;
      if (!((UnityEngine.Object) mesh != (UnityEngine.Object) null))
        return;
      List<int> intList = new List<int>();
      this.vertices = new List<Vector3>();
      this.rigidities = (List<float>) null;
      this.colors = new List<Color>();
      for (int index = 0; index < this.strands.Length; ++index)
      {
        if (this.strands[index].vertices != null)
        {
          intList.Add(index);
          this.colors.Add(Color.black);
          this.vertices.AddRange((IEnumerable<Vector3>) this.strands[index].vertices);
        }
      }
      this.hairRootToScalpIndices = intList.ToArray();
      List<List<Vector3>> hairVerticesGroups = new List<List<Vector3>>();
      hairVerticesGroups.Add(this.vertices);
      float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(mesh) * 0.1f;
      List<int> scalpIndices = new List<int>();
      if (this._ScalpProvider.materialsToUse != null && this._ScalpProvider.materialsToUse.Length > 0)
      {
        for (int index1 = 0; index1 < this._ScalpProvider.materialsToUse.Length; ++index1)
        {
          foreach (int index2 in mesh.GetIndices(this._ScalpProvider.materialsToUse[index1]))
            scalpIndices.Add(index2);
        }
      }
      else
      {
        int subMeshCount = this._ScalpProvider.Mesh.subMeshCount;
        for (int submesh = 0; submesh < subMeshCount; ++submesh)
        {
          foreach (int index in mesh.GetIndices(submesh))
            scalpIndices.Add(index);
        }
      }
      this.indices = ScalpProcessingTools.ProcessIndices(scalpIndices, ((IEnumerable<Vector3>) mesh.vertices).ToList<Vector3>(), hairVerticesGroups, this.Segments, accuracy).ToArray();
    }

    public void Process()
    {
      if ((UnityEngine.Object) this._ScalpProvider == (UnityEngine.Object) null || !this._ScalpProvider.Validate(true))
        return;
      this.GenerateOutput();
      this.isProcessed = true;
    }

    public override void Dispatch()
    {
      if (!((UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null))
        return;
      this._ScalpProvider.provideToWorldMatrices = true;
      this._ScalpProvider.Dispatch();
    }

    public override bool Validate(bool log) => (UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null && this._ScalpProvider.Validate(log);

    private void OnDestroy()
    {
      if (!((UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null))
        return;
      this._ScalpProvider.Dispose();
    }

    public override Bounds GetBounds() => this.transform.TransformBounds(this.Bounds);

    public override int GetSegmentsNum() => this.Segments;

    public override int GetStandsNum() => this.vertices.Count / this.Segments;

    public override int[] GetIndices() => this.indices;

    public override List<Vector3> GetVertices() => this.vertices;

    public override void SetVertices(List<Vector3> verts)
    {
      this.vertices = verts;
      int index1 = 0;
      for (int index2 = 0; index2 < this.hairRootToScalpIndices.Length; ++index2)
      {
        int rootToScalpIndex = this.hairRootToScalpIndices[index2];
        List<Vector3> vector3List = new List<Vector3>();
        this.strands[rootToScalpIndex].vertices = vector3List;
        for (int index3 = 0; index3 < this.Segments; ++index3)
        {
          vector3List.Add(this.vertices[index1]);
          ++index1;
        }
      }
    }

    public override List<float> GetRigidities() => this.rigidities;

    public override void SetRigidities(List<float> rigiditiesList) => this.rigidities = rigiditiesList;

    private bool AddToHashSet(
      HashSet<Vector4> set,
      int i1,
      int i2,
      float distance,
      float distanceRatio)
    {
      return i1 != -1 && i2 != -1 && set.Add(i1 <= i2 ? new Vector4((float) i2, (float) i1, distance, distanceRatio) : new Vector4((float) i1, (float) i2, distance, distanceRatio));
    }

    public void ClearNearbyVertexGroups()
    {
      this.nearbyVertexGroups = new List<Vector4ListContainer>();
      this.nearbyVertexGroups.Add(new Vector4ListContainer());
    }

    public void CancelCalculateNearbyVertexGroups() => this.cancelCalculateNearbyVertexGroups = true;

    public void PrepareCalculateNearbyVertexGroups() => this.toWorldMatrix = this.GetToWorldMatrix();

    public override void CalculateNearbyVertexGroups()
    {
      this.cancelCalculateNearbyVertexGroups = false;
      this.nearbyVertexGroups = new List<Vector4ListContainer>();
      this.status = "Preparing vertices";
      List<Vector3> vector3List = new List<Vector3>();
      foreach (Vector3 vertex in this.vertices)
      {
        Vector3 vector3 = this.toWorldMatrix.MultiplyPoint3x4(vertex);
        vector3List.Add(vector3);
      }
      HashSet<Vector4> vector4Set = new HashSet<Vector4>();
      HashSet<int> intSet1 = new HashSet<int>();
      bool flag1 = this.MaxNearbyVertsPerVert == 1;
      if (this.cancelCalculateNearbyVertexGroups)
        return;
      for (int index1 = 0; index1 < vector3List.Count; ++index1)
      {
        if (index1 % this.Segments != 0 && (!flag1 || !intSet1.Contains(index1)))
        {
          int num1 = index1 / this.Segments;
          List<RuntimeHairGeometryCreator.VertDistance> vertDistanceList = new List<RuntimeHairGeometryCreator.VertDistance>();
          for (int index2 = 0; index2 < vector3List.Count; ++index2)
          {
            if (index2 % this.Segments != 0 && (!flag1 || !intSet1.Contains(index2)))
            {
              int num2 = index2 / this.Segments;
              if (num1 != num2)
              {
                float num3 = Vector3.Distance(vector3List[index1], vector3List[index2]);
                if ((double) num3 < (double) this.NearbyVertexSearchDistance && (double) num3 > (double) this.NearbyVertexSearchMinDistance)
                {
                  RuntimeHairGeometryCreator.VertDistance vertDistance;
                  vertDistance.vert = index2;
                  vertDistance.distance = num3;
                  vertDistanceList.Add(vertDistance);
                }
              }
            }
          }
          vertDistanceList.Sort((Comparison<RuntimeHairGeometryCreator.VertDistance>) ((vd1, vd2) => vd1.distance.CompareTo(vd2.distance)));
          int num4 = 0;
          foreach (RuntimeHairGeometryCreator.VertDistance vertDistance in vertDistanceList)
          {
            if (num4 < this.MaxNearbyVertsPerVert)
            {
              if (flag1)
              {
                intSet1.Add(index1);
                intSet1.Add(vertDistance.vert);
              }
              this.AddToHashSet(vector4Set, index1, vertDistance.vert, vertDistance.distance, (this.NearbyVertexSearchDistance - vertDistance.distance) / this.NearbyVertexSearchDistance);
              ++num4;
            }
            else
              break;
          }
          this.status = "Processed " + (object) index1 + " of " + (object) vector3List.Count + " vertices";
          if (this.cancelCalculateNearbyVertexGroups)
            return;
        }
      }
      Debug.Log((object) ("Found " + (object) vector4Set.Count + " nearby vertex pairs"));
      List<Vector4> list = vector4Set.ToList<Vector4>();
      List<HashSet<int>> intSetList = new List<HashSet<int>>();
      this.status = "Converting to vertex groups";
      foreach (Vector4 vector4 in list)
      {
        bool flag2 = false;
        int x = (int) vector4.x;
        int y = (int) vector4.y;
        for (int index = 0; index < intSetList.Count; ++index)
        {
          HashSet<int> intSet2 = intSetList[index];
          if (!intSet2.Contains(x) && !intSet2.Contains(y))
          {
            flag2 = true;
            intSet2.Add(x);
            intSet2.Add(y);
            this.nearbyVertexGroups[index].List.Add(vector4);
            break;
          }
        }
        if (!flag2)
        {
          HashSet<int> intSet3 = new HashSet<int>();
          intSetList.Add(intSet3);
          intSet3.Add(x);
          intSet3.Add(y);
          this.nearbyVertexGroups.Add(new Vector4ListContainer()
          {
            List = {
              vector4
            }
          });
        }
        if (this.cancelCalculateNearbyVertexGroups)
        {
          this.nearbyVertexGroups = new List<Vector4ListContainer>();
          return;
        }
      }
      this.status = "Complete";
      Debug.Log((object) ("Created " + (object) this.nearbyVertexGroups.Count + " nearby vertex pair groups"));
    }

    public override List<Vector4ListContainer> GetNearbyVertexGroups() => this.nearbyVertexGroups;

    public override List<Color> GetColors() => this.colors;

    public override Matrix4x4 GetToWorldMatrix() => (UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null ? this._ScalpProvider.ToWorldMatrix : Matrix4x4.identity;

    public override GpuBuffer<Matrix4x4> GetTransformsBuffer() => (UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null ? this._ScalpProvider.ToWorldMatricesBuffer : (GpuBuffer<Matrix4x4>) null;

    public override GpuBuffer<Vector3> GetNormalsBuffer() => (UnityEngine.Object) this._ScalpProvider != (UnityEngine.Object) null ? this._ScalpProvider.NormalsBuffer : (GpuBuffer<Vector3>) null;

    public override int[] GetHairRootToScalpMap() => this.hairRootToScalpIndices;

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
      if (!this.DebugDraw || !this._ScalpProvider.Validate(false))
        return;
      Bounds bounds = this.GetBounds();
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    [Serializable]
    public class ScalpMask
    {
      public string name;
      public bool[] vertices;

      public ScalpMask(int vertexCount)
      {
        this.vertices = new bool[vertexCount];
        this.name = string.Empty;
      }

      public void CopyVerticesFrom(RuntimeHairGeometryCreator.ScalpMask otherMask)
      {
        if (this.vertices.Length == otherMask.vertices.Length)
        {
          for (int index = 0; index < this.vertices.Length; ++index)
            this.vertices[index] = otherMask.vertices[index];
        }
        else
          Debug.LogError((object) "Attempted to copy mask vertices from a mask that has different vertex count");
      }

      public void StoreToBinaryWriter(BinaryWriter binWriter)
      {
        binWriter.Write(this.name);
        binWriter.Write(this.vertices.Length);
        for (int index = 0; index < this.vertices.Length; ++index)
          binWriter.Write(this.vertices[index]);
      }

      public void LoadFromBinaryReader(BinaryReader binReader)
      {
        this.name = binReader.ReadString();
        int length = binReader.ReadInt32();
        this.vertices = new bool[length];
        for (int index = 0; index < length; ++index)
          this.vertices[index] = binReader.ReadBoolean();
      }

      public void ClearAll()
      {
        for (int index = 0; index < this.vertices.Length; ++index)
          this.vertices[index] = false;
      }

      public void SetAll()
      {
        for (int index = 0; index < this.vertices.Length; ++index)
          this.vertices[index] = true;
      }
    }

    [Serializable]
    public class Strand
    {
      public int scalpIndex;
      public List<Vector3> vertices;

      public Strand(int index) => this.scalpIndex = index;

      public void StoreToBinaryWriter(BinaryWriter binWriter)
      {
        binWriter.Write(this.scalpIndex);
        if (this.vertices != null)
        {
          binWriter.Write(this.vertices.Count);
          for (int index = 0; index < this.vertices.Count; ++index)
          {
            binWriter.Write(this.vertices[index].x);
            binWriter.Write(this.vertices[index].y);
            binWriter.Write(this.vertices[index].z);
          }
        }
        else
          binWriter.Write(0);
      }

      public void LoadFromBinaryReader(BinaryReader binReader)
      {
        this.scalpIndex = binReader.ReadInt32();
        int num = binReader.ReadInt32();
        if (num == 0)
        {
          this.vertices = (List<Vector3>) null;
        }
        else
        {
          this.vertices = new List<Vector3>();
          for (int index = 0; index < num; ++index)
          {
            Vector3 vector3;
            vector3.x = binReader.ReadSingle();
            vector3.y = binReader.ReadSingle();
            vector3.z = binReader.ReadSingle();
            this.vertices.Add(vector3);
          }
        }
      }
    }

    protected struct VertDistance
    {
      public int vert;
      public float distance;
    }
  }
}
