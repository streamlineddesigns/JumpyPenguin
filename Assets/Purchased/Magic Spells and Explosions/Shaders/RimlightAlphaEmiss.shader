 Shader "MagicalFX/RimNAlphaEmiss" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _BumpMap ("Bumpmap", 2D) = "bump" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
      _AlphaPower ("Alpha Power", Range(0.0,1.0)) = 1.0
      _MaskEmPower ("Mask Power", Range(5.0,50.0)) = 5.0
    }
    SubShader {
	  Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
      CGPROGRAM
      #pragma surface surf Lambert alpha
      struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 viewDir;
      };
      sampler2D _MainTex;
      sampler2D _BumpMap;
      float4 _RimColor;
      float _RimPower;
      float _AlphaPower;
      float _MaskEmPower;
      
      void surf (Input IN, inout SurfaceOutput o) 
      {
      	  float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
          o.Albedo = mainTex.rgb * (mainTex.a * _MaskEmPower);
          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = _RimColor.rgb * pow (rim, _RimPower);
          o.Alpha = _AlphaPower;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }