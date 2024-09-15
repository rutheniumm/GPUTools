// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Wind.Perlin2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Wind
{
  public class Perlin2D
  {
    private readonly byte[] permutationTable;

    public Perlin2D(int seed = 0)
    {
      System.Random random = new System.Random(seed);
      this.permutationTable = new byte[1024];
      random.NextBytes(this.permutationTable);
    }

    private static float QunticCurve(float t) => (float) ((double) t * (double) t * (double) t * ((double) t * ((double) t * 6.0 - 15.0) + 10.0));

    private Vector2 GetPseudoRandomGradientVector(int x, int y)
    {
      switch ((int) this.permutationTable[(int) (((long) (x * 1836311903) ^ (long) y * 2971215073L + 4807526976L) & 1023L)] & 3)
      {
        case 0:
          return new Vector2(1f, 0.0f);
        case 1:
          return new Vector2(-1f, 0.0f);
        case 2:
          return new Vector2(0.0f, 1f);
        default:
          return new Vector2(0.0f, -1f);
      }
    }

    public float Noise(Vector2 fp)
    {
      int x = (int) Math.Floor((double) fp.x);
      int y = (int) Math.Floor((double) fp.y);
      float num1 = fp.x - (float) x;
      float num2 = fp.y - (float) y;
      Vector2 randomGradientVector1 = this.GetPseudoRandomGradientVector(x, y);
      Vector2 randomGradientVector2 = this.GetPseudoRandomGradientVector(x + 1, y);
      Vector2 randomGradientVector3 = this.GetPseudoRandomGradientVector(x, y + 1);
      Vector2 randomGradientVector4 = this.GetPseudoRandomGradientVector(x + 1, y + 1);
      Vector2 lhs1 = new Vector2(num1, num2);
      Vector2 lhs2 = new Vector2(num1 - 1f, num2);
      Vector2 lhs3 = new Vector2(num1, num2 - 1f);
      Vector2 lhs4 = new Vector2(num1 - 1f, num2 - 1f);
      float a1 = Vector3.Dot((Vector3) lhs1, (Vector3) randomGradientVector1);
      float b1 = Vector3.Dot((Vector3) lhs2, (Vector3) randomGradientVector2);
      float a2 = Vector3.Dot((Vector3) lhs3, (Vector3) randomGradientVector3);
      float b2 = Vector3.Dot((Vector3) lhs4, (Vector3) randomGradientVector4);
      float t1 = Perlin2D.QunticCurve(num1);
      float t2 = Perlin2D.QunticCurve(num2);
      return Mathf.Lerp(Mathf.Lerp(a1, b1, t1), Mathf.Lerp(a2, b2, t1), t2);
    }

    public float Noise(Vector2 fp, int octaves, float persistence = 0.5f)
    {
      float num1 = 1f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      while (octaves-- > 0)
      {
        num2 += num1;
        num3 += this.Noise(fp) * num1;
        num1 *= persistence;
        fp.x *= 2f;
        fp.y *= 2f;
      }
      return num3 / num2;
    }

    public float Noise(Vector2 fp, List<NoiseOctave> octaves)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index = 0; index < octaves.Count; ++index)
      {
        NoiseOctave octave = octaves[index];
        num2 += octave.Amplitude;
        num1 += this.Noise(fp * octave.Scale) * octave.Amplitude;
      }
      return num1 / num2;
    }
  }
}
