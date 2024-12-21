Shader "Roundy/FastUnlitTerrainShader"
{
    Properties
    {
        [Header(Main Properties)]
        [Space(15)]
        _SplatTex ("Splat Map", 2D) = "white" {}
        _TextureArray ("Texture Array", 2DArray) = "" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        
        [Space(10)]
        [Header(Tiling Properties)]
        [Space(15)]
        _Tiling0 ("Tiling 0 (Scale XY, Offset ZW)", Vector) = (1,1,0,0)
        _Tiling1 ("Tiling 1 (Scale XY, Offset ZW)", Vector) = (1,1,0,0)
        _Tiling2 ("Tiling 2 (Scale XY, Offset ZW)", Vector) = (1,1,0,0)
        _Tiling3 ("Tiling 3 (Scale XY, Offset ZW)", Vector) = (1,1,0,0)
        
        [Space(10)]
        [Header(Blending Properties)]
        [Space(15)]
        _HeightBlendDistance ("Height Blend Distance", Range(0.01, 1)) = 0.01
        _HeightBlendStrength ("Height Blend Strength", Range(0.01, 8)) = 1
        
        [Space(10)]
        [Header(Smoothness Properties)]
        [Space(15)]
        [Toggle(ENABLE_SMOOTHNESS)] _EnableSmoothnessFlag("Enable Alpha Channel Smoothness", Float) = 0
        [Space(10)]
        
        [Header(Smoothness Effect)]
        [Space(10)]
        _SmoothnessColor("Smoothness Color", Color) = (1,1,1,1)
        _FresnelPower("Fresnel Power", Range(1, 10)) = 5
        _FresnelIntensity("Fresnel Intensity", Range(0, 2)) = 1
        _SpecularPower("Specular Power", Range(1, 256)) = 100
        _SpecularIntensity("Specular Intensity", Range(0, 2)) = 1
        
        [Header(Per Texture Smoothness)]
        [Space(10)]
        _Texture1("Texture 1", Range(0, 1)) = 1
        _Texture2("Texture 2", Range(0, 1)) = 1
        _Texture3("Texture 3", Range(0, 1)) = 1
        _Texture4("Texture 4", Range(0, 1)) = 1
        
        [Space(10)]
        [Header(Resampling Properties)]
        [Space(15)]
        [Toggle(ENABLE_RESAMPLING)] _EnableResampling("Enable Distance Resampling", Float) = 0
        _ResampleDistance ("Distance (Start, End)", Vector) = (20, 50, 0, 0)
        _ResampleTiling ("Tiling", Vector) = (0.2, 0.2, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ ENABLE_RESAMPLING
            #pragma shader_feature_local _ ENABLE_SMOOTHNESS
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD3;
                float3 viewDir : TEXCOORD4;
                #ifdef ENABLE_RESAMPLING
                    float detailBlend : TEXCOORD5;
                #endif
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            sampler2D _SplatTex;
            UNITY_DECLARE_TEX2DARRAY(_TextureArray);
            float4 _Tiling0;
            float4 _Tiling1;
            float4 _Tiling2;
            float4 _Tiling3;
            float _HeightBlendDistance;
            float _HeightBlendStrength;
            fixed4 _TintColor;
            fixed4 _SmoothnessColor;
            float _FresnelPower;
            float _FresnelIntensity;
            float _SpecularPower;
            float _SpecularIntensity;
            float _Texture1;
            float _Texture2;
            float _Texture3;
            float _Texture4;
            
            #ifdef ENABLE_RESAMPLING
                float4 _ResampleDistance;
                float4 _ResampleTiling;
            #endif

            #define WEIGHT_THRESHOLD 0.01

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                #ifdef LIGHTMAP_ON
                    o.uv2 = v.uv2 * unity_LightmapST.xy + unity_LightmapST.zw;
                #else
                    o.uv2 = v.uv2;
                #endif

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
                
                #ifdef ENABLE_RESAMPLING
                    float dist = distance(_WorldSpaceCameraPos, o.worldPos);
                    o.detailBlend = smoothstep(_ResampleDistance.x, _ResampleDistance.y, dist);
                #endif
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            void GetTextureAndHeight(float2 uv, float layerIndex, float4 tiling, float smoothnessMult,
                                   #ifdef ENABLE_RESAMPLING
                                       float detailBlend,
                                   #endif
                                   float weight,
                                   out fixed4 color, out float height, out float smoothness)
            {
                if(weight < WEIGHT_THRESHOLD)
                {
                    color = fixed4(0,0,0,1);
                    height = 0;
                    smoothness = 0;
                    return;
                }

                float2 baseUV = uv * tiling.xy + tiling.zw;
                fixed4 texSample = UNITY_SAMPLE_TEX2DARRAY(_TextureArray, float3(baseUV, layerIndex));
                
                #ifdef ENABLE_RESAMPLING
                    fixed4 detailColor = UNITY_SAMPLE_TEX2DARRAY(_TextureArray, float3(baseUV * _ResampleTiling.xy, layerIndex));
                    texSample = lerp(texSample, detailColor, detailBlend);
                #endif
                
                color = texSample;
                height = texSample.r * weight;
                
                #ifdef ENABLE_SMOOTHNESS
                    // Get smoothness from alpha channel (black = smooth, white = rough)
                    smoothness = (1.0 - texSample.a) * smoothnessMult * weight;
                #else
                    smoothness = 0;
                #endif
            }

            #ifdef ENABLE_SMOOTHNESS
            float3 ApplySmoothnessEffect(float3 color, float3 normal, float3 viewDir, float smoothness) 
            {
                // Calculate fresnel effect with configurable power
                float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), _FresnelPower);
                fresnel *= smoothness * _FresnelIntensity;
                
                // Add specular highlight with configurable power
                float3 halfVector = normalize(viewDir + normalize(_WorldSpaceLightPos0.xyz));
                float specular = pow(saturate(dot(normal, halfVector)), _SpecularPower);
                specular *= smoothness * _SpecularIntensity;
                
                // Apply smoothness color to the effect
                float3 smoothnessEffect = (fresnel + specular) * _SmoothnessColor.rgb;
                
                // Blend between diffuse and specular based on smoothness
                return lerp(color, color + smoothnessEffect, smoothness);
            }
            #endif

            fixed4 frag (v2f i) : SV_Target
            {
                float4 splat = tex2D(_SplatTex, i.uv);
                
                float weightSum = 0;
                float4 weights = float4(
                    splat.r > WEIGHT_THRESHOLD ? splat.r : 0,
                    splat.g > WEIGHT_THRESHOLD ? splat.g : 0,
                    splat.b > WEIGHT_THRESHOLD ? splat.b : 0,
                    splat.a > WEIGHT_THRESHOLD ? splat.a : 0
                );
                
                weightSum = weights.r + weights.g + weights.b + weights.a;
                if(weightSum > 0)
                {
                    weights /= weightSum;
                }

                fixed4 col0, col1, col2, col3;
                float h0, h1, h2, h3;
                float s0, s1, s2, s3;

                #ifdef ENABLE_RESAMPLING
                    GetTextureAndHeight(i.uv, 0, _Tiling0, _Texture1, i.detailBlend, weights.r, col0, h0, s0);
                    GetTextureAndHeight(i.uv, 1, _Tiling1, _Texture2, i.detailBlend, weights.g, col1, h1, s1);
                    GetTextureAndHeight(i.uv, 2, _Tiling2, _Texture3, i.detailBlend, weights.b, col2, h2, s2);
                    GetTextureAndHeight(i.uv, 3, _Tiling3, _Texture4, i.detailBlend, weights.a, col3, h3, s3);
                #else
                    GetTextureAndHeight(i.uv, 0, _Tiling0, _Texture1, weights.r, col0, h0, s0);
                    GetTextureAndHeight(i.uv, 1, _Tiling1, _Texture2, weights.g, col1, h1, s1);
                    GetTextureAndHeight(i.uv, 2, _Tiling2, _Texture3, weights.b, col2, h2, s2);
                    GetTextureAndHeight(i.uv, 3, _Tiling3, _Texture4, weights.a, col3, h3, s3);
                #endif

                float maxH = max(max(h0, h1), max(h2, h3));
                
                float4 heightBlend = saturate(1 - (maxH - float4(h0, h1, h2, h3)) / _HeightBlendDistance);
                heightBlend = pow(heightBlend, _HeightBlendStrength);
                heightBlend *= weights > WEIGHT_THRESHOLD;

                float heightSum = max(dot(heightBlend, float4(1,1,1,1)), 0.0001);
                heightBlend /= heightSum;

                fixed4 finalColor = 
                    col0 * heightBlend.r +
                    col1 * heightBlend.g +
                    col2 * heightBlend.b +
                    col3 * heightBlend.a;

                finalColor.rgb *= _TintColor.rgb;

                #ifdef ENABLE_SMOOTHNESS
                    // Calculate final smoothness value using the same height-based blending
                    float finalSmoothness = 
                        s0 * heightBlend.r +
                        s1 * heightBlend.g +
                        s2 * heightBlend.b +
                        s3 * heightBlend.a;

                    // Apply smoothness effect
                    finalColor.rgb = ApplySmoothnessEffect(finalColor.rgb, normalize(i.worldNormal), i.viewDir, finalSmoothness);
                #endif

                #ifdef LIGHTMAP_ON
                    half3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
                    finalColor.rgb *= lm;
                #endif

                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}