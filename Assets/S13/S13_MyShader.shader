Shader "CG/S13_MyShader"
{
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 vert(float3 positionOS : POSITION) : SV_POSITION
            {
                return TransformObjectToHClip(positionOS);   // P × V × M
            }

            half4 frag() : SV_Target
            {
                return half4(1, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}
