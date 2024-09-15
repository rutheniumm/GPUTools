// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Tools.BoneCollidersPlacer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Tools
{
  public class BoneCollidersPlacer : MonoBehaviour
  {
    [SerializeField]
    public SkinnedMeshRenderer Skin;
    [SerializeField]
    public int Depth = 5;
    private Vector3[] vertices;

    [ContextMenu("Process")]
    public void Process()
    {
      this.Clear();
      this.Init();
      this.PlaceRecursive(this.transform, this.Depth);
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
      foreach (Object componentsInChild in this.gameObject.GetComponentsInChildren<LineSphereCollider>())
        Object.DestroyImmediate(componentsInChild);
    }

    public void Fit()
    {
      this.Init();
      foreach (LineSphereCollider componentsInChild in this.gameObject.GetComponentsInChildren<LineSphereCollider>())
      {
        for (int index = 0; index < 20; ++index)
          this.Rotate(componentsInChild, 0.01f);
      }
    }

    [ContextMenu("Grow")]
    public void Grow()
    {
      this.Init();
      foreach (LineSphereCollider componentsInChild in this.gameObject.GetComponentsInChildren<LineSphereCollider>())
      {
        componentsInChild.RadiusA += 0.01f;
        componentsInChild.RadiusB += 0.01f;
      }
    }

    private void Init()
    {
      Mesh mesh = new Mesh();
      this.Skin.BakeMesh(mesh);
      this.vertices = mesh.vertices;
    }

    private void PlaceRecursive(Transform bone, int depth)
    {
      --depth;
      if (depth == 0)
        return;
      for (int index = 0; index < bone.childCount; ++index)
      {
        Transform child = bone.GetChild(index);
        this.AddLineSpheres(bone, child);
        this.PlaceRecursive(child, depth);
      }
    }

    private void AddLineSpheres(Transform bone, Transform child)
    {
      LineSphereCollider lineSphereCollider = bone.gameObject.AddComponent<LineSphereCollider>();
      lineSphereCollider.B = child.localPosition;
      lineSphereCollider.RadiusA = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(lineSphereCollider.WorldA));
      lineSphereCollider.RadiusB = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(lineSphereCollider.WorldB));
    }

    private float FindNearestMeshDistnce(Vector3 point)
    {
      float f = (this.vertices[0] - point).sqrMagnitude;
      for (int index = 1; index < this.vertices.Length; ++index)
      {
        float sqrMagnitude = (this.vertices[index] - point).sqrMagnitude;
        if ((double) sqrMagnitude < (double) f)
          f = sqrMagnitude;
      }
      return Mathf.Sqrt(f);
    }

    private void Rotate(LineSphereCollider lineSphere, float step)
    {
      for (int index = 0; index < 50; ++index)
      {
        Vector3 position = lineSphere.WorldA + this.RandomVector() * step;
        float nearestMeshDistnce = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(position));
        if ((double) nearestMeshDistnce > (double) lineSphere.WorldRadiusA)
        {
          lineSphere.WorldRadiusA = nearestMeshDistnce;
          lineSphere.WorldA = position;
          break;
        }
      }
      for (int index = 0; index < 50; ++index)
      {
        Vector3 position = lineSphere.WorldB + this.RandomVector() * step;
        float nearestMeshDistnce = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(position));
        if ((double) nearestMeshDistnce > (double) lineSphere.WorldRadiusB)
        {
          lineSphere.WorldRadiusA = nearestMeshDistnce;
          lineSphere.WorldA = position;
          break;
        }
      }
    }

    private Vector3 RandomVector() => new Vector3((float) Random.Range(-1, 2), (float) Random.Range(-1, 2), (float) Random.Range(-1, 2));
  }
}
