// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ScreenPositionEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				float4 scr_pos : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 scr_pos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				o.scr_pos = ComputeScreenPos(o.vertex);

				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 image = tex2D(_MainTex, i.uv);

				image = image * i.scr_pos;

				//Get position of current rendered pixel
				float2 position = i.scr_pos.xy * _ScreenParams.xy / i.scr_pos.w;
				if ((uint)position.y % 3 == 0) image *= float4(0, 0, 0, 1);
				if ((uint)position.x % 3 == 0) image *= float4(0, 0, 0, 1);

				return image;
			}
			ENDCG
		}
	}
}
