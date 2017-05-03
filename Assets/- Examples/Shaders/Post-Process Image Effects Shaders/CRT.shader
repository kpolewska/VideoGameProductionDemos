// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CRT" 
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _VertsColor("Verts fill color", Float) = 0
        _VertsColor2("Verts fill color 2", Float) = 0
        _Contrast("Contrast", Float) = 0
        _Br("Brightness", Float) = 0
        _ScanColor("Scan Color", Float) = 0
    }

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off Fog { Mode off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            #pragma target 3.0

            struct v2f 
            {
                float4 pos      : POSITION;
                float2 uv       : TEXCOORD0;
                float4 scr_pos : TEXCOORD1;
            };

            uniform sampler2D _MainTex;
            uniform float _VertsColor;
            uniform float _VertsColor2;
            uniform float _Contrast;
            uniform float _Br;
            uniform float _ScanColor;

            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
                o.scr_pos = ComputeScreenPos(o.pos);
                
                return o;
            }

            half4 frag(v2f i): COLOR
            {
                half4 color = tex2D(_MainTex, i.uv);
                
                float2 ps = i.scr_pos.xy *_ScreenParams.xy / i.scr_pos.w;
                if ((uint)ps.y % 3 == 0) color *= float4(_ScanColor, _ScanColor, _ScanColor, 1);
                
                int pp = (uint)ps.x % 3;
                float4 outcolor = float4(0, 0, 0, 1);
                //Spread the color + added color bleeding
                if (pp == 1) { outcolor.r = 1; outcolor.g = _VertsColor; outcolor.b = _VertsColor2; }
                else if (pp == 2) { outcolor.g = 1; outcolor.b = _VertsColor; outcolor.r = _VertsColor2; }
                else { outcolor.b = 1; outcolor.r = _VertsColor; outcolor.g = _VertsColor2;} 
                
                color = color * outcolor;
                
                color += (_Br / 255);
                color = color - _Contrast * (color - 1.0) * color *(color - 0.5);

				half4 offset = tex2D(_MainTex, i.uv);
                
                return color;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}