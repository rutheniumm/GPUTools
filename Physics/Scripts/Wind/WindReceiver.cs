// Decompiled with JetBrains decompiler
// Type: GPUTools.Physics.Scripts.Wind.WindReceiver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Wind
{
  public class WindReceiver
  {
    private float angle;
    private readonly WindZone[] winds;
    private readonly Perlin2D perlin = new Perlin2D(566);
    private readonly List<NoiseOctave> octaves = new List<NoiseOctave>();

    public WindReceiver()
    {
      this.winds = Object.FindObjectsOfType<WindZone>();
      this.octaves.Add(new NoiseOctave(1f, 1f));
      this.octaves.Add(new NoiseOctave(5f, 0.6f));
      this.octaves.Add(new NoiseOctave(10f, 0.4f));
      this.octaves.Add(new NoiseOctave(20f, 0.3f));
    }

    public Vector3 Vector { get; set; }

    public Vector3 GetWind(Vector3 position)
    {
      this.Vector = Vector3.zero;
      foreach (WindZone wind in this.winds)
      {
        if (wind.mode == WindZoneMode.Directional)
          this.UpdateDirectionalWind(wind);
        else
          this.UpdateSphericalWind(wind, position);
      }
      return this.Vector;
    }

    private void UpdateDirectionalWind(WindZone wind)
    {
      Vector3 dirrection = wind.transform.rotation * Vector3.forward;
      this.Vector += this.GetAmplitude(wind, dirrection);
    }

    private void UpdateSphericalWind(WindZone wind, Vector3 center)
    {
      Vector3 vector3 = center - wind.transform.position;
      if ((double) vector3.magnitude > (double) wind.radius)
        return;
      this.Vector += this.GetAmplitude(wind, vector3.normalized);
    }

    private Vector3 GetAmplitude(WindZone wind, Vector3 dirrection)
    {
      this.angle += wind.windPulseFrequency;
      float noise = this.GetNoise(this.angle);
      float num = wind.windMain + noise * wind.windPulseMagnitude;
      return dirrection * num;
    }

    private float GetNoise(float angle) => Mathf.Abs(this.perlin.Noise(new Vector2(0.0f, angle), this.octaves));
  }
}
