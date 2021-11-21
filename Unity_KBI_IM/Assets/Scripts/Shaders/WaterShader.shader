Shader "Unlit/WaterShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _WaterColor ("Water Color", Color) = (0, 0, 0 , 0)
        _CellSize("Cell Size", Range(0, 1)) = 0.05
    }
    SubShader {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 worldPos : TEXCOOD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _WaterColor;
            float _CellSize;

            float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)){
            	float2 smallValue = sin(value);
            	float random = dot(smallValue, dotDir);
            	random = frac(sin(random) * 143758.5453);
            	return random;
            }
            
            float2 rand2dTo2d(float2 value){
            	return float2(
            		rand2dTo1d(value, float2(12.989, 78.233)),
            		rand2dTo1d(value, float2(39.346, 11.135))
            	);
            }

       	    float voronoiNoise(float2 value){
                float2 baseCell = floor(value);

                float minDistToCell = 10;
                [unroll]
                for(int x=-1; x<=1; x++){
                    [unroll]
                    for(int y=-1; y<=1; y++){
                        float2 cell = baseCell + float2(x, y);
                        float2 cellPosition = cell + rand2dTo2d(cell);
                        float2 toCell = cellPosition - value;
                        float distToCell = length(toCell);
                        if(distToCell < minDistToCell){
                            minDistToCell = distToCell;
                        }
                    }
                }
                return minDistToCell;
		    }

            VertOut vert (VertIn v) {
                VertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (VertOut i) : SV_Target {
                float2 value = i.worldPos.xz / _CellSize;
			    float noise = voronoiNoise(value + _Time.yy);

                return float4(noise, noise, noise, 1);

                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _WaterColor;
            }
            ENDCG
        }
    }
}
