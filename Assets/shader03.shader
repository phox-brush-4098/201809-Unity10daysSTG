// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "flyugel/UnlitTest"
{
	Properties
	{
		// _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader
	{
		Tags 
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
			"PreviewType" = "Sphere"
			"ForceNoShadowCasting" = "True"
		}

		Pass
		{
			Tags { "LightMode" = "Always"}
			Lighting Off
			ZTest Always
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			
			struct appdata 
			{
				half4 vertex : POSITION;
				// float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID 
			};

			struct v2f { 
				// float2 uv : TEXCOORD0;
				half4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				return o;
			}

			fixed4 _Color;
			fixed4 frag() : COLOR 
			{
                return _Color;
            }			

			ENDCG
		}
	}
}
