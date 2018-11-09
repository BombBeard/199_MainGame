// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Shaders101/Tiled"
{		
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_TileAmount("Tile Amount", Range(0, 10)) = 0
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos( v.vertex );
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			fixed4 _Color;
			float _TileAmount;
			
			fixed4 frag (v2f i) : SV_Target
			{
			//Interpolates between the two textures based on the _Tween range value
			fixed4 col = tex2D(_MainTex, i.uv * _TileAmount) ;
			//Calculates Luminance from the texture
			float lum = col.r * 0.3 + col.g * 0.59 + col.b * 0.11;
			float4 grayscale = float4(lum, lum, lum, col.a);
			return _Color * grayscale;
			}
			ENDCG
		}
	}
}
