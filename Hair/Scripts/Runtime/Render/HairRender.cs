// Decompiled with JetBrains decompiler
// Type: GPUTools.Hair.Scripts.Runtime.Render.HairRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5114BBD-6DFF-47E8-AA93-8D4A462F893C
// Assembly location: C:\Users\nic10\VaM_Updater\VaM_Data\Managed\Assembly-CSharp.dll

using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
  public class HairRender : MonoBehaviour
  {
    private Mesh mesh;
    private HairDataFacade data;
    private MeshRenderer rend;

    private void Awake()
    {
      this.mesh = new Mesh();
      this.rend = this.gameObject.AddComponent<MeshRenderer>();
      this.gameObject.AddComponent<MeshFilter>().mesh = this.mesh;
    }

    public void Initialize(HairDataFacade data)
    {
      this.data = data;
      this.InitializeMaterial();
      this.InitializeMesh();
    }

    public void InitializeMesh()
    {
      this.mesh.triangles = (int[]) null;
      this.mesh.vertices = new Vector3[(int) this.data.Size.x];
      this.mesh.SetIndices(this.data.Indices, MeshTopology.Triangles, 0);
    }

    private void InitializeMaterial()
    {
      if ((Object) this.data.material != (Object) null)
        this.rend.material = this.data.material;
      else
        this.rend.material = Resources.Load<Material>("Materials/Hair");
      if (this.data.StyleMode)
      {
        if (this.data.BarycentricsFixed != null)
          this.rend.material.SetBuffer("_Barycentrics", this.data.BarycentricsFixed.ComputeBuffer);
        else
          this.rend.material.SetBuffer("_Barycentrics", (ComputeBuffer) null);
      }
      else if (this.data.Barycentrics != null)
        this.rend.material.SetBuffer("_Barycentrics", this.data.Barycentrics.ComputeBuffer);
      else
        this.rend.material.SetBuffer("_Barycentrics", (ComputeBuffer) null);
      if (this.data.TessRenderParticles != null)
        this.rend.material.SetBuffer("_Particles", this.data.TessRenderParticles.ComputeBuffer);
      else
        this.rend.material.SetBuffer("_Particles", (ComputeBuffer) null);
    }

    public void Dispatch()
    {
      this.UpdateBounds();
      this.UpdateMaterial();
      this.UpdateRenderer();
    }

    public Shader GetShader()
    {
      if ((Object) this.rend != (Object) null && (Object) this.rend.material != (Object) null)
        return this.rend.material.shader;
      return (Object) this.data.material != (Object) null ? this.data.material.shader : (Shader) null;
    }

    public void SetShader(Shader s)
    {
      if (!((Object) this.rend.material.shader != (Object) s))
        return;
      this.rend.material.shader = s;
    }

    private void UpdateBounds() => this.mesh.bounds = this.transform.InverseTransformBounds(this.data.Bounds);

    private void UpdateMaterial()
    {
      if (this.data.StyleMode)
      {
        if (this.data.BarycentricsFixed != null)
          this.rend.material.SetBuffer("_Barycentrics", this.data.BarycentricsFixed.ComputeBuffer);
        else
          this.rend.material.SetBuffer("_Barycentrics", (ComputeBuffer) null);
      }
      else if (this.data.Barycentrics != null)
        this.rend.material.SetBuffer("_Barycentrics", this.data.Barycentrics.ComputeBuffer);
      else
        this.rend.material.SetBuffer("_Barycentrics", (ComputeBuffer) null);
      if (this.data.TessRenderParticles != null)
        this.rend.material.SetBuffer("_Particles", this.data.TessRenderParticles.ComputeBuffer);
      else
        this.rend.material.SetBuffer("_Particles", (ComputeBuffer) null);
      this.rend.material.SetVector("_LightCenter", (Vector4) this.data.LightCenter);
      if (this.data.StyleMode)
      {
        Vector2 vector2;
        vector2.x = 4f;
        vector2.y = this.data.TessFactor.y;
        this.rend.material.SetFloat("_RandomBarycentric", 0.0f);
        this.rend.material.SetVector("_TessFactor", (Vector4) vector2);
        this.rend.material.SetFloat("_StandWidth", 1f / 1000f * this.data.WorldScale);
        this.rend.material.SetFloat("_MaxSpread", 1f);
      }
      else
      {
        this.rend.material.SetFloat("_RandomBarycentric", 1f);
        this.rend.material.SetVector("_TessFactor", (Vector4) this.data.TessFactor);
        this.rend.material.SetFloat("_StandWidth", this.data.StandWidth * this.data.WorldScale);
        this.rend.material.SetFloat("_MaxSpread", this.data.MaxSpread * this.data.WorldScale);
      }
      this.rend.material.SetFloat("_SpecularShift", this.data.SpecularShift);
      this.rend.material.SetFloat("_PrimarySpecular", this.data.PrimarySpecular);
      this.rend.material.SetFloat("_SecondarySpecular", this.data.SecondarySpecular);
      this.rend.material.SetColor("_SpecularColor", this.data.SpecularColor);
      this.rend.material.SetFloat("_Diffuse", this.data.Diffuse);
      this.rend.material.SetFloat("_FresnelPower", this.data.FresnelPower);
      this.rend.material.SetFloat("_FresnelAtten", this.data.FresnelAttenuation);
      this.rend.material.SetVector("_WavinessAxis", (Vector4) this.data.WavinessAxis);
      this.rend.material.SetVector("_Length", (Vector4) this.data.Length);
      this.rend.material.SetFloat("_Volume", this.data.Volume);
      this.rend.material.SetVector("_Size", this.data.Size);
      this.rend.material.SetFloat("_RandomTexColorPower", this.data.RandomTexColorPower);
      this.rend.material.SetFloat("_RandomTexColorOffset", this.data.RandomTexColorOffset);
      this.rend.material.SetFloat("_IBLFactor", this.data.IBLFactor);
    }

    private void UpdateRenderer()
    {
      this.rend.shadowCastingMode = !this.data.CastShadows ? ShadowCastingMode.Off : ShadowCastingMode.On;
      this.rend.receiveShadows = this.data.ReseiveShadows;
    }

    public bool IsVisible => this.rend.isVisible;
  }
}
