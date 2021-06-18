Shader "Particles/Gradient 3Color Additive Intensify Custom" {
	Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_ColorTop ("Top Color", Color) = (1,1,1,1)
		_ColorMid ("Mid Color", Color) = (1,1,1,1)
		_ColorBot ("Bot Color", Color) = (1,1,1,1)
		_Middle ("Middle", Range(0.001, 0.999)) = 1
		_Shear ("Shear", Range(-4,4)) = 0
		_Radian ("Rotate", Range(-3.14159,3.14159)) = 0
        _Glow ("Intensity", Range(0, 10)) = 1
		_Saturation ("Saturation", Range(0, 8)) = 1
	}

	SubShader {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane"}
		LOD 100
		Cull Off
		ZWrite Off
		Blend SrcAlpha One

		Pass {
		CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed4 _ColorTop;
			fixed4 _ColorMid;
			fixed4 _ColorBot;
			float  _Middle;
			half _Glow;
			half _Saturation;
			half _Shear;
			float _Radian;
			
			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				float4 custom1 : TEXCOORD1;
				half4 custom2 : TEXCOORD2;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				half4 custom1 : TEXCOORD1;
				half2 custom2 : TEXCOORD2;
			};

			v2f vert (appdata v) {
				v2f o;
				v.vertex.xy = mul(v.vertex.xy,float2x2(1,0,_Shear,1));
				v.vertex.xz = mul(v.vertex.xz,float2x2(cos(_Radian),sin(_Radian),-sin(_Radian),cos(_Radian)));
				o.pos = UnityObjectToClipPos (v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color * _TintColor;
				o.custom1 = v.custom1;
				o.custom2 = v.custom2;
				return o;
			}

			fixed4 frag (v2f i) : COLOR {
				_ColorTop.rgb *= i.color.rgb;
				_ColorMid *= float4 (i.texcoord.zw,i.custom1.xy);
				_ColorBot *= float4 (i.custom1.zw,i.custom2);
				fixed4 col = lerp(_ColorBot, _ColorMid, i.texcoord.y / _Middle) * step(i.texcoord.y, _Middle);
				col += lerp(_ColorMid, _ColorTop, (i.texcoord.y - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.y);
				col *= tex2D(_MainTex, i.texcoord);
				col.rgb = pow(col.rgb * _Glow, _Saturation);
				//col.rgb *= i.color.rgb;
				col.a *= i.color.a;
				return col;
			}
		ENDCG
		}
	}
	FallBack "Mobile/Particles/Additive"
}