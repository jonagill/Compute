Shader "Custom/Cube"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
         _Smoothness("Smoothness", Range(0, 1)) = 0
         _Metallic("Metallic", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Cull Off

        CGPROGRAM

		#include "ComputeShared.cginc"

        #pragma surface surf Standard vertex:vert
        #pragma target 3.5

		#if SHADER_TARGET >= 35 && (defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_XBOXONE) || defined(SHADER_API_PSSL) || defined(SHADER_API_SWITCH) || defined(SHADER_API_VULKAN) || (defined(SHADER_API_METAL) && defined(UNITY_COMPILER_HLSLCC)))
			#define SUPPORT_STRUCTUREDBUFFER
		#endif

        struct appdata
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
			float4 tangent : TANGENT;
            uint vid : SV_VertexID;
        };

        struct Input
        {
            float vface : VFACE;
        };

        half4 _Color;
		half _Smoothness;
        half _Metallic;

#if defined(SUPPORT_STRUCTUREDBUFFER)
        StructuredBuffer<ComputeVertex> VertexBuffer;
#endif

        void vert(inout appdata v)
        {
#if defined(SUPPORT_STRUCTUREDBUFFER)
			ComputeVertex vertex = VertexBuffer[v.vid];
			v.vertex.xyz = vertex.position;
			v.normal.xyz = vertex.normal;
#endif
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
			o.Albedo = _Color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
            o.Normal = float3(0, 0, IN.vface < 0 ? -1 : 1);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
