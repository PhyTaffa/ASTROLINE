
Shader "Custom/DitheredFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Fade ("Fade", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Fade;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.screenPos = OUT.positionHCS;
                return OUT;
            }

            float Dither4x4Bayer(int x, int y)
            {
                int bayer[16] = {
                    0,  8,  2, 10,
                    12, 4, 14, 6,
                    3, 11, 1,  9,
                    15, 7, 13, 5
                };
                return bayer[y * 4 + x] / 16.0;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                screenUV = 0.5 * (float2(screenUV.x + 1, screenUV.y + 1));
                int2 pixelCoord = int2(fmod(screenUV * _ScreenParams.xy, 4));

                float threshold = Dither4x4Bayer(pixelCoord.x, pixelCoord.y);
                if (_Fade < threshold)
                    discard;

                half4 col = tex2D(_MainTex, IN.uv) * _Color;
                col.a *= _Fade;
                return col;
            }
            ENDHLSL
        }
    }
}
