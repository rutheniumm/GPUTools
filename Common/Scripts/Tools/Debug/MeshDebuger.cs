// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.MeshDebuger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class MeshDebuger : MonoBehaviour
  {
    [SerializeField]
    private MeshFilter filter;

    private void Start() => UnityEngine.Debug.Log((object) ("VerticesNum" + (object) this.filter.mesh.vertices.Length));

    private void OnDrawGizmos()
    {
    }
  }
}
