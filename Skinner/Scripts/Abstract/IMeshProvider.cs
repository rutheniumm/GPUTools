// Decompiled with JetBrains decompiler
// Type: GPUTools.Skinner.Scripts.Abstract.IMeshProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Abstract
{
  public interface IMeshProvider
  {
    Matrix4x4 ToWorldMatrix { get; }

    GpuBuffer<Matrix4x4> ToWorldMatricesBuffer { get; }

    GpuBuffer<Vector3> PreCalculatedVerticesBuffer { get; }

    GpuBuffer<Vector3> NormalsBuffer { get; }

    Mesh Mesh { get; }

    Mesh MeshForImport { get; }

    bool Validate(bool log);

    void Dispatch();

    void Dispose();
  }
}
