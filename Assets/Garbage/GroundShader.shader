Shader "Custom/SmoothHeightSlopeBlend"
{
    Properties
    {
        [Header(Textures)]
        _BaseTexture ("Base Texture (���� ��)", 2D) = "white" {}
        _MidTexture ("Mid Texture (�߰� ����)", 2D) = "white" {}
        _TopTexture ("Top Texture (���� ��)", 2D) = "white" {}
        _NoiseTexture ("Noise Texture (������)", 2D) = "gray" {}
        
        [Header(Height Settings)]
        _HeightScale ("Height Scale", Float) = 10.0
        _MidHeight ("Mid Height Threshold", Range(0, 1)) = 0.4
        _TopHeight ("Top Height Threshold", Range(0, 1)) = 0.7
        _HeightBlendRange ("Height Blend Range", Range(0.01, 1.0)) = 0.3
        _HeightContrast ("Height Blend Contrast", Range(0.1, 5.0)) = 1.5
        
        [Header(Slope Settings)]
        _SlopeThreshold ("Slope Threshold", Range(0, 1)) = 0.6
        _SlopeBlendRange ("Slope Blend Range", Range(0.01, 1.0)) = 0.4
        _SlopeInfluence ("Slope Influence", Range(0, 3)) = 1.2
        _SlopeContrast ("Slope Blend Contrast", Range(0.1, 5.0)) = 2.0
        
        [Header(Noise Settings)]
        _NoiseScale ("Noise Scale", Float) = 5.0
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.3
        _NoiseContrast ("Noise Contrast", Range(0.1, 3.0)) = 1.0
        
        [Header(Texture Scaling)]
        _BaseScale ("Base Texture Scale", Float) = 1.0
        _MidScale ("Mid Texture Scale", Float) = 1.0
        _TopScale ("Top Texture Scale", Float) = 1.0
        
        [Header(Advanced Blending)]
        _OverlayStrength ("Overlay Blending Strength", Range(0, 1)) = 0.2
        _HeightVariation ("Height Variation", Range(0, 0.5)) = 0.1
        _EdgeSoftness ("Edge Softness", Range(0, 2)) = 1.0
        
        [Header(Material Properties)]
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float height : TEXCOORD3;
                float slope : TEXCOORD4;
                float3 worldPos : TEXCOORD5;
            };
            
            TEXTURE2D(_BaseTexture);
            SAMPLER(sampler_BaseTexture);
            TEXTURE2D(_MidTexture);
            SAMPLER(sampler_MidTexture);
            TEXTURE2D(_TopTexture);
            SAMPLER(sampler_TopTexture);
            TEXTURE2D(_NoiseTexture);
            SAMPLER(sampler_NoiseTexture);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseTexture_ST;
                float4 _MidTexture_ST;
                float4 _TopTexture_ST;
                float4 _NoiseTexture_ST;
                float _HeightScale;
                float _MidHeight;
                float _TopHeight;
                float _HeightBlendRange;
                float _HeightContrast;
                float _SlopeThreshold;
                float _SlopeBlendRange;
                float _SlopeInfluence;
                float _SlopeContrast;
                float _NoiseScale;
                float _NoiseStrength;
                float _NoiseContrast;
                float _BaseScale;
                float _MidScale;
                float _TopScale;
                float _OverlayStrength;
                float _HeightVariation;
                float _EdgeSoftness;
                float _Smoothness;
                float _Metallic;
            CBUFFER_END
            
            // �ε巯�� ���� �Լ� (�� �ڿ������� ��ȯ)
            float SmoothStep3(float edge0, float edge1, float x, float contrast)
            {
                float t = saturate((x - edge0) / (edge1 - edge0));
                t = pow(t, contrast);
                return t * t * (3.0 - 2.0 * t);
            }
            
            // ������ ��� ����
            float GetNoiseBlend(float2 uv, float2 worldPos)
            {
                float2 noiseUV = uv * _NoiseScale + worldPos.xy * 0.01;
                float noise1 = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, noiseUV).r;
                
                // ���� ��Ÿ�� ������
                float noise2 = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, noiseUV * 2.3 + 0.5).r;
                float noise3 = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, noiseUV * 4.7 + 1.0).r;
                
                float combinedNoise = noise1 * 0.6 + noise2 * 0.3 + noise3 * 0.1;
                return pow(combinedNoise, _NoiseContrast);
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionHCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.uv = input.uv;
                output.worldPos = vertexInput.positionWS;
                
                // ���� ��� (���� ��ǥ ����)
                output.height = (output.positionWS.y) / _HeightScale;
                
                // ��� ��� (��� ���Ϳ� ���� ������ ����)
                output.slope = dot(normalInput.normalWS, float3(0, 1, 0));
                
                return output;
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                // ����ȭ�� ���� (0~1)
                float normalizedHeight = saturate(input.height);
                
                // ������ ����
                float noiseBlend = GetNoiseBlend(input.uv, input.worldPos.xz);
                
                // ���̿� ������� ��ȭ�� ����
                float heightWithVariation = normalizedHeight + (noiseBlend - 0.5) * _HeightVariation;
                heightWithVariation = saturate(heightWithVariation);
                
                // ��翡 ���� ����ġ (�ε巯�� ��ȯ)
                float slopeNormalized = saturate((input.slope - (1.0 - _SlopeThreshold)) / _SlopeBlendRange);
                float slopeWeight = SmoothStep3(0, 1, slopeNormalized, _SlopeContrast);
                slopeWeight = pow(slopeWeight, _SlopeInfluence);
                
                // �ؽ�ó ���ø�
                float2 baseUV = input.uv * _BaseScale;
                float2 midUV = input.uv * _MidScale;
                float2 topUV = input.uv * _TopScale;
                
                float4 baseColor = SAMPLE_TEXTURE2D(_BaseTexture, sampler_BaseTexture, baseUV);
                float4 midColor = SAMPLE_TEXTURE2D(_MidTexture, sampler_MidTexture, midUV);
                float4 topColor = SAMPLE_TEXTURE2D(_TopTexture, sampler_TopTexture, topUV);
                
                // ���� ��� ���� ����ġ ��� (�ε巯�� ��ȯ)
                float midStart = _MidHeight - _HeightBlendRange * 0.5;
                float midEnd = _MidHeight + _HeightBlendRange * 0.5;
                float topStart = _TopHeight - _HeightBlendRange * 0.5;
                float topEnd = _TopHeight + _HeightBlendRange * 0.5;
                
                // ����� ������ �ε巯�� ����
                float heightNoise = (noiseBlend - 0.5) * _NoiseStrength;
                
                float baseWeight = 1.0 - SmoothStep3(midStart + heightNoise, midEnd + heightNoise, heightWithVariation, _HeightContrast);
                float topWeight = SmoothStep3(topStart + heightNoise, topEnd + heightNoise, heightWithVariation, _HeightContrast);
                float midWeight = SmoothStep3(midStart - heightNoise, midEnd - heightNoise, heightWithVariation, _HeightContrast) - topWeight;
                
                midWeight = max(0, midWeight);
                
                // ����� ������ �� �ε巴�� ����
                float slopeInfluenceSmooth = lerp(0.2, 1.0, slopeWeight);
                baseWeight = lerp(baseWeight, lerp(baseWeight, 1.0, 0.6), (1.0 - slopeWeight) * _EdgeSoftness);
                midWeight *= slopeInfluenceSmooth;
                topWeight *= slopeInfluenceSmooth;
                
                // ����ġ ����ȭ
                float totalWeight = max(0.001, baseWeight + midWeight + topWeight);
                baseWeight /= totalWeight;
                midWeight /= totalWeight;
                topWeight /= totalWeight;
                
                // Overlay �������� �� �ڿ������� ���� �ռ�
                float4 blendedColor = baseColor * baseWeight + midColor * midWeight + topColor * topWeight;
                
                // �������� ���� �߰�
                float4 overlayBase = lerp(blendedColor, baseColor, baseWeight * _OverlayStrength);
                float4 overlayMid = lerp(overlayBase, midColor, midWeight * _OverlayStrength);
                float4 finalColor = lerp(overlayMid, topColor, topWeight * _OverlayStrength);
                
                // ���� ���� �ణ�� ��ȭ �߰�
                finalColor.rgb = lerp(finalColor.rgb, blendedColor.rgb, 0.7);
                
                // ���� ���
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float3 normal = normalize(input.normalWS);
                
                float NdotL = saturate(dot(normal, lightDir));
                float3 lighting = mainLight.color * NdotL + unity_AmbientSky.rgb;
                
                finalColor.rgb *= lighting;
                
                return finalColor;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }
            
            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}