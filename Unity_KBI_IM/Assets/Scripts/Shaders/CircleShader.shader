Shader "Unlit/CircleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // challenge: make circle red + a little transparent
            // red: (1, 0, 0)
            // a = 0.75
            // float4(1, 0, 0, 0.75)
            // a = 1, opaque
            // a = 0, transparent

            fixed4 frag(v2f i) : SV_Target
            {
                // Parameters
                float4 circleColor = float4(1, 1, 1, 0.6);       // challenge: float4 circleColor = float4(1, 0, 0, 0.5);
                float thickness = 0.2;
                float fade = 0.005;

                // -1 -> 1 local space
                float2 uv = i.uv * 2.0 - 1.0;

                // Calculate distance and fill circle with white
                float distance = 1.0 - length(uv);
                float4 colorMask = smoothstep(0.0, fade, distance);                 // outer circle
                colorMask *= smoothstep(thickness + fade, thickness, distance);     // inner circle

                // Set output color
                float4 fragColor = float4(colorMask) * circleColor;

                return fragColor;
            }
            ENDCG
        }
    }
}
