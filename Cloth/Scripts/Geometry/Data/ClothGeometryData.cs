// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Geometry.Data.ClothGeometryData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Data
{
  [Serializable]
  public class ClothGeometryData
  {
    [NonSerialized]
    public string status = string.Empty;
    public int[] AllTringles;
    public Vector3[] Particles;
    public int[] MeshToPhysicsVerticesMap;
    public int[] PhysicsToMeshVerticesMap;
    public List<Int2ListContainer> JointGroups = new List<Int2ListContainer>();
    public List<Int2ListContainer> StiffnessJointGroups = new List<Int2ListContainer>();
    public List<Int2ListContainer> NearbyJointGroups = new List<Int2ListContainer>();
    public int[] ParticleToNeibor;
    public int[] ParticleToNeiborCounts;
    public float[] ParticlesBlend;
    public float[] ParticlesStrength;
    public bool IsProcessed;

    public void ResetParticlesBlend()
    {
      for (int index = 0; index < this.ParticlesBlend.Length; ++index)
        this.ParticlesBlend[index] = 0.0f;
    }

    public bool LoadFromBinaryReader(BinaryReader reader)
    {
      try
      {
        if (reader.ReadString() != nameof (ClothGeometryData))
        {
          Debug.LogError((object) "Binary file corrupted. Tried to read ClothGeometryData in wrong section");
          return false;
        }
        string str = reader.ReadString();
        if (str != "1.0")
        {
          Debug.LogError((object) ("ClothGeometryData schema " + str + " is not compatible with this version of software"));
          return false;
        }
        int length1 = reader.ReadInt32();
        this.AllTringles = new int[length1];
        for (int index = 0; index < length1; ++index)
          this.AllTringles[index] = reader.ReadInt32();
        int length2 = reader.ReadInt32();
        this.Particles = new Vector3[length2];
        for (int index = 0; index < length2; ++index)
        {
          Vector3 vector3;
          vector3.x = reader.ReadSingle();
          vector3.y = reader.ReadSingle();
          vector3.z = reader.ReadSingle();
          this.Particles[index] = vector3;
        }
        int length3 = reader.ReadInt32();
        this.MeshToPhysicsVerticesMap = new int[length3];
        for (int index = 0; index < length3; ++index)
          this.MeshToPhysicsVerticesMap[index] = reader.ReadInt32();
        int length4 = reader.ReadInt32();
        this.PhysicsToMeshVerticesMap = new int[length4];
        for (int index = 0; index < length4; ++index)
          this.PhysicsToMeshVerticesMap[index] = reader.ReadInt32();
        int num1 = reader.ReadInt32();
        this.JointGroups = new List<Int2ListContainer>();
        for (int index1 = 0; index1 < num1; ++index1)
        {
          Int2ListContainer int2ListContainer = new Int2ListContainer();
          this.JointGroups.Add(int2ListContainer);
          int num2 = reader.ReadInt32();
          int2ListContainer.List = new List<Int2>();
          for (int index2 = 0; index2 < num2; ++index2)
            int2ListContainer.List.Add(new Int2()
            {
              X = reader.ReadInt32(),
              Y = reader.ReadInt32()
            });
        }
        int num3 = reader.ReadInt32();
        this.StiffnessJointGroups = new List<Int2ListContainer>();
        for (int index3 = 0; index3 < num3; ++index3)
        {
          Int2ListContainer int2ListContainer = new Int2ListContainer();
          this.StiffnessJointGroups.Add(int2ListContainer);
          int num4 = reader.ReadInt32();
          int2ListContainer.List = new List<Int2>();
          for (int index4 = 0; index4 < num4; ++index4)
            int2ListContainer.List.Add(new Int2()
            {
              X = reader.ReadInt32(),
              Y = reader.ReadInt32()
            });
        }
        int num5 = reader.ReadInt32();
        this.NearbyJointGroups = new List<Int2ListContainer>();
        for (int index5 = 0; index5 < num5; ++index5)
        {
          Int2ListContainer int2ListContainer = new Int2ListContainer();
          this.NearbyJointGroups.Add(int2ListContainer);
          int num6 = reader.ReadInt32();
          int2ListContainer.List = new List<Int2>();
          for (int index6 = 0; index6 < num6; ++index6)
            int2ListContainer.List.Add(new Int2()
            {
              X = reader.ReadInt32(),
              Y = reader.ReadInt32()
            });
        }
        int length5 = reader.ReadInt32();
        this.ParticleToNeibor = new int[length5];
        for (int index = 0; index < length5; ++index)
          this.ParticleToNeibor[index] = reader.ReadInt32();
        int length6 = reader.ReadInt32();
        this.ParticleToNeiborCounts = new int[length6];
        for (int index = 0; index < length6; ++index)
          this.ParticleToNeiborCounts[index] = reader.ReadInt32();
        int length7 = reader.ReadInt32();
        this.ParticlesBlend = new float[length7];
        for (int index = 0; index < length7; ++index)
          this.ParticlesBlend[index] = reader.ReadSingle();
        int length8 = reader.ReadInt32();
        this.ParticlesStrength = new float[length8];
        for (int index = 0; index < length8; ++index)
          this.ParticlesStrength[index] = reader.ReadSingle();
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error while loading ClothGeometryData from binary reader " + (object) ex));
        return false;
      }
      return true;
    }

    public bool StoreToBinaryWriter(BinaryWriter writer)
    {
      try
      {
        writer.Write(nameof (ClothGeometryData));
        writer.Write("1.0");
        writer.Write(this.AllTringles.Length);
        for (int index = 0; index < this.AllTringles.Length; ++index)
          writer.Write(this.AllTringles[index]);
        writer.Write(this.Particles.Length);
        for (int index = 0; index < this.Particles.Length; ++index)
        {
          writer.Write(this.Particles[index].x);
          writer.Write(this.Particles[index].y);
          writer.Write(this.Particles[index].z);
        }
        writer.Write(this.MeshToPhysicsVerticesMap.Length);
        for (int index = 0; index < this.MeshToPhysicsVerticesMap.Length; ++index)
          writer.Write(this.MeshToPhysicsVerticesMap[index]);
        writer.Write(this.PhysicsToMeshVerticesMap.Length);
        for (int index = 0; index < this.PhysicsToMeshVerticesMap.Length; ++index)
          writer.Write(this.PhysicsToMeshVerticesMap[index]);
        writer.Write(this.JointGroups.Count);
        for (int index1 = 0; index1 < this.JointGroups.Count; ++index1)
        {
          Int2ListContainer jointGroup = this.JointGroups[index1];
          writer.Write(jointGroup.List.Count);
          for (int index2 = 0; index2 < jointGroup.List.Count; ++index2)
          {
            writer.Write(jointGroup.List[index2].X);
            writer.Write(jointGroup.List[index2].Y);
          }
        }
        writer.Write(this.StiffnessJointGroups.Count);
        for (int index3 = 0; index3 < this.StiffnessJointGroups.Count; ++index3)
        {
          Int2ListContainer stiffnessJointGroup = this.StiffnessJointGroups[index3];
          writer.Write(stiffnessJointGroup.List.Count);
          for (int index4 = 0; index4 < stiffnessJointGroup.List.Count; ++index4)
          {
            writer.Write(stiffnessJointGroup.List[index4].X);
            writer.Write(stiffnessJointGroup.List[index4].Y);
          }
        }
        writer.Write(this.NearbyJointGroups.Count);
        for (int index5 = 0; index5 < this.NearbyJointGroups.Count; ++index5)
        {
          Int2ListContainer nearbyJointGroup = this.NearbyJointGroups[index5];
          writer.Write(nearbyJointGroup.List.Count);
          for (int index6 = 0; index6 < nearbyJointGroup.List.Count; ++index6)
          {
            writer.Write(nearbyJointGroup.List[index6].X);
            writer.Write(nearbyJointGroup.List[index6].Y);
          }
        }
        writer.Write(this.ParticleToNeibor.Length);
        for (int index = 0; index < this.ParticleToNeibor.Length; ++index)
          writer.Write(this.ParticleToNeibor[index]);
        writer.Write(this.ParticleToNeiborCounts.Length);
        for (int index = 0; index < this.ParticleToNeiborCounts.Length; ++index)
          writer.Write(this.ParticleToNeiborCounts[index]);
        writer.Write(this.ParticlesBlend.Length);
        for (int index = 0; index < this.ParticlesBlend.Length; ++index)
          writer.Write(this.ParticlesBlend[index]);
        writer.Write(this.ParticlesStrength.Length);
        for (int index = 0; index < this.ParticlesStrength.Length; ++index)
          writer.Write(this.ParticlesStrength[index]);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error while storeing ClothGeometryData to binary writer " + (object) ex));
        return false;
      }
      return true;
    }

    public void LogStatistics()
    {
      Debug.Log((object) ("Vertices Num: " + (object) this.MeshToPhysicsVerticesMap.Length));
      Debug.Log((object) ("Physics Vertices Num: " + (object) this.Particles.Length));
      Debug.Log((object) ("Mesh Vertex To Neibor Num: " + (object) this.ParticleToNeibor.Length));
      Debug.Log((object) ("Joints Num: " + (object) this.JointGroups.Sum<Int2ListContainer>((Func<Int2ListContainer, int>) (container => container.List.Count))));
      Debug.Log((object) ("Stiffness Joints Num: " + (object) this.StiffnessJointGroups.Sum<Int2ListContainer>((Func<Int2ListContainer, int>) (container => container.List.Count))));
      Debug.Log((object) ("Nearby Joints Num: " + (object) this.NearbyJointGroups.Sum<Int2ListContainer>((Func<Int2ListContainer, int>) (container => container.List.Count))));
    }
  }
}
