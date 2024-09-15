// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Geometry.MayaImport.Commands.ComputeStandsToScalpVerticesMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Skinner.Scripts.Providers;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
  public class ComputeStandsToScalpVerticesMap : CacheChainCommand
  {
    private readonly MayaHairGeometryImporter importer;

    public ComputeStandsToScalpVerticesMap(MayaHairGeometryImporter importer) => this.importer = importer;

    protected override void OnCache() => this.importer.Data.HairRootToScalpMap = this.ProcessHairRootToScalpMap();

    private int[] ProcessHairRootToScalpMap() => this.importer.ScalpProvider.Type == ScalpMeshType.Skinned ? ScalpProcessingTools.HairRootToScalpIndices(((IEnumerable<Vector3>) this.importer.ScalpProvider.Mesh.vertices).ToList<Vector3>(), this.importer.Data.Vertices, this.importer.Data.Segments).ToArray() : new int[this.importer.Data.Vertices.Count / this.importer.Data.Segments];
  }
}
