Shader "Custom/RainbowSurface" {
    Properties {
        _Speed ("Color Cycle Speed", Range(0.1, 10)) = 1
        _Brightness ("Color Brightness", Range(0, 1)) = 0.8
        _Saturation ("Color Saturation", Range(0, 1)) = 0.8
    }

    SubShader {
        Tags {
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline" 
            "Queue" = "Geometry"
        }
        LOD 300

        Pass {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            // Include necessary URP libraries
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            // Shader properties
            CBUFFER_START(UnityPerMaterial)
                float _Speed;
                float _Brightness;
                float _Saturation;
            CBUFFER_END

            // Vertex input structure
            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            // Vertex output structure
            struct Vertex {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            // HSV to RGB conversion
            float3 hsv2rgb(float3 c) {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            // Vertex shader
            Vertex vert(Attributes input) {
                Vertex output;
                
                // Transform position from object space to world space
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                
                // Transform position from world space to clip space
                output.positionHCS = TransformWorldToHClip(output.positionWS);
                
                // Transform normal to world space
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                return output;
            }

            // Fragment shader
            half4 frag(Vertex input) : SV_Target {
                // Use time to cycle through hue
                float time = _Time.y * _Speed;
                
                // Create a smooth hue cycle
                float hue = frac(time * 0.1);
                
                // Create color with full saturation and brightness
                float3 rainbowColor = hsv2rgb(float3(hue, _Saturation, _Brightness));
                
                // Get main light direction and color
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(input.positionWS));
                
                // Simple diffuse lighting calculation
                float3 lighting = mainLight.color * saturate(dot(normalize(input.normalWS), mainLight.direction));
                
                // Combine rainbow color with lighting
                float3 finalColor = rainbowColor * (1 + lighting);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}