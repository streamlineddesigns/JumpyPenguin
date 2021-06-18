Shader "Particles/Additive Intensify Rotate Shear" {
    Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Glow ("Intensity", Range(0, 127)) = 1
		_Shear ("Shear", Range(-4,4)) = 0
		_Radian ("Rotate", Range(-3.14159,3.14159)) = 0
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

                sampler2D _MainTex;
                half4 _MainTex_ST;
                half4 _TintColor;
                half _Glow;
				half _Shear;
				float _Radian;

                struct vertIn {
                    float4 pos : POSITION;
                    half2 tex : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    half2 tex : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                v2f vert (vertIn v) {
                    v2f o;
					
					v.pos.xy = mul(v.pos.xy,float2x2(1,0,_Shear,1));
					v.pos.xz = mul(v.pos.xz,float2x2(cos(_Radian),sin(_Radian),-sin(_Radian),cos(_Radian)));
                    o.pos = UnityObjectToClipPos(v.pos);
                    o.tex = v.tex * _MainTex_ST.xy + _MainTex_ST.zw;
                    o.color = v.color * _TintColor;
                    o.color.rgb *= _Glow;
                    return o;
                }

                fixed4 frag (v2f f) : SV_Target 
				{
                    fixed4 col = tex2D(_MainTex, f.tex);
                    col *= f.color;
                    return col;
                }
            ENDCG
        }
    }
	FallBack "Mobile/Particles/Additive"
}