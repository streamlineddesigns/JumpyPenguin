// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Ground" {
	Properties {
		_Tess ("Tessellation", Range(1,32)) = 4
		_GroundColor ("Ground Color", Color) = (1,1,1,1)
		_GroundTex ("Ground (RGB)", 2D) = "white" {}
		_UpGroundColor ("UpGround Color", Color) = (1,1,1,1)
		_UpGroundTex ("UpGround (RGB)", 2D) = "white" {}
		_DownGroundColor ("DownGround Color", Color) = (1,1,1,1)
		_DownGroundTex ("DownGround (RGB)", 2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "white" {}
		_UpSplat ("UpSplatMap", 2D) = "black" {}
		_DownSplat ("DownSplatMap", 2D) = "black" {}
		_UpDisplacement ("UpDisplacement", Range(0, 3)) = 0.3
		_DownDisplacement ("DownDisplacement", Range(0, 3)) = 0.3
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance
		
		#pragma target 4.6

		 #include "Tessellation.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            float _Tess;

            float4 tessDistance (appdata v0, appdata v1, appdata v2) {
                float minDist = 10.0;
                float maxDist = 25.0;
                return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
            }

            sampler2D _DownSplat;
			sampler2D _UpSplat;
            float _UpDisplacement;
			float _DownDisplacement;

            void disp (inout appdata v)
            {
                float d = tex2Dlod(_DownSplat, float4(v.texcoord.x, v.texcoord.y,0,0)).r * _DownDisplacement;
				float u = tex2Dlod(_UpSplat, float4(v.texcoord.x, v.texcoord.y,0,0)).r * _UpDisplacement;
			
                v.vertex.xyz -= v.normal * d;
				v.vertex.xyz += v.normal * u;
				v.vertex.xyz += v.normal * _DownDisplacement;
				v.vertex.xyz -= v.normal * _UpDisplacement;				
            }			
			
		sampler2D _GroundTex;
		fixed4 _GroundColor;
		sampler2D _UpGroundTex;
		fixed4 _UpGroundColor;
		sampler2D _DownGroundTex;
		fixed4 _DownGroundColor;
		sampler2D _NormalMap;


		struct Input {
			float2 uv_GroundTex;
			float2 uv_UpGroundTex;
			float2 uv_DownGroundTex;
			float2 uv_DownSplat;
			float2 uv_UpSplat;
			float2 uv_NormalMap;
		};

		half _Glossiness;
		half _Metallic;
		

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			half DownAmount = tex2Dlod(_DownSplat, float4(IN.uv_DownSplat.x,IN.uv_DownSplat.y,0,0)).r;
			half UpAmount = tex2Dlod(_UpSplat, float4(IN.uv_UpSplat.x,IN.uv_UpSplat.y,0,0)).r;
			//fixed4 d = tex2D (_GroundTex, IN.uv_GroundTex) * _GroundColor;
			
			fixed4 u = lerp(tex2D (_GroundTex, IN.uv_GroundTex) * _GroundColor, tex2D (_UpGroundTex, IN.uv_UpGroundTex) * _UpGroundColor, UpAmount);
			fixed4 d = lerp(tex2D (_GroundTex, IN.uv_GroundTex) * _GroundColor, tex2D (_DownGroundTex, IN.uv_DownGroundTex) * _DownGroundColor, DownAmount);

			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Albedo += u.rgb;
			o.Albedo += d.rgb;
			o.Albedo += d.rgb;
			
			//o.Albedo = d.rgb;
			o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_NormalMap));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;			
			o.Alpha = d.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
