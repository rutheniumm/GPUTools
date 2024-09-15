// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.JointsPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.Tools.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class JointsPass : ICacheCommand
  {
    private readonly ClothGeometryData data;
    private List<HashSet<int>> jointGroupsSet;
    private List<Int2ListContainer> jointGroups;
    protected bool cancelCache;

    public JointsPass(ClothSettings settings) => this.data = settings.GeometryData;

    public void CancelCache() => this.cancelCache = true;

    public void PrepCache() => this.cancelCache = false;

    public void Cache()
    {
      if (this.data == null)
        return;
      List<Vector2> jointsList = this.CreateJointsList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
      if (this.cancelCache)
        return;
      this.data.JointGroups = this.CreateJointsGroups(jointsList);
    }

    private List<Vector2> CreateJointsList(int[] indices, int[] meshToPhysicsVerticesMap)
    {
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      for (int index = 0; index < indices.Length; index += 3)
      {
        if (this.cancelCache)
          return (List<Vector2>) null;
        int toPhysicsVertices1 = meshToPhysicsVerticesMap[indices[index]];
        int toPhysicsVertices2 = meshToPhysicsVerticesMap[indices[index + 1]];
        int toPhysicsVertices3 = meshToPhysicsVerticesMap[indices[index + 2]];
        this.AddToHashSet(vector2Set, toPhysicsVertices1, toPhysicsVertices2);
        this.AddToHashSet(vector2Set, toPhysicsVertices2, toPhysicsVertices3);
        this.AddToHashSet(vector2Set, toPhysicsVertices3, toPhysicsVertices1);
      }
      return vector2Set.ToList<Vector2>();
    }

    private void AddToHashSet(HashSet<Vector2> set, int i1, int i2)
    {
      if (i1 == -1 || i2 == -1)
        return;
      set.Add(i1 <= i2 ? new Vector2((float) i2, (float) i1) : new Vector2((float) i1, (float) i2));
    }

    private List<Int2ListContainer> CreateJointsGroups(List<Vector2> list)
    {
      this.jointGroupsSet = new List<HashSet<int>>();
      this.jointGroups = new List<Int2ListContainer>();
      foreach (Vector2 vector2 in list)
      {
        if (this.cancelCache)
          return (List<Int2ListContainer>) null;
        this.AddJoint(new Int2((int) vector2.x, (int) vector2.y));
      }
      return this.jointGroups;
    }

    private void AddJoint(Int2 item)
    {
      for (int index = 0; index < this.jointGroupsSet.Count; ++index)
      {
        HashSet<int> jointGroups = this.jointGroupsSet[index];
        List<Int2> list = this.jointGroups[index].List;
        if (!jointGroups.Contains(item.X) && !jointGroups.Contains(item.Y))
        {
          jointGroups.Add(item.X);
          jointGroups.Add(item.Y);
          list.Add(item);
          return;
        }
      }
      this.CreateNewGroup(item);
    }

    private void CreateNewGroup(Int2 item)
    {
      this.jointGroupsSet.Add(new HashSet<int>()
      {
        item.X,
        item.Y
      });
      List<Int2> int2List = new List<Int2>() { item };
      this.jointGroups.Add(new Int2ListContainer()
      {
        List = int2List
      });
    }
  }
}
