// Decompiled with JetBrains decompiler
// Type: GPUTools.Common.Scripts.Tools.Debug.Validator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace GPUTools.Common.Scripts.Tools.Debug
{
  public class Validator
  {
    public static bool TestList<T>(List<T> list) => list.Count != 0 && !list.Any<T>((Func<T, bool>) (item => (object) item == null));
  }
}
