Shader "Custom/AlphaCircularScanDirectionalThickWithSpeedURP"
{
    Properties
    {
        _MainTex("Shape Texture", 2D) = "white" {}

        _LineColor("Line Color", Color) = (1,1,1,1)
        _Speed("Rotation Speed", Float) = 1

        _ArcWidth("Arc Width", Float) = 0.15

        _Sharpness("Front Sharpness", Float) = 50
        _TailBlur("Tail Blur Amount", Float) = 5

        _LineThickness("Line Thickness", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            float4 _LineColor;
            float _Speed;
            float _ArcWidth;

            float _Sharpness;
            float _TailBlur;

            float _LineThickness;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                if (tex.a < 0.01)
                    discard;

                float2 uv = i.uv - 0.5;

                float angle = atan2(uv.y, uv.x);
                angle = (angle + 3.14159265) / (2 * 3.14159265);

                float rot = frac(1.0 - (_Time.y * _Speed));
                float diff = abs(frac(angle - rot + 1.0));

                diff /= max(_LineThickness, 0.01);

                float front = exp(-diff * _Sharpness);
                float tail = exp(-diff * _TailBlur);

                float arc = front * tail;
                arc *= smoothstep(_ArcWidth, 0.0, diff);

                half4 col = _LineColor * arc;
                col.a *= tex.a;

                return col;
            }
            ENDHLSL
        }
    }
}
