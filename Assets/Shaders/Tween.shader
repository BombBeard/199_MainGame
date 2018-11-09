// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Shaders101/Tween"
{		
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//Used to interpolate between the two Textures
		_SecondTex("Second Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Tween("Tween", Range(0, 1)) = 0 
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
			sampler2D _SecondTex;
			fixed4 _Color;
			float _Tween;
			
			fixed4 frag (v2f i) : SV_Target
			{
			//Interpolates between the two textures based on the _Tween range value
			fixed4 col = lerp(tex2D(_MainTex, i.uv), tex2D(_SecondTex, i.uv), _Tween);
			//Tints the texture
			col *= _Color;


				return col;
			}
			ENDCG
		}
	}
}
