Shader "Hidden/ScotopicLuminance"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScotopicStrength ("Scotopic Strength", float) = 0
		_ScotopicColor ("Scotopic color", Color) = (0.83, 1, 0.76, 1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _ScotopicStrength;
			half4 _ScotopicColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 image = tex2D(_MainTex, i.uv);

				//Scotopic luminosity
				//For very low levels of intensity (scotopic vision), the sensitivity of the eye is mediated by rods, not cones, and shifts toward the violet
				//Convert RGB color space to XYZ color space
				float X = dot(image.rgb, float3(0.4124, 0.3576, 0.1805));
				float Y = dot(image.rgb, float3(0.2126, 0.7152, 0.0722));
				float Z = dot(image.rgb, float3(0.0193, 0.1192, 0.9505));

				//Add Scotopic function
				float V = Y*(1.33*(1+(Y+Z)/X) -1.68);

				float nRed = V*_ScotopicColor.r*_ScotopicColor.a;
				float nGreen = V*_ScotopicColor.g*_ScotopicColor.a;
				float nBlue = V*_ScotopicColor.b*_ScotopicColor.a;

				image.rgb = lerp(image.rgb, half3(nRed,nGreen,nBlue), _ScotopicStrength);

				return image;
			}
			ENDCG
		}
	}
}
