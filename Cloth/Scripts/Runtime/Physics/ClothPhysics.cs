// Decompiled with JetBrains decompiler
// Type: GPUTools.Cloth.Scripts.Runtime.Physics.ClothPhysics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Cloth.Scripts.Runtime.Data;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Physics
{
  public class ClothPhysics : MonoBehaviour
  {
    private ClothPhysicsWorld world;

    public void Initialize(ClothDataFacade data) => this.world = new ClothPhysicsWorld(data);

    public void ResetPhysics() => this.world.Reset();

    public void PartialResetPhysics() => this.world.PartialReset();

    public void FixedDispatch() => this.world.FixedDispatch();

    public void DispatchCopyToOld() => this.world.DispatchCopyToOld();

    public void Dispatch() => this.world.Dispatch();

    private void OnDestroy() => this.world.Dispose();

    private void OnDrawGizmos() => this.world.DebugDraw();
  }
}
