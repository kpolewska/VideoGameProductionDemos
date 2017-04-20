Shader "Hidden/ScotopicLuminance"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

////////////////////
////SCOTOPIC LUMINANCE BLUE SHIFT
////////////////////
//half4 c = tex2D (_Diffuse, IN.uv_Diffuse);
//
////конвертим цветовое пространство RGB в цветовое пространство XYZ
//float X = dot(c.rgb,float3(0.4124, 0.3576, 0.1805));
//float Y = dot(c.rgb,float3(0.2126, 0.7152, 0.0722));
//float Z = dot(c.rgb,float3(0.0193, 0.1192, 0.9505));
//
////считаем scotopic luminance
//float V = Y*(1.33*(1+(Y+Z)/X) -1.68);
//
////tint the scotopic luminance V with a blue shift
//float nRed = V*1.05;
//float nBlue = V*0.97;
//float nGreen = V*1.27;
//
//c.rgb = lerp(c.rgb, half3(nRed,nGreen,nBlue), _BlueShift);
//
//o.Albedo = c.rgb;
//
////////////////////
////BLUE SHIFT END
////////////////////

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
