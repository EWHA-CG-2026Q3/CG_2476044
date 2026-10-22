Shader "CG/S13_MyShader_Finish"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 0, 0, 1)
    }

    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
            CBUFFER_END

            float4 vert(float3 positionOS : POSITION) : SV_POSITION
            {
                return TransformObjectToHClip(positionOS);
            }

            half4 frag() : SV_Target
            {
                return _BaseColor;
            }
            ENDHLSL
        }
    }
}
