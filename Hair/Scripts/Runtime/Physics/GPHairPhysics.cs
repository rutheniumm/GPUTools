// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Physics.GPHairPhysics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Hair.Scripts.Runtime.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Physics
{
  public class GPHairPhysics : MonoBehaviour
  {
    private HairPhysicsWorld world;

    public void Initialize(HairDataFacade data) => this.world = new HairPhysicsWorld(data);

    public void FixedDispatch() => this.world.FixedDispatch();

    public void Dispatch() => this.world.Dispatch();

    public void RebindData() => this.world.RebindData();

    public void ResetPhysics() => this.world.Reset();

    public void PartialResetPhysics() => this.world.PartialReset();

    private void OnDestroy() => this.world.Dispose();

    private void OnDrawGizmos() => this.world.DebugDraw();
  }
}
