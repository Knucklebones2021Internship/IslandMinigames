Shader "Unlit/WaterShader" {
    Properties {
        _WaterColor ("Water Color", Color) = (0, 0, 0 , 0)
        [HDR] _RippleColor ("Ripple Color", Color) = (0, 0, 0 , 0)
        _CellDensity("Cell Density", float) = 1
        _ShearStrength("Shear Strength", float) = 1
        _RippleIntensity("Ripple Intensity", float) = 1
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
            };

            float4 _WaterColor;
            float4 _RippleColor;
            float _CellDensity;
            float _ShearStrength;
            float _RippleIntensity;

            inline float2 unity_voronoi_noise_randomVector (float2 UV, float offset) {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                UV = frac(sin(mul(UV, m)) * 46839.32);
                return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
            }
            
            void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells) {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float t = 8.0;
                float3 res = float3(8.0, 0.0, 0.0);
            
                for(int y=-1; y<=1; y++)
                {
                    for(int x=-1; x<=1; x++)
                    {
                        float2 lattice = float2(x,y);
                        float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);
                        if(d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                            Out = res.x;
                            Cells = res.y;
                        }
                    }
                }
            }

            void Unity_RadialShear_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out) {
                float2 delta = UV - Center;
                float delta2 = dot(delta.xy, delta.xy);
                float2 delta_offset = delta2 * Strength;
                Out = UV + float2(delta.y, -delta.x) * delta_offset + Offset;
            }

            VertOut vert (VertIn v) {
                VertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(VertOut i) : SV_Target{
                // apply a radial shear to our UVs
                float2 voronoiUV;
                Unity_RadialShear_float(i.uv, float2(0.5, 0.5), _ShearStrength, float2(0, 0), voronoiUV);

                // calculate voronoi value at this pixel
                float noise, cells;
                Unity_Voronoi_float(voronoiUV, _Time.y + 30, _CellDensity, noise, cells);

                // exponentiate the noise to get sharper lines
                noise = pow(noise, _RippleIntensity);

                //return float4(noise, noise, noise, 1);

                float4 rippleColor = float4(noise, noise, noise, 1) * _RippleColor;

                return _WaterColor + rippleColor;
            }
            ENDCG
        }
    }
}
