﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SimpleShader"
{
	Properties
	{
		col("Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode off }

			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag

			uniform StructuredBuffer<float3> VertexBuffer;
			uniform float3 col;

			struct v2f
			{
				float4  pos : SV_POSITION;
			};

			v2f vert(uint id : SV_VertexID)
			{
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(float4(VertexBuffer[id], 1));
				return OUT;
			}

			float4 frag(v2f IN) : COLOR
			{
				return float4(col,1);
			}

			ENDCG
		}
	}
}