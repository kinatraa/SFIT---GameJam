Shader "Custom/BlobColor"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Speed ("Flow Speed", Float) = 0.5
        _Scale ("Noise Scale", Float) = 5
        _Brightness ("Brightness", Float) = 1
        _Blackness ("Black Mix", Range(0,1)) = 0.6
        _BlendStrength ("Overlay Strength", Range(0,1)) = 0.5
        _EffectEnabled ("Enable Effect", Range(0,1)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Speed;
            float _Scale;
            float _Brightness;
            float _Blackness;
            float _BlendStrength;
            bool _EffectEnabled;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            fixed3 rainbow(float t)
            {
                t = frac(t);
                float3 c;
                c.r = abs(sin(6.2831 * t));
                c.g = abs(sin(6.2831 * (t + 0.33)));
                c.b = abs(sin(6.2831 * (t + 0.66)));
                return c;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 baseColor = tex2D(_MainTex, uv) * i.color;

                float n = noise(uv * _Scale + _Time.y * _Speed);
                float3 rainbowCol = rainbow(n) * _Brightness;
                float3 finalEffect = lerp(float3(0, 0, 0), rainbowCol, n * (1.0 - _Blackness));
                float blend = _BlendStrength * _EffectEnabled;
                float3 finalColor = lerp(baseColor.rgb, finalEffect, blend);
                return float4(finalColor, baseColor.a);
            }
            ENDCG
        }
    }
}