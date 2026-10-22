Shader "CG/S13_SDFCircle_Finish"
{
    Properties
    {
        _Radius ("Radius", Range(0, 0.5)) = 0.4
        _CircleColor ("Circle Color", Color) = (1, 0.5, 0, 1)
        _BackColor ("Back Color", Color) = (0, 0, 0, 1)
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
                float _Radius;
                half4 _CircleColor;
                half4 _BackColor;
            CBUFFER_END

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            Varyings vert(float3 positionOS : POSITION, float2 uv : TEXCOORD0)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(positionOS);
                OUT.uv = uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // UV 중심까지의 거리로 원 안팎 판정
                float d = distance(IN.uv, float2(0.5, 0.5));
                float inside = step(d, _Radius);
                return lerp(_BackColor, _CircleColor, inside);
            }
            ENDHLSL
        }
    }
}
