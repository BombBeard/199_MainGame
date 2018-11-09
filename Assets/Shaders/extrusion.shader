// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Shaders101/Extrusion"
{		
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ExtrudeAmount("Extrude Amount", Range(.01, 360)) = 0.01
		_Amp("Amplitude", Range(.01, 10)) = 0.0
	}
	SubShader
	{
		Tags{
			"Queue" = "Transparent"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			//One One
			

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv: TEXCOORD0;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv: TEXCOORD0;
				float3 normal: NORMAL;
			};

			float _ExtrudeAmount;
			float _Amp;

			void vert (appdata_full v)
			{
				v.vertex.xyz += v.normal * _ExtrudeAmount;
				//v.vertex.x
			}

			sampler2D _MainTex;
			
			fixed4 frag (v2f i) : SV_Target
			{
				//Interpolates between the two textures based on the _Tween range value
				fixed4 col = tex2D(_MainTex, i.uv) ;
				
				return col;
			}
			ENDCG
		}
	}
}
