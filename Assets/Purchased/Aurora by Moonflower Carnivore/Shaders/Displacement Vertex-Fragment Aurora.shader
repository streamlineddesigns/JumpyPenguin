Shader "Displacement/Vertex-Fragment Aurora" {
	Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_ColorTop ("Top Color", Color) = (1,1,1,1)
		_ColorMid ("Mid Color", Color) = (1,1,1,1)
		_ColorBot ("Bot Color", Color) = (1,1,1,1)
		//_Middle ("Middle", Range(0.001, 0.999)) = 1
		
		_MainTex ("Main Texture. R: Opacity mask; G: Highlight; B: Banding", 2D) = "white" {}
		//_Mask ("Alpha Mask", 2D) = "white" {}
		
		_NoiseTex ("Displacement Map", 2D) = "bump" {}
		_Displace ("Vertex Displace", Vector) = (0, 0, 0, 0)
		_Freq ("Frequency", Vector) = (0, 0, 0, 0)
		
		_Shear ("Shear", Range(-4,4)) = 0
		_Radian ("Rotate", Range(-3.14159,3.14159)) = 0
		
        _Banding ("Banding", Range(0, 1)) = 1
        _Highlight ("Highlight", Range(0, 1)) = 1
        _Distortion ("Distortion", Range(0, 10)) = 0
        _Glow ("Intensity", Range(0, 40)) = 1
		_Saturation ("Saturation", Range(0, 8)) = 1
	}
    SubShader {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane"}
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha One

        Pass {
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _TintColor;
			fixed4 _ColorTop;
			fixed4 _ColorMid;
			fixed4 _ColorBot;
			half _Middle;
			
			sampler2D _MainTex;
			//sampler2D _Mask;
			sampler2D _NoiseTex;
			half4 _MainTex_ST;
			half4 _NoiseTex_ST;
			//half4 _Mask_ST;
			
			half _Shear;
			half3 _Displace;
			half3 _Freq;
			half _Radian;
			
			half _Distortion;
			half _Banding;
			half _Highlight;
			half _Glow;
			half _Saturation;

			struct vertIn {
				half4 pos : POSITION;
				half3 normal : NORMAL;
				half4 tex : TEXCOORD0;
				half4 tex1 : TEXCOORD1;
				fixed4 color : COLOR;
			};

			struct v2f {
				half4 pos : SV_POSITION;
				half4 tex : TEXCOORD0;
				half3 tex1 : TEXCOORD1;
				fixed4 color : COLOR;
			};

			v2f vert (vertIn v) {
				v2f o;
				
				half3 viewDir = ObjSpaceViewDir(v.pos);
				
				half2 texcoordNoiseTexX = (v.tex.x + v.tex.y) * 0.5 * _Freq.x * v.tex.w;
				half2 texcoordNoiseTexY = v.tex.xy * _Freq.y * v.tex.w;
				half2 texcoordNoiseTexZ = v.tex.x * _Freq.z * v.tex.w;
				//texcoordNoiseTex += v.tex.w;
				half3 noisetex2Dlod = half3(1,1,1);
				noisetex2Dlod.x = tex2Dlod(_NoiseTex, half4(texcoordNoiseTexX,0,0));
				noisetex2Dlod.y = tex2Dlod(_NoiseTex, half4(texcoordNoiseTexY,0,0));
				noisetex2Dlod.z = tex2Dlod(_NoiseTex, half4(texcoordNoiseTexZ,0,0));
				
				//v.pos = mul(unity_ObjectToWorld, v.pos);
					half3 noise = noisetex2Dlod * _Displace.xyz * v.tex1.y * v.normal;
					v.pos.xyz += noise;
					v.pos.xy = mul(v.pos.xy,half2x2(1,0,_Shear,1));
					v.pos.xz = mul(v.pos.xz,half2x2(cos(_Radian+v.tex1.w),sin(_Radian+v.tex1.w),-sin(_Radian+v.tex1.w),cos(_Radian+v.tex1.w)));
				//v.pos = mul(unity_WorldToObject, v.pos);
								
				o.tex.zw = v.tex.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				
				o.pos = UnityObjectToClipPos(v.pos);
				_MainTex_ST.z = v.tex.z;
				_MainTex_ST.w = v.tex.z * -0.5;
				o.tex.xy = v.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				o.color = v.color * _TintColor;
				
				o.tex1.x = v.tex1.x;
				o.tex1.yz = o.tex.xy;
				return o;
			}

			fixed4 frag (v2f i) : COLOR	{
				i.tex1.y *= _Distortion * tex2D(_MainTex, i.tex.zw).b * 0.1 + 1;
				
				_Middle = i.tex1.x;
				fixed4 grad = lerp(_ColorBot, _ColorMid, i.tex.w / _Middle) * step(i.tex.w, _Middle);
				grad += lerp(_ColorMid, _ColorTop, (i.tex.w - _Middle) / (1 - _Middle)) * step(_Middle, i.tex.w);
				fixed4 col = i.color * grad;
				half tex_xy_r = tex2D(_MainTex, i.tex.xy).r;
				col.a *= ((tex_xy_r * (1 - _Banding)) + (tex2D(_MainTex, i.tex1.yz).b * _Banding)) * tex2D(_MainTex, i.tex.zw).r * tex_xy_r;
				half absGlow = abs(_Glow - 1);
				//half tex1_yz_g = tex2D(_MainTex, i.tex1.yz).g;
				half tex1_yz_g = tex2D(_MainTex, i.tex.zw).g;
				col.rgb = pow(col.rgb * (((_Glow + tex1_yz_g) + (absGlow + tex1_yz_g * absGlow) * 0.2) * _Highlight + 1), _Saturation);
				return col;
			}
		ENDCG
        }
    }
	FallBack "Mobile/Particles/Additive"
}