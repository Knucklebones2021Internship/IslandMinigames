Shader "Unlit/AnimatedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

	        static float TAU = 6.28318530718f;

            v2f vert (appdata v) {
                int numWaves = 8; float amplitude = 0.25f;
                float wave = amplitude * sin(v.uv.x * TAU * numWaves + _Time.y) *v.uv.y;
                v.vertex.z += wave;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target{
                int numWaves = 8; // cool animated pattern
                float wave = sin(i.uv.x * TAU * numWaves + _Time.y) * i.uv.y;
                return wave * 0.5 + 0.5;

                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
