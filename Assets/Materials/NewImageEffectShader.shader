Shader "Effects/NewImageEffectShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Contrast("Contrast", range(0, 2)) = 1
		_Radius("Radius", int) = 1
		_BlurEffect("Blur Effect", range(0, 1)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
//		GrapPass{};
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos: TEXCOORD2;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Contrast;
			float _BlurEffect;
			int _Radius;

			fixed4 frag (v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, IN.uv);
				// just invert the colors
				col.rgb = ((col.rgb-0.5) * _Contrast)+0.5;
				return col;
			}
			ENDCG
		}
	}
}
