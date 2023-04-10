Shader "Custom/MapShader" {
    Properties {
        _MainTex ("Base Texture", 2D) = "white" {}
        _WaterTex ("Water Texture", 2D) = "white" {}
        _SandTex ("Sand Texture", 2D) = "white" {}
        _GrassTex ("Grass Texture", 2D) = "white" {}
        _MountainTex ("Mountain Texture", 2D) = "white" {}
        _BlendTex ("Blend Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _WaterTex;
            sampler2D _SandTex;
            sampler2D _GrassTex;
            sampler2D _MountainTex;
            sampler2D _BlendTex;

            float4 _MainTex_ST;
            float4 _WaterTex_ST;
            float4 _SandTex_ST;
            float4 _GrassTex_ST;
            float4 _MountainTex_ST;
            float4 _BlendTex_ST;

            float _WaterLevel;
            float _SandLevel;
            float _GrassLevel;
            float _MountainLevel;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float4 noise = tex2D(_MainTex, i.uv);
                float4 blend = tex2D(_BlendTex, i.uv);

                if (noise.r <= _WaterLevel) {
                    return tex2D(_WaterTex, i.uv);
                } else if (noise.r <= _SandLevel) {
                    return tex2D(_SandTex, i.uv);
                } else if (noise.r <= _GrassLevel) {
                    return tex2D(_GrassTex, i.uv);
                } else if (noise.r <= _MountainLevel) {
                    return tex2D(_MountainTex, i.uv);
                } else {
                    return blend;
                }
            }
            ENDCG
        }
    }
}