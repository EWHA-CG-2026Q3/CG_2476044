Shader "CG/S17_MyTextureAbs"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _BaseMap;

            void vert(float3 positionOS : POSITION,
                      float2 uv : TEXCOORD0,
                      out float2 uvOut : TEXCOORD0,
                      out float4 positionCS : SV_POSITION)
            {
                uvOut = uv;
                positionCS = TransformObjectToHClip(positionOS);
            }

            float4 frag(float2 uv : TEXCOORD0) : SV_Target
            {
                float2 p = uv;

                if (uv.x > 0.5 || uv.y < 0.5) return float4(0.5, 0.5, 0.5, 1);
                p = uv + float2(0.5, -0.5);

                return tex2D(_BaseMap, p);
            }
            ENDHLSL
        }
    }
}