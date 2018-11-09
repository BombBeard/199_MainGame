// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Shaders101/Basic"
{		
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//Used to interpolate between the two Textures
		//_SecondTex("Second Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		//_Tween("Tween", Range(0, 1)) = 0 
	}
	SubShader
	{
		Tags{
			"Queue" = "Transparent"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

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
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			//Tints the texture
			col *= _Color;
			//Adds in UV colors
			col *= fixed4(i.uv.r, i.uv.g, 0, 1);

				return col;
			}
			ENDCG
		}
	}
}
