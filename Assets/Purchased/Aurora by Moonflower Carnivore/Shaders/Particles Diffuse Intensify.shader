Shader "Particles/Diffuse Intensify" {
	Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glow ("Intensity", Range(0, 9)) = 1
        _Offset ("Diffuse Offset", Range(0, 1)) = 0
		_Saturation ("Saturation", Range(0, 8)) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
			#pragma surface surf Standard addshadow fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			half4 _TintColor;
			half _Glow;
			half _Saturation;
			half _Offset;

			struct Input {
				float2 uv_MainTex;
				float3 color:COLOR;
			};

			void surf (Input IN, inout SurfaceOutputStandard o) {
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
				o.Albedo = pow((c.rgb * IN.color.rgb * _TintColor.rgb - _Offset) * (1 + _Glow), _Saturation);
			}
		ENDCG
	}
	FallBack "Unlit/Texture"
}
