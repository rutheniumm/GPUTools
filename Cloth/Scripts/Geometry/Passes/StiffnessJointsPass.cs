// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Passes.StiffnessJointsPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.Tools.Commands;
using System.Collections.Generic;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
  public class StiffnessJointsPass : ICacheCommand
  {
    private readonly ClothGeometryData data;
    private List<HashSet<int>> jointGroupsSet;
    private List<Int2ListContainer> jointGroups;
    protected bool cancelCache;

    public StiffnessJointsPass(ClothSettings settings) => this.data = settings.GeometryData;

    public void CancelCache() => this.cancelCache = true;

    public void PrepCache() => this.cancelCache = false;

    public void Cache()
    {
      if (this.data == null)
        return;
      List<Int2> jointsList = this.CreateJointsList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
      if (this.cancelCache)
        return;
      this.data.StiffnessJointGroups = this.CreateJointsGroups(jointsList);
    }

    private void AddNeighbors(
      Dictionary<int, HashSet<int>> vertToNeighborVerts,
      int index,
      int neighbor1,
      int neighbor2)
    {
      HashSet<int> intSet;
      if (!vertToNeighborVerts.TryGetValue(index, out intSet))
      {
        intSet = new HashSet<int>();
        vertToNeighborVerts.Add(index, intSet);
      }
      intSet.Add(neighbor1);
      intSet.Add(neighbor2);
    }

    private List<Int2> CreateJointsList(int[] indices, int[] meshToPhysicsVerticesMap)
    {
      HashSet<Int2> int2Set = new HashSet<Int2>();
      Dictionary<int, HashSet<int>> vertToNeighborVerts = new Dictionary<int, HashSet<int>>();
      for (int index = 0; index < indices.Length; index += 3)
      {
        if (this.cancelCache)
          return (List<Int2>) null;
        int toPhysicsVertices1 = meshToPhysicsVerticesMap[indices[index]];
        int toPhysicsVertices2 = meshToPhysicsVerticesMap[indices[index + 1]];
        int toPhysicsVertices3 = meshToPhysicsVerticesMap[indices[index + 2]];
        this.AddNeighbors(vertToNeighborVerts, toPhysicsVertices1, toPhysicsVertices2, toPhysicsVertices3);
        this.AddNeighbors(vertToNeighborVerts, toPhysicsVertices2, toPhysicsVertices1, toPhysicsVertices3);
        this.AddNeighbors(vertToNeighborVerts, toPhysicsVertices3, toPhysicsVertices1, toPhysicsVertices2);
      }
      for (int index = 0; index < indices.Length; ++index)
      {
        if (this.cancelCache)
          return (List<Int2>) null;
        if (index % 100 == 0)
          this.data.status = "Stiffness Joints: " + (object) (index * 100 / indices.Length) + "%";
        int toPhysicsVertices = meshToPhysicsVerticesMap[indices[index]];
        HashSet<int> intSet1;
        if (vertToNeighborVerts.TryGetValue(toPhysicsVertices, out intSet1))
        {
          foreach (int num in intSet1)
          {
            this.AddToHashSet(int2Set, toPhysicsVertices, num);
            HashSet<int> intSet2;
            if (vertToNeighborVerts.TryGetValue(num, out intSet2))
            {
              foreach (int i2 in intSet2)
              {
                if (toPhysicsVertices != i2 && !intSet1.Contains(i2))
                  this.AddToHashSet(int2Set, toPhysicsVertices, i2);
              }
            }
          }
        }
      }
      return int2Set.ToList<Int2>();
    }

    private void AddToHashSet(HashSet<Int2> set, int i1, int i2)
    {
      if (i1 == -1 || i2 == -1)
        return;
      set.Add(i1 <= i2 ? new Int2(i2, i1) : new Int2(i1, i2));
    }

    private List<Int2ListContainer> CreateJointsGroups(List<Int2> list)
    {
      this.jointGroupsSet = new List<HashSet<int>>();
      this.jointGroups = new List<Int2ListContainer>();
      foreach (Int2 int2 in list)
      {
        if (this.cancelCache)
          return (List<Int2ListContainer>) null;
        this.AddJoint(int2);
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
