// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Commands.Physics.BuildPointJoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Physics.Scripts.Types.Joints;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
  public class BuildPointJoints : BuildChainCommand
  {
    private readonly HairSettings settings;

    public BuildPointJoints(HairSettings settings) => this.settings = settings;

    protected override void OnBuild()
    {
      GPPointJoint[] gpPointJointArray = new GPPointJoint[this.settings.StandsSettings.Provider.GetVertices().Count];
      this.CreatePointJoints(gpPointJointArray);
      if (this.settings.RuntimeData.PointJoints != null)
        this.settings.RuntimeData.PointJoints.Dispose();
      if (gpPointJointArray.Length > 0)
        this.settings.RuntimeData.PointJoints = new GpuBuffer<GPPointJoint>(gpPointJointArray, GPPointJoint.Size());
      else
        this.settings.RuntimeData.PointJoints = (GpuBuffer<GPPointJoint>) null;
    }

    protected override void OnUpdateSettings()
    {
      if (this.settings.RuntimeData.PointJoints == null)
        return;
      this.CreatePointJoints(this.settings.RuntimeData.PointJoints.Data);
      this.settings.RuntimeData.PointJoints.PushData();
    }

    public void UpdateSettingsPreserveData()
    {
      this.settings.RuntimeData.PointJoints.PullData();
      this.CreatePointJoints(this.settings.RuntimeData.PointJoints.Data, true);
      this.settings.RuntimeData.PointJoints.PushData();
    }

    public void RebuildFromGPUData()
    {
      this.settings.RuntimeData.PointJoints.PullData();
      GPPointJoint[] data = this.settings.RuntimeData.PointJoints.Data;
      List<Vector3> verts = new List<Vector3>();
      List<float> rigidities = new List<float>();
      for (int index = 0; index < data.Length; ++index)
      {
        verts.Add(data[index].Point);
        rigidities.Add(data[index].Rigidity);
      }
      this.settings.StandsSettings.Provider.SetVertices(verts);
      if (this.settings.PhysicsSettings.UsePaintedRigidity)
        this.settings.StandsSettings.Provider.SetRigidities(rigidities);
      else
        this.settings.StandsSettings.Provider.SetRigidities((List<float>) null);
      this.OnUpdateSettings();
    }

    private void CreatePointJoints(GPPointJoint[] pointJoints, bool reuse = false)
    {
      List<Vector3> vertices = this.settings.StandsSettings.Provider.GetVertices();
      List<float> rigidities = this.settings.StandsSettings.Provider.GetRigidities();
      int segments = this.settings.StandsSettings.Segments;
      int[] hairRootToScalpMap = this.settings.StandsSettings.Provider.GetHairRootToScalpMap();
      Vector3 zero = Vector3.zero;
      bool usePaintedRigidity = this.settings.PhysicsSettings.UsePaintedRigidity;
      float rootRigidity = this.settings.PhysicsSettings.RootRigidity;
      float mainRigidity = this.settings.PhysicsSettings.MainRigidity;
      float tipRigidity = this.settings.PhysicsSettings.TipRigidity;
      float rigidityRolloffPower = this.settings.PhysicsSettings.RigidityRolloffPower;
      for (int index1 = 0; index1 < vertices.Count; ++index1)
      {
        Vector3 vector3 = vertices[index1];
        int index2 = index1 / segments;
        int matrixId = hairRootToScalpMap[index2];
        int num1 = index1 % segments;
        float num2;
        if (num1 == 0)
          num2 = 1.1f;
        else if (usePaintedRigidity && rigidities != null)
          num2 = rigidities[index1];
        else if (num1 == 1)
        {
          num2 = rootRigidity;
        }
        else
        {
          float t = Mathf.Pow(1f - ((float) num1 - 1f) / (float) (segments - 2), rigidityRolloffPower);
          num2 = Mathf.Lerp(tipRigidity, mainRigidity, t);
        }
        float num3 = num2 + this.JointAreaAdd(vector3);
        if (reuse)
          pointJoints[index1].Rigidity = num1 != 0 ? Mathf.Clamp01(num3) : 1.1f;
        else
          pointJoints[index1] = num1 != 0 ? new GPPointJoint(index1, matrixId, vector3, Mathf.Clamp01(num3)) : new GPPointJoint(index1, matrixId, vector3, 1.1f);
      }
    }

    private float JointAreaAdd(Vector3 vertex)
    {
      float num1 = 0.0f;
      foreach (HairJointArea jointArea in this.settings.PhysicsSettings.JointAreas)
      {
        float magnitude = (vertex - jointArea.transform.localPosition).magnitude;
        if ((double) magnitude < (double) jointArea.Radius)
        {
          float num2 = (jointArea.Radius - magnitude) / jointArea.Radius;
          num1 += num2 * this.settings.PhysicsSettings.JointRigidity;
        }
      }
      return num1;
    }

    protected override void OnDispose()
    {
      if (this.settings.RuntimeData.PointJoints == null)
        return;
      this.settings.RuntimeData.PointJoints.Dispose();
    }
  }
}
