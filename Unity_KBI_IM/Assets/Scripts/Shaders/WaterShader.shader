Shader "Unlit/WaterShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _WaterColor ("Water Color", Color) = (0, 0, 0 , 0)
        _CellSize("Cell Size", Range(0, 2)) = 2
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertIn {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertOut {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _WaterColor;
            float _CellSize;

            VertOut vert (VertIn v) {
                VertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (VertOut i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _WaterColor;
            }
            ENDCG
        }
    }
}
