// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.Procedural.ProceduralScalp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Procedural
{
  public class ProceduralScalp : MonoBehaviour
  {
    [SerializeField]
    public CurveGrid Grid;

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      for (int index1 = 0; index1 <= this.Grid.ViewSizeX; ++index1)
      {
        for (int index2 = 0; index2 <= this.Grid.ViewSizeY; ++index2)
        {
          Vector3 from = this.transform.TransformPoint(this.Grid.GetSplinePoint((float) index1 / (float) this.Grid.ViewSizeX, (float) index2 / (float) this.Grid.ViewSizeY));
          if (index1 < this.Grid.ViewSizeX)
          {
            Vector3 to = this.transform.TransformPoint(this.Grid.GetSplinePoint((float) (index1 + 1) / (float) this.Grid.ViewSizeX, (float) index2 / (float) this.Grid.ViewSizeY));
            Gizmos.DrawLine(from, to);
          }
          if (index2 < this.Grid.ViewSizeY)
          {
            Vector3 to = this.transform.TransformPoint(this.Grid.GetSplinePoint((float) index1 / (float) this.Grid.ViewSizeX, (float) (index2 + 1) / (float) this.Grid.ViewSizeY));
            Gizmos.DrawLine(from, to);
          }
        }
      }
    }
  }
}
