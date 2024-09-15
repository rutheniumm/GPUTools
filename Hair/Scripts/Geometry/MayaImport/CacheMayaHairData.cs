// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.CacheMayaHairData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Commands;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport
{
  public class CacheMayaHairData : CacheChainCommand
  {
    private readonly MayaHairGeometryImporter importer;

    public CacheMayaHairData(MayaHairGeometryImporter importer)
    {
      this.importer = importer;
      this.Add((ICacheCommand) new StripsToLinesMayaPass(importer));
      this.Add((ICacheCommand) new AssignHairLinesToTriangles(importer));
      this.Add((ICacheCommand) new MoveHairLinesToScalpVertices(importer));
      this.Add((ICacheCommand) new ComputeStandsToScalpVerticesMap(importer));
    }

    protected override void OnCache()
    {
      base.OnCache();
      Debug.Log((object) ("segments:" + (object) this.importer.Data.Segments));
      Debug.Log((object) ("vertices:" + (object) this.importer.Data.Vertices.Count));
      Debug.Log((object) ("stands:" + (object) (this.importer.Data.Vertices.Count / this.importer.Data.Segments)));
      Debug.Log((object) ("scalp triangles:" + (object) (this.importer.Data.Indices.Length / 3)));
    }
  }
}
